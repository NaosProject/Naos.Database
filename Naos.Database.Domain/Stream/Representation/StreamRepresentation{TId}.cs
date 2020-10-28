// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRepresentation{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;

    /// <summary>
    /// Stream description to allow the <see cref="StreamFactory{TId}"/> to produce the correct stream.
    /// </summary>
    /// <typeparam name="TId">The type of ID of the stream.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class StreamRepresentation<TId> : StreamRepresentationBase<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRepresentation{TId}"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        public StreamRepresentation(
            string name)
            : base(name)
        {
            /* no-op */
        }
    }
}
