// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationExecutor.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Migrator
{
    using System;
    using System.Data.SqlClient;
    using static System.FormattableString;
    using System.IO;

    using System.Linq;
    using System.Reflection;

    using FluentMigrator.Runner;
    using FluentMigrator.Runner.Announcers;
    using FluentMigrator.Runner.Initialization;
    using FluentMigrator.Runner.Processors.SqlServer;

    /// <summary>
    /// Class to run migrations from code, especially useful for setting up test databases or staging tables.
    /// </summary>
    public static class MigrationExecutor
    {
        /// <summary>
        /// Migrate up to a latest version.
        /// </summary>
        /// <param name="migrationAssembly">Assembly that the migration lives in.</param>
        /// <param name="connectionString">Connection string to the target database.</param>
        /// <param name="databaseName">Database name to target.</param>
        /// <param name="announcer">Optional lambda to pass announcements out during process (string messages of progress and status), default is Console.WriteLine.</param>
        /// <param name="timeout">Command timeout for the command(s) executed as part of the migration.</param>
        /// <param name="applicationContext">Optional application context (default is null).</param>
        /// <param name="dependantAssemblyPath">Optional path to load dependant assemblies from, default is none.</param>
        /// <param name="useAutomaticTransactionManagement">Optional whether or not to use automatic transaction management (default is true).</param>
        public static void Up(
            Assembly migrationAssembly,
            string connectionString,
            string databaseName,
            Action<string> announcer = null,
            TimeSpan timeout = default(TimeSpan),
            object applicationContext = null,
            string dependantAssemblyPath = null,
            bool useAutomaticTransactionManagement = true)
        {
            Up(migrationAssembly, connectionString, databaseName, null, announcer, timeout, applicationContext, dependantAssemblyPath);
        }

        /// <summary>
        /// Migrate up to a specific version.
        /// </summary>
        /// <param name="migrationAssembly">Assembly that the migration lives in.</param>
        /// <param name="connectionString">Connection string to the target database.</param>
        /// <param name="databaseName">Database name to target.</param>
        /// <param name="targetVersion">Optional version to migrate up to, default is null which will be latest.</param>
        /// <param name="announcer">Optional lambda to pass announcements out during process (string messages of progress and status), default is Console.WriteLine.</param>
        /// <param name="timeout">Command timeout for the command(s) executed as part of the migration.</param>
        /// <param name="applicationContext">Optional application context (default is null).</param>
        /// <param name="dependantAssemblyPath">Optional path to load dependant assemblies from, default is none.</param>
        /// <param name="useAutomaticTransactionManagement">Optional whether or not to use automatic transaction management (default is true).</param>
        public static void Up(
            Assembly migrationAssembly,
            string connectionString,
            string databaseName,
            long? targetVersion = null,
            Action<string> announcer = null,
            TimeSpan timeout = default(TimeSpan),
            object applicationContext = null,
            string dependantAssemblyPath = null,
            bool useAutomaticTransactionManagement = true)
        {
            var runner = GetMigrationRunner(
                migrationAssembly,
                connectionString,
                databaseName,
                announcer,
                timeout,
                applicationContext);

            if (string.IsNullOrEmpty(dependantAssemblyPath))
            {
                if (targetVersion == null)
                {
                    runner.MigrateUp(useAutomaticTransactionManagement);
                }
                else
                {
                    runner.MigrateUp((long)targetVersion, useAutomaticTransactionManagement);
                }
            }
            else
            {
                var allFilePaths = Directory.GetFiles(dependantAssemblyPath, "*", SearchOption.AllDirectories);
                ResolveEventHandler resolve = (sender, args) =>
                {
                    var dllNameWithoutExtension = args.Name.Split(',')[0];
                    var dllName = dllNameWithoutExtension + ".dll";
                    var fullDllPath = allFilePaths.FirstOrDefault(_ => _.EndsWith(dllName));
                    if (fullDllPath == null)
                    {
                        var message = Invariant($"Assembly not found Name: {args.Name}, Requesting Assembly FullName: {args.RequestingAssembly?.FullName}");
                        throw new TypeInitializationException(message, null);
                    }

                    // since the assembly might have been already loaded as a depdendency of another assembly...
                    var pathAsUri = new Uri(fullDllPath).ToString();
                    var alreadyLoaded =
                        AppDomain.CurrentDomain.GetAssemblies()
                            .Where(a => !a.IsDynamic)
                            .SingleOrDefault(_ => _.CodeBase == pathAsUri || _.Location == pathAsUri);

                    var ret = alreadyLoaded ?? Assembly.LoadFrom(fullDllPath);

                    return ret;
                };

                AppDomain.CurrentDomain.AssemblyResolve += resolve;

                if (targetVersion == null)
                {
                    runner.MigrateUp(useAutomaticTransactionManagement);
                }
                else
                {
                    runner.MigrateUp((long)targetVersion, useAutomaticTransactionManagement);
                }

                AppDomain.CurrentDomain.AssemblyResolve -= resolve;
            }
        }

        /// <summary>
        /// Migrate up to a specific version.
        /// </summary>
        /// <param name="migrationAssembly">The assembly that the migration lives in.</param>
        /// <param name="connectionString">The connection string to the target database.</param>
        /// <param name="databaseName">The database name to target.</param>
        /// <param name="targetVersion">Version to migrate up to, null will take latest.</param>
        /// <param name="announcer">Lambda to pass announcements out during process (string messages of progress and status).</param>
        /// <param name="timeout">The command timeout for the command(s) executed as part of the migration.</param>
        /// <param name="applicationContext">Optional application context (default is null).</param>
        /// <param name="dependantAssemblyPath">Optional path to load dependant assemblies from, default is none.</param>
        /// <param name="useAutomaticTransactionManagement">Optional whether or not to use automatic transaction management (default is true).</param>
        public static void Down(
            Assembly migrationAssembly,
            string connectionString,
            string databaseName,
            long targetVersion,
            Action<string> announcer = null,
            TimeSpan timeout = default(TimeSpan),
            object applicationContext = null,
            string dependantAssemblyPath = null,
            bool useAutomaticTransactionManagement = true)
        {
            var runner = GetMigrationRunner(migrationAssembly, connectionString, databaseName, announcer, timeout, applicationContext);

            if (string.IsNullOrEmpty(dependantAssemblyPath))
            {
                runner.MigrateDown(targetVersion, useAutomaticTransactionManagement);
            }
            else
            {
                var allFilePaths = Directory.GetFiles(dependantAssemblyPath, "*", SearchOption.AllDirectories);
                ResolveEventHandler resolve = (sender, args) =>
                    {
                        var dllNameWithoutExtension = args.Name.Split(',')[0];
                        var dllName = dllNameWithoutExtension + ".dll";
                        var fullDllPath = allFilePaths.FirstOrDefault(_ => _.EndsWith(dllName));
                        if (fullDllPath == null)
                        {
                            var message = Invariant($"Assembly not found Name: {args.Name}, Requesting Assembly FullName: {args.RequestingAssembly?.FullName}");
                            throw new TypeInitializationException(message, null);
                        }

                        // since the assembly might have been already loaded as a depdendency of another assembly...
                        var pathAsUri = new Uri(fullDllPath).ToString();
                        var alreadyLoaded =
                            AppDomain.CurrentDomain.GetAssemblies()
                                .Where(a => !a.IsDynamic)
                                .SingleOrDefault(_ => _.CodeBase == pathAsUri || _.Location == pathAsUri);

                        var ret = alreadyLoaded ?? Assembly.LoadFrom(fullDllPath);

                        return ret;
                    };

                AppDomain.CurrentDomain.AssemblyResolve += resolve;

                runner.MigrateDown(targetVersion, useAutomaticTransactionManagement);

                AppDomain.CurrentDomain.AssemblyResolve -= resolve;
            }
        }

        private static MigrationRunner GetMigrationRunner(
            Assembly migrationAssembly,
            string connectionString,
            string databaseName,
            Action<string> announcer,
            TimeSpan timeout,
            object applicationContext)
        {
            if (announcer == null)
            {
                announcer = Console.WriteLine;
            }

            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            var realConnectionString = ReplaceOrAddInitialCatalogInConnectionString(connectionString, databaseName);

            var fluentMigratorAnnouncer = new TextWriterAnnouncer(announcer) { ShowElapsedTime = true, ShowSql = true };

            var options = new MigrationOptions { PreviewOnly = false, Timeout = (int)timeout.TotalSeconds };

            var migrationContext = new RunnerContext(fluentMigratorAnnouncer)
                                       {
                                           ApplicationContext = applicationContext,
                                           Database = databaseName,
                                           Timeout = (int)timeout.TotalSeconds,
                                       };

            var factory = new SqlServer2014ProcessorFactory();
            var processor = factory.Create(realConnectionString, fluentMigratorAnnouncer, options);
            var runner = new MigrationRunner(migrationAssembly, migrationContext, processor);
            return runner;
        }

        private static string ReplaceOrAddInitialCatalogInConnectionString(string connectionString, string databaseName)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = databaseName
        };

            var ret = sqlConnectionStringBuilder.ConnectionString;
            return ret;
        }
    }
}
