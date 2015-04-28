// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupDetails.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Backup
{
    using System;

    /// <summary>
    /// Captures the details of a backup operation.
    /// </summary>
    public class BackupDetails
    {
        /// <summary>
        /// Gets or sets the location at which to save the backup (i.e. file path or URL)
        /// </summary>
        public Uri BackupTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable checksums.
        /// </summary>
        public ChecksumOption ChecksumOption { get; set; }

        /// <summary>
        /// Gets or sets the cipher to use when encrypting the backup.
        /// </summary>
        public Cipher Cipher { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to compress the backup.
        /// </summary>
        public CompressionOption CompressionOption { get; set; }

        /// <summary>
        /// Gets or sets the name of the credential to use when backing up to a URL.
        /// </summary>
        public string Credential { get; set; }

        /// <summary>
        /// Gets or sets a description of the backup.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the device to backup to.
        /// </summary>
        public Device Device { get; set; }

        /// <summary>
        /// Gets or sets the encryptor to use when encrypting the backup.
        /// </summary>
        public Encryptor Encryptor { get; set; }

        /// <summary>
        /// Gets or sets the name of the encryptor (i.e. server certificate name or asymmetric key name)
        /// </summary>
        public string EncryptorName { get; set; }

        /// <summary>
        /// Gets or sets the error handling method to use.
        /// </summary>
        public ErrorHandling ErrorHandling { get; set; }

        /// <summary>
        /// Gets or sets the name of the backup.
        /// </summary>
        public string Name { get; set; }
    }
}
