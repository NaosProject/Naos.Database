﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationWithAndWithoutVersion.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

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
    }
}