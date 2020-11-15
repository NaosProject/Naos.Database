// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadata.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Metadata of a stream entry.
    /// </summary>
    public partial class StreamRecordMetadata : IHaveTags, IModelViaCodeGen, IHaveTimestampUtc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordMetadata"/> class.
        /// </summary>
        /// <param name="stringSerializedId">The identifier serialized as a string.</param>
        /// <param name="typeRepresentationOfId">The type representation of the identifier.</param>
        /// <param name="typeRepresentationOfObject">The type representation of the object.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="timestampUtc">Timestamp of the record in UTC.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StreamRecordMetadata(
            string stringSerializedId,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfId,
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfObject,
            IReadOnlyDictionary<string, string> tags,
            DateTime timestampUtc)
        {
            tags.MustForArg(nameof(tags)).NotBeNull();
            typeRepresentationOfId.MustForArg(nameof(typeRepresentationOfId)).NotBeNull();
            typeRepresentationOfObject.MustForArg(nameof(typeRepresentationOfObject)).NotBeNull();

            this.StringSerializedId = stringSerializedId;
            this.Tags = tags;
            this.TypeRepresentationOfId = typeRepresentationOfId;
            this.TypeRepresentationOfObject = typeRepresentationOfObject;

            if (timestampUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("The timestamp must be in UTC format; it is: " + timestampUtc.Kind, nameof(timestampUtc));
            }

            this.TimestampUtc = timestampUtc;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string StringSerializedId { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }

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
    }
}
