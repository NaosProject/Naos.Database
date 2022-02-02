// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExistingDatabaseStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Strategy to use when trying to create a database that already exists.
    /// </summary>
    public enum ExistingDatabaseStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Skip the creation.
        /// </summary>
        Skip,

        /// <summary>
        /// Re-create the database.
        /// </summary>
        Overwrite,

        /// <summary>
        /// Throw exception.
        /// </summary>
        Throw,
    }
}
