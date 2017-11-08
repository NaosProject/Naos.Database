// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoDatabaseManager.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Mongo
{
    using System;
    using System.Threading.Tasks;

    using Its.Log.Instrumentation;

    using Naos.Database.Domain;

    using Spritely.Recipes;

    /// <summary>
    /// Documenter for database objects.
    /// </summary>
    public static class MongoDatabaseManager
    {
        /// <summary>
        /// Perform a database backup.
        /// </summary>
        /// <param name="connectionString">Connection string to the intended database server.</param>
        /// <param name="databaseName">Name of the database to backup.</param>
        /// <param name="backupDetails">The details of how to perform the backup.</param>
        /// <param name="timeout">The command timeout (default is 30 seconds).</param>
        /// <returns>Task to support async await calling.</returns>
        public static async Task BackupFullAsync(string connectionString, string databaseName, BackupDetails backupDetails, TimeSpan timeout = default(TimeSpan))
        {
            new { connectionString }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { databaseName }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { backupDetails }.Must().NotBeNull().OrThrowFirstFailure();

            backupDetails.ThrowIfInvalid();

            using (var activity = Log.Enter(() => new { Database = databaseName, BackupDetails = backupDetails }))
            {
                await Task.Run(() => throw new NotImplementedException());

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
        /// <returns>Task to support async await calling.</returns>
        public static async Task RestoreFullAsync(
            string connectionString,
            string databaseName,
            RestoreDetails restoreDetails,
            TimeSpan timeout = default(TimeSpan))
        {
            new { connectionString }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { databaseName }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { restoreDetails }.Must().NotBeNull().OrThrowFirstFailure();

            restoreDetails.ThrowIfInvalid();

            using (var activity = Log.Enter(() => new { Database = databaseName, RestoreDetails = restoreDetails }))
            {
                await Task.Run(() => throw new NotImplementedException());

                activity.Trace(() => "Completed successfully.");
            }
        }
    }
}