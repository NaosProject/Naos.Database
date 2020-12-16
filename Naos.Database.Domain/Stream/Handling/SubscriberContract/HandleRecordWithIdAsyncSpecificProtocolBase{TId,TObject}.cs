// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordWithIdAsyncSpecificProtocolBase{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Base class which will implement <see cref="ISyncAndAsyncVoidProtocol{TOperation}"/> for <see cref="HandleRecordWithIdOp{TId, TObject}"/> for <typeparamref name="TObject"/>.
    /// Only the Asynchronous execution implementation is required, thd Synchronous will wrap from base class.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the record to handle.</typeparam>
    /// <typeparam name="TObject">The type of the object to handle.</typeparam>
    public abstract class HandleRecordWithIdAsyncSpecificProtocolBase<TId, TObject> : AsyncSpecificVoidProtocolBase<HandleRecordWithIdOp<TId, TObject>>
    {
    }
}
