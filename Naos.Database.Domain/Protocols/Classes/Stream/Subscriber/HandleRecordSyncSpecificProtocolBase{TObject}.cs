// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordSyncSpecificProtocolBase{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class implementation of an <see cref="ISyncAndAsyncVoidProtocol{TOperation}"/> for <see cref="HandleRecordOp{TObject}"/> for <typeparamref name="TObject"/>.
    /// </summary>
    /// <typeparam name="TObject">Type of the object in the record.</typeparam>
    public abstract class HandleRecordSyncSpecificProtocolBase<TObject> : SyncSpecificVoidProtocolBase<HandleRecordOp<TObject>>
    {
    }
}
