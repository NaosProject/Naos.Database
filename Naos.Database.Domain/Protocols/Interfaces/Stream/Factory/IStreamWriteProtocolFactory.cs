// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteProtocolFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
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
        /// Builds the <see cref="IStreamWriteWithIdProtocols{TId}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <returns>Protocols for write operations.</returns>
        IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>();

        /// <summary>
        /// Builds the <see cref="IStreamWriteWithIdProtocols{TId,TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for write operations.</returns>
        IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>();
    }
}
