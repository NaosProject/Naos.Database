// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardTryHandleRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Try to handle a record.
    /// If handling is blocked OR there are no records to handle, then <see cref="TryHandleRecordResult.RecordToHandle"/> should be null.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardTryHandleRecordOp : ReturningOperationBase<TryHandleRecordResult>, IHaveHandleRecordConcern, IHaveDetails, IHaveTags, ISpecifyRecordFilter, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardTryHandleRecordOp"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="recordFilter">The filter to apply to the set of records to consider for handling.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <param name="details">OPTIONAL details to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no details.</param>
        /// <param name="minimumInternalRecordId">OPTIONAL minimum internal record identifier to consider for handling (this will allow for ordinal traversal and handle each record once before starting over which can be a desired behavior on protocols that self-cancel and are long running). DEFAULT is no minimum internal identifier.</param>
        /// <param name="inheritRecordTags">OPTIONAL value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.  DEFAULT is to not inherit tags.</param>
        /// <param name="tags">OPTIONAL tags to add to any new entries.</param>
        /// <param name="streamRecordItemsToInclude">OPTIONAL value that determines which aspects of a <see cref="StreamRecord"/> to include with the result.  DEFAULT is to include both the metadata and the payload.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardTryHandleRecordOp(
            string concern,
            RecordFilter recordFilter,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending,
            string details = null,
            long? minimumInternalRecordId = null,
            bool inheritRecordTags = false,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            StreamRecordItemsToInclude streamRecordItemsToInclude = StreamRecordItemsToInclude.MetadataAndPayload,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            orderRecordsBy.MustForArg(nameof(orderRecordsBy)).NotBeEqualTo(OrderRecordsBy.Unknown);
            streamRecordItemsToInclude.MustForArg(nameof(streamRecordItemsToInclude)).NotBeEqualTo(StreamRecordItemsToInclude.Unknown);

            this.Concern = concern;
            this.RecordFilter = recordFilter;
            this.OrderRecordsBy = orderRecordsBy;
            this.Details = details;
            this.MinimumInternalRecordId = minimumInternalRecordId;
            this.InheritRecordTags = inheritRecordTags;
            this.Tags = tags;
            this.StreamRecordItemsToInclude = streamRecordItemsToInclude;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <inheritdoc />
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets a value that specifies how to order the resulting records.
        /// </summary>
        public OrderRecordsBy OrderRecordsBy { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <summary>
        /// Gets the minimum internal record identifier to consider for handling or null for no minimum identifier.
        /// </summary>
        /// <remarks>
        /// This will allow for ordinal traversal and handle each record once before starting over which can be a desired behavior on protocols that self-cancel and are long running.
        /// </remarks>
        public long? MinimumInternalRecordId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.
        /// </summary>
        public bool InheritRecordTags { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets a value that determines which aspects of a <see cref="StreamRecord"/> to include with the result.
        /// </summary>
        public StreamRecordItemsToInclude StreamRecordItemsToInclude { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
