// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardStreamRecordHandlingProtocols.cs" company="Naos Project">
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
            ISyncReturningProtocol<StandardTryHandleRecordOp, TryHandleRecordResult>,
            ISyncReturningProtocol<StandardGetHandlingStatusOp, IReadOnlyDictionary<long, HandlingStatus>>,
            ISyncReturningProtocol<StandardGetHandlingHistoryOp, IReadOnlyList<StreamRecordHandlingEntry>>,
            ISyncVoidProtocol<StandardUpdateHandlingStatusForStreamOp>,
            ISyncVoidProtocol<StandardUpdateHandlingStatusForRecordOp>
    {
    }
}
