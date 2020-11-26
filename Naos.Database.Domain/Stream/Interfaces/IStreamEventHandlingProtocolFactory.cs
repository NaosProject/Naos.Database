// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamEventHandlingProtocolFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Interface to get the protocols for the basic event handling operations.
    /// </summary>
    public interface IStreamEventHandlingProtocolFactory
    {
        /// <summary>
        /// Gets the stream event handling protocols.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <returns>Protocol to handle events.</returns>
        IStreamEventHandlingProtocols<TEvent> GetStreamEventHandlingProtocols<TEvent>()
            where TEvent : IEvent;
    }
}
