// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressionOption.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
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
