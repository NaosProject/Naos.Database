// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseObjectCopier.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Its.Log.Instrumentation;

    using Microsoft.SqlServer.Management.Smo;

    using Naos.Database.Domain;

    using OBeautifulCode.Validation.Recipes;

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
            new { orderedObjectNamesToCopy }.Must().NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            new { sourceDatabaseConnectionString }.Must().NotBeNullNorWhiteSpace();
            new { targetDatabaseConnectionString }.Must().NotBeNullNorWhiteSpace();

            var scriptedObjects = Scripter.ScriptObjectsFromDatabase(sourceDatabaseConnectionString, orderedObjectNamesToCopy);

            using (var targetConnection = SqlServerDatabaseManager.OpenConnection(targetDatabaseConnectionString))
            {
                async Task RunScriptOnServer(ScriptedObject scriptedObject, string scriptToRun)
                {
                    Log.Write(() => Invariant($"Applying create script for '{scriptedObject.Name}' of type '{scriptedObject.DatabaseObjectType}'"));
                    try
                    {
                        async Task ServerAction(Server server)
                        {
                            // because it might contain "GO" statements most likely this needs to be executed via the SMO connection.
                            server.ConnectionContext.ExecuteNonQuery(scriptToRun);
                            await Task.Run(() => { });
                        }

                        await SqlServerDatabaseManager.RunOperationOnSmoServerAsync(ServerAction, targetConnection);
                    }
                    catch (Exception ex)
                    {
                        throw new FailedOperationException(
                            Invariant($"Failed to run script on database {ConnectionStringHelper.GetDatabaseNameFromConnectionString(targetDatabaseConnectionString)}; {scriptToRun}"),
                            ex);
                    }
                }

                foreach (var scriptedObject in scriptedObjects.Reverse())
                {
                    await RunScriptOnServer(scriptedObject, scriptedObject.DropScript);
                }

                foreach (var scriptedObject in scriptedObjects)
                {
                    await RunScriptOnServer(scriptedObject, scriptedObject.CreateScript);
                }

                var tables = scriptedObjects.Where(_ => _.DatabaseObjectType == ScriptableObjectType.Table).ToList();
                if (tables.Any())
                {
                    var copyOptions = SqlBulkCopyOptions.CheckConstraints
                                    | SqlBulkCopyOptions.FireTriggers
                                    | SqlBulkCopyOptions.KeepIdentity
                                    | SqlBulkCopyOptions.KeepNulls
                                    | SqlBulkCopyOptions.TableLock;
                    using (var sourceConnection = SqlServerDatabaseManager.OpenConnection(sourceDatabaseConnectionString))
                    {
                        foreach (var table in tables)
                        {
                            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(table.Name);
                            using (var transaction = targetConnection.BeginTransaction("BcpTable-" + table.Name))
                            {
                                using (var bcp = new SqlBulkCopy(targetConnection, copyOptions, transaction) { DestinationTableName = table.Name })
                                {
                                    using (var command = sourceConnection.CreateCommand())
                                    {
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = "SELECT * FROM " + table.Name;
                                        using (var reader = await command.ExecuteReaderAsync())
                                        {
                                            await bcp.WriteToServerAsync(reader);
                                        }
                                    }
                                }

                                transaction.Commit();
                            }
                        }
                    }
                }
            }
        }
    }
}
