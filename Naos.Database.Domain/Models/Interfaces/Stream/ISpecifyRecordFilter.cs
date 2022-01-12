// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpecifyRecordFilter.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface to expose the <see cref="RecordFilter"/> to use.
    /// </summary>
    public interface ISpecifyRecordFilter
    {
        /// <summary>
        /// Gets the filter to apply to the set of records.
        /// </summary>
        RecordFilter RecordFilter { get; }
    }
}
