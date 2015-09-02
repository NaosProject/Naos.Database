// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Device.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
{
    /// <summary>
    /// The device to backup to or restore from.
    /// </summary>
    public enum Device
    {
        /// <summary>
        /// Backup to or restore from disk.
        /// </summary>
        Disk,

        /// <summary>
        /// Backup to or restore from a blob store.
        /// </summary>
        Url
    }
}
