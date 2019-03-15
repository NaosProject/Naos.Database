// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Device.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
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
        Url,
    }
}
