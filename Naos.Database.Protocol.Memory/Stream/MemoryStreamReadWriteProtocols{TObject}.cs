// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System.Threading.Tasks;
    using Naos.Database.Domain;

    /// <summary>
    /// Set of protocols:
    /// Implements the <see cref="IStreamReadProtocols{TObject}" />
    /// Implements the <see cref="IStreamWriteProtocols{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the t object.</typeparam>
    /// <seealso cref="IStreamReadProtocols{TObject}" />
    /// <seealso cref="IStreamWriteProtocols{TObject}" />
    public partial class MemoryStreamReadWriteProtocols<TObject> :
        IStreamReadProtocols<TObject>,
        IStreamWriteProtocols<TObject>
    {
        private readonly MemoryStreamReadWriteWithIdProtocols<NullStreamIdentifier, TObject> delegatedProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamReadWriteProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="readWriteStream">The stream.</param>
        public MemoryStreamReadWriteProtocols(
            MemoryReadWriteStream readWriteStream)
        {
            this.delegatedProtocols = new MemoryStreamReadWriteWithIdProtocols<NullStreamIdentifier, TObject>(readWriteStream);
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {
            var delegatedOperation = new PutAndReturnInternalRecordIdOp<TObject>(operation.ObjectToPut, operation.Tags);
            this.Execute(delegatedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TObject> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }

        /// <inheritdoc />
        public long Execute(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var delegatedOperation = new PutWithIdAndReturnInternalRecordIdOp<NullStreamIdentifier, TObject>(null, operation.ObjectToPut, operation.Tags);
            var result = this.delegatedProtocols.Execute(delegatedOperation);
            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectOp<TObject> operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public StreamRecord<TObject> Execute(
            GetLatestRecordOp<TObject> operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<StreamRecord<TObject>> ExecuteAsync(
            GetLatestRecordOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
