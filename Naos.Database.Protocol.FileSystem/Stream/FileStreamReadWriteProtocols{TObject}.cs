// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System.Threading.Tasks;
    using Naos.Database.Domain;

    /// <summary>
    /// File system implementation of <see cref="IStreamReadProtocols{TObject}"/>
    /// and <see cref="IStreamWriteProtocols{TObject}"/>.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class FileStreamReadWriteProtocols<TObject>
        : IStreamReadProtocols<TObject>,
          IStreamWriteProtocols<TObject>
    {
        private readonly FileStreamReadWriteWithIdProtocols<NullStreamIdentifier, TObject> delegatedProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamReadWriteProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="readWriteStream">The stream.</param>
        public FileStreamReadWriteProtocols(
            FileReadWriteStream readWriteStream)
        {
            this.delegatedProtocols = new FileStreamReadWriteWithIdProtocols<NullStreamIdentifier, TObject>(readWriteStream);
        }

        /// <inheritdoc />
        public long Execute(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var delegatedOperation = new PutWithIdAndReturnInternalRecordIdOp<NullStreamIdentifier, TObject>(
                new NullStreamIdentifier(),
                operation.ObjectToPut,
                operation.Tags);
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
        public void Execute(
            PutOp<TObject> operation)
        {
            var delegatedOperation = new PutAndReturnInternalRecordIdOp<TObject>(
                operation.ObjectToPut,
                operation.Tags);
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
