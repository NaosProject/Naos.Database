// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordHandlingEntry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// A stream record handling entry.
    /// </summary>
    public partial class StreamRecordHandlingEntry : IHaveTags, IModelViaCodeGen, IHaveTimestampUtc, IHaveInternalRecordId, IHaveHandleRecordConcern, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordHandlingEntry"/> class.
        /// </summary>
        /// <param name="internalHandlingEntryId">The internal handling entry identifier.</param>
        /// <param name="internalRecordId">The internal record identifier of the record that is the subject of handling.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="status">The status of the entry.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="details">The details.</param>
        /// <param name="timestampUtc">The timestamp of the handling entry in UTC.</param>
        public StreamRecordHandlingEntry(
            long internalHandlingEntryId,
            long internalRecordId,
            string concern,
            HandlingStatus status,
            IReadOnlyCollection<NamedValue<string>> tags,
            string details,
            DateTime timestampUtc)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            if (timestampUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException(Invariant($"{nameof(timestampUtc)} must be in UTC format."));
            }

            this.InternalHandlingEntryId = internalHandlingEntryId;
            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.Status = status;
            this.Tags = tags;
            this.Details = details;
            this.TimestampUtc = timestampUtc;
        }

        /// <summary>
        /// Gets the internal handling entry identifier.
        /// </summary>
        public long InternalHandlingEntryId { get; private set; }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the status of the entry.
        /// </summary>
        public HandlingStatus Status { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public DateTime TimestampUtc { get; private set; }
    }
}
