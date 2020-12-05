// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Extensions to <see cref="StreamRecord"/>.
    /// </summary>
    public static class StreamRecordExtensions
    {
        /// <summary>
        /// Fuzzy matches the record on inputs.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="identifierType">Type of the identifier (null will skip logic).</param>
        /// <param name="objectType">Type of the object (null will skip logic).</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <returns><c>true</c> if matches per inputs, <c>false</c> otherwise.</returns>
        public static bool FuzzyMatch(
            this StreamRecord record,
            TypeRepresentationWithAndWithoutVersion identifierType,
            TypeRepresentationWithAndWithoutVersion objectType,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            if (identifierType == null && objectType == null)
            {
                // filters are not enabled.
                return true;
            }
            else if (identifierType != null && objectType == null)
            {
                var result = identifierType.EqualsAccordingToStrategy(record.Metadata.TypeRepresentationOfId, typeVersionMatchStrategy);
                return result;
            }
            else if (identifierType == null && objectType != null)
            {
                var result = objectType.EqualsAccordingToStrategy(record.Metadata.TypeRepresentationOfObject, typeVersionMatchStrategy);
                return result;
            }
            else if (identifierType != null && objectType != null)
            {
                var result =
                    identifierType.EqualsAccordingToStrategy(record.Metadata.TypeRepresentationOfId, typeVersionMatchStrategy)
                 && objectType.EqualsAccordingToStrategy(record.Metadata.TypeRepresentationOfObject, typeVersionMatchStrategy);
                return result;
            }
            else
            {
                throw new InvalidOperationException("This should not have been reached.");
            }
        }
    }
}
