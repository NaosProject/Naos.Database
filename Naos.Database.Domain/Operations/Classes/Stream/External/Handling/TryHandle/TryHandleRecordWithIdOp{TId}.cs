// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordWithIdOp{TId}.cs" company="Naos Project">
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
    /// Try to handle a record of type using object identifier type <typeparamref name="TId"/> for a specified concern.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier of the record.</typeparam>
    public partial class TryHandleRecordWithIdOp<TId> : ReturningOperationBase<StreamRecordWithId<TId>>, ITryHandleRecordOp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordWithIdOp{TId}"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no tags.</param>
        /// <param name="details">OPTIONAL details to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no details.</param>
        /// <param name="minimumInternalRecordId">OPTIONAL minimum internal record identifier to consider for handling (this will allow for ordinal traversal and handle each record once before starting over which can be a desired behavior on protocols that self-cancel and are long running). DEFAULT is no minimum internal identifier.</param>
        /// <param name="inheritRecordTags">OPTIONAL value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.  DEFAULT is to not inherit tags.</param>
        public TryHandleRecordWithIdOp(
            string concern,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            string details = null,
            long? minimumInternalRecordId = null,
            bool inheritRecordTags = false)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            this.Concern = concern;
            this.ObjectType = objectType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.OrderRecordsBy = orderRecordsBy;
            this.Tags = tags;
            this.Details = details;
            this.MinimumInternalRecordId = minimumInternalRecordId;
            this.InheritRecordTags = inheritRecordTags;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the type of the object to filter on.
        /// </summary>
        public TypeRepresentation ObjectType { get; private set; }

        /// <inheritdoc />
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <inheritdoc />
        public OrderRecordsBy OrderRecordsBy { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public long? MinimumInternalRecordId { get; private set; }

        /// <inheritdoc />
        public bool InheritRecordTags { get; private set; }
    }
}
