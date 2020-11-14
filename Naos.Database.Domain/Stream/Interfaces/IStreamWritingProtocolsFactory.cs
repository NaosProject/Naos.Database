// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWritingProtocolsFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to get the protocols for the basic stream write operations.
    /// </summary>
    public interface IStreamWritingProtocolsFactory
    {
        /// <summary>
        /// Builds the <see cref="IStreamWritingProtocols{TObject}"/> protocol.
        /// </summary>
        /// <returns>Protocols for write operations.</returns>
        IStreamWritingProtocols GetStreamWritingProtocols();

        /// <summary>
        /// Builds the <see cref="IStreamWritingProtocols{TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for write operations.</returns>
        IStreamWritingProtocols<TObject> GetStreamWritingProtocols<TObject>();

        /// <summary>
        /// Builds the <see cref="IStreamWritingProtocols{TId,TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for write operations.</returns>
        IStreamWritingProtocols<TId, TObject> GetStreamWritingProtocols<TId, TObject>();
    }
}
