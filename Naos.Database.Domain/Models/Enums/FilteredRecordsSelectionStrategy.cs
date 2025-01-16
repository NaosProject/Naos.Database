// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilteredRecordsSelectionStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Strategy for selecting records after applying a <see cref="RecordFilter"/>.
    /// </summary>
    public enum FilteredRecordsSelectionStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Select all records.
        /// </summary>
        All,

        /// <summary>
        /// Groups by identifier and selects the latest record within each group.
        /// </summary>
        LatestById,
    }
}