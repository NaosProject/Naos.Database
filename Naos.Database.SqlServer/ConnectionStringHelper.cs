// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringHelper.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer
{
    using System.Data.SqlClient;

    using Spritely.Recipes;

    /// <summary>
    /// Utility class to assist in connection string manipulation operations.
    /// </summary>
    public static class ConnectionStringHelper
    {
        /// <summary>
        /// Will add replace the initial catalog in the connection string as the provided database name.
        /// </summary>
        /// <param name="connectionString">The connection string to the intended server.</param>
        /// <param name="databaseName">The name of the target database.</param>
        /// <returns>An adjusted connection string, specifying the provided database as the initial catalog.</returns>
        public static string SpecifyInitialCatalogInConnectionString(string connectionString, string databaseName)
        {
            new { connectionString }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { databaseName }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            var builder = new SqlConnectionStringBuilder(connectionString)
                              {
                                  InitialCatalog = databaseName,
                              };

            return builder.ConnectionString;
        }

        /// <summary>
        /// Builds a connection string from provided details, simple wrapper around <see cref="SqlConnectionStringBuilder" />.
        /// </summary>
        /// <param name="serverName">Name, DNS, or IP of the server.</param>
        /// <param name="database">Database name; optional DEFAULT is master.</param>
        /// <param name="userName">Username to authenticate with; optional DEFAULT is null and will use trusted authentication.</param>
        /// <param name="password">Password to authenticate with; optional DEFAULT is null and will use trusted authentication.</param>
        /// <returns>Built connection string.</returns>
        public static string BuildConnectionString(string serverName, string database = null, string userName = null, string password = null)
        {
            new { serverName }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            var builder = new SqlConnectionStringBuilder
                              {
                                  DataSource = serverName,
                                  InitialCatalog = string.IsNullOrWhiteSpace(database) ? SqlServerDatabaseManager.MasterDatabaseName : database,
                                  IntegratedSecurity = string.IsNullOrWhiteSpace(userName),
                              };

            if (!string.IsNullOrWhiteSpace(userName))
            {
                builder.UserID = userName;
                builder.Password = password;
            }

            return builder.ConnectionString;
        }

        /// <summary>
        /// Gets the database name (<see cref="SqlConnectionStringBuilder.InitialCatalog" /> from the connection string.
        /// </summary>
        /// <param name="connectionString">Connection string to inspect.</param>
        /// <returns>Datbase name.</returns>
        public static string GetDatabaseNameFromConnectionString(string connectionString)
        {
            new { connectionString }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }
    }
}
