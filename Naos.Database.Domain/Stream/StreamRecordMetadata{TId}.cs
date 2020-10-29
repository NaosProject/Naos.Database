// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadata{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Strategy to use when trying to create a stream that already exists.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    public partial class StreamRecordMetadata<TId> : IIdentifiableBy<TId>, IHaveTags, IModelViaCodeGen, IHaveTimestampUtc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordMetadata{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="typeRepresentationWithVersion">The versioned type representation of the object.</param>
        /// <param name="typeRepresentationWithoutVersion">The un-version type representation of the object.</param>
        /// <param name="timestampUtc">Timestamp of the record in UTC.</param>
        public StreamRecordMetadata(
            TId id,
            IReadOnlyDictionary<string, string> tags,
            TypeRepresentation typeRepresentationWithVersion,
            TypeRepresentation typeRepresentationWithoutVersion,
            DateTime timestampUtc)
        {
            this.Id = id;
            this.Tags = tags;
            this.TypeRepresentationWithVersion = typeRepresentationWithVersion;
            this.TypeRepresentationWithoutVersion = typeRepresentationWithoutVersion;

            if (timestampUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("The timestamp must be in UTC format; it is: " + timestampUtc.Kind, nameof(timestampUtc));
            }

            this.TimestampUtc = timestampUtc;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }

        /// <summary>
        /// Gets the versioned type representation of the object..
        /// </summary>
        /// <value>The versioned type representation of the object.</value>
        public TypeRepresentation TypeRepresentationWithVersion { get; private set; }

        /// <summary>
        /// Gets the un-versioned type representation of the object.
        /// </summary>
        /// <value>The un-versioned type representation of the object.</value>
        public TypeRepresentation TypeRepresentationWithoutVersion { get; private set; }

        /// <inheritdoc />
        public DateTime TimestampUtc { get; private set; }
    }
}
