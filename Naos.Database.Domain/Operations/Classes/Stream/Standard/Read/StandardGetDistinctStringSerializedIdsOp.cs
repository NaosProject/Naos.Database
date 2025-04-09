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
    public partial class StandardGetDistinctStringSerializedIdsOp : ReturningOperationBase<IReadOnlyCollection<StringSerializedIdentifier>>, ISpecifyRecordFilter, ISpecifyRecordsToFilterCriteria, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetDistinctStringSerializedIdsOp"/> class.
        /// </summary>
        /// <param name="recordFilter">The filter to apply to the set of records to consider.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into <paramref name="recordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetDistinctStringSerializedIdsOp(
            RecordFilter recordFilter,
            RecordsToFilterCriteria recordsToFilterCriteria = null,
            IResourceLocator specifiedResourceLocator = null)
        {
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();

            recordsToFilterCriteria = recordsToFilterCriteria ?? new RecordsToFilterCriteria();

            this.RecordFilter = recordFilter;
            this.RecordsToFilterCriteria = recordsToFilterCriteria;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public RecordFilter RecordFilter { get; private set; }

        /// <inheritdoc />
        public RecordsToFilterCriteria RecordsToFilterCriteria { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
