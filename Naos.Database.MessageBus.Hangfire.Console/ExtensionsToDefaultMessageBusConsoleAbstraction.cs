// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsToDefaultMessageBusConsoleAbstraction.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Hangfire.Console
{
    using System;
    using System.IO;

    using CLAP;

    using Its.Configuration;

    using Naos.Database.Domain;
    using Naos.Database.MessageBus.Scheduler;
    using Naos.Database.Mongo;
    using Naos.Database.SqlServer;
    using Naos.Logging.Domain;

    using Naos.Recipes.RunWithRetry;

    using static System.FormattableString;

    /// <summary>
    /// Abstraction for use with <see cref="CLAP" /> to provide basic command line interaction.
    /// </summary>
    public partial class DefaultMessageBusConsoleAbstraction
    {
        /// <summary>
        /// Backup a database to a file.
        /// </summary>
        /// <param name="databaseName">Name of database.</param>
        /// <param name="targetFilePath">Path to create backup at.</param>
        /// <param name="debug">Launches the debugger.</param>
        [Verb(Aliases = "backupsql", Description = "Backup MS SQL Server database.")]
        public static void BackupSqlDatabase(
            [Required] [Aliases("name")] [Description("Name of database.")] string databaseName,
            [Required] [Aliases("file")] [Description("Path to create back at.")] string targetFilePath,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug)
        {
            CommonSetup(debug, null, new LogWritingSettings(new[] { new ConsoleLogConfig(LogItemOrigins.All, LogItemOrigins.None), }));

            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            var connectionDefinition = settings.DatabaseNameToLocalhostConnectionDefinitionMap[databaseName.ToUpperInvariant()];
            var connectionString = connectionDefinition.ToSqlServerConnectionString();

            var errorHandling = ErrorHandling.StopOnError;
            var compressionOption = CompressionOption.NoCompression;

            var backupFilePathUri = new Uri(targetFilePath);
            var backupDetails = new BackupDetails
            {
                Name = Invariant($"{databaseName}DatabaseBackup"),
                BackupTo = backupFilePathUri,
                ChecksumOption = ChecksumOption.NoChecksum,
                Cipher = Cipher.NoEncryption,
                CompressionOption = compressionOption,
                Description = null,
                Device = Device.Disk,
                ErrorHandling = errorHandling,
            };

            Run.TaskUntilCompletion(SqlServerDatabaseManager.BackupFullAsync(connectionString, databaseName, backupDetails));
        }

        /// <summary>
        /// Backup a database to a file.
        /// </summary>
        /// <param name="databaseName">Name of database.</param>
        /// <param name="targetFilePath">Path to create backup at.</param>
        /// <param name="utilityPath">Path to find supporting utilities (only needed for Mongo kind - should have mongodump.exe and mongorestore.exe).</param>
        /// <param name="workingDirectory">Path to write temp file (DEFAULT is parent of targetFilePath).</param>
        /// <param name="debug">Launches the debugger.</param>
        [Verb(Aliases = "backupmongo", Description = "Backup Mongo database.")]
        public static void BackupMongoDatabase(
            [Required] [Aliases("name")] [Description("Name of database.")] string databaseName,
            [Required] [Aliases("file")] [Description("Path to create back at.")] string targetFilePath,
            [Aliases("utility")] [Description("Path to find supporting utilities (should have mongodump.exe & mongorestore.exe).")] string utilityPath,
            [Aliases("temp")] [Description("Path to write temp file (DEFAULT is parent of targetFilePath).")] string workingDirectory,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug)
        {
            CommonSetup(debug, null, new LogWritingSettings(new[] { new ConsoleLogConfig(LogItemOrigins.All, LogItemOrigins.None), }));

            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            var connectionDefinition = settings.DatabaseNameToLocalhostConnectionDefinitionMap[databaseName.ToUpperInvariant()];

            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                workingDirectory = Path.GetDirectoryName(targetFilePath);
            }

            var errorHandling = ErrorHandling.None;
            var compressionOption = CompressionOption.Compression;

            var backupFilePathUri = new Uri(targetFilePath);
            var backupDetails = new BackupDetails
            {
                Name = Invariant($"{databaseName}DatabaseBackup"),
                BackupTo = backupFilePathUri,
                ChecksumOption = ChecksumOption.NoChecksum,
                Cipher = Cipher.NoEncryption,
                CompressionOption = compressionOption,
                Description = null,
                Device = Device.Disk,
                ErrorHandling = errorHandling,
            };

            Run.TaskUntilCompletion(MongoDatabaseManager.BackupFullAsync(connectionDefinition, databaseName, backupDetails, workingDirectory, utilityPath));
        }

        /// <summary>
        /// Restore a database from a file.
        /// </summary>
        /// <param name="databaseName">Name of database.</param>
        /// <param name="sourceFilePath">Path to create back at.</param>
        /// <param name="dataDirectory">Directory housing data and log files.</param>
        /// <param name="debug">Launches the debugger.</param>
        [Verb(Aliases = "restoresql", Description = "Restore MS SQL Server database.")]
        public static void RestoreSqlDatabase(
            [Required] [Aliases("name")] [Description("Name of database.")] string databaseName,
            [Required] [Aliases("file")] [Description("Path to load backup from.")] string sourceFilePath,
            [Required] [Aliases("data")] [Description("Directory housing data and log files.")] string dataDirectory,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug)
        {
            CommonSetup(debug, null, new LogWritingSettings(new[] { new ConsoleLogConfig(LogItemOrigins.All, LogItemOrigins.None), }));

            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            var connectionDefinition = settings.DatabaseNameToLocalhostConnectionDefinitionMap[databaseName.ToUpperInvariant()];
            var connectionString = connectionDefinition.ToSqlServerConnectionString();

            var dataFilePath = Path.Combine(dataDirectory, databaseName + "Dat.mdf");
            var logFilePath = Path.Combine(dataDirectory, databaseName + "Log.ldf");

            var errorHandling = ErrorHandling.StopOnError;
            var recoveryOption = RecoveryOption.Recovery;

            var backupFilePathUri = new Uri(sourceFilePath);
            var restoreDetails = new RestoreDetails
            {
                ChecksumOption = ChecksumOption.NoChecksum,
                Device = Device.Disk,
                ErrorHandling = errorHandling,
                DataFilePath = dataFilePath,
                LogFilePath = logFilePath,
                RecoveryOption = recoveryOption,
                ReplaceOption = ReplaceOption.ReplaceExistingDatabase,
                RestoreFrom = backupFilePathUri,
                RestrictedUserOption = RestrictedUserOption.Normal,
            };

            Run.TaskUntilCompletion(SqlServerDatabaseManager.RestoreFullAsync(connectionString, databaseName, restoreDetails));
        }

        /// <summary>
        /// Restore a database from a file.
        /// </summary>
        /// <param name="databaseName">Name of database.</param>
        /// <param name="sourceFilePath">Path to create back at.</param>
        /// <param name="utilityPath">Path to find supporting utilities (only needed for Mongo kind - should have mongodump.exe and mongorestore.exe).</param>
        /// <param name="workingDirectory">Path to write temp file (DEFAULT is parent of targetFilePath).</param>
        /// <param name="debug">Launches the debugger.</param>
        [Verb(Aliases = "restoremongo", Description = "Restore Mongo database.")]
        public static void RestoreMongoDatabase(
            [Required] [Aliases("name")] [Description("Name of database.")] string databaseName,
            [Required] [Aliases("file")] [Description("Path to load backup from.")] string sourceFilePath,
            [Aliases("utility")] [Description("Path to find supporting utilities (should have mongodump.exe & mongorestore.exe).")] string utilityPath,
            [Aliases("temp")] [Description("Path to write temp file (DEFAULT is parent of sourceFilePath).")] string workingDirectory,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug)
        {
            CommonSetup(debug, null, new LogWritingSettings(new[] { new ConsoleLogConfig(LogItemOrigins.All, LogItemOrigins.None), }));

            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            var connectionDefinition = settings.DatabaseNameToLocalhostConnectionDefinitionMap[databaseName.ToUpperInvariant()];

            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                workingDirectory = Path.GetDirectoryName(sourceFilePath);
            }

            var errorHandling = ErrorHandling.None;
            var recoveryOption = RecoveryOption.NoRecovery;

            var backupFilePathUri = new Uri(sourceFilePath);
            var restoreDetails = new RestoreDetails
            {
                ChecksumOption = ChecksumOption.NoChecksum,
                Device = Device.Disk,
                ErrorHandling = errorHandling,
                RecoveryOption = recoveryOption,
                ReplaceOption = ReplaceOption.ReplaceExistingDatabase,
                RestoreFrom = backupFilePathUri,
                RestrictedUserOption = RestrictedUserOption.Normal,
            };

            Run.TaskUntilCompletion(MongoDatabaseManager.RestoreFullAsync(connectionDefinition, databaseName, restoreDetails, workingDirectory, utilityPath));
        }
    }
}