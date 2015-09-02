// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Encryptor.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
{
    /// <summary>
    /// Specifies the encryptor to use when encrypting backups.
    /// </summary>
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
        ServerAsymmetricKey
    }
}
