// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadataExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Extensions on <see cref="StreamRecordMetadata"/> and <see cref="StreamRecordMetadata{TId}"/>.
    /// </summary>
    public static class StreamRecordMetadataExtensions
    {
        /// <summary>
        /// Determines whether the object in a <see cref="StreamRecord"/>
        /// is matched by a specified object type and/or object identifier type.
        /// </summary>
        /// <param name="streamRecordMetadata">The stream record metadata.</param>
        /// <param name="identifierTypes">Types of the object's identifier; null will exclude from matching.</param>
        /// <param name="objectTypes">Types of the object; null will exclude from matching.</param>
        /// <param name="versionMatchStrategy">The strategy to use for matching the version of the object type and the version of the object's identifier type.</param>
        /// <returns>
        /// <c>true</c> if the record is a match, otherwise <c>false</c>.
        /// </returns>
        public static bool FuzzyMatchTypes(
            this StreamRecordMetadata streamRecordMetadata,
            IReadOnlyCollection<TypeRepresentation> identifierTypes,
            IReadOnlyCollection<TypeRepresentation> objectTypes,
            VersionMatchStrategy versionMatchStrategy)
        {
            streamRecordMetadata.MustForArg(nameof(streamRecordMetadata)).NotBeNull();

            var match = true;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse -- prefer this for safety in refactoring order
            if (match && (identifierTypes != null))
            {
                match = identifierTypes.Any(_ => streamRecordMetadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(versionMatchStrategy).EqualsAccordingToStrategy(_, versionMatchStrategy));
            }

            if (match && (objectTypes != null))
            {
                match = objectTypes.Any(_ => streamRecordMetadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(versionMatchStrategy).EqualsAccordingToStrategy(_, versionMatchStrategy));
            }

            return match;
        }

        /// <summary>
        /// Determines whether the object in a <see cref="StreamRecord"/>
        /// is matched by a specified object type and/or object identifier type,
        /// along with the identifier value.
        /// </summary>
        /// <param name="streamRecordMetadata">The stream record metadata.</param>
        /// <param name="stringSerializedId">The object's identifier serialized as a string.</param>
        /// <param name="identifierType">Type of the object's identifier; null will exclude from matching.</param>
        /// <param name="objectType">Type of the object; null will exclude from matching.</param>
        /// <param name="versionMatchStrategy">The strategy to use for matching the version of the object type and the version of the object's identifier type.</param>
        /// <returns>
        /// <c>true</c> if matching, <c>false</c> otherwise.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static bool FuzzyMatchTypesAndId(
            this StreamRecordMetadata streamRecordMetadata,
            string stringSerializedId,
            TypeRepresentation identifierType,
            TypeRepresentation objectType,
            VersionMatchStrategy versionMatchStrategy)
        {
            streamRecordMetadata.MustForArg(nameof(streamRecordMetadata)).NotBeNull();

            var result = streamRecordMetadata.FuzzyMatchTypes(
                identifierType == null
                    ? null
                    : new[]
                      {
                          identifierType,
                      },
                objectType == null
                    ? null
                    : new[]
                      {
                          objectType,
                      },
                versionMatchStrategy);

            if (result && (stringSerializedId != streamRecordMetadata.StringSerializedId))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Converts a <see cref="StreamRecordMetadata"/> object to a <see cref="StreamRecordMetadata{TId}"/> object
        /// given the identifier to use.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="metadata">The stream record metadata to convert.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// A <see cref="StreamRecordMetadata{TId}"/> object converted from the specified <see cref="StreamRecordMetadata"/> object.
        /// </returns>
        public static StreamRecordMetadata<TId> ToStreamRecordMetadata<TId>(
            this StreamRecordMetadata metadata,
            TId id)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();

            var result = new StreamRecordMetadata<TId>(
                id,
                metadata.SerializerRepresentation,
                metadata.TypeRepresentationOfId,
                metadata.TypeRepresentationOfObject,
                metadata.Tags,
                metadata.TimestampUtc,
                metadata.ObjectTimestampUtc);

            return result;
        }

        /// <summary>
        /// Converts a <see cref="StreamRecordMetadata"/> object to a <see cref="StreamRecordMetadata{TId}"/> object
        /// given a serializer to use to deserialize the identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="metadata">The stream record metadata to convert.</param>
        /// <param name="identifierDeserializer">The serializer to use to deserialize the identifier.</param>
        /// <returns>
        /// A <see cref="StreamRecordMetadata{TId}"/> object converted from the specified <see cref="StreamRecordMetadata"/> object.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Deserializer", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static StreamRecordMetadata<TId> ToStreamRecordMetadata<TId>(
            this StreamRecordMetadata metadata,
            IStringDeserialize identifierDeserializer)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            identifierDeserializer.MustForArg(nameof(identifierDeserializer)).NotBeNull();

            var id = identifierDeserializer.Deserialize<TId>(metadata.StringSerializedId);

            var result = metadata.ToStreamRecordMetadata(id);

            return result;
        }

        /// <summary>
        /// Converts a <see cref="StreamRecordMetadata"/> object to a <see cref="StreamRecordMetadata{TId}"/> object
        /// given a stream to use to deserialize the identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="metadata">The stream record metadata to convert.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// A <see cref="StreamRecordMetadata{TId}"/> object converted from the specified <see cref="StreamRecordMetadata"/> object.
        /// </returns>
        public static StreamRecordMetadata<TId> ToStreamRecordMetadata<TId>(
            this StreamRecordMetadata metadata,
            IStream stream)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            stream.MustForArg(nameof(stream)).NotBeNull();

            var identifierSerializer = stream
                .SerializerFactory
                .BuildSerializer(stream.DefaultSerializerRepresentation);

            var result = metadata.ToStreamRecordMetadata<TId>(identifierSerializer);

            return result;
        }
    }
}
