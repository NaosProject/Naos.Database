// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoBackupAndRestoreDetailsValidationExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Mongo
{
    using System;

    using Naos.Database.Domain;
    using Naos.Database.Mongo.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extension methods for types in the namespace.
    /// </summary>
    public static class MongoBackupAndRestoreDetailsValidationExtensions
    {
        /// <summary>
        /// Throws an exception if the <see cref="BackupMongoDatabaseDetails"/> is invalid.
        /// </summary>
        /// <param name="backupDetails">The backup details to validate.</param>
        public static void ThrowIfInvalid(this BackupMongoDatabaseDetails backupDetails)
        {
            new { backupDetails }.AsArg().Must().NotBeNull();
            new { backupDetails.BackupTo }.AsArg().Must().NotBeNull();

            if (!string.IsNullOrWhiteSpace(backupDetails.Name))
            {
                if (backupDetails.Name.Length > 128)
                {
                    throw new ArgumentException("Name cannot be more than 128 characters in length.");
                }
            }

            if (!string.IsNullOrWhiteSpace(backupDetails.Description))
            {
                if (backupDetails.Description.Length > 255)
                {
                    throw new ArgumentException("Description cannot be more than 255 characters in length.");
                }
            }
        }

        /// <summary>
        /// Throws an exception if the <see cref="RestoreMongoDatabaseDetails"/> is invalid.
        /// </summary>
        /// <param name="restoreDetails">The restore details to validate.</param>
        public static void ThrowIfInvalid(this RestoreMongoDatabaseDetails restoreDetails)
        {
            new { restoreDetails }.AsArg().Must().NotBeNull();
            new { restoreDetails.RestoreFrom }.AsArg().Must().NotBeNull();
        }
    }
}
