// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream writing operations without a typed identifier and without a typed record payload.
    /// </summary>
    public interface IStreamWriteProtocols :
        ISyncAndAsyncReturningProtocol<GetNextUniqueLongOp, long>
    {
    }
}
