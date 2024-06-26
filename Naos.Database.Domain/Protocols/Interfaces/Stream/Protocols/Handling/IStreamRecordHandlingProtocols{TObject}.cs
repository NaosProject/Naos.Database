﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordHandlingProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the stream record handling operations without a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IStreamRecordHandlingProtocols<TObject>
        : ITryHandleRecord<TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="TryHandleRecordOp{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface ITryHandleRecord<TObject> :
        ISyncAndAsyncReturningProtocol<TryHandleRecordOp<TObject>, StreamRecord<TObject>>
    {
    }
}
