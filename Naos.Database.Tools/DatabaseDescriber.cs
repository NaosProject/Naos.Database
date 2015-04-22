// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseDescriber.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    using Dapper;

    /// <summary>
    /// Utility to retrieve information about a database and its contents.
    /// </summary>
    public static class DatabaseDescriber
    {
        /// <summary>
        /// Get detailed information about a table's makeup.
        /// </summary>
        /// <param name="connectionString">The connection string to the intended server.</param>
        /// <param name="databaseName">The name of the target database.</param>
        /// <param name="tableName">The name of the target table to describe.</param>
        /// <param name="tableSchema">The schema of the table to query details for.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>Detailed information about a table's makeup.</returns>
        public static TableDescription GetTableDescription(string connectionString, string databaseName, string tableName, string tableSchema = "dbo", TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(databaseName);
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(tableName);
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(tableSchema);

            var sqlParams = new
                                {
                                    DatabaseName = databaseName,
                                    Schema = tableSchema,
                                    TableName = tableName,
                                };

            var commandText =
                @"SELECT
                        COLUMN_NAME as ColumnName,
                        ORDINAL_POSITION as OrdinalPosition,
                        COLUMN_DEFAULT as ColumnDefault,
                        (CASE UPPER(IS_NULLABLE)
						WHEN 'YES' THEN 1 ELSE 0 END) as IsNullable,
                        DATA_TYPE as DataType
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE 
                    TABLE_CATALOG   = @DatabaseName
                    AND TABLE_SCHEMA= @Schema
                    AND TABLE_NAME  = @TableName
                ORDER BY ORDINAL_POSITION";

            ColumnDescription[] columns;
            var targetedDatabaseConnectionString =
                ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(connectionString, databaseName);
            using (var connection = new SqlConnection(targetedDatabaseConnectionString))
            {
                connection.Open();
                columns = connection.Query<ColumnDescription>(commandText, sqlParams, null, true, (int?)timeout.TotalSeconds).ToArray();
                connection.Close();
            }

            var ret = new TableDescription()
                          {
                              DatabaseName = databaseName,
                              TableName = tableName,
                              TableSchema = tableSchema,
                              Columns = columns,
                          };
            return ret;
        }
    }
}
