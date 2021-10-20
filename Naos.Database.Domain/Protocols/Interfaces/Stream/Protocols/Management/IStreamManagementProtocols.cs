// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamManagementProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the stream management operations.
    /// </summary>
    public interface IStreamManagementProtocols
        : ISyncAndAsyncReturningProtocol<CreateStreamOp, CreateStreamResult>,
          ISyncAndAsyncVoidProtocol<DeleteStreamOp>,
          ISyncAndAsyncVoidProtocol<PruneBeforeInternalRecordDateOp>,
          ISyncAndAsyncVoidProtocol<PruneBeforeInternalRecordIdOp>
    {
    }
}
