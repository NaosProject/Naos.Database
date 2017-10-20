// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Encryptor.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
{
    /// <summary>
    /// Specifies the encryptor to use when encrypting backups.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Encryptor", Justification = "Spelling/name is correct.")]
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
