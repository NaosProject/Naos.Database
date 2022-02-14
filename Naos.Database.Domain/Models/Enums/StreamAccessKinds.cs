// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamAccessKinds.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    /// <summary>
    /// Specifies the kind of access that is provided to a stream.
    /// </summary>
    [Flags]
    public enum StreamAccessKinds
    {
        /// <summary>
        /// None (default).
        /// </summary>
        None = 0,

        /// <summary>
        /// Read access.
        /// </summary>
        Read = 1,

        /// <summary>
        /// Write access.
        /// </summary>
        Write = 2,

        /// <summary>
        /// Handle access.
        /// </summary>
        Handle = 4,

        /// <summary>
        /// Management access.
        /// </summary>
        Manage = 8,
    }
}
