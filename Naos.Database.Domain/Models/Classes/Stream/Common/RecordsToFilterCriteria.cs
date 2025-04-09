// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordsToFilterCriteria.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Specifies how to determine the records that are input into a <see cref="RecordFilter"/>.
    /// </summary>
    public partial class RecordsToFilterCriteria : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsToFilterCriteria"/> class.
        /// </summary>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to select all records (none are excluded from the set of records that are input into a <see cref="RecordFilter"/>).</param>
        public RecordsToFilterCriteria(
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            recordsToFilterSelectionStrategy.MustForArg(nameof(recordsToFilterSelectionStrategy)).NotBeEqualTo(RecordsToFilterSelectionStrategy.Unknown);

            this.RecordsToFilterSelectionStrategy = recordsToFilterSelectionStrategy;
        }

        /// <summary>
        /// Gets the strategy for selecting records that are input into a <see cref="RecordFilter"/>.
        /// </summary>
        public RecordsToFilterSelectionStrategy RecordsToFilterSelectionStrategy { get; private set; }
    }
}
