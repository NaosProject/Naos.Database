// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadingProtocolsFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to get the protocols for the basic stream read operations.
    /// </summary>
    public interface IStreamReadingProtocolsFactory
    {
        /// <summary>
        /// Builds the <see cref="IStreamReadingProtocols"/> protocol.
        /// </summary>
        /// <returns>Protocols for read operations.</returns>
        IStreamReadingProtocols GetStreamReadingProtocols();

        /// <summary>
        /// Builds the <see cref="IStreamReadingProtocols{TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for read operations.</returns>
        IStreamReadingProtocols<TObject> GetStreamReadingProtocols<TObject>();

        /// <summary>
        /// Builds the <see cref="IStreamReadingProtocols{TId,TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for read operations.</returns>
        IStreamReadingProtocols<TId, TObject> GetStreamReadingProtocols<TId, TObject>();
    }
}
