// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExistingRecordNotEncounteredStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Strategy on how to handle an existing record not encountered during a read.
    /// </summary>
    public enum ExistingRecordNotEncounteredStrategy
    {
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
