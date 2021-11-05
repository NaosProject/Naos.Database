// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordAsyncSpecificProtocolBase{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class implementation of an <see cref="ISyncAndAsyncVoidProtocol{TOperation}"/> for <see cref="HandleRecordOp{TObject}"/> for <typeparamref name="TObject"/>.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public abstract class HandleRecordAsyncSpecificProtocolBase<TObject> : AsyncSpecificVoidProtocolBase<HandleRecordOp<TObject>>
    {
    }
}
