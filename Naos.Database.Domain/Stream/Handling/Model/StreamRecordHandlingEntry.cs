// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordHandlingEntry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Metadata of a stream entry.
    /// </summary>
    public partial class StreamRecordHandlingEntry : IHaveTags, IModelViaCodeGen, IHaveTimestampUtc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordHandlingEntry"/> class.
        /// </summary>
        /// <param name="internalHandlingEntryId">The internal handling entry identifier.</param>
        /// <param name="concern">The concern.</param>
        /// <param name="typeRepresentationOfEntry">The type representation of entry.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="timestampUtc">The timestamp UTC.</param>
        public StreamRecordHandlingEntry(
            long internalHandlingEntryId,
            string concern,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfEntry,
            DescribedSerialization payload,
            IReadOnlyDictionary<string, string> tags,
            DateTime timestampUtc)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            typeRepresentationOfEntry.MustForArg(nameof(typeRepresentationOfEntry)).NotBeNull();
            payload.MustForArg(nameof(payload)).NotBeNull();
            if (timestampUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("The timestamp must be in UTC format; it is: " + timestampUtc.Kind, nameof(timestampUtc));
            }

            this.InternalHandlingEntryId = internalHandlingEntryId;
            this.Concern = concern;
            this.TypeRepresentationOfEntry = typeRepresentationOfEntry;
            this.Payload = payload;
            this.Tags = tags ?? new Dictionary<string, string>();
            this.TimestampUtc = timestampUtc;
        }

        /// <summary>
        /// Gets the internal handling entry identifier.
        /// </summary>
        /// <value>The internal handling entry identifier.</value>
        public long InternalHandlingEntryId { get; private set; }

        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the type representation of the entry.
        /// </summary>
        /// <value>The type representation of entry.</value>
        public TypeRepresentationWithAndWithoutVersion TypeRepresentationOfEntry { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        public DescribedSerialization Payload { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }

        /// <inheritdoc />
        public DateTime TimestampUtc { get; private set; }
    }
}
