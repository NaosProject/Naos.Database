// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream data operations with a known identifier.
    /// </summary>
    /// <typeparam name="TId">Type of ID used.</typeparam>
    /// <typeparam name="TObject">Type of object used.</typeparam>
    public interface IStreamReadWithIdProtocols<TId, TObject> :
        ISyncAndAsyncReturningProtocol<GetLatestObjectByIdOp<TId, TObject>, TObject>,
        ISyncAndAsyncReturningProtocol<GetLatestRecordByIdOp<TId, TObject>, StreamRecordWithId<TId, TObject>>
    {
    }
}
