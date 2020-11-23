// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamEventHandlingProtocols{TEvent}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Protocols to handle events from streams.
    /// </summary>
    /// <typeparam name="TEvent">The type of the t event.</typeparam>
    public interface IStreamEventHandlingProtocols<TEvent> : ISyncAndAsyncReturningProtocol<TryHandleEventOp<TEvent>, EventToHandle<TEvent>>
        where TEvent : IEvent
    {
    }
}
