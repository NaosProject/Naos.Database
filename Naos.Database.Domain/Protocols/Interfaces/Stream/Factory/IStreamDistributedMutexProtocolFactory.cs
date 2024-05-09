// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamDistributedMutexProtocolFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Interface to get the protocols to use a stream to manage a distributed mutex.
    /// </summary>
    public interface IStreamDistributedMutexProtocolFactory
    {
        /// <summary>
        /// Builds the <see cref="IStreamDistributedMutexProtocols"/> protocol.
        /// </summary>
        /// <returns>Protocols for using a stream to manage a distributed mutex.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Prefer method.")]
        IStreamDistributedMutexProtocols GetStreamDistributedMutexProtocols();
    }
}
