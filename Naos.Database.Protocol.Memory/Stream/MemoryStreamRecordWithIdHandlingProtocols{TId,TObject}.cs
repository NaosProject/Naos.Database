// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamRecordWithIdHandlingProtocols{TId,TObject}.cs" company="Naos Project">
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
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <seealso cref="IStreamReadWithIdProtocols{TId,TObject}" />
    /// <seealso cref="IStreamWriteWithIdProtocols{TId,TObject}" />
    public partial class MemoryStreamRecordWithIdHandlingProtocols<TId, TObject> :
        IStreamRecordWithIdHandlingProtocols<TId, TObject>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
        private readonly MemoryReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamRecordWithIdHandlingProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamRecordWithIdHandlingProtocols(
            MemoryReadWriteStream stream)
        {
            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
