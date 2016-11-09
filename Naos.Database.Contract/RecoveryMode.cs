// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecoveryMode.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
{
    /// <summary>
    /// The device to backup to or restore from.
    /// </summary>
    public enum RecoveryMode
    {
        /// <summary>
        /// No recovery mode specified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// No log backups.
        /// Automatically reclaims log space to keep space requirements small, essentially eliminating the need to manage the transaction log space.
        /// Operations that require transaction log backups are not supported by the simple recovery model. The following features cannot be used in simple recovery mode:
        /// -Log shipping
        /// -Always On or Database mirroring
        /// -Media recovery without data loss
        /// -Point-in-time restores.
        /// </summary>
        Simple,

        /// <summary>
        /// Requires log backups.
        /// No work is lost due to a lost or damaged data file.
        /// Can recover to an arbitrary point in time (for example, prior to application or user error).
        /// </summary>
        Full,

        /// <summary>
        /// Requires log backups.
        /// An adjunct of the full recovery model that permits high-performance bulk copy operations.
        /// Reduces log space usage by using minimal logging for most bulk operations.
        /// </summary>
        BulkLogged
    }
}
