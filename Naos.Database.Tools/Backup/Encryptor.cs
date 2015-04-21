// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Encryptor.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Backup
{
    /// <summary>
    /// Specifies the encryptor to use when encrypting backups.
    /// </summary>
    public enum Encryptor
    {
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
