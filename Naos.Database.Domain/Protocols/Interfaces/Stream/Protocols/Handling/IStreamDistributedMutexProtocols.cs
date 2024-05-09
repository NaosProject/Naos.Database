// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamDistributedMutexProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the use of a stream to manage a distributed mutex.
    /// </summary>
    public interface IStreamDistributedMutexProtocols
        : IWaitOne,
          IReleaseMutex
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="WaitOneOp" />.
    /// </summary>
    public interface IWaitOne :
        ISyncAndAsyncReturningProtocol<WaitOneOp, ReleaseMutexOp>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="ReleaseMutexOp" />.
    /// </summary>
    public interface IReleaseMutex :
        ISyncAndAsyncVoidProtocol<ReleaseMutexOp>
    {
    }
}
