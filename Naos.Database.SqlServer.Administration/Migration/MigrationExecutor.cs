// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationExecutor.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Administration
{
    using System;
    using System.Data.SqlClient;
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
        /// <param name="useAutomaticTransactionManagement">Optional whether or not to use automatic transaction management (default is true).</param>
        public static void Up(
            Assembly migrationAssembly,
            string connectionString,
            string databaseName,
            Action<string> announcer = null,
            TimeSpan timeout = default(TimeSpan),
            object applicationContext = null,
            bool useAutomaticTransactionManagement = true)
        {
            void NullAnnouncer(string message)
            {
                /* no-op */
            }

            Up(migrationAssembly, connectionString, databaseName, null, announcer ?? NullAnnouncer, timeout, applicationContext, useAutomaticTransactionManagement);
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
        /// <param name="useAutomaticTransactionManagement">Optional whether or not to use automatic transaction management (default is true).</param>
        public static void Up(
            Assembly migrationAssembly,
            string connectionString,
            string databaseName,
            long? targetVersion = null,
            Action<string> announcer = null,
            TimeSpan timeout = default(TimeSpan),
            object applicationContext = null,
            bool useAutomaticTransactionManagement = true)
        {
            void NullAnnouncer(string message)
            {
                /* no-op */
            }

            var runner = GetMigrationRunner(migrationAssembly, connectionString, databaseName, announcer ?? NullAnnouncer, timeout, applicationContext);
            if (targetVersion == null)
            {
                runner.MigrateUp(useAutomaticTransactionManagement);
            }
            else
            {
                runner.MigrateUp((long)targetVersion, useAutomaticTransactionManagement);
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
        /// <param name="useAutomaticTransactionManagement">Optional whether or not to use automatic transaction management (default is true).</param>
        public static void Down(
            Assembly migrationAssembly,
            string connectionString,
            string databaseName,
            long targetVersion,
            Action<string> announcer = null,
            TimeSpan timeout = default(TimeSpan),
            object applicationContext = null,
            bool useAutomaticTransactionManagement = true)
        {
            void NullAnnouncer(string message)
            {
                /* no-op */
            }

            var runner = GetMigrationRunner(migrationAssembly, connectionString, databaseName, announcer ?? NullAnnouncer, timeout, applicationContext);
            runner.MigrateDown(targetVersion, useAutomaticTransactionManagement);
        }

        private static MigrationRunner GetMigrationRunner(
            Assembly migrationAssembly,
            string connectionString,
            string databaseName,
            Action<string> announcer,
            TimeSpan timeout,
            object applicationContext)
        {
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
                InitialCatalog = databaseName,
            };

            var ret = sqlConnectionStringBuilder.ConnectionString;
            return ret;
        }
    }
}