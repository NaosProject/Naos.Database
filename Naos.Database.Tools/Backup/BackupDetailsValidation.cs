// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupDetailsValidation.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Backup
{
    using System;

    using CuttingEdge.Conditions;

    /// <summary>
    /// Validates a <see cref="BackupDetails"/>
    /// </summary>
    public static class BackupDetailsValidation
    {
        /// <summary>
        /// Throws an exception if the <see cref="BackupDetails"/> is invalid.
        /// </summary>
        /// <param name="backupDetails">The backup details to validate.</param>
        public static void ThrowIfInvalid(this BackupDetails backupDetails)
        {
            Condition.Requires(backupDetails.BackupTo, "backupDetails.BackupTo").IsNotNull();

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

            if (backupDetails.Cipher != Cipher.NoEncryption)
            {
                if (backupDetails.Encryptor == Encryptor.None)
                {
                    throw new ArgumentException("Encryptor is required when any Cipher != NoEncryption");
                }

                if (string.IsNullOrWhiteSpace(backupDetails.EncryptorName))
                {
                    throw new ArgumentException("EncryptorName is required when any Cipher != NoEncryption.");
                }

                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(backupDetails.EncryptorName);
            }

            if (backupDetails.ChecksumOption == ChecksumOption.Checksum)
            {
                if (backupDetails.ErrorHandling == ErrorHandling.None)
                {
                    throw new ArgumentException("ErrorHandling cannot be None when using checksum.");
                }
            }
        }
    }
}
