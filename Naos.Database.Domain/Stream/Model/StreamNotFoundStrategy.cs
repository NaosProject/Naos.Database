// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamNotFoundStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Strategy to use when trying to get or delete a stream that does not exists.
    /// </summary>
    public enum StreamNotFoundStrategy
    {
        /// <summary>
        /// Skip the creation.
        /// </summary>
        Skip,

        /// <summary>
        /// Throw exception.
        /// </summary>
        Throw,
    }
}
