// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordWithIdHandlingProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Protocols to handle records from streams.
    /// </summary>
    /// <typeparam name="TId">The type of identifier of the record.</typeparam>
    public interface IStreamRecordWithIdHandlingProtocols<TId>
        : ISyncAndAsyncReturningProtocol<TryHandleRecordWithIdOp<TId>, StreamRecordWithId<TId>>,
          ISyncAndAsyncReturningProtocol<GetHandlingStatusOfRecordsByIdOp<TId>, HandlingStatus>
    {
    }
}
