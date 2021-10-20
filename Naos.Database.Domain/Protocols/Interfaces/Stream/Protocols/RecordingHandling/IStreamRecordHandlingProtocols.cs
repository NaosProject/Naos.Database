// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordHandlingProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the stream record handling operations without a typed identifier and without a typed record payload.
    /// </summary>
    public interface IStreamRecordHandlingProtocols
        : ISyncAndAsyncReturningProtocol<TryHandleRecordOp, TryHandleRecordResult>,
          ISyncAndAsyncReturningProtocol<GetHandlingHistoryOfRecordOp, IReadOnlyList<StreamRecordHandlingEntry>>,
          ISyncAndAsyncReturningProtocol<GetHandlingStatusOfRecordByInternalRecordIdOp, HandlingStatus>,
          ISyncAndAsyncReturningProtocol<GetHandlingStatusOfRecordsByIdOp, HandlingStatus>,
          ISyncAndAsyncReturningProtocol<GetHandlingStatusOfRecordSetByTagOp, HandlingStatus>,
          ISyncAndAsyncVoidProtocol<BlockRecordHandlingOp>,
          ISyncAndAsyncVoidProtocol<CancelBlockedRecordHandlingOp>,
          ISyncAndAsyncVoidProtocol<CancelHandleRecordExecutionRequestOp>,
          ISyncAndAsyncVoidProtocol<CancelRunningHandleRecordExecutionOp>,
          ISyncAndAsyncVoidProtocol<CompleteRunningHandleRecordExecutionOp>,
          ISyncAndAsyncVoidProtocol<FailRunningHandleRecordExecutionOp>,
          ISyncAndAsyncVoidProtocol<SelfCancelRunningHandleRecordExecutionOp>,
          ISyncAndAsyncVoidProtocol<RetryFailedHandleRecordExecutionOp>
    {
    }
}
