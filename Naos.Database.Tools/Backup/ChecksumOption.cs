﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChecksumOption.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Backup
{
    /// <summary>
    /// Determines whether backup checksums are enabled.
    /// </summary>
    public enum ChecksumOption
    {
        /// <summary>
        /// Explicitly disables the generation of backup checksums (and the validation of page checksums).
        /// </summary>
        NoChecksum,

        /// <summary>
        /// Specifies that the backup operation will verify each page for checksum and torn pages, if enabled and available, and generate a checksum for the entire backup.
        /// Using backup checksums may affect workload and backup throughput.
        /// </summary>
        Checksum
    }
}
