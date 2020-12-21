// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to protocol the basic stream data operations with a known identifier.
    /// </summary>
    /// <typeparam name="TId">Type of ID used.</typeparam>
    public interface IStreamReadWithIdProtocols<TId> :
        ISyncAndAsyncReturningProtocol<GetLatestRecordByIdOp<TId>, StreamRecordWithId<TId>>,
        ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp<TId>, bool>
    {
    }
}
