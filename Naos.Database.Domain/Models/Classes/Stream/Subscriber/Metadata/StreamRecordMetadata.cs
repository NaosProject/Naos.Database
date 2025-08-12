// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadata.cs" company="Naos Project">
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
    /// Metadata of a stream record.
    /// </summary>
    public partial class StreamRecordMetadata : IHaveTags, IModelViaCodeGen, IHaveTimestampUtc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordMetadata"/> class.
        /// </summary>
        /// <param name="stringSerializedId">The object's identifier serialized as a string.</param>
        /// <param name="serializerRepresentation">The representation of the serializer used to serialize the object (not the identifier).</param>
        /// <param name="typeRepresentationOfId">The type representation of the object's identifier.</param>
        /// <param name="typeRepresentationOfObject">The type representation of the object.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="timestampUtc">The timestamp of the record in UTC.</param>
        /// <param name="objectTimestampUtc">The timestamp of the object (if the object is an <see cref="IHaveTimestampUtc"/>), otherwise null.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StreamRecordMetadata(
            string stringSerializedId,
            SerializerRepresentation serializerRepresentation,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfId,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfObject,
            IReadOnlyCollection<NamedValue<string>> tags,
            DateTime timestampUtc,
            DateTime? objectTimestampUtc)
        {
            serializerRepresentation.MustForArg(nameof(serializerRepresentation)).NotBeNull();
            typeRepresentationOfId.MustForArg(nameof(typeRepresentationOfId)).NotBeNull();
            typeRepresentationOfObject.MustForArg(nameof(typeRepresentationOfObject)).NotBeNull();
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            if (timestampUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException(Invariant($"{nameof(timestampUtc)} must be in UTC format."));
            }

            if (objectTimestampUtc != null && objectTimestampUtc?.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException(Invariant($"{nameof(objectTimestampUtc)} must be in UTC format."));
            }

            this.StringSerializedId = stringSerializedId;
            this.SerializerRepresentation = serializerRepresentation;
            this.Tags = tags;
            this.TypeRepresentationOfId = typeRepresentationOfId;
            this.TypeRepresentationOfObject = typeRepresentationOfObject;
            this.TimestampUtc = timestampUtc;
            this.ObjectTimestampUtc = objectTimestampUtc;
        }

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

        /// <summary>
        /// Gets the timestamp of the object (if the object is an <see cref="IHaveTimestampUtc"/>), otherwise null.
        /// </summary>
        public DateTime? ObjectTimestampUtc { get; private set; }
    }
}
