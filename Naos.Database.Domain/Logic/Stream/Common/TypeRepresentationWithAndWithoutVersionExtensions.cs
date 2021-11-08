// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationWithAndWithoutVersionExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Extensions for <see cref="TypeRepresentationWithAndWithoutVersion"/>.
    /// </summary>
    public static class TypeRepresentationWithAndWithoutVersionExtensions
    {
        private static readonly VersionlessTypeRepresentationEqualityComparer VersionlessComparer = new VersionlessTypeRepresentationEqualityComparer();

        /// <summary>
        /// Converts a <see cref="TypeRepresentation"/> to a <see cref="TypeRepresentationWithAndWithoutVersion"/>.
        /// </summary>
        /// <param name="typeRepresentationWithVersion">The type representation with version to convert.</param>
        /// <returns>
        /// Converted <see cref="TypeRepresentationWithAndWithoutVersion"/>.
        /// </returns>
        public static TypeRepresentationWithAndWithoutVersion ToWithAndWithoutVersion(
            this TypeRepresentation typeRepresentationWithVersion)
        {
            typeRepresentationWithVersion.MustForArg(nameof(typeRepresentationWithVersion)).NotBeNull();

            var result = new TypeRepresentationWithAndWithoutVersion(typeRepresentationWithVersion);

            return result;
        }

        /// <summary>
        /// Gets the correct <see cref="TypeRepresentation"/> by the provided <see cref="VersionMatchStrategy"/>.
        /// </summary>
        /// <param name="typeRepresentationWithAndWithoutVersion">The type representation with and without version.</param>
        /// <param name="versionMatchStrategy">The type version match strategy.</param>
        /// <returns>
        /// Appropriate <see cref="TypeRepresentation"/> according to the specified version match strategy.
        /// </returns>
        public static TypeRepresentation GetTypeRepresentationByStrategy(
            this TypeRepresentationWithAndWithoutVersion typeRepresentationWithAndWithoutVersion,
            VersionMatchStrategy versionMatchStrategy)
        {
            typeRepresentationWithAndWithoutVersion.MustForArg(nameof(typeRepresentationWithAndWithoutVersion)).NotBeNull();

            switch (versionMatchStrategy)
            {
                case VersionMatchStrategy.Any:
                    return typeRepresentationWithAndWithoutVersion.WithoutVersion;
                case VersionMatchStrategy.SpecifiedVersion:
                    return typeRepresentationWithAndWithoutVersion.WithVersion;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(VersionMatchStrategy)} is not supported: {versionMatchStrategy}."));
            }
        }

        /// <summary>
        /// Compares two <see cref="TypeRepresentation"/> objects using the provided <see cref="VersionMatchStrategy"/> to determine whether or not to compare using version.
        /// </summary>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="versionMatchStrategy">The type version match strategy.</param>
        /// <returns>
        /// <c>true</c> if the type representations are equal, otherwise <c>false</c>.
        /// </returns>
        public static bool EqualsAccordingToStrategy(
            this TypeRepresentation first,
            TypeRepresentation second,
            VersionMatchStrategy versionMatchStrategy)
        {
            first.MustForArg(nameof(first)).NotBeNull();
            second.MustForArg(nameof(second)).NotBeNull();

            bool result;

            if (versionMatchStrategy == VersionMatchStrategy.Any)
            {
                result = VersionlessComparer.Equals(first, second);
            }
            else if (versionMatchStrategy == VersionMatchStrategy.SpecifiedVersion)
            {
                // Wrapping in a TypeRepresentationWithAndWithoutVersion ensures that both first and second have a version,
                // which is required to compare by specified version.
                var firstWithAndWithoutVersion = new TypeRepresentationWithAndWithoutVersion(first);

                var secondWithAndWithoutVersion = new TypeRepresentationWithAndWithoutVersion(second);

                result = firstWithAndWithoutVersion.Equals(secondWithAndWithoutVersion);
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(VersionMatchStrategy)} is not supported: {versionMatchStrategy}."));
            }

            return result;
        }
    }
}