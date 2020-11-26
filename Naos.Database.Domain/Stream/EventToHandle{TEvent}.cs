// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventToHandle{TEvent}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Model containing the event to handle as well as any additional relevant information.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    public partial class EventToHandle<TEvent> : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventToHandle{TEvent}"/> class.
        /// </summary>
        /// <param name="event">The event.</param>
        public EventToHandle(
            TEvent @event)
        {
            @event.MustForArg(nameof(@event)).NotBeNull();
            this.Event = @event;
        }

        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <value>The event.</value>
        public TEvent Event { get; private set; }
    }
}
