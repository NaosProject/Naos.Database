// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Set of protocols:
    /// Implements the <see cref="IStreamReadProtocols{TObject}" />
    /// Implements the <see cref="IStreamWriteProtocols{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the t object.</typeparam>
    /// <seealso cref="IStreamReadProtocols{TObject}" />
    /// <seealso cref="IStreamWriteProtocols{TObject}" />
    public class StandardStreamReadWriteProtocols<TObject> :
        IStreamReadProtocols<TObject>,
        IStreamWriteProtocols<TObject>
    {
        private readonly IStandardStream stream;
        private readonly StandardStreamReadWriteWithIdProtocols<NullStreamIdentifier, TObject> delegatedWithIdProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamReadWriteProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamReadWriteProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.delegatedWithIdProtocols = new StandardStreamReadWriteWithIdProtocols<NullStreamIdentifier, TObject>(stream);
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {
            var delegatedOperation = new PutAndReturnInternalRecordIdOp<TObject>(operation.ObjectToPut, operation.Tags, operation.ExistingRecordStrategy, operation.RecordRetentionCount, operation.VersionMatchStrategy);
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
        public long? Execute(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var delegatedOperation = new PutWithIdAndReturnInternalRecordIdOp<NullStreamIdentifier, TObject>(null, operation.ObjectToPut, operation.Tags, operation.ExistingRecordStrategy, operation.RecordRetentionCount, operation.VersionMatchStrategy);
            var result = this.delegatedWithIdProtocols.Execute(delegatedOperation);
            return result;
        }

        /// <inheritdoc />
        public async Task<long?> ExecuteAsync(
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
            var standardizedOperation = operation.Standardize();
            var record = this.stream.Execute(standardizedOperation);

            var result = record == null
                ? default(TObject)
                : record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

            return result;
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
            var standardizedOperation = operation.Standardize();
            var record = this.stream.Execute(standardizedOperation);

            if (record == null)
            {
                return null;
            }

            var payload = record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);
            var result = new StreamRecord<TObject>(record.InternalRecordId, record.Metadata, payload);
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecord<TObject>> ExecuteAsync(
            GetLatestRecordOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectByTagOp<TObject> operation)
        {
            var standardizedOperation = operation.Standardize();
            var record = this.stream.Execute(standardizedOperation);

            var result = record == null
                ? default(TObject)
                : record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

            return result;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectByTagOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
