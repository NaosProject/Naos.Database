// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationWithAndWithoutVersion.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Container to hold two <see cref="TypeRepresentation"/>'s, one with the version and one without.
    /// </summary>
    public partial class TypeRepresentationWithAndWithoutVersion : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRepresentationWithAndWithoutVersion"/> class.
        /// </summary>
        /// <param name="withVersion">The with version.</param>
        /// <param name="withoutVersion">The optional without version; DEFAULT will be to strip version from <paramref name="withVersion"/>.</param>
        public TypeRepresentationWithAndWithoutVersion(
            TypeRepresentation withVersion,
            TypeRepresentation withoutVersion = null)
        {
            withVersion.MustForArg(nameof(withVersion)).NotBeNull();

            this.WithVersion = withVersion;
            this.WithoutVersion = withoutVersion ?? withVersion.RemoveAssemblyVersions();
        }

        /// <summary>
        /// Gets the with version.
        /// </summary>
        /// <value>The with version.</value>
        public TypeRepresentation WithVersion { get; private set; }

        /// <summary>
        /// Gets the without version.
        /// </summary>
        /// <value>The without version.</value>
        public TypeRepresentation WithoutVersion { get; private set; }
    }

    /// <summary>
    /// Extensions for <see cref="TypeRepresentationWithAndWithoutVersion"/>.
    /// </summary>
    public static class TypeRepresentationWithAndWithoutVersionExtensions
    {
        private static readonly VersionlessTypeRepresentationEqualityComparer VersionlessComparer = new VersionlessTypeRepresentationEqualityComparer();

        /// <summary>
        /// Converts to a <see cref="TypeRepresentation"/> to a <see cref="TypeRepresentationWithAndWithoutVersion"/>.
        /// </summary>
        /// <param name="typeRepresentationWithVersion">The type representation with version to convert.</param>
        /// <returns>Converted <see cref="TypeRepresentationWithAndWithoutVersion"/>.</returns>
        public static TypeRepresentationWithAndWithoutVersion ToWithAndWithoutVersion(
            this TypeRepresentation typeRepresentationWithVersion)
        {
            var result = new TypeRepresentationWithAndWithoutVersion(typeRepresentationWithVersion);
            return result;
        }

        /// <summary>
        /// Gets the correct <see cref="TypeRepresentation"/> by the provided <see cref="TypeVersionMatchStrategy"/>.
        /// </summary>
        /// <param name="typeRepresentationWithAndWithoutVersion">The type representation with and without version.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <returns>Appropriate <see cref="TypeRepresentation"/> according to strategy.</returns>
        public static TypeRepresentation GetTypeRepresentationByStrategy(
            this TypeRepresentationWithAndWithoutVersion typeRepresentationWithAndWithoutVersion,
            TypeVersionMatchStrategy typeVersionMatchStrategy)
        {
            switch (typeVersionMatchStrategy)
            {
                case TypeVersionMatchStrategy.Any:
                    return typeRepresentationWithAndWithoutVersion.WithoutVersion;
                case TypeVersionMatchStrategy.Specific:
                    return typeRepresentationWithAndWithoutVersion.WithVersion;
                default:
                    throw new NotSupportedException(Invariant($"The {nameof(TypeVersionMatchStrategy)} '{typeVersionMatchStrategy}' is not supported (only '{nameof(TypeVersionMatchStrategy.Any)}' and '{nameof(TypeVersionMatchStrategy.Specific)}')."));
            }
        }

        /// <summary>
        /// Compares a <see cref="TypeRepresentation"/> to an external <see cref="TypeRepresentation"/>
        /// using the provided <see cref="TypeVersionMatchStrategy"/> to determine whether or not to include the version.
        /// </summary>
        /// <param name="first">The <see cref="TypeRepresentation"/> to compare.</param>
        /// <param name="second">The <see cref="TypeRepresentation"/> to compare against.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <returns><c>true</c> if equal according to strategy, <c>false</c> otherwise.</returns>
        public static bool EqualsAccordingToStrategy(
            this TypeRepresentation first,
            TypeRepresentation second,
            TypeVersionMatchStrategy typeVersionMatchStrategy)
        {
            bool result;
            switch (typeVersionMatchStrategy)
            {
                case TypeVersionMatchStrategy.Any:
                    result = VersionlessComparer.Equals(first, second);
                    break;
                case TypeVersionMatchStrategy.Specific:
                    result = second.Equals(first);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(TypeVersionMatchStrategy)} {typeVersionMatchStrategy} is not supported."));
            }

            return result;
        }
    }
}
