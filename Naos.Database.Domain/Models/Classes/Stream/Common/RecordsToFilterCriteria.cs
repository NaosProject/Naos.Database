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
        /// <param name="versionMatchStrategy">OPTIONAL strategy on how to treat the version of the types being grouped on (e.g. object type when <paramref name="recordsToFilterSelectionStrategy"/> is <see cref="Domain.RecordsToFilterSelectionStrategy.LatestByIdAndObjectType"/>).  DEFAULT is treat different version of the same type as grouped together.</param>
        public RecordsToFilterCriteria(
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            recordsToFilterSelectionStrategy.MustForArg(nameof(recordsToFilterSelectionStrategy)).NotBeEqualTo(RecordsToFilterSelectionStrategy.Unknown);
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.RecordsToFilterSelectionStrategy = recordsToFilterSelectionStrategy;
            this.VersionMatchStrategy = versionMatchStrategy;
        }

        /// <summary>
        /// Gets the strategy for selecting records that are input into a <see cref="RecordFilter"/>.
        /// </summary>
        public RecordsToFilterSelectionStrategy RecordsToFilterSelectionStrategy { get; private set; }

        /// <summary>
        /// OPTIONAL strategy on how to treat the version of the types being grouped on
        /// (e.g. object type when <see cref="RecordsToFilterSelectionStrategy"/> is <see cref="Domain.RecordsToFilterSelectionStrategy.LatestByIdAndObjectType"/>).
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }
    }
}
