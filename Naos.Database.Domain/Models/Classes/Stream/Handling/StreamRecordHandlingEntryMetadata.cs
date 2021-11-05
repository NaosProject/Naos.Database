// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordHandlingEntryMetadata.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Metadata of a stream record handling entry.
    /// </summary>
    public partial class StreamRecordHandlingEntryMetadata : IHaveTags, IModelViaCodeGen, IHaveTimestampUtc, IHaveInternalRecordId, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordHandlingEntryMetadata"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier of the record that is the subject of handling.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="status">The status of the entry.</param>
        /// <param name="stringSerializedId">The object's identifier serialized as a string.</param>
        /// <param name="serializerRepresentation">The representation of the serializer used to serialize the object.</param>
        /// <param name="typeRepresentationOfId">The type representation of the object's identifier.</param>
        /// <param name="typeRepresentationOfObject">The type representation of the object.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="timestampUtc">The timestamp of the handling entry in UTC.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StreamRecordHandlingEntryMetadata(
            long internalRecordId,
            string concern,
            HandlingStatus status,
            string stringSerializedId,
            SerializerRepresentation serializerRepresentation,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfId,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfObject,
            IReadOnlyCollection<NamedValue<string>> tags,
            DateTime timestampUtc)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            serializerRepresentation.MustForArg(nameof(serializerRepresentation)).NotBeNull();
            typeRepresentationOfId.MustForArg(nameof(typeRepresentationOfId)).NotBeNull();
            typeRepresentationOfObject.MustForArg(nameof(typeRepresentationOfObject)).NotBeNull();
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            if (timestampUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException(Invariant($"{nameof(timestampUtc)} must be in UTC format."));
            }

            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.Status = status;
            this.StringSerializedId = stringSerializedId;
            this.SerializerRepresentation = serializerRepresentation;
            this.Tags = tags;
            this.TypeRepresentationOfId = typeRepresentationOfId;
            this.TypeRepresentationOfObject = typeRepresentationOfObject;
            this.TimestampUtc = timestampUtc;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the status of the entry.
        /// </summary>
        public HandlingStatus Status { get; private set; }

        /// <summary>
        /// Gets the object's identifier serialized as a string.
        /// </summary>
        public string StringSerializedId { get; private set; }

        /// <summary>
        /// Gets the representation of the serializer used to serialize the object.
        /// </summary>
        public SerializerRepresentation SerializerRepresentation { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets the type representation of the object's identifier.
        /// </summary>
        public TypeRepresentationWithAndWithoutVersion TypeRepresentationOfId { get; private set; }

        /// <summary>
        /// Gets the type representation of object.
        /// </summary>
        public TypeRepresentationWithAndWithoutVersion TypeRepresentationOfObject { get; private set; }

        /// <inheritdoc />
        public DateTime TimestampUtc { get; private set; }
    }
}
