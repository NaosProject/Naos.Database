// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProtocolFactoryStreamObjectWriteOperations{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to get the protocols for the basic stream write operations.
    /// </summary>
    /// <typeparam name="TId">Type of ID used.</typeparam>
    public interface IProtocolFactoryStreamObjectWriteOperations<TId>
    {
        /// <summary>
        /// Gets the <see cref="PutOp{TObject}"/> protocol.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>TProtocol.</returns>
        ISyncAndAsyncVoidProtocol<PutOp<TObject>> BuildPutProtocol<TObject>();
    }
}
