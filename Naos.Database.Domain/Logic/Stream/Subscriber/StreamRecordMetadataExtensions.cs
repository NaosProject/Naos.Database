// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadataExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
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
        /// <param name="identifierType">Type of the object's identifier; null will exclude from matching.</param>
        /// <param name="objectType">Type of the object; null will exclude from matching.</param>
        /// <param name="versionMatchStrategy">The strategy to use for matching the version of the object type and the version of the object's identifier type.</param>
        /// <returns>
        /// <c>true</c> if the record is a match, otherwise <c>false</c>.
        /// </returns>
        public static bool FuzzyMatchTypes(
            this StreamRecordMetadata streamRecordMetadata,
            TypeRepresentation identifierType,
            TypeRepresentation objectType,
            VersionMatchStrategy versionMatchStrategy)
        {
            streamRecordMetadata.MustForArg(nameof(streamRecordMetadata)).NotBeNull();

            var match = true;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse -- prefer this for safety in refactoring order
            if (match && (identifierType != null))
            {
                match = streamRecordMetadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(versionMatchStrategy).EqualsAccordingToStrategy(identifierType, versionMatchStrategy);
            }

            if (match && (objectType != null))
            {
                match = streamRecordMetadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(versionMatchStrategy).EqualsAccordingToStrategy(objectType, versionMatchStrategy);
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

            var result = streamRecordMetadata.FuzzyMatchTypes(identifierType, objectType, versionMatchStrategy);

            if (result && (stringSerializedId != streamRecordMetadata.StringSerializedId))
            {
                result = false;
            }

            return result;
        }
    }
}
