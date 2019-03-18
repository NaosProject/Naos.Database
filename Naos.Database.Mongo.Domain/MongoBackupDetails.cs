// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoBackupDetails.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Mongo.Domain
{
    using System;

    /// <summary>
    /// Captures the details of a backup operation.
    /// </summary>
    public class MongoBackupDetails
    {
        /// <summary>
        /// Gets or sets the name of the backup (not the name of the backup file,
        /// but rather the name of the backup set identifying the backup within the file).
        /// If not specified, it is blank.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description of the backup.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the location at which to save the backup (i.e. file path or URL).
        /// For local/network storage, must be a file and NOT a directory (i.e. c:\MyBackups\TodayBackup.bak, not c:\MyBackups).
        /// </summary>
        public Uri BackupTo { get; set; }

        /// <summary>
        /// Gets or sets the name of the credential to use when backing up to a URL.
        /// </summary>
        public string Credential { get; set; }
    }
}
