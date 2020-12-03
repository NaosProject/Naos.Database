// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordWithIdHandlingProtocols{TId,TObject}.cs" company="Naos Project">
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
    /// <typeparam name="TObject">The type of object in the record.</typeparam>
    public interface IStreamRecordWithIdHandlingProtocols<TId, TObject>
        : ISyncAndAsyncReturningProtocol<TryHandleRecordWithIdOp<TId, TObject>, StreamRecordWithId<TId, TObject>>
    {
    }
}
