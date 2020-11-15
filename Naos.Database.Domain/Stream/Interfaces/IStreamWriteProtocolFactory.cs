// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteProtocolFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to get the protocols for the basic stream write operations.
    /// </summary>
    public interface IStreamWriteProtocolFactory
    {
        /// <summary>
        /// Builds the <see cref="IStreamWriteProtocols{TObject}"/> protocol.
        /// </summary>
        /// <returns>Protocols for write operations.</returns>
        IStreamWriteProtocols GetStreamWritingProtocols();

        /// <summary>
        /// Builds the <see cref="IStreamWriteProtocols{TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for write operations.</returns>
        IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>();

        /// <summary>
        /// Builds the <see cref="IStreamWriteProtocols{TId,TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for write operations.</returns>
        IStreamWriteProtocols<TId, TObject> GetStreamWritingProtocols<TId, TObject>();
    }
}
