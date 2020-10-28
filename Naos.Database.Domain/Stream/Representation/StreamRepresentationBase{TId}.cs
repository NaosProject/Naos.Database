// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRepresentationBase{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Stream description to allow the <see cref="StreamFactory{TId}"/> to produce the correct stream.
    /// </summary>
    /// <typeparam name="TId">The type of ID of the stream.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public abstract partial class StreamRepresentationBase<TId> : IStreamRepresentation<TId>, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRepresentationBase{TId}"/> class.
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
        /// <value>The name of the stream.</value>
        public string Name { get; private set; }
    }
}
