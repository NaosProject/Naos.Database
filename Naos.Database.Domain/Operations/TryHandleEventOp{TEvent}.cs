// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleEventOp{TEvent}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Attempts to put a <see cref="HandlingEventEvent{TId,TEvent}"/>.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    public partial class TryHandleEventOp<TEvent> : ReturningOperationBase<TEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleEventOp{TEvent}"/> class.
        /// </summary>
        /// <param name="details">The details about the handler.</param>
        public TryHandleEventOp(
            string details)
        {
            this.Details = details;
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }
    }
}
