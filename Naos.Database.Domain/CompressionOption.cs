// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressionOption.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Specifies whether backup compression is performed.
    /// </summary>
    public enum CompressionOption
    {
        /// <summary>
        /// Explicitly disable backup compression.
        /// </summary>
        NoCompression,

        /// <summary>
        /// Explicitly enable backup compression.
        /// </summary>
        Compression,
    }
}
