// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadataExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
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
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <returns><c>true</c> if matching, <c>false</c> otherwise.</returns>
        public static bool FuzzyMatchTypes(
            this StreamRecordMetadata streamRecordMetadata,
            TypeRepresentationWithAndWithoutVersion identifierType,
            TypeRepresentationWithAndWithoutVersion objectType,
            TypeVersionMatchStrategy typeVersionMatchStrategy)
        {
            var match = true;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse -- prefer this for safety in refactoring order
            if (match && identifierType != null)
            {
                match = streamRecordMetadata.TypeRepresentationOfId.EqualsAccordingToStrategy(identifierType.WithVersion, typeVersionMatchStrategy);
            }

            if (match && objectType != null)
            {
                match = streamRecordMetadata.TypeRepresentationOfObject.EqualsAccordingToStrategy(objectType.WithVersion, typeVersionMatchStrategy);
            }

            return match;
        }

        /// <summary>
        /// Matches the metadata against the inputs (allowing for skipping on the match).
        /// </summary>
        /// <param name="streamRecordMetadata">The stream record metadata.</param>
        /// <param name="stringSerializedId">The identifier serialized to a string using the same as the object serializer.</param>
        /// <param name="identifierType">Type type of the identifier; null will exclude from match.</param>
        /// <param name="objectType">Type of the object; null will exclude from match.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <returns><c>true</c> if matching, <c>false</c> otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static bool FuzzyMatchTypesAndId(
            this StreamRecordMetadata streamRecordMetadata,
            string stringSerializedId,
            TypeRepresentationWithAndWithoutVersion identifierType,
            TypeRepresentationWithAndWithoutVersion objectType,
            TypeVersionMatchStrategy typeVersionMatchStrategy)
        {
            var result = streamRecordMetadata.FuzzyMatchTypes(identifierType, objectType, typeVersionMatchStrategy);

            if (result && stringSerializedId != streamRecordMetadata.StringSerializedId)
            {
                result = false;
            }

            return result;
        }
    }
}
