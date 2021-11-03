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
        : ISyncAndAsyncReturningProtocol<GetHandlingHistoryOfRecordOp, IReadOnlyList<StreamRecordHandlingEntry>>,
          ISyncAndAsyncReturningProtocol<GetHandlingStatusOfRecordByInternalRecordIdOp, HandlingStatus>,
          ISyncAndAsyncReturningProtocol<GetCompositeHandlingStatusOfRecordsByIdOp, CompositeHandlingStatus>,
          ISyncAndAsyncReturningProtocol<GetCompositeHandlingStatusOfRecordsByTagOp, CompositeHandlingStatus>,
          ISyncAndAsyncVoidProtocol<DisableRecordHandlingForStreamOp>,
          ISyncAndAsyncVoidProtocol<EnableRecordHandlingForStreamOp>,
          ISyncAndAsyncVoidProtocol<DisableRecordHandlingForRecordOp>,
          ISyncAndAsyncVoidProtocol<CancelRunningHandleRecordExecutionOp>,
          ISyncAndAsyncVoidProtocol<CompleteRunningHandleRecordExecutionOp>,
          ISyncAndAsyncVoidProtocol<FailRunningHandleRecordExecutionOp>,
          ISyncAndAsyncVoidProtocol<RetryFailedHandleRecordExecutionOp>,
          ISyncAndAsyncVoidProtocol<SelfCancelRunningHandleRecordExecutionOp>
    {
    }
}
