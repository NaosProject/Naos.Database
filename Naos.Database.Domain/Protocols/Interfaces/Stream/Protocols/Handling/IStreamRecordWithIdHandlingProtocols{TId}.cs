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
    public interface IStreamRecordWithIdHandlingProtocols<TId>
        : ISyncAndAsyncReturningProtocol<TryHandleRecordWithIdOp<TId>, StreamRecordWithId<TId>>,
          ISyncAndAsyncReturningProtocol<GetCompositeHandlingStatusByIdsOp<TId>, CompositeHandlingStatus>
    {
    }
}
