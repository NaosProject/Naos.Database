// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingStatus.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Strategy on how to handle an existing record encountered during a write.
    /// </summary>
    public enum HandlingStatus
    {
        /// <summary>
        /// There is an unknown status.
        /// </summary>
        Unknown,

        /// <summary>
        /// There is no status recorded.
        /// </summary>
        None,

        /// <summary>
        /// The operation has been requested.
        /// </summary>
        Requested,

        /// <summary>
        /// The operation is executing.
        /// </summary>
        Running,

        /// <summary>
        /// The operation executed without error.
        /// </summary>
        Completed,

        /// <summary>
        /// The operation executed with an error.
        /// </summary>
        Failed,

        /// <summary>
        /// The operation was canceled externally.
        /// </summary>
        Canceled,

        /// <summary>
        /// The operation's protocol canceled it's own execution.
        /// </summary>
        SelfCanceled,
    }
}
