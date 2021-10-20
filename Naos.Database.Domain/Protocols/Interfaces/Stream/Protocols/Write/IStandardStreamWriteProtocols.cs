// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardStreamWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the standard stream writing operations.
    /// </summary>
    public interface IStandardStreamWriteProtocols
        :
            ISyncReturningProtocol<StandardPutRecordOp, PutRecordResult>,
            ISyncReturningProtocol<StandardGetNextUniqueLongOp, long>
    {
    }
}
