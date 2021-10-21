// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExistingStreamStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Strategy to use when trying to create a stream that already exists.
    /// </summary>
    public enum ExistingStreamStrategy
    {
        /// <summary>
        /// Skip the creation.
        /// </summary>
        Skip,

        /// <summary>
        /// Re-create the stream.
        /// </summary>
        Overwrite,

        /// <summary>
        /// Throw exception.
        /// </summary>
        Throw,
    }
}
