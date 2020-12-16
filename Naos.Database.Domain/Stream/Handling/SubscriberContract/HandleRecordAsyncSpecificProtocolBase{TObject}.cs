// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordAsyncSpecificProtocolBase{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Base class which will implement <see cref="ISyncAndAsyncVoidProtocol{TOperation}"/> for <see cref="HandleRecordOp{TObject}"/> for <typeparamref name="TObject"/>.
    /// Only the Asynchronous execution implementation is required, thd Synchronous will wrap it from base class.
    /// </summary>
    /// <typeparam name="TObject">The type of the object to handle.</typeparam>
    public abstract class HandleRecordAsyncSpecificProtocolBase<TObject> : AsyncSpecificVoidProtocolBase<HandleRecordOp<TObject>>
    {
    }
}
