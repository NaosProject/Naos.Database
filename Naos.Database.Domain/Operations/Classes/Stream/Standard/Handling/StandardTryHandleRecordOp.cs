// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardTryHandleRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Try to handle a record.
    /// </summary>
    public partial class StandardTryHandleRecordOp : ReturningOperationBase<TryHandleRecordResult>, ITryHandleRecordOp, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardTryHandleRecordOp"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="identifierType">OPTIONAL type of the object identifier to filter on.  DEFAULT is no filter.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no tags.</param>
        /// <param name="details">OPTIONAL details to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no details.</param>
        /// <param name="minimumInternalRecordId">OPTIONAL minimum internal record identifier to consider for handling (this will allow for ordinal traversal and handle each record once before starting over which can be a desired behavior on protocols that self-cancel and are long running). DEFAULT is no minimum internal identifier.</param>
        /// <param name="inheritRecordTags">OPTIONAL value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.  DEFAULT is to not inherit tags.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardTryHandleRecordOp(
            string concern,
            TypeRepresentation identifierType = null,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            string details = null,
            long? minimumInternalRecordId = null,
            bool inheritRecordTags = false,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Concern = concern;
            this.IdentifierType = identifierType;
            this.ObjectType = objectType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.OrderRecordsBy = orderRecordsBy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
            this.Tags = tags;
            this.Details = details;
            this.MinimumInternalRecordId = minimumInternalRecordId;
            this.InheritRecordTags = inheritRecordTags;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the type of the identifier to filter on or null when not matching on object identifier type.
        /// </summary>
        public TypeRepresentation IdentifierType { get; private set; }

        /// <summary>
        /// Gets the type of the object to filter on or null when not matching on object type.
        /// </summary>
        public TypeRepresentation ObjectType { get; private set; }

        /// <inheritdoc />
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <inheritdoc />
        public OrderRecordsBy OrderRecordsBy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }

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
