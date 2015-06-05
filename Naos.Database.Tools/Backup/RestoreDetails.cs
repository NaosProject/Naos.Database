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
        /// Gets or sets an enum value indicating whether to enable checksums.
        /// </summary>
        public ChecksumOption ChecksumOption { get; set; }

        /// <summary>
        /// Gets or sets the name of the credential to use when restoring from a URL.
        /// </summary>
        public string Credential { get; set; }

        /// <summary>
        /// Gets or sets the path to write the data file to.
        /// Must be a file and NOT a directory (i.e. <code>c:\SqlServer\Data\MyDatabase.mdf</code>, not <code>c:\SqlServer\Data\</code>)
        /// </summary>
        /// <remarks>
        /// If null then to use the path where the data file was backed up from.
        /// </remarks>
        public string DataFilePath { get; set; }

        /// <summary>
        /// Gets or sets an enum value indicating the device to restore from.
        /// </summary>
        public Device Device { get; set; }

        /// <summary>
        /// Gets or sets an enum value for the error handling method to use.
        /// </summary>
        public ErrorHandling ErrorHandling { get; set; }

        /// <summary>
        /// Gets or sets the full path to the log file.
        /// Must be a file and NOT a directory (i.e. <code>c:\SqlServer\Data\MyDatabase.mdf</code>, not <code>c:\SqlServer\Data\</code>)
        /// </summary>
        /// <remarks>
        /// If null then to use the path where the log file was backed up from.
        /// </remarks>
        public string LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets an enum value for the recovery option.
        /// </summary>
        public RecoveryOption RecoveryOption { get; set; }

        /// <summary>
        /// Gets or sets an enum value with instructions on what to do when restoring to a database that already exists.
        /// </summary>
        public ReplaceOption ReplaceOption { get; set; }

        /// <summary>
        /// Gets or sets the location at which to pull the backup for restoration (i.e. file path or URL)
        /// For local/network storage, must be a file and NOT a directory (i.e. <code>c:\MyBackups\TodayBackup.bak</code>, not <code>c:\MyBackups</code>)
        /// </summary>
        public Uri RestoreFrom { get; set; }

        /// <summary>
        /// Gets or sets an enum value that indicates whether or not to put the database into restricted user mode after restoring.
        /// </summary>
        public RestrictedUserOption RestrictedUserOption { get; set; }
    }
}
