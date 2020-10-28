// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamRepresentation{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Stream description to allow the <see cref="StreamFactory{TId}"/> to produce the correct stream.
    /// </summary>
    /// <typeparam name="TId">The type of ID of the stream.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class MemoryStreamRepresentation<TId> : StreamRepresentationBase<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamRepresentation{TId}"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="id">The process name.</param>
        public MemoryStreamRepresentation(
            string name,
            string id)
            : base(name)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();

            this.Id = id;
        }

        /// <summary>
        /// Gets the name of the process name.
        /// </summary>
        /// <value>The name of the process name.</value>
        public string Id { get; private set; }
    }
}
