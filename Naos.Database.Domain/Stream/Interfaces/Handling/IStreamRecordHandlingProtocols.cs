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
    /// Protocols to handle records from streams.
    /// </summary>
    public interface IStreamRecordHandlingProtocols
        : ISyncAndAsyncReturningProtocol<TryHandleRecordOp, TryHandleRecordResult>,
          ISyncAndAsyncReturningProtocol<GetHandlingHistoryOfRecordOp, IReadOnlyList<StreamRecordHandlingEntry>>,
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
