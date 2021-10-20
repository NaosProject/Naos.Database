// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRepresentationBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Stream description to allow the <see cref="GetStreamFromRepresentationByNameProtocolFactory"/> to produce the correct stream.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public abstract partial class StreamRepresentationBase : IStreamRepresentation, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRepresentationBase"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        protected StreamRepresentationBase(string name)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        public string Name { get; private set; }
    }
}
