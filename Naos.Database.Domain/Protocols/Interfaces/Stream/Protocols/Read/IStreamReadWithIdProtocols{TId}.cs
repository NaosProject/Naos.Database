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
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IStreamReadWithIdProtocols<TId> :
        IGetLatestRecordById<TId>,
        IGetAllRecordsById<TId>,
        IGetLatestRecordMetadataById<TId>,
        IGetAllRecordsMetadataById<TId>,
        IDoesAnyExistById<TId>,
        IGetLatestStringSerializedObjectById<TId>,
        IGetDistinctIds<TId>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="DoesAnyExistByIdOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IDoesAnyExistById<TId> :
        ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp<TId>, bool>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetAllRecordsByIdOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IGetAllRecordsById<TId> :
        ISyncAndAsyncReturningProtocol<GetAllRecordsByIdOp<TId>, IReadOnlyList<StreamRecordWithId<TId>>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetAllRecordsMetadataByIdOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IGetAllRecordsMetadataById<TId> :
        ISyncAndAsyncReturningProtocol<GetAllRecordsMetadataByIdOp<TId>, IReadOnlyList<StreamRecordMetadata<TId>>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetDistinctIdsOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IGetDistinctIds<TId> :
        ISyncAndAsyncReturningProtocol<GetDistinctIdsOp<TId>, IReadOnlyCollection<TId>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestRecordByIdOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IGetLatestRecordById<TId> :
        ISyncAndAsyncReturningProtocol<GetLatestRecordByIdOp<TId>, StreamRecordWithId<TId>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestRecordMetadataByIdOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IGetLatestRecordMetadataById<TId> :
        ISyncAndAsyncReturningProtocol<GetLatestRecordMetadataByIdOp<TId>, StreamRecordMetadata<TId>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestStringSerializedObjectByIdOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IGetLatestStringSerializedObjectById<TId> :
        ISyncAndAsyncReturningProtocol<GetLatestStringSerializedObjectByIdOp<TId>, string>
    {
    }
}
