// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadataExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Extensions on <see cref="StreamRecordMetadata"/> and <see cref="StreamRecordMetadata{TId}"/>.
    /// </summary>
    public static class StreamRecordMetadataExtensions
    {
        /// <summary>
        /// Matches the metadata against the inputs (allowing for skipping on the match).
        /// </summary>
        /// <param name="streamRecordMetadata">The stream record metadata.</param>
        /// <param name="identifierType">Type type of the identifier; null will exclude from match.</param>
        /// <param name="objectType">Type of the object; null will exclude from match.</param>
        /// <param name="versionMatchStrategy">The type version match strategy.</param>
        /// <returns><c>true</c> if matching, <c>false</c> otherwise.</returns>
        public static bool FuzzyMatchTypes(
            this StreamRecordMetadata streamRecordMetadata,
            TypeRepresentation identifierType,
            TypeRepresentation objectType,
            VersionMatchStrategy versionMatchStrategy)
        {
            var match = true;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse -- prefer this for safety in refactoring order
            if (match && identifierType != null)
            {
                match = streamRecordMetadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(versionMatchStrategy).EqualsAccordingToStrategy(identifierType, versionMatchStrategy);
            }

            if (match && objectType != null)
            {
                match = streamRecordMetadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(versionMatchStrategy).EqualsAccordingToStrategy(objectType, versionMatchStrategy);
            }

            return match;
        }

        /// <summary>
        /// Matches the metadata against the inputs (allowing for skipping on the match).
        /// </summary>
        /// <param name="streamRecordMetadata">The stream record metadata.</param>
        /// <param name="stringSerializedId">The object's identifier serialized as a string.</param>
        /// <param name="identifierType">Type type of the identifier; null will exclude from match.</param>
        /// <param name="objectType">Type of the object; null will exclude from match.</param>
        /// <param name="versionMatchStrategy">The type version match strategy.</param>
        /// <returns><c>true</c> if matching, <c>false</c> otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static bool FuzzyMatchTypesAndId(
            this StreamRecordMetadata streamRecordMetadata,
            string stringSerializedId,
            TypeRepresentation identifierType,
            TypeRepresentation objectType,
            VersionMatchStrategy versionMatchStrategy)
        {
            var result = streamRecordMetadata.FuzzyMatchTypes(identifierType, objectType, versionMatchStrategy);

            if (result && stringSerializedId != streamRecordMetadata.StringSerializedId)
            {
                result = false;
            }

            return result;
        }
    }
}
