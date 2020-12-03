// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamRecordWithIdHandlingProtocols{TId}.cs" company="Naos Project">
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
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <seealso cref="IStreamReadWithIdProtocols{TId}" />
    /// <seealso cref="IStreamWriteWithIdProtocols{TId}" />
    public partial class MemoryStreamRecordWithIdHandlingProtocols<TId> :
        IStreamRecordWithIdHandlingProtocols<TId>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
        private readonly MemoryReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamRecordWithIdHandlingProtocols{TEvent}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamRecordWithIdHandlingProtocols(
            MemoryReadWriteStream stream)
        {
            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            TryHandleRecordWithIdOp<TId> operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId>> ExecuteAsync(
            TryHandleRecordWithIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
