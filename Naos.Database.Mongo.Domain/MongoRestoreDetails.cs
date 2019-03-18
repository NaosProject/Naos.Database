// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoRestoreDetails.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    /// <summary>
    /// Captures the details of a restore operation.
    /// </summary>
    public class MongoRestoreDetails
    {
        /// <summary>
        /// Gets or sets the location at which to pull the backup for restoration (i.e. file path or URL)
        /// For local/network storage, must be a file and NOT a directory (i.e. c:\MyBackups\TodayBackup.bak, not c:\MyBackups).
        /// </summary>
        public Uri RestoreFrom { get; set; }

        /// <summary>
        /// Gets or sets an enum value with instructions on what to do when restoring to a database that already exists.
        /// </summary>
        public ReplaceOption ReplaceOption { get; set; }
    }
}
