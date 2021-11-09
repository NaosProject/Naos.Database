// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullIdentifier.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// A null object to be used as the id of an object when one is needed but there isn't one.
    /// </summary>
    /// <remarks>
    /// For example, this can be used when writing an object that does not have an identifier into a  <see cref="IReadWriteStream"/>.
    /// </remarks>
    public partial class NullIdentifier : IModelViaCodeGen
    {
        /// <summary>
        /// The type representation of <see cref="NullIdentifier"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = NaosSuppressBecause.CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable)]
        public static readonly TypeRepresentationWithAndWithoutVersion TypeRepresentation =
            typeof(NullIdentifier).ToRepresentation().ToWithAndWithoutVersion();
    }
}
