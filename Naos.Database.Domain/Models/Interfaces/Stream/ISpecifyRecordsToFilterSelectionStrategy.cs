// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpecifyRecordsToFilterSelectionStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to expose the <see cref="RecordsToFilterSelectionStrategy"/> to use.
    /// </summary>
    public interface ISpecifyRecordsToFilterSelectionStrategy
    {
        /// <summary>
        /// Gets the strategy for selecting records before applying the <see cref="RecordFilter"/>.
        /// </summary>
        RecordsToFilterSelectionStrategy RecordsToFilterSelectionStrategy { get; }
    }
}
