// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadProtocolFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to get the protocols for the basic stream read operations.
    /// </summary>
    public interface IStreamReadProtocolFactory
    {
        /// <summary>
        /// Builds the <see cref="IStreamReadProtocols"/> protocol.
        /// </summary>
        /// <returns>Protocols for read operations.</returns>
        IStreamReadProtocols GetStreamReadingProtocols();

        /// <summary>
        /// Builds the <see cref="IStreamReadProtocols{TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for read operations.</returns>
        IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>();

        /// <summary>
        /// Builds the <see cref="IStreamReadWithIdProtocols{TId}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <returns>Protocols for read operations.</returns>
        IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>();

        /// <summary>
        /// Builds the <see cref="IStreamReadWithIdProtocols{TId,TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for read operations.</returns>
        IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>();
    }
}
