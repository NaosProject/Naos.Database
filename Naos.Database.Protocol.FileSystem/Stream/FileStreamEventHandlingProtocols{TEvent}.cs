// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamEventHandlingProtocols{TEvent}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IStreamEventHandlingProtocols{TEvent}"/>.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event to handle.</typeparam>
    public class FileStreamEventHandlingProtocols<TEvent>
        : IStreamEventHandlingProtocols<TEvent>
        where TEvent : IEvent
    {
        private readonly FileReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamEventHandlingProtocols{TEvent}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamEventHandlingProtocols(FileReadWriteStream stream)
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
            throw new NotImplementedException();

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
