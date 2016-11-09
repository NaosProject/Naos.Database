// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseManager.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Conditions;

    using Dapper;

    using Its.Log.Instrumentation;

    using Naos.Database.Contract;
    using Naos.Database.Tools.Backup;

    /// <summary>
    /// Class with tools for adding, removing, and updating databases.
    /// </summary>
    public static class DatabaseManager
    {
        /// <summary>
        /// Puts the database into single user mode.
        /// </summary>
        /// <param name="connectionString">The connection string to the intended server.</param>
        /// <param name="databaseName">The name of the target database.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void PutDatabaseInSingleUserMode(string connectionString, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                PutDatabaseInSingleUserMode(connection, databaseName, timeout);

                connection.Close();
            }
        }

        /// <summary>
        /// Put the database into multiple user mode.
        /// </summary>
        /// <param name="connectionString">The connection string to the intended server.</param>
        /// <param name="databaseName">The name of the target database.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void PutDatabaseIntoMultiUserMode(string connectionString, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                PutDatabaseIntoMultiUserMode(connection, databaseName, timeout);
                
                connection.Close();
            }
        }

        /// <summary>
        /// Set the database to off line.
        /// </summary>
        /// <param name="connectionString">The connection string to the intended server.</param>
        /// <param name="databaseName">The name of the target database.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void TakeDatabaseOffline(string connectionString, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(databaseName);
            var commandText = "ALTER DATABASE " + databaseName + " SET offline WITH ROLLBACK IMMEDIATE";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);
                connection.Close();
            }
        }

        /// <summary>
        /// Set the database to on line.
        /// </summary>
        /// <param name="connectionString">The connection string to the intended server.</param>
        /// <param name="databaseName">The name of the target database.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void BringDatabaseOnline(string connectionString, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(databaseName);
            var commandText = "ALTER DATABASE " + databaseName + " SET online";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);
                connection.Close();
            }
        }

        /// <summary>
        /// Create a new database using provided definition.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="configuration">Detailed information about the database.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void Create(string connectionString, DatabaseConfiguration configuration, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            ThrowIfBadOnCreateOrModify(configuration);
            var databaseFileMaxSize = configuration.DataFileMaxSizeInKb == Constants.InfinityMaxSize ? "UNLIMITED" : configuration.DataFileMaxSizeInKb + "KB";
            var logFileMaxSize = configuration.LogFileMaxSizeInKb == Constants.InfinityMaxSize ? "UNLIMITED" : configuration.LogFileMaxSizeInKb + "KB";
            var commandText =
                $@"CREATE DATABASE {configuration.DatabaseName}
                        ON
                        ( NAME = '{
                    configuration.DataFileLogicalName}',
                        FILENAME = '{configuration.DataFilePath}',
                        SIZE = {
                    configuration.DataFileCurrentSizeInKb}KB,
                        MAXSIZE = {databaseFileMaxSize},
                        FILEGROWTH = {
                    configuration.DataFileGrowthSizeInKb}KB )
                        LOG ON
                        ( NAME = '{
                    configuration.LogFileLogicalName}',
                        FILENAME = '{configuration.LogFilePath}',
                        SIZE = {
                    configuration.LogFileCurrentSizeInKb}KB,
                        MAXSIZE = {logFileMaxSize},
                        FILEGROWTH = {
                    configuration.LogFileGrowthSizeInKb}KB )";
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);

                if (configuration.RecoveryMode != RecoveryMode.Unspecified)
                {
                    SetRecoveryModeAsync(connection, configuration.DatabaseName, configuration.RecoveryMode, timeout).Wait();
                }

                connection.Close();
            }
        }

        /// <summary>
        /// List databases from server.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>All databases from server.</returns>
        public static DatabaseConfiguration[] Retrieve(string connectionString, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            // using the database name is the only good way to differentiate System from User databases
            // see: http://stackoverflow.com/a/9682659/356790.  This is how the SMO does it.
            const string Query = @"
                SELECT
                    d.name as DatabaseName,
                    df.name as DataFileLogicalName,
                    df.physical_name as DataFilePath,
                    lf.name as LogFileLogicalName,
                    lf.physical_name as LogFilePath,
                    REPLACE(d.recovery_model_desc, '_', '') as RecoveryMode,
                    case when d.name in ('master','model','msdb','tempdb') then 'System' else 'User' end as DatabaseType
                FROM sys.databases d 
                INNER JOIN sys.master_files df
                    ON d.database_id = df.database_id AND df.type = 0
                INNER JOIN sys.master_files lf
                    ON d.database_id = lf.database_id AND lf.type = 1";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var ret = connection.Query<DatabaseConfiguration>(Query, null, null, true, (int?)timeout.TotalSeconds).ToArray();
                connection.Close();

                return ret;
            }
        }

        /// <summary>
        /// Update a database to the new definition.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="currentConfiguration">Detailed information about how the database is.</param>
        /// <param name="newConfiguration">Detailed information about how the database should look after the update.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void Update(
            string connectionString,
            DatabaseConfiguration currentConfiguration,
            DatabaseConfiguration newConfiguration,
            TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            ThrowIfBadOnCreateOrModify(currentConfiguration);
            ThrowIfBadOnCreateOrModify(newConfiguration);

            var realConnectionString = ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(connectionString, currentConfiguration.DatabaseName); // make sure it's going to take the only connection when it goes in single user

            using (var connection = new SqlConnection(realConnectionString))
            {
                connection.Open();

                PutDatabaseInSingleUserMode(connection, currentConfiguration.DatabaseName, timeout);

                if (currentConfiguration.DatabaseName != newConfiguration.DatabaseName)
                {
                    var renameDatabaseText = @"ALTER DATABASE " + currentConfiguration.DatabaseName + " MODIFY NAME = "
                                             + newConfiguration.DatabaseName;
                    connection.Execute(renameDatabaseText, null, null, (int?)timeout.TotalSeconds);
                }

                if ((currentConfiguration.DataFileLogicalName != newConfiguration.DataFileLogicalName)
                    && (currentConfiguration.DataFilePath != newConfiguration.DataFilePath))
                {
                    var updateDataFileText =
                        $@"ALTER DATABASE {newConfiguration.DatabaseName} MODIFY FILE (
                        NAME = '{
                            currentConfiguration.DataFileLogicalName}',
                        NEWNAME = '{newConfiguration.DataFileLogicalName
                            }',
                        FILENAME = '{newConfiguration.DataFilePath}')";
                    connection.Execute(updateDataFileText, null, null, (int?)timeout.TotalSeconds);
                }

                if ((currentConfiguration.LogFileLogicalName != newConfiguration.LogFileLogicalName)
                    && (currentConfiguration.LogFilePath != newConfiguration.LogFilePath))
                {
                    var updateLogFileText =
                        $@"ALTER DATABASE {newConfiguration.DatabaseName} MODIFY FILE (
                        NAME = '{
                            currentConfiguration.LogFileLogicalName}',
                        NEWNAME = '{newConfiguration.LogFileLogicalName
                            }',
                        FILENAME = '{newConfiguration.LogFilePath}')";
                    connection.Execute(updateLogFileText, null, null, (int?)timeout.TotalSeconds);
                }

                if ((newConfiguration.RecoveryMode != RecoveryMode.Unspecified) && (currentConfiguration.RecoveryMode != newConfiguration.RecoveryMode))
                {
                    SetRecoveryModeAsync(connection, newConfiguration.DatabaseName, newConfiguration.RecoveryMode, timeout).Wait();
                }

                PutDatabaseIntoMultiUserMode(connection, newConfiguration.DatabaseName, timeout);

                connection.Close();
            }
        }

        /// <summary>
        /// Delete a database.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="databaseName">Name of the database to delete.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void Delete(string connectionString, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(databaseName);
            var realConnectionString = ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(connectionString, databaseName); // make sure it's going to take the only connection when it goes in single user
            var commandText = "USE master; DROP DATABASE " + databaseName;
            using (var connection = new SqlConnection(realConnectionString))
            {
                PutDatabaseInSingleUserMode(connection, databaseName, timeout);
                connection.Open();
                connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);
                connection.Close();
            }
        }

        /// <summary>
        /// Retrieves the default location that the server will save data files when creating a new database (Only works on MS SQL Server 2012 and higher, otherwise null).
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>Default location that the server will save data files to.</returns>
        public static string GetInstanceDefaultDataPath(string connectionString, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            string ret;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                ret = connection.ExecuteScalar<string>("SELECT CONVERT(sysname, SERVERPROPERTY('InstanceDefaultDataPath'))", null, null, (int?)timeout.TotalSeconds);
                connection.Close();
            }

            return ret;
        }

        /// <summary>
        /// Retrieves the default location that the server will save log files when creating a new database (Only works on MS SQL Server 2012 and higher, otherwise null).
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>Default location that the server will save log files to.</returns>
        public static string GetInstanceDefaultLogPath(string connectionString, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            string ret;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                ret = connection.ExecuteScalar<string>("SELECT CONVERT(sysname, SERVERPROPERTY('InstanceDefaultLogPath'))", null, null, (int?)timeout.TotalSeconds);
                connection.Close();
            }

            return ret;
        }

        /// <summary>
        /// Perform a full (non-differential) database backup.
        /// </summary>
        /// <remarks>
        /// During a full or differential database backup, SQL Server backs up enough of the transaction log to produce a consistent database when the backup is restored.
        /// When you restore a backup created by BACKUP DATABASE (a data backup), the entire backup is restored. Only a log backup can be restored to a specific time or transaction within the backup
        /// This method does not support appending a backup to an existing file nor any of the methods to age/overwrite backups in an existing file.
        /// This method will always overwrite an existing file.  It's more difficult to get SQL Server to emit an error if a file already exists.  
        /// See: <a href="http://dba.stackexchange.com/questions/98536/how-to-generate-an-error-when-backing-up-to-an-existing-file"/>
        /// </remarks>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="databaseName">Name of the database to backup.</param>
        /// <param name="backupDetails">The details of how to perform the backup.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void BackupFull(
            string connectionString,
            string databaseName,
            BackupDetails backupDetails,
            TimeSpan timeout = default(TimeSpan))
        {
            BackupFullAsync(connectionString, databaseName, backupDetails, timeout).Wait();
        }

        /// <summary>
        /// Perform a full (non-differential) database backup.
        /// </summary>
        /// <remarks>
        /// During a full or differential database backup, SQL Server backs up enough of the transaction log to produce a consistent database when the backup is restored.
        /// When you restore a backup created by BACKUP DATABASE (a data backup), the entire backup is restored. Only a log backup can be restored to a specific time or transaction within the backup
        /// This method does not support appending a backup to an existing file nor any of the methods to age/overwrite backups in an existing file.
        /// This method will always overwrite an existing file.  It's more difficult to get SQL Server to emit an error if a file already exists.  
        /// See: <a href="http://dba.stackexchange.com/questions/98536/how-to-generate-an-error-when-backing-up-to-an-existing-file"/>
        /// </remarks>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="databaseName">Name of the database to backup.</param>
        /// <param name="backupDetails">The details of how to perform the backup.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>Task to support async await calling.</returns>
        public static async Task BackupFullAsync(string connectionString, string databaseName, BackupDetails backupDetails, TimeSpan timeout = default(TimeSpan))
        {
            using (var activity = Log.Enter(() => new { Database = databaseName, BackupDetails = backupDetails }))
            {
                // check parameters
                Condition.Requires(connectionString, "connectionString").IsNotNullOrWhiteSpace();
                Condition.Requires(databaseName, "databaseName").IsNotNullOrWhiteSpace();
                Condition.Requires(backupDetails, "backupDetails").IsNotNull();
                backupDetails.ThrowIfInvalid();

                // construct the non-options portion of the backup command
                var commandBuilder = new StringBuilder();
                string backupDatabase = $"BACKUP DATABASE [{databaseName}]";
                commandBuilder.AppendLine(backupDatabase);

                string deviceName;
                string backupLocation;
                if (backupDetails.Device == Device.Disk)
                {
                    deviceName = "DISK";
                    backupLocation = backupDetails.BackupTo.LocalPath;
                }
                else if (backupDetails.Device == Device.Url)
                {
                    deviceName = "URL";
                    backupLocation = backupDetails.BackupTo.ToString();
                }
                else
                {
                    throw new NotSupportedException("This device is not supported: " + backupDetails.Device);
                }

                string backupTo = $"TO {deviceName} = '{backupLocation}'";
                commandBuilder.AppendLine(backupTo);

                // construct the WITH options
                commandBuilder.AppendLine("WITH");
                var withOptions = new List<string>();

                if (!string.IsNullOrWhiteSpace(backupDetails.Credential))
                {
                    string credential = $"CREDENTIAL = '{backupDetails.Credential}'";
                    withOptions.Add(credential);
                }

                string checksumOption;
                if (backupDetails.ChecksumOption == ChecksumOption.Checksum)
                {
                    checksumOption = "CHECKSUM";
                    withOptions.Add(checksumOption);

                    string errorHandling;
                    if (backupDetails.ErrorHandling == ErrorHandling.ContinueAfterError)
                    {
                        errorHandling = "CONTINUE_AFTER_ERROR";
                    }
                    else if (backupDetails.ErrorHandling == ErrorHandling.StopOnError)
                    {
                        errorHandling = "STOP_ON_ERROR";
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "This error handling option is not supported: " + backupDetails.ErrorHandling);
                    }

                    withOptions.Add(errorHandling);
                }
                else if (backupDetails.ChecksumOption == ChecksumOption.NoChecksum)
                {
                    checksumOption = "NO_CHECKSUM";
                    withOptions.Add(checksumOption);
                }
                else
                {
                    throw new NotSupportedException(
                        "This checksum option is not supported: " + backupDetails.ChecksumOption);
                }

                string compressionOption;
                if (backupDetails.CompressionOption == CompressionOption.Compression)
                {
                    compressionOption = "COMPRESSION";
                }
                else if (backupDetails.CompressionOption == CompressionOption.NoCompression)
                {
                    compressionOption = "NO_COMPRESSION";
                }
                else
                {
                    throw new NotSupportedException(
                        "This compression option is not supported: " + backupDetails.CompressionOption);
                }

                withOptions.Add(compressionOption);

                if (!string.IsNullOrWhiteSpace(backupDetails.Name))
                {
                    string name = $"NAME = '{backupDetails.Name}'";
                    withOptions.Add(name);
                }

                if (!string.IsNullOrWhiteSpace(backupDetails.Description))
                {
                    string description = $"DESCRIPTION = '{backupDetails.Description}'";
                    withOptions.Add(description);
                }

                if (backupDetails.Cipher != Cipher.NoEncryption)
                {
                    string cipher;
                    if (backupDetails.Cipher == Cipher.Aes128)
                    {
                        cipher = "AES_128";
                    }
                    else if (backupDetails.Cipher == Cipher.Aes192)
                    {
                        cipher = "AES_192";
                    }
                    else if (backupDetails.Cipher == Cipher.Aes256)
                    {
                        cipher = "AES_256";
                    }
                    else if (backupDetails.Cipher == Cipher.TripleDes3Key)
                    {
                        cipher = "TRIPLE_DES_3KEY";
                    }
                    else
                    {
                        throw new NotSupportedException("This cipher is not supported: " + backupDetails.Cipher);
                    }

                    string encryptor;
                    if (backupDetails.Encryptor == Encryptor.ServerCertificate)
                    {
                        encryptor = "SERVER CERTIFICATE";
                    }
                    else if (backupDetails.Encryptor == Encryptor.ServerAsymmetricKey)
                    {
                        encryptor = "SERVER ASYMMETRIC KEY";
                    }
                    else
                    {
                        throw new NotSupportedException("This encryptor is not supported: " + backupDetails.Encryptor);
                    }

                    string encryption = $"ENCRYPTION ( ALGORITHM = {cipher}, {encryptor} = {backupDetails.EncryptorName})";
                    withOptions.Add(encryption);
                }

                withOptions.Add("FORMAT");

                // append the WITH options
                string withOptionsCsv =
                    withOptions.Aggregate((current, next) => current + "," + Environment.NewLine + next);
                commandBuilder.AppendLine(withOptionsCsv);

                // execute the backup
                string command = commandBuilder.ToString();
                activity.Trace(() => "Running command: " + command);

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                        {
                            Log.Write(() => $"Server Message: {e.Message}");
                        };

                    await connection.ExecuteScalarAsync(command, commandTimeout: (int?)timeout.TotalSeconds);

                    connection.Close();
                }

                activity.Trace(() => "Completed successfully.");
            }
        }

        /// <summary>
        /// Restores an entire database from a full database backup.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="databaseName">Name of the database to restore.</param>
        /// <param name="restoreDetails">The details of how to perform the restore.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        public static void RestoreFull(
            string connectionString,
            string databaseName,
            RestoreDetails restoreDetails,
            TimeSpan timeout = default(TimeSpan))
        {
            RestoreFullAsync(connectionString, databaseName, restoreDetails, timeout).Wait();
        }

        /// <summary>
        /// Restores an entire database from a full database backup.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="databaseName">Name of the database to restore.</param>
        /// <param name="restoreDetails">The details of how to perform the restore.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>Task to support async await calling.</returns>
        public static async Task RestoreFullAsync(
            string connectionString,
            string databaseName,
            RestoreDetails restoreDetails,
            TimeSpan timeout = default(TimeSpan))
        {
            // check parameters
            Condition.Requires(connectionString, "connectionString").IsNotNullOrWhiteSpace();
            Condition.Requires(databaseName, "databaseName").IsNotNullOrWhiteSpace();
            Condition.Requires(restoreDetails, "restoreDetails").IsNotNull();

            restoreDetails.ThrowIfInvalid();

            using (var activity = Log.Enter(() => new { Database = databaseName, RestoreDetails = restoreDetails }))
            {
                // construct the non-options portion of the backup command
                var commandBuilder = new StringBuilder();
                string backupDatabase = $"RESTORE DATABASE [{databaseName}]";
                commandBuilder.AppendLine(backupDatabase);

                string deviceName;
                string backupLocation;
                if (restoreDetails.Device == Device.Disk)
                {
                    deviceName = "DISK";
                    backupLocation = restoreDetails.RestoreFrom.LocalPath;
                }
                else if (restoreDetails.Device == Device.Url)
                {
                    deviceName = "URL";
                    backupLocation = restoreDetails.RestoreFrom.ToString();
                }
                else
                {
                    throw new NotSupportedException("This device is not supported: " + restoreDetails.Device);
                }

                string restoreFrom = $"FROM {deviceName} = '{backupLocation}'";
                commandBuilder.AppendLine(restoreFrom);

                // construct the WITH options
                commandBuilder.AppendLine("WITH");
                var withOptions = new List<string>();

                switch (restoreDetails.RecoveryOption)
                {
                    case RecoveryOption.Recovery:
                        withOptions.Add("RECOVERY");
                        break;
                    case RecoveryOption.NoRecovery:
                        withOptions.Add("NORECOVERY");
                        break;
                    default:
                        throw new ArgumentException("Unsupported recovery option: " + restoreDetails.RecoveryOption);
                }

                if (!string.IsNullOrWhiteSpace(restoreDetails.Credential))
                {
                    string credential = $"CREDENTIAL = '{restoreDetails.Credential}'";
                    withOptions.Add(credential);
                }

                // should the backup be restored to a specific path?
                bool useSpecifiedDataFilePath = !string.IsNullOrWhiteSpace(restoreDetails.DataFilePath);
                bool useSpecifiedLogFilePath = !string.IsNullOrWhiteSpace(restoreDetails.LogFilePath);
                if (useSpecifiedDataFilePath || useSpecifiedLogFilePath)
                {
                    activity.Trace(() => "Confirming that the specific file is in the server file list.");
                    string fileListCommand = "RESTORE FILELISTONLY " + restoreFrom;
                    List<RestoreFile> restoreFiles;
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        restoreFiles =
                            connection.Query<RestoreFile>(fileListCommand, commandTimeout: (int?)timeout.TotalSeconds)
                                .ToList();
                        connection.Close();
                    }

                    if (useSpecifiedDataFilePath)
                    {
                        IEnumerable<RestoreFile> dataFiles = restoreFiles.Where(_ => _.Type == "D").ToList();
                        if (dataFiles.Count() != 1)
                        {
                            throw new InvalidOperationException(
                                "Cannot restore from a backup with multiple data files when the file path to restore the data to is specified in the restore details.");
                        }

                        string moveTo = $"MOVE '{dataFiles.First().LogicalName}' TO '{restoreDetails.DataFilePath}'";
                        withOptions.Add(moveTo);
                    }

                    if (useSpecifiedLogFilePath)
                    {
                        IEnumerable<RestoreFile> logFiles = restoreFiles.Where(_ => _.Type == "L").ToList();
                        if (logFiles.Count() != 1)
                        {
                            throw new InvalidOperationException(
                                "Cannot restore from a backup with multiple log files when the file path to restore the log to is specified in the restore details.");
                        }

                        string moveTo = $"MOVE '{logFiles.First().LogicalName}' TO '{restoreDetails.LogFilePath}'";
                        withOptions.Add(moveTo);
                    }
                }

                string checksumOption;
                if (restoreDetails.ChecksumOption == ChecksumOption.Checksum)
                {
                    checksumOption = "CHECKSUM";
                    withOptions.Add(checksumOption);

                    string errorHandling;
                    if (restoreDetails.ErrorHandling == ErrorHandling.ContinueAfterError)
                    {
                        errorHandling = "CONTINUE_AFTER_ERROR";
                    }
                    else if (restoreDetails.ErrorHandling == ErrorHandling.StopOnError)
                    {
                        errorHandling = "STOP_ON_ERROR";
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "This error handling option is not supported: " + restoreDetails.ErrorHandling);
                    }

                    withOptions.Add(errorHandling);
                }
                else if (restoreDetails.ChecksumOption == ChecksumOption.NoChecksum)
                {
                    checksumOption = "NO_CHECKSUM";
                    withOptions.Add(checksumOption);
                }
                else
                {
                    throw new NotSupportedException(
                        "This checksum option is not supported: " + restoreDetails.ChecksumOption);
                }

                if (restoreDetails.RestrictedUserOption == RestrictedUserOption.Normal)
                {
                }
                else if (restoreDetails.RestrictedUserOption == RestrictedUserOption.RestrictedUser)
                {
                    withOptions.Add("RESTRICTED_USER");
                }
                else
                {
                    throw new NotSupportedException(
                        "This restricted user option is not supported: " + restoreDetails.RestrictedUserOption);
                }

                if (restoreDetails.ReplaceOption == ReplaceOption.DoNotReplaceExistingDatabaseAndThrow)
                {
                }
                else if (restoreDetails.ReplaceOption == ReplaceOption.ReplaceExistingDatabase)
                {
                    withOptions.Add("REPLACE");
                }
                else
                {
                    throw new NotSupportedException(
                        "This replace option is not supported: " + restoreDetails.ReplaceOption);
                }

                // append the WITH options
                string withOptionsCsv =
                    withOptions.Aggregate((current, next) => current + "," + Environment.NewLine + next);
                commandBuilder.AppendLine(withOptionsCsv);

                // execute the restore
                string command = commandBuilder.ToString();
                activity.Trace(() => "Running command: " + command);

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                        {
                            Log.Write(() => $"Server Message: {e.Message}");
                        };

                    await connection.ExecuteScalarAsync(command, commandTimeout: (int?)timeout.TotalSeconds);
                    connection.Close();
                }

                activity.Trace(() => "Completed successfully.");
            }
        }

        /// <summary>
        /// Sets the recovery mode of the database.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="databaseName">Name of the database to restore.</param>
        /// <param name="recoveryMode">Recovery mode to set database to.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>Task for async.</returns>
        public static async Task SetRecoveryModeAsync(string connectionString, string databaseName, RecoveryMode recoveryMode, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                    {
                        Log.Write(() => $"Server Message: {e.Message}");
                    };

                await SetRecoveryModeAsync(connection, databaseName, recoveryMode, timeout);
                connection.Close();
            }
        }

        /// <summary>
        /// Gets the recovery mode of the database.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="databaseName">Name of the database to restore.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>Recovery mode.</returns>
        public static async Task<RecoveryMode> GetRecoveryModeAsync(string connectionString, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(databaseName);

            var commandText = $"SELECT recovery_model_desc FROM sys.databases WHERE name = '{databaseName}'";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                    {
                        Log.Write(() => $"Server Message: {e.Message}");
                    };

                var recoveryModeRaw = await connection.ExecuteScalarAsync<string>(commandText, commandTimeout: (int?)timeout.TotalSeconds);
                connection.Close();

                var recoveryMode = Enum.Parse(typeof(RecoveryMode), recoveryModeRaw.ToUpperInvariant().Replace("_", string.Empty), true);
                return (RecoveryMode)recoveryMode;
            }
        }

        private static async Task SetRecoveryModeAsync(IDbConnection connection, string databaseName, RecoveryMode recoveryMode, TimeSpan timeout)
        {
            string modeMixIn;
            switch (recoveryMode)
            {
                case RecoveryMode.Simple:
                    modeMixIn = "SIMPLE";
                    break;
                case RecoveryMode.Full:
                    modeMixIn = "FULL";
                    break;
                case RecoveryMode.BulkLogged:
                    modeMixIn = "BULK_LOGGED";
                    break;
                default:
                    throw new NotSupportedException($"Unsupported recovery mode to set: {recoveryMode}");
            }

            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(databaseName);

            var commandText = "ALTER DATABASE " + databaseName + " SET RECOVERY " + modeMixIn;

            await connection.ExecuteScalarAsync(commandText, commandTimeout: (int?)timeout.TotalSeconds);
        }

        private static void ThrowIfBad(DatabaseConfiguration configuration)
        {
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(configuration.DatabaseName);
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(configuration.DataFileLogicalName);
            SqlInjectorChecker.ThrowIfNotValidPath(configuration.DataFilePath);
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(configuration.LogFileLogicalName);
            SqlInjectorChecker.ThrowIfNotValidPath(configuration.LogFilePath);            
        }

        private static void ThrowIfBadOnCreateOrModify(DatabaseConfiguration configuration)
        {
            ThrowIfBad(configuration);
            if (configuration.DatabaseType == DatabaseType.System)
            {
                throw new InvalidOperationException("Cannot create nor modify system databases.");
            }
        }

        private static void PutDatabaseInSingleUserMode(IDbConnection connection, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(databaseName);
            var commandText = "ALTER DATABASE " + databaseName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
            connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);
        }

        private static void PutDatabaseIntoMultiUserMode(IDbConnection connection, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(databaseName);
            var commandText = "ALTER DATABASE " + databaseName + " SET MULTI_USER WITH ROLLBACK IMMEDIATE";
            connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);
        }
    }
}
