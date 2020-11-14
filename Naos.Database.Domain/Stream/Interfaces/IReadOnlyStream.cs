// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadOnlyStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Stream interface, a stream is a list of objects ordered by timestamp, only read operations are supported.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public interface IReadOnlyStream
        : IStreamReadingProtocolsFactory
    {
        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        /// <value>The name of the stream.</value>
        string Name { get; }

        /// <summary>
        /// Gets the resource locator protocol.
        /// </summary>
        /// <value>The resource locator protocol.</value>
        IResourceLocatorProtocols ResourceLocatorProtocols { get; }

        /// <summary>
        /// Gets the representation of the stream.
        /// </summary>
        /// <value>The representation of the stream.</value>
        IStreamRepresentation StreamRepresentation { get; }
    }
}
