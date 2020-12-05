// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStreamIdentifier.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// A null object to be used as the id of an object in a <see cref="IReadWriteStream"/> that does not have an actual identifier.
    /// </summary>
    public partial class NullStreamIdentifier : IModelViaCodeGen
    {
        /// <summary>
        /// The type representation of <see cref="NullStreamIdentifier"/>.
        /// </summary>
        public static readonly TypeRepresentationWithAndWithoutVersion TypeRepresentation =
            typeof(NullStreamIdentifier).ToRepresentation().ToWithAndWithoutVersion();
    }
}
