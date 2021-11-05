// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordWithIdAsyncSpecificProtocolBase{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class implementation of an <see cref="ISyncAndAsyncVoidProtocol{TOperation}"/> for <see cref="HandleRecordWithIdOp{TId, TObject}"/> for <typeparamref name="TObject"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public abstract class HandleRecordWithIdAsyncSpecificProtocolBase<TId, TObject> : AsyncSpecificVoidProtocolBase<HandleRecordWithIdOp<TId, TObject>>
    {
    }
}
