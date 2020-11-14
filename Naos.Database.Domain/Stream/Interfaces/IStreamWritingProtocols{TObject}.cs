// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWritingProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to protocol the basic stream data operations without a known identifier.
    /// </summary>
    /// <typeparam name="TObject">Type of object used.</typeparam>
    public interface IStreamWritingProtocols<TObject> :
        ISyncAndAsyncReturningProtocol<PutAndReturnInternalRecordIdOp<TObject>, long>,
        ISyncAndAsyncVoidProtocol<PutOp<TObject>>
    {
    }
}
