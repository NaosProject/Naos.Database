// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExistingRecordEncounteredStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Strategy on how to handle an existing record encountered during a write.
    /// </summary>
    public enum ExistingRecordEncounteredStrategy
    {
        /// <summary>
        /// Do not apply any strategy, will put a new record regardless of any existing records.
        /// </summary>
        None,

        /// <summary>
        /// Do not write if found by identifier
        /// </summary>
        DoNotWriteIfFoundById,

        /// <summary>
        /// Do not write if found by identifier and type
        /// </summary>
        DoNotWriteIfFoundByIdAndType,

        /// <summary>
        /// Do not write if found by identifier and type and content
        /// </summary>
        DoNotWriteIfFoundByIdAndTypeAndContent,

        /// <summary>
        /// Throw if found by identifier
        /// </summary>
        ThrowIfFoundById,

        /// <summary>
        /// Throw if found by identifier and type
        /// </summary>
        ThrowIfFoundByIdAndType,

        /// <summary>
        /// Throw if found by identifier and type and content
        /// </summary>
        ThrowIfFoundByIdAndTypeAndContent,

        /// <summary>
        /// Prune if found by identifier
        /// </summary>
        PruneIfFoundById,

        /// <summary>
        /// Prune if found by identifier and type
        /// </summary>
        PruneIfFoundByIdAndType,
    }
}
