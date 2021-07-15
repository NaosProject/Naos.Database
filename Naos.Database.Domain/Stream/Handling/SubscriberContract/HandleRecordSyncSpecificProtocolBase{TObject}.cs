// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordSyncSpecificProtocolBase{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;


    /// <summary>
    /// Base class which will implement <see cref="ISyncAndAsyncVoidProtocol{TOperation}"/> for <see cref="HandleRecordOp{TObject}"/> for <typeparamref name="TObject"/>.
    /// Only the Synchronous execution implementation is required, thd Asynchronous will wrap it from base class.
    /// </summary>
    /// <typeparam name="TObject">The type of the object to handle.</typeparam>
    public abstract class HandleRecordSyncSpecificProtocolBase<TObject> : SyncSpecificVoidProtocolBase<HandleRecordOp<TObject>>
    {
    }
}
