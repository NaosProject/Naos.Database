// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationWithAndWithoutVersion.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
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
        /// <param name="withVersion">The type representation with a version.</param>
        /// <param name="withoutVersion">OPTIONAL type representation without a version.  DEFAULT will be to strip version from <paramref name="withVersion"/>.</param>
        public TypeRepresentationWithAndWithoutVersion(
            TypeRepresentation withVersion,
            TypeRepresentation withoutVersion = null)
        {
            withVersion.MustForArg(nameof(withVersion)).NotBeNull();
            withVersion.AssemblyVersion.MustForArg(Invariant($"{nameof(withVersion)}.{nameof(TypeRepresentation.AssemblyVersion)}")).NotBeNullNorWhiteSpace();

            this.WithVersion = withVersion;
            this.WithoutVersion = withoutVersion ?? withVersion.RemoveAssemblyVersions();
        }

        /// <summary>
        /// Gets the type representation with a version.
        /// </summary>
        public TypeRepresentation WithVersion { get; private set; }

        /// <summary>
        /// Gets the type representation without a version.
        /// </summary>
        public TypeRepresentation WithoutVersion { get; private set; }
    }
}
