// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to protocol the basic stream data operations with a known identifier.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    /// <typeparam name="TObject">Type of the object.</typeparam>
    public interface IStreamWriteWithIdProtocols<TId, TObject> :
        ISyncAndAsyncReturningProtocol<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>, long?>,
        ISyncAndAsyncVoidProtocol<PutWithIdOp<TId, TObject>>
    {
    }
}
