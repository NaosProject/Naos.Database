// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Encryptor.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Domain
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Specifies the encryptor to use when encrypting backups.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Encryptor", Justification = "Spelling/name is correct.")]
    public enum Encryptor
    {
        /// <summary>
        /// No encryptor.
        /// </summary>
        None,

        /// <summary>
        /// Backup using a server certificate.
        /// </summary>
        ServerCertificate,

        /// <summary>
        /// Backup using an asymmetric key.
        /// </summary>
        ServerAsymmetricKey,
    }
}
