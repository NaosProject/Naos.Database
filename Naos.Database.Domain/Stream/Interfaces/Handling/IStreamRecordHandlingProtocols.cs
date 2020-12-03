// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordHandlingProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Protocols to handle records from streams.
    /// </summary>
    public interface IStreamRecordHandlingProtocols
        : ISyncAndAsyncReturningProtocol<TryHandleRecordOp, StreamRecord>
    {
    }
}
