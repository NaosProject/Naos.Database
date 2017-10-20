// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringHelper.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    using System.Data.SqlClient;

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
            var builder = new SqlConnectionStringBuilder(connectionString)
                              {
                                  InitialCatalog = databaseName,
                              };

            return builder.ConnectionString;
        }
    }
}
