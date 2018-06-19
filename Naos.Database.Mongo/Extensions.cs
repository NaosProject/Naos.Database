// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Mongo
{
    using System;

    using Naos.Database.Domain;

    using OBeautifulCode.Validation.Recipes;

    /// <summary>
    /// Extension methods for types in the namespace.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Throws an exception if the <see cref="BackupDetails"/> is invalid.
        /// </summary>
        /// <param name="backupDetails">The backup details to validate.</param>
        public static void ThrowIfInvalid(this BackupDetails backupDetails)
        {
            new { backupDetails }.Must().NotBeNull();
            new { backupDetails.BackupTo }.Must().NotBeNull();

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

            if (backupDetails.ChecksumOption != ChecksumOption.NoChecksum)
            {
                throw new ArgumentException("ChecksumOption must be NoChecksum for Mongo");
            }

            if (backupDetails.Cipher != Cipher.NoEncryption)
            {
                throw new ArgumentException("Cipher must be NoEncryption for Mongo");
            }

            if (backupDetails.CompressionOption != CompressionOption.Compression)
            {
                throw new ArgumentException("CompressionOption must be Compression for Mongo");
            }

            if (backupDetails.Device != Device.Disk)
            {
                throw new ArgumentException("Device must be Disk for Mongo.");
            }

            if (backupDetails.Encryptor != Encryptor.None)
            {
                throw new ArgumentException("Encryptor must be None for Mongo");
            }

            if (backupDetails.EncryptorName != null)
            {
                throw new ArgumentException("EncryptorName must be null for Mongo");
            }

            if (backupDetails.ErrorHandling != ErrorHandling.None)
            {
                throw new ArgumentException("ErrorHandling must be None for Mongo");
            }
        }

        /// <summary>
        /// Throws an exception if the <see cref="RestoreDetails"/> is invalid.
        /// </summary>
        /// <param name="restoreDetails">The restore details to validate.</param>
        public static void ThrowIfInvalid(this RestoreDetails restoreDetails)
        {
            new { restoreDetails }.Must().NotBeNull();
            new { restoreDetails.RestoreFrom }.Must().NotBeNull();

            if (restoreDetails.ChecksumOption != ChecksumOption.NoChecksum)
            {
                throw new ArgumentException("ChecksumOption must be NoChecksum for Mongo");
            }

            if (restoreDetails.Device != Device.Disk)
            {
                throw new ArgumentException("Device must be Disk for Mongo.");
            }

            if (restoreDetails.ErrorHandling != ErrorHandling.None)
            {
                throw new ArgumentException("ErrorHandling must be None for Mongo");
            }

            if (restoreDetails.RecoveryOption != RecoveryOption.NoRecovery)
            {
                throw new ArgumentException("RecoveryOption must be NoRecovery for Mongo");
            }

            if (restoreDetails.RestrictedUserOption != RestrictedUserOption.Normal)
            {
                throw new ArgumentException("RestrictedUserOption must be Normal for Mongo");
            }
        }
    }
}
