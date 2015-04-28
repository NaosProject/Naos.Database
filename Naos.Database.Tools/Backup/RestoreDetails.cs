// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreDetails.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Backup
{
    using System;

    /// <summary>
    /// Captures the details of a restore operation.
    /// </summary>
    public class RestoreDetails
    {
        /// <summary>
        /// Gets or sets a value indicating whether to enable checksums.
        /// </summary>
        public ChecksumOption ChecksumOption { get; set; }

        /// <summary>
        /// Gets or sets the name of the credential to use when restoring from a URL.
        /// </summary>
        public string Credential { get; set; }

        /// <summary>
        /// Gets or sets the path to write the data file to.
        /// </summary>
        /// <remarks>
        /// If null then to use the path where the data file was backed up from.
        /// </remarks>
        public string DataFilePath { get; set; }

        /// <summary>
        /// Gets or sets the device to restore from.
        /// </summary>
        public Device Device { get; set; }

        /// <summary>
        /// Gets or sets the error handling method to use.
        /// </summary>
        public ErrorHandling ErrorHandling { get; set; }

        /// <summary>
        /// Gets or sets the full path to the log file.
        /// </summary>
        /// <remarks>
        /// If null then to use the path where the log file was backed up from.
        /// </remarks>
        public string LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets the recovery option.
        /// </summary>
        public RecoveryOption RecoveryOption { get; set; }

        /// <summary>
        /// Gets or sets instructions on what to do when restoring to a database that already exists.
        /// </summary>
        public ReplaceOption ReplaceOption { get; set; }

        /// <summary>
        /// Gets or sets the location at which to pull the backup for restoration (i.e. file path or URL)
        /// </summary>
        public Uri RestoreFrom { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether or not to put the database into restricted user mode after restoring.
        /// </summary>
        public RestrictedUserOption RestrictedUserOption { get; set; }
    }
}
