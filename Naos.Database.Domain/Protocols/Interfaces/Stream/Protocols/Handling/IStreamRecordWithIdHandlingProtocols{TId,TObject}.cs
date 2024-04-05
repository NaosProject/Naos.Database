// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordWithIdHandlingProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the stream record handling operations with a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IStreamRecordWithIdHandlingProtocols<TId, TObject>
        : ITryHandleRecordWithId<TId, TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="TryHandleRecordWithIdOp{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface ITryHandleRecordWithId<TId, TObject> :
        ISyncAndAsyncReturningProtocol<TryHandleRecordWithIdOp<TId, TObject>, StreamRecordWithId<TId, TObject>>
    {
    }
}
