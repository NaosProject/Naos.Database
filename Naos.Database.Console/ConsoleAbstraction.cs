// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleAbstraction.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Console
{
    using System;
    using System.IO;

    using CLAP;

    using Naos.Database.Domain;
    using Naos.Database.Mongo;
    using Naos.Database.SqlServer;
    using Naos.Recipes.RunWithRetry;
    using Naos.Serialization.Factory;

    using OBeautifulCode.Validation.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Abstraction for use with <see cref="CLAP" /> to provide basic command line interaction.
    /// </summary>
    public class ConsoleAbstraction : ConsoleAbstractionBase
    {
        /// <summary>
        /// Backup a database to a file.
        /// </summary>
        /// <param name="databaseName">Name of database.</param>
        /// <param name="connectionString">SQL Server connection string to the database.</param>
        /// <param name="targetFilePath">Path to create backup at.</param>
        /// <param name="environment">Sets the Its.Configuration precedence to use specific settings.</param>
        /// <param name="debug">Launches the debugger.</param>
        [Verb(Aliases = "backupsql", Description = "Backup MS SQL Server database.")]
        public static void BackupSqlDatabase(
            [Required] [Aliases("name")] [Description("Name of database.")] string databaseName,
            [Required] [Aliases("connection")] [Description("SQL Server connection string to the database.")] string connectionString,
            [Required] [Aliases("file")] [Description("Path to create back at.")] string targetFilePath,
            [Aliases("")] [Description("Sets the Its.Configuration precedence to use specific settings.")] [DefaultValue(null)] string environment,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug)
        {
            CommonSetup(debug, environment);

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
        /// <param name="connectionDefinitionJson"><see cref="ConnectionDefinition" /> serialized as Config File JSON with credentials to the database.</param>
        /// <param name="targetFilePath">Path to create backup at.</param>
        /// <param name="utilityPath">Path to find supporting utilities (only needed for Mongo kind - should have mongodump.exe and mongorestore.exe).</param>
        /// <param name="workingDirectory">Path to write temp file (DEFAULT is parent of targetFilePath).</param>
        /// <param name="environment">Sets the Its.Configuration precedence to use specific settings.</param>
        /// <param name="debug">Launches the debugger.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Keeping this way.")]
        [Verb(Aliases = "backupmongo", Description = "Backup Mongo database.")]
        public static void BackupMongoDatabase(
            [Required] [Aliases("name")] [Description("Name of database.")] string databaseName,
            [Required] [Aliases("connection")] [Description("ConnectionDefinition serialized as Config File JSON.")] string connectionDefinitionJson,
            [Required] [Aliases("file")] [Description("Path to create back at.")] string targetFilePath,
            [Aliases("utility")] [Description("Path to find supporting utilities (should have mongodump.exe & mongorestore.exe).")] string utilityPath,
            [Aliases("temp")] [Description("Path to write temp file (DEFAULT is parent of targetFilePath).")] string workingDirectory,
            [Aliases("")] [Description("Sets the Its.Configuration precedence to use specific settings.")] [DefaultValue(null)] string environment,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug)
        {
            CommonSetup(debug, environment);

            new { connectionDefinitionJson }.Must().NotBeNullNorWhiteSpace();
            var serializer = SerializerFactory.Instance.BuildSerializer(Config.ConfigFileSerializationDescription);
            var connectionDefinition = serializer.Deserialize<ConnectionDefinition>(connectionDefinitionJson);

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
        /// <param name="connectionString">SQL Server connection string to the database.</param>
        /// <param name="sourceFilePath">Path to create back at.</param>
        /// <param name="dataDirectory">Directory housing data and log files.</param>
        /// <param name="environment">Sets the Its.Configuration precedence to use specific settings.</param>
        /// <param name="debug">Launches the debugger.</param>
        [Verb(Aliases = "restoresql", Description = "Restore MS SQL Server database.")]
        public static void RestoreSqlDatabase(
            [Required] [Aliases("name")] [Description("Name of database.")] string databaseName,
            [Required] [Aliases("connection")] [Description("SQL Server connection string to the database.")] string connectionString,
            [Required] [Aliases("file")] [Description("Path to load backup from.")] string sourceFilePath,
            [Required] [Aliases("data")] [Description("Directory housing data and log files.")] string dataDirectory,
            [Aliases("")] [Description("Sets the Its.Configuration precedence to use specific settings.")] [DefaultValue(null)] string environment,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug)
        {
            CommonSetup(debug, environment);

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
        /// <param name="connectionDefinitionJson"><see cref="ConnectionDefinition" /> serialized as Config File JSON with credentials to the database.</param>
        /// <param name="sourceFilePath">Path to create back at.</param>
        /// <param name="utilityPath">Path to find supporting utilities (only needed for Mongo kind - should have mongodump.exe and mongorestore.exe).</param>
        /// <param name="workingDirectory">Path to write temp file (DEFAULT is parent of targetFilePath).</param>
        /// <param name="environment">Sets the Its.Configuration precedence to use specific settings.</param>
        /// <param name="debug">Launches the debugger.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Keeping this way.")]
        [Verb(Aliases = "restoremongo", Description = "Restore Mongo database.")]
        public static void RestoreMongoDatabase(
            [Required] [Aliases("name")] [Description("Name of database.")] string databaseName,
            [Required] [Aliases("connection")] [Description("ConnectionDefinition serialized as Config File JSON.")] string connectionDefinitionJson,
            [Required] [Aliases("file")] [Description("Path to load backup from.")] string sourceFilePath,
            [Aliases("utility")] [Description("Path to find supporting utilities (should have mongodump.exe & mongorestore.exe).")] string utilityPath,
            [Aliases("temp")] [Description("Path to write temp file (DEFAULT is parent of sourceFilePath).")] string workingDirectory,
            [Aliases("")] [Description("Sets the Its.Configuration precedence to use specific settings.")] [DefaultValue(null)] string environment,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug)
        {
            CommonSetup(debug, environment);

            new { connectionDefinitionJson }.Must().NotBeNullNorWhiteSpace();
            var serializer = SerializerFactory.Instance.BuildSerializer(Config.ConfigFileSerializationDescription);
            var connectionDefinition = serializer.Deserialize<ConnectionDefinition>(connectionDefinitionJson);

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