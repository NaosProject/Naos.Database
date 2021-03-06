﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordHandlingEntryMetadata.cs" company="Naos Project">
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
    /// Metadata of a stream handling entry.
    /// </summary>
    public partial class StreamRecordHandlingEntryMetadata : IHaveTags, IModelViaCodeGen, IHaveTimestampUtc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordHandlingEntryMetadata"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier of the record that originated the handling.</param>
        /// <param name="concern">The concern.</param>
        /// <param name="status">The status of the entry.</param>
        /// <param name="stringSerializedId">The identifier serialized as a string.</param>
        /// <param name="serializerRepresentation">The representation of the serializer used.</param>
        /// <param name="typeRepresentationOfId">The type representation of the identifier.</param>
        /// <param name="typeRepresentationOfObject">The type representation of the object.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="timestampUtc">The timestamp of the record in UTC.</param>
        /// <param name="objectTimestampUtc">The object's timestamp in UTC (if applicable).</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StreamRecordHandlingEntryMetadata(
            long internalRecordId,
            string concern,
            HandlingStatus status,
            string stringSerializedId,
            SerializerRepresentation serializerRepresentation,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfId,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfObject,
            IReadOnlyCollection<NamedValue<string>> tags,
            DateTime timestampUtc,
            DateTime? objectTimestampUtc)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            serializerRepresentation.MustForArg(nameof(serializerRepresentation)).NotBeNull();
            typeRepresentationOfId.MustForArg(nameof(typeRepresentationOfId)).NotBeNull();
            typeRepresentationOfObject.MustForArg(nameof(typeRepresentationOfObject)).NotBeNull();

            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.Status = status;
            this.StringSerializedId = stringSerializedId;
            this.SerializerRepresentation = serializerRepresentation;
            this.Tags = tags;
            this.TypeRepresentationOfId = typeRepresentationOfId;
            this.TypeRepresentationOfObject = typeRepresentationOfObject;

            if (timestampUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("The timestamp must be in UTC format; it is: " + timestampUtc.Kind, nameof(timestampUtc));
            }

            if (objectTimestampUtc != null && objectTimestampUtc?.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("The timestamp must be in UTC format; it is: " + timestampUtc.Kind, nameof(timestampUtc));
            }

            this.TimestampUtc = timestampUtc;
            this.ObjectTimestampUtc = objectTimestampUtc;
        }

        /// <summary>
        /// Gets the internal record identifier.
        /// </summary>
        /// <value>The internal record identifier.</value>
        public long InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public HandlingStatus Status { get; private set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string StringSerializedId { get; private set; }

        /// <summary>
        /// Gets the serializer representation.
        /// </summary>
        /// <value>The serializer representation.</value>
        public SerializerRepresentation SerializerRepresentation { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets the type representation of identifier.
        /// </summary>
        /// <value>The type representation of identifier.</value>
        public TypeRepresentationWithAndWithoutVersion TypeRepresentationOfId { get; private set; }

        /// <summary>
        /// Gets the type representation of object.
        /// </summary>
        /// <value>The type representation of object.</value>
        public TypeRepresentationWithAndWithoutVersion TypeRepresentationOfObject { get; private set; }

        /// <inheritdoc />
        public DateTime TimestampUtc { get; private set; }

        /// <summary>
        /// Gets the object timestamp in UTC (if applicable).
        /// </summary>
        /// <value>The object timestamp in UTC (if applicable).</value>
        public DateTime? ObjectTimestampUtc { get; private set; }
    }
}
