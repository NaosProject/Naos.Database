// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationExecutor.cs" company="Naos">
//   Copyright 2014 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Utils.Database.Migrator
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
        /// Enumeration for the direction of migration.
        /// </summary>
        public enum MigrationDirection
        {
            /// <summary>
            /// Migrate up to a version.
            /// </summary>
            Up,

            /// <summary>
            /// Migrate down to a version (rollback).
            /// </summary>
            Down,
        }

        /// <summary>
        /// Migrate up to a specific version.
        /// </summary>
        /// <param name="migrationAssembly">The assembly that the migration lives in.</param>
        /// <param name="connectionString">The connection string to the target database.</param>
        /// <param name="databaseName">The database name to target.</param>
        /// <param name="targetVersion">The version to migrate up to.</param>
        /// <param name="direction">The direction of migration.</param>
        /// <param name="announcer">Lambda to pass announcements out during process (string messages of progress and status).</param>
        /// <param name="timeout">The timeout for the command(s) that are executed as part of the migration.</param>
        /// <param name="applicationContext">Optional application context (default is null).</param>
        /// <param name="useAutomaticTransactionManagement">Optional whether or not to use automatic transaction management (default is true).</param>
        public static void Migrate(
            Assembly migrationAssembly, 
            string connectionString, 
            string databaseName, 
            long targetVersion, 
            MigrationDirection direction, 
            Action<string> announcer,
            TimeSpan timeout = default(TimeSpan),
            object applicationContext = null, 
            bool useAutomaticTransactionManagement = true)
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            var realConnectionString = ReplaceOrAddInitialCatalogInConnectionString(connectionString, databaseName);

            var fluentMigratorAnnouncer = new TextWriterAnnouncer(announcer)
            {
                ShowElapsedTime = true,
                ShowSql = true
            };

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 600 };

            var migrationContext = new RunnerContext(fluentMigratorAnnouncer)
            {
                ApplicationContext = applicationContext,
                Database = databaseName,
                Timeout = (int)timeout.TotalSeconds,
            };

            var factory = new SqlServer2014ProcessorFactory();
            var processor = factory.Create(realConnectionString, fluentMigratorAnnouncer, options);
            var runner = new MigrationRunner(migrationAssembly, migrationContext, processor);

            switch (direction)
            {
                case MigrationDirection.Up:
                    runner.MigrateUp(targetVersion, useAutomaticTransactionManagement);
                    break;
                case MigrationDirection.Down:
                    runner.MigrateDown(targetVersion, useAutomaticTransactionManagement);
                    break;
                default:
                    throw new ArgumentException("Unsupported MigrationDirection: " + direction, "direction");
            }
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
