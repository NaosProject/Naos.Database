﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetDistinctStringSerializedIdsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the latest record.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetDistinctStringSerializedIdsOp : ReturningOperationBase<IReadOnlyCollection<StringSerializedIdentifier>>, ISpecifyRecordFilter, ISpecifyRecordsToFilterSelectionStrategy, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetDistinctStringSerializedIdsOp"/> class.
        /// </summary>
        /// <param name="recordFilter">The filter to apply to the set of records to consider.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before applying the <paramref name="recordFilter"/>.  DEFAULT is to select all records.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetDistinctStringSerializedIdsOp(
            RecordFilter recordFilter,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All,
            IResourceLocator specifiedResourceLocator = null)
        {
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            recordsToFilterSelectionStrategy.MustForArg(nameof(recordsToFilterSelectionStrategy)).NotBeEqualTo(RecordsToFilterSelectionStrategy.Unknown);

            this.RecordFilter = recordFilter;
            this.RecordsToFilterSelectionStrategy = recordsToFilterSelectionStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public RecordFilter RecordFilter { get; private set; }

        /// <inheritdoc />
        public RecordsToFilterSelectionStrategy RecordsToFilterSelectionStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
