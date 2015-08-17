// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Backup
{
    using System;

    using Conditions;

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
            Condition.Requires(backupDetails.BackupTo, "backupDetails.BackupTo").IsNotNull();

            if (backupDetails.Device == Device.Url)
            {
                if (string.IsNullOrWhiteSpace(backupDetails.Credential))
                {
                    throw new ArgumentException("Credential cannot be null or whitespace when Device is URL");
                }

                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(backupDetails.Credential);
            }

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

            if (!string.IsNullOrWhiteSpace(backupDetails.Name))
            {
                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(backupDetails.Name);
            }

            if (!string.IsNullOrWhiteSpace(backupDetails.Description))
            {
                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(backupDetails.Description);
            }
        }

        /// <summary>
        /// Throws an exception if the <see cref="RestoreDetails"/> is invalid.
        /// </summary>
        /// <param name="restoreDetails">The restore details to validate.</param>
        public static void ThrowIfInvalid(this RestoreDetails restoreDetails)
        {
            Condition.Requires(restoreDetails.RestoreFrom, "backupDetails.RestoreFrom").IsNotNull();

            if (restoreDetails.Device == Device.Url)
            {
                if (string.IsNullOrWhiteSpace(restoreDetails.Credential))
                {
                    throw new ArgumentException("Credential cannot be null or whitespace when Device is URL");
                }

                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(restoreDetails.Credential);
            }

            if (!string.IsNullOrWhiteSpace(restoreDetails.DataFilePath))
            {
                SqlInjectorChecker.ThrowIfNotValidPath(restoreDetails.DataFilePath);
            }

            if (!string.IsNullOrWhiteSpace(restoreDetails.LogFilePath))
            {
                SqlInjectorChecker.ThrowIfNotValidPath(restoreDetails.LogFilePath);
            }

            if (restoreDetails.ChecksumOption == ChecksumOption.Checksum)
            {
                if (restoreDetails.ErrorHandling == ErrorHandling.None)
                {
                    throw new ArgumentException("ErrorHandling cannot be None when using checksum.");
                }
            }
        }
    }
}
