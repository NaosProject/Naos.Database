// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseManager.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Dapper;

    /// <summary>
    /// Class with tools for adding, removing, and updating databases.
    /// </summary>
    public static class DatabaseManager
    {
        #region Public Methods

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

            SqlInjectorChecker.ThrowIfNotAlphanumeric(databaseName);
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

            SqlInjectorChecker.ThrowIfNotAlphanumeric(databaseName);
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

            ThrowIfBad(configuration);
            var commandText = string.Format(
                @"CREATE DATABASE {0}
                        ON
                        ( NAME = '{1}',
                        FILENAME = '{2}',
                        SIZE = {3}KB,
                        MAXSIZE = {4},
                        FILEGROWTH = {5}KB )
                        LOG ON
                        ( NAME = '{6}',
                        FILENAME = '{7}',
                        SIZE = {8}KB,
                        MAXSIZE = {9},
                        FILEGROWTH = {10}KB )",
                        configuration.DatabaseName,
                        configuration.DataFileLogicalName,
                        configuration.DataFilePath,
                        configuration.DataFileCurrentSizeInKb,
                        configuration.DataFileMaxSizeInKb == Constants.InfinityMaxSize ? "UNLIMITED" : configuration.DataFileMaxSizeInKb + "KB",
                        configuration.DataFileGrowthSizeInKb,
                        configuration.LogFileLogicalName,
                        configuration.LogFilePath,
                        configuration.LogFileCurrentSizeInKb,
                        configuration.LogFileMaxSizeInKb == Constants.InfinityMaxSize ? "UNLIMITED" : configuration.LogFileMaxSizeInKb + "KB",
                        configuration.LogFileGrowthSizeInKb); 
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);
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

            var query = @"
                SELECT
                    d.name as DatabaseName,
                    df.name as DataFileLogicalName,
                    df.physical_name as DataFilePath,
                    lf.name as LogFileLogicalName,
                    lf.physical_name as LogFilePath
                FROM sys.databases d 
                INNER JOIN sys.master_files df
                    ON d.database_id = df.database_id AND df.type = 0
                INNER JOIN sys.master_files lf
                    ON d.database_id = lf.database_id AND lf.type = 1";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var ret = connection.Query<DatabaseConfiguration>(query, null, null, true, (int?)timeout.TotalSeconds).ToArray();
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

            ThrowIfBad(currentConfiguration);
            ThrowIfBad(newConfiguration);

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
                    var updateDataFileText = string.Format(
                        @"ALTER DATABASE {0} MODIFY FILE (
                        NAME = '{1}',
                        NEWNAME = '{2}',
                        FILENAME = '{3}')",
                        newConfiguration.DatabaseName,
                        currentConfiguration.DataFileLogicalName,
                        newConfiguration.DataFileLogicalName,
                        newConfiguration.DataFilePath);
                    connection.Execute(updateDataFileText, null, null, (int?)timeout.TotalSeconds);
                }

                if ((currentConfiguration.LogFileLogicalName != newConfiguration.LogFileLogicalName)
                    && (currentConfiguration.LogFilePath != newConfiguration.LogFilePath))
                {
                    var updateLogFileText = string.Format(
                        @"ALTER DATABASE {0} MODIFY FILE (
                        NAME = '{1}',
                        NEWNAME = '{2}',
                        FILENAME = '{3}')",
                        newConfiguration.DatabaseName,
                        currentConfiguration.LogFileLogicalName,
                        newConfiguration.LogFileLogicalName,
                        newConfiguration.LogFilePath);
                    connection.Execute(updateLogFileText, null, null, (int?)timeout.TotalSeconds);
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

            SqlInjectorChecker.ThrowIfNotAlphanumeric(databaseName);
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

        #endregion

        #region Private Methods

        private static void ThrowIfBad(DatabaseConfiguration configuration)
        {
            SqlInjectorChecker.ThrowIfNotAlphanumeric(configuration.DatabaseName);
            SqlInjectorChecker.ThrowIfNotAlphanumeric(configuration.DataFileLogicalName);
            SqlInjectorChecker.ThrowIfNotValidPath(configuration.DataFilePath);
            SqlInjectorChecker.ThrowIfNotAlphanumeric(configuration.LogFileLogicalName);
            SqlInjectorChecker.ThrowIfNotValidPath(configuration.LogFilePath);
        }

        private static void PutDatabaseInSingleUserMode(IDbConnection connection, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            SqlInjectorChecker.ThrowIfNotAlphanumeric(databaseName);
            var commandText = "ALTER DATABASE " + databaseName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
            connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);
        }

        private static void PutDatabaseIntoMultiUserMode(IDbConnection connection, string databaseName, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            SqlInjectorChecker.ThrowIfNotAlphanumeric(databaseName);
            var commandText = "ALTER DATABASE " + databaseName + " SET MULTI_USER WITH ROLLBACK IMMEDIATE";
            connection.Execute(commandText, null, null, (int?)timeout.TotalSeconds);
        }
        #endregion
    }
}
