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
        : ISyncAndAsyncReturningProtocol<GetHandlingHistoryOp, IReadOnlyList<StreamRecordHandlingEntry>>,
          ISyncAndAsyncReturningProtocol<GetHandlingStatusOp, HandlingStatus>,
          ISyncAndAsyncReturningProtocol<GetCompositeHandlingStatusByIdsOp, CompositeHandlingStatus>,
          ISyncAndAsyncReturningProtocol<GetCompositeHandlingStatusByTagsOp, CompositeHandlingStatus>,
          ISyncAndAsyncVoidProtocol<DisableHandlingForStreamOp>,
          ISyncAndAsyncVoidProtocol<EnableHandlingForStreamOp>,
          ISyncAndAsyncVoidProtocol<DisableHandlingForRecordOp>,
          ISyncAndAsyncVoidProtocol<CancelRunningHandleRecordOp>,
          ISyncAndAsyncVoidProtocol<CompleteRunningHandleRecordOp>,
          ISyncAndAsyncVoidProtocol<FailRunningHandleRecordOp>,
          ISyncAndAsyncVoidProtocol<ResetFailedHandleRecordOp>,
          ISyncAndAsyncVoidProtocol<SelfCancelRunningHandleRecordOp>
    {
    }
}
