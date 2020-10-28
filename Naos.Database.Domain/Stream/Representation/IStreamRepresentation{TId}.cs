// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRepresentation{TId}.cs" company="Naos Project">
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
    public interface IStreamRepresentation<TId>
    {
        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        /// <value>The name of the stream.</value>
        string Name { get; }
    }
}
