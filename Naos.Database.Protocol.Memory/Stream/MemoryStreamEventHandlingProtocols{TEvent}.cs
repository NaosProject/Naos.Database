// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamEventHandlingProtocols{TEvent}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Set of protocols to handle <see cref="IEvent"/>'s in a stream.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <seealso cref="IStreamReadProtocols{TObject}" />
    /// <seealso cref="IStreamWriteProtocols{TObject}" />
    /// <seealso cref="IStreamReadProtocols{TId,TObject}" />
    /// <seealso cref="IStreamWriteProtocols{TId,TObject}" />
    public partial class MemoryStreamEventHandlingProtocols<TEvent> :
        IStreamEventHandlingProtocols<TEvent>
        where TEvent : IEvent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
        private readonly MemoryReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamEventHandlingProtocols{TEvent}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamEventHandlingProtocols(
            MemoryReadWriteStream stream)
        {
            this.stream = stream;
        }

        /// <inheritdoc />
        public EventToHandle<TEvent> Execute(
            TryHandleEventOp<TEvent> operation)
        {
            // lock
            //      look for an existing 'Handled' or 'Handling'
            //      if none then add 'Handling'
            //                   return event
            //      else
            //                   return default
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<EventToHandle<TEvent>> ExecuteAsync(
            TryHandleEventOp<TEvent> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
