﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;


    /// <summary>
    /// Interface to protocol the basic stream data operations without a known identifier.
    /// </summary>
    /// <typeparam name="TObject">Type of the object.</typeparam>
    public interface IStreamWriteProtocols<TObject> :
        ISyncAndAsyncReturningProtocol<PutAndReturnInternalRecordIdOp<TObject>, long?>,
        ISyncAndAsyncVoidProtocol<PutOp<TObject>>
    {
    }
}
