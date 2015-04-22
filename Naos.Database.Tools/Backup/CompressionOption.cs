// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressionOption.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Backup
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
        Compression
    }
}
