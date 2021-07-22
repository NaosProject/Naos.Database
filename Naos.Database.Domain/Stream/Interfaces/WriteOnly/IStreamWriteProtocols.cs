// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream data operations without a known identifier.
    /// </summary>
    public interface IStreamWriteProtocols :
        ISyncAndAsyncReturningProtocol<GetNextUniqueLongOp, long>,
        ISyncAndAsyncReturningProtocol<PutRecordOp, PutRecordResult>
    {
    }
}
