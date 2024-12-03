// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordHandlingProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the stream record handling operations without a typed identifier and without a typed record payload.
    /// </summary>
    public interface IStreamRecordHandlingProtocols :
        IGetHandlingHistory,
        IGetHandlingStatus,
        IGetCompositeHandlingStatusByIds,
        IGetCompositeHandlingStatusByTags,
        IDisableHandlingForStream,
        IEnableHandlingForStream,
        IDisableHandlingForRecord,
        ICancelRunningHandleRecord,
        ICompleteRunningHandleRecord,
        IFailRunningHandleRecord,
        IArchiveFailureToHandleRecord,
        IResetFailedHandleRecord,
        IResetCompletedHandleRecord,
        ISelfCancelRunningHandleRecord
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetHandlingHistoryOp" />.
    /// </summary>
    public interface IGetHandlingHistory :
        ISyncAndAsyncReturningProtocol<GetHandlingHistoryOp, IReadOnlyList<StreamRecordHandlingEntry>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetHandlingStatusOp" />.
    /// </summary>
    public interface IGetHandlingStatus :
        ISyncAndAsyncReturningProtocol<GetHandlingStatusOp, HandlingStatus>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetCompositeHandlingStatusByIdsOp" />.
    /// </summary>
    public interface IGetCompositeHandlingStatusByIds :
        ISyncAndAsyncReturningProtocol<GetCompositeHandlingStatusByIdsOp, CompositeHandlingStatus>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetCompositeHandlingStatusByTagsOp" />.
    /// </summary>
    public interface IGetCompositeHandlingStatusByTags :
        ISyncAndAsyncReturningProtocol<GetCompositeHandlingStatusByTagsOp, CompositeHandlingStatus>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="DisableHandlingForStreamOp" />.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Correct suffix for the domain.")]
    public interface IDisableHandlingForStream :
        ISyncAndAsyncVoidProtocol<DisableHandlingForStreamOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="EnableHandlingForStreamOp" />.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Correct suffix for the domain.")]
    public interface IEnableHandlingForStream :
        ISyncAndAsyncVoidProtocol<EnableHandlingForStreamOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="IDisableHandlingForRecord" />.
    /// </summary>
    public interface IDisableHandlingForRecord :
        ISyncAndAsyncVoidProtocol<DisableHandlingForRecordOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="CancelRunningHandleRecordOp" />.
    /// </summary>
    public interface ICancelRunningHandleRecord :
        ISyncAndAsyncVoidProtocol<CancelRunningHandleRecordOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="CompleteRunningHandleRecordOp" />.
    /// </summary>
    public interface ICompleteRunningHandleRecord :
        ISyncAndAsyncVoidProtocol<CompleteRunningHandleRecordOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="FailRunningHandleRecordOp" />.
    /// </summary>
    public interface IFailRunningHandleRecord :
        ISyncAndAsyncVoidProtocol<FailRunningHandleRecordOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="ArchiveFailureToHandleRecordOp" />.
    /// </summary>
    public interface IArchiveFailureToHandleRecord :
        ISyncAndAsyncVoidProtocol<ArchiveFailureToHandleRecordOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="ResetFailedHandleRecordOp" />.
    /// </summary>
    public interface IResetFailedHandleRecord :
        ISyncAndAsyncVoidProtocol<ResetFailedHandleRecordOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="ResetCompletedHandleRecordOp" />.
    /// </summary>
    public interface IResetCompletedHandleRecord :
        ISyncAndAsyncVoidProtocol<ResetCompletedHandleRecordOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="SelfCancelRunningHandleRecordOp" />.
    /// </summary>
    public interface ISelfCancelRunningHandleRecord :
        ISyncAndAsyncVoidProtocol<SelfCancelRunningHandleRecordOp>
    {
    }
}
