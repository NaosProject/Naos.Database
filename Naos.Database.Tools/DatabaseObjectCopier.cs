// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseObjectCopier.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Its.Log.Instrumentation;

    using Spritely.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Logic to copy objects from one database to another.
    /// </summary>
    public static class DatabaseObjectCopier
    {
        /// <summary>
        /// Copies objects from one database to another.
        /// </summary>
        /// <param name="orderedObjectNamesToCopy">Names of objects to copy in order of expected execution.</param>
        /// <param name="sourceDatabaseConnectionString">Connection string to the source database.</param>
        /// <param name="targetDatabaseConnectionString">Connection string to the target database.</param>
        /// <returns>Task for async.</returns>
        public static async Task CopyObjects(IReadOnlyList<string> orderedObjectNamesToCopy, string sourceDatabaseConnectionString, string targetDatabaseConnectionString)
        {
            new { orderedObjectNamesToCopy }.Must().NotBeNull().And().NotBeEmptyEnumerable<string>().OrThrowFirstFailure();
            new { sourceDatabaseConnectionString }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { targetDatabaseConnectionString }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            var scriptedObjects = Scripter.ScriptObjectsFromDatabase(sourceDatabaseConnectionString, orderedObjectNamesToCopy);

            using (var targetConnection = DatabaseManager.OpenConnection(targetDatabaseConnectionString))
            {
                foreach (var scriptedObject in scriptedObjects)
                {
                    Log.Write(() => Invariant($"Applying object script for '{scriptedObject.Name}' of type '{scriptedObject.DatabaseObjectType}'"));
                    await targetConnection.ExecuteAsync(scriptedObject.Script);
                }

                var tables = scriptedObjects.Where(_ => _.DatabaseObjectType == ScriptableObjectType.Table).ToList();
                if (tables.Any())
                {
                    using (var sourceConnection = DatabaseManager.OpenConnection(sourceDatabaseConnectionString))
                    {
                        foreach (var table in tables)
                        {
                            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(table.Name);
                            using (var bcp = new SqlBulkCopy(targetConnection))
                            {
                                var command = sourceConnection.CreateCommand();
                                command.CommandType = CommandType.Text;
                                command.CommandText = "SELECT * FROM " + table.Name;
                                var reader = await command.ExecuteReaderAsync();
                                await bcp.WriteToServerAsync(reader);
                            }
                        }
                    }
                }
            }
        }
    }
}
