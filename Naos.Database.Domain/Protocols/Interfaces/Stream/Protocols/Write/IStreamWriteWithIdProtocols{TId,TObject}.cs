// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream writing operations with a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IStreamWriteWithIdProtocols<TId, TObject> :
        IPutWithIdAndReturnInternalRecordId<TId, TObject>,
        IPutWithId<TId, TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="PutWithIdAndReturnInternalRecordIdOp{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IPutWithIdAndReturnInternalRecordId<TId, TObject> :
        ISyncAndAsyncReturningProtocol<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>, long?>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="PutWithIdOp{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IPutWithId<TId, TObject> :
        ISyncAndAsyncVoidProtocol<PutWithIdOp<TId, TObject>>
    {
    }
}
