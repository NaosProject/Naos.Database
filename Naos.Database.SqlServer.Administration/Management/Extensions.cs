// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Administration
{
    using System;

    using Naos.Database.Domain;
    using Naos.Database.SqlServer.Domain;
    using OBeautifulCode.Assertion.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Extension methods for types in the namespace.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Builds a localhost connection string from the configuration.
        /// </summary>
        /// <param name="connectionDefinition">Connection information.</param>
        /// <returns>Localhost connection string.</returns>
        public static string ToSqlServerConnectionString(this SqlServerConnectionDefinition connectionDefinition)
        {
            new { connectionDefinition }.AsArg().Must().NotBeNull();

            var instanceName = string.IsNullOrWhiteSpace(connectionDefinition.InstanceName) ? string.Empty : Invariant($"\\{connectionDefinition.InstanceName}");
            var ret = Invariant($"Server={connectionDefinition.Server}{instanceName}; user id={connectionDefinition.UserName}; password={connectionDefinition.Password}");
            return ret;
        }

        /// <summary>
        /// Throws an exception if the <see cref="BackupSqlServerDatabaseDetails"/> is invalid.
        /// </summary>
        /// <param name="backupDetails">The backup details to validate.</param>
        public static void ThrowIfInvalid(this BackupSqlServerDatabaseDetails backupDetails)
        {
            new { backupDetails }.AsArg().Must().NotBeNull();
            new { backupDetails.BackupTo }.AsArg().Must().NotBeNull();

            if (backupDetails.Device == Device.Url)
            {
                if (string.IsNullOrWhiteSpace(backupDetails.Credential))
                {
                    throw new ArgumentException("Credential cannot be null or whitespace when Device is URL");
                }

                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(backupDetails.Credential);
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

                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(backupDetails.EncryptorName);
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
                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(backupDetails.Name);
            }

            if (!string.IsNullOrWhiteSpace(backupDetails.Description))
            {
                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(backupDetails.Description);
            }
        }

        /// <summary>
        /// Throws an exception if the <see cref="RestoreSqlServerDatabaseDetails"/> is invalid.
        /// </summary>
        /// <param name="restoreDetails">The restore details to validate.</param>
        public static void ThrowIfInvalid(this RestoreSqlServerDatabaseDetails restoreDetails)
        {
            new { restoreDetails }.AsArg().Must().NotBeNull();
            new { restoreDetails.RestoreFrom }.AsArg().Must().NotBeNull();

            if (restoreDetails.Device == Device.Url)
            {
                if (string.IsNullOrWhiteSpace(restoreDetails.Credential))
                {
                    throw new ArgumentException("Credential cannot be null or whitespace when Device is URL");
                }

                SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(restoreDetails.Credential);
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
