// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRepresentationBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class implementation of an <see cref="IStreamRepresentation"/>.
    /// </summary>
    public abstract partial class StreamRepresentationBase : IStreamRepresentation, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRepresentationBase"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        protected StreamRepresentationBase(
            string name)
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
