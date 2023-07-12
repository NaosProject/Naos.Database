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
        IGetNextUniqueLong
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetNextUniqueLongOp" />.
    /// </summary>
    public interface IGetNextUniqueLong : ISyncAndAsyncReturningProtocol<GetNextUniqueLongOp, long>
    {
    }
}
