﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream reading operations without a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TObject">Type of object used.</typeparam>
    public interface IStreamReadProtocols<TObject>
        : ISyncAndAsyncReturningProtocol<GetLatestObjectOp<TObject>, TObject>,
          ISyncAndAsyncReturningProtocol<GetLatestObjectByTagOp<TObject>, TObject>,
          ISyncAndAsyncReturningProtocol<GetLatestRecordOp<TObject>, StreamRecord<TObject>>
    {
    }
}