// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProtocolFactoryStreamObjectWriteOperations.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to get the protocols for the basic stream write operations.
    /// </summary>
    public interface IProtocolFactoryStreamObjectWriteOperations
    {
        /// <summary>
        /// Builds the <see cref="IProtocolStreamObjectWriteOperations{TId,TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for write operations.</returns>
        IProtocolStreamObjectWriteOperations<TId, TObject> GetObjectWriteOperationsProtocol<TId, TObject>();

        /// <summary>
        /// Builds the <see cref="IProtocolStreamObjectWriteOperations{TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Protocols for write operations.</returns>
        IProtocolStreamObjectWriteOperations<TObject> GetObjectWriteOperationsProtocol<TObject>();
    }
}
