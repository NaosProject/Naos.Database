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
    /// Handles a record.
    /// </summary>
    public partial class StandardTryHandleRecordOp : ReturningOperationBase<TryHandleRecordResult>, ITryHandleRecordOp, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardTryHandleRecordOp"/> class.
        /// </summary>
        /// <param name="concern">The concern.</param>
        /// <param name="identifierType">The optional type of the identifier; default is no filter.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is Any.</param>
        /// <param name="orderRecordsBy">The optional ordering for the records; DEFAULT is ascending by internal record identifier.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        /// <param name="tags">The optional tags to write with produced events.</param>
        /// <param name="details">The optional details to write with produced events.</param>
        /// <param name="minimumInternalRecordId">The optional minimum record identifier to consider for handling (this will allow for ordinal traversal and handle each record once before starting over which can be desired behavior on things that self-cancel and are long running).</param>
        /// <param name="inheritRecordTags">The optional value indicating whether handling entries should include any tags on the record being handled; DEFAULT is 'false'.</param>
        public StandardTryHandleRecordOp(
            string concern,
            TypeRepresentation identifierType = null,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending,
            IResourceLocator specifiedResourceLocator = null,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            string details = null,
            long? minimumInternalRecordId = null,
            bool inheritRecordTags = false)
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
        /// Gets the type of the identifier.
        /// </summary>
        /// <value>The type of the identifier.</value>
        public TypeRepresentation IdentifierType { get; private set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
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
