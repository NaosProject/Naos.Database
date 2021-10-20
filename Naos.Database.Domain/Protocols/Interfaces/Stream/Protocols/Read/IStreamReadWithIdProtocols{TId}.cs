// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream reading operations with a typed identifier and without a typed record payload.
    /// </summary>
    /// <typeparam name="TId">Type of ID used.</typeparam>
    public interface IStreamReadWithIdProtocols<TId> :
        ISyncAndAsyncReturningProtocol<GetLatestRecordByIdOp<TId>, StreamRecordWithId<TId>>,
        ISyncAndAsyncReturningProtocol<GetAllRecordsByIdOp<TId>, IReadOnlyList<StreamRecordWithId<TId>>>,
        ISyncAndAsyncReturningProtocol<GetLatestRecordMetadataByIdOp<TId>, StreamRecordMetadata<TId>>,
        ISyncAndAsyncReturningProtocol<GetAllRecordsMetadataByIdOp<TId>, IReadOnlyList<StreamRecordMetadata<TId>>>,
        ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp<TId>, bool>
    {
    }
}
