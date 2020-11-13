// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProtocolFactoryStreamObjectReadOperations.cs" company="Naos Project">
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
    public interface IProtocolFactoryStreamObjectReadOperations
    {
        /// <summary>
        /// Builds the <see cref="IProtocolStreamObjectReadOperations{TId, TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for read operations.</returns>
        IProtocolStreamObjectReadOperations<TId, TObject> GetObjectReadOperationsProtocol<TId, TObject>();

        /// <summary>
        /// Builds the <see cref="IProtocolStreamObjectReadOperations{TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for read operations.</returns>
        IProtocolStreamObjectReadOperations<TObject> GetObjectReadOperationsProtocol<TObject>();
    }
}
