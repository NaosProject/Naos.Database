// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamAccessMode.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    /// <summary>
    /// Access modes used for streams.
    /// </summary>
    [Flags]
    public enum StreamAccessMode
    {
        /// <summary>
        /// There is no mode specified ("null object" option).
        /// </summary>
        None = 0,

        /// <summary>
        /// The read option.
        /// </summary>
        Read = 1,

        /// <summary>
        /// The write option.
        /// </summary>
        Write = 2,

        /// <summary>
        /// The handling option.
        /// </summary>
        Handling = 4,

        /// <summary>
        /// The management option.
        /// </summary>
        Management = 8,

        /// <summary>
        /// The read and write option.
        /// </summary>
        ReadWrite = Read | Write,

        /// <summary>
        /// All options.
        /// </summary>
        All = Read | Write | Handling | Management,
    }
}
