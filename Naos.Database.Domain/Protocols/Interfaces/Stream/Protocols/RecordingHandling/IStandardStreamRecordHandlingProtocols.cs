// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardStreamHandleProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the standard record handling operations.
    /// </summary>
    public interface IStandardStreamRecordHandlingProtocols
        :
            ISyncReturningProtocol<GetHandlingHistoryOfRecordOp, IReadOnlyList<StreamRecordHandlingEntry>>,
            ISyncReturningProtocol<GetHandlingStatusOfRecordByInternalRecordIdOp, HandlingStatus>,
            ISyncReturningProtocol<GetHandlingStatusOfRecordsByIdOp, HandlingStatus>,
            ISyncReturningProtocol<GetHandlingStatusOfRecordSetByTagOp, HandlingStatus>,
            ISyncReturningProtocol<TryHandleRecordOp, TryHandleRecordResult>,
            ISyncVoidProtocol<BlockRecordHandlingOp>,
            ISyncVoidProtocol<CancelBlockedRecordHandlingOp>,
            ISyncVoidProtocol<CancelHandleRecordExecutionRequestOp>,
            ISyncVoidProtocol<CancelRunningHandleRecordExecutionOp>,
            ISyncVoidProtocol<CompleteRunningHandleRecordExecutionOp>,
            ISyncVoidProtocol<FailRunningHandleRecordExecutionOp>,
            ISyncVoidProtocol<SelfCancelRunningHandleRecordExecutionOp>,
            ISyncVoidProtocol<RetryFailedHandleRecordExecutionOp>
    {
    }
}
