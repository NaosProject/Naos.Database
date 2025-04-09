// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpecifyRecordsToFilterCriteria.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to expose the <see cref="RecordsToFilterCriteria"/> to use.
    /// </summary>
    public interface ISpecifyRecordsToFilterCriteria
    {
        /// <summary>
        /// Gets an object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.
        /// </summary>
        RecordsToFilterCriteria RecordsToFilterCriteria { get; }
    }
}
