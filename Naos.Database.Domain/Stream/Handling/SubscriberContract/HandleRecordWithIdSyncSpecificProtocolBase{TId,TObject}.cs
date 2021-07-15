// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordWithIdSyncSpecificProtocolBase{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;


    /// <summary>
    /// Base class which will implement <see cref="ISyncAndAsyncVoidProtocol{TOperation}"/> for <see cref="HandleRecordWithIdOp{TId, TObject}"/> for <typeparamref name="TObject"/>.
    /// Only the Synchronous execution implementation is required, thd Asynchronous will wrap from base class.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the record to handle.</typeparam>
    /// <typeparam name="TObject">The type of the object to handle.</typeparam>
    public abstract class HandleRecordWithIdSyncSpecificProtocolBase<TId, TObject> : SyncSpecificVoidProtocolBase<HandleRecordWithIdOp<TId, TObject>>
    {
    }
}
