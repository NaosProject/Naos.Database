// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordNotFoundStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Determines what to do when no records were found.
    /// </summary>
    public enum RecordNotFoundStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Returns default of queried type.
        /// </summary>
        ReturnDefault,

        /// <summary>
        /// Throw exception.
        /// </summary>
        Throw,
    }
}
