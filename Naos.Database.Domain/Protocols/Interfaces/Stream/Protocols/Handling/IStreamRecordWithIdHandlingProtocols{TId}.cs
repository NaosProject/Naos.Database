// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordWithIdHandlingProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the stream record handling operations with a typed identifier and without a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IStreamRecordWithIdHandlingProtocols<TId> :
        ITryHandleRecordWithId<TId>,
        IGetCompositeHandlingStatusByIds<TId>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="TryHandleRecordWithIdOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface ITryHandleRecordWithId<TId> :
        ISyncAndAsyncReturningProtocol<TryHandleRecordWithIdOp<TId>, StreamRecordWithId<TId>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetCompositeHandlingStatusByIdsOp{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public interface IGetCompositeHandlingStatusByIds<TId> :
        ISyncAndAsyncReturningProtocol<GetCompositeHandlingStatusByIdsOp<TId>, CompositeHandlingStatus>
    {
    }
}
