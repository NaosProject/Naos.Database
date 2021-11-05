// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream reading operations with a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IStreamReadWithIdProtocols<TId, TObject> :
        ISyncAndAsyncReturningProtocol<GetLatestObjectByIdOp<TId, TObject>, TObject>,
        ISyncAndAsyncReturningProtocol<GetLatestRecordByIdOp<TId, TObject>, StreamRecordWithId<TId, TObject>>
    {
    }
}
