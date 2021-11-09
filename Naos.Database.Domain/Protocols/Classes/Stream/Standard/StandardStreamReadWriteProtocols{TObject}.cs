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
    /// Set of protocols to execute read and write operations on a stream,
    /// without a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class StandardStreamReadWriteProtocols<TObject> :
        IStreamReadProtocols<TObject>,
        IStreamWriteProtocols<TObject>
    {
        private readonly IStandardStream stream;

        private readonly StandardStreamReadWriteWithIdProtocols<NullIdentifier, TObject> delegatedWithIdProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamReadWriteProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamReadWriteProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.delegatedWithIdProtocols = new StandardStreamReadWriteWithIdProtocols<NullIdentifier, TObject>(stream);
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOp = new PutAndReturnInternalRecordIdOp<TObject>(
                operation.ObjectToPut,
                operation.Tags,
                operation.ExistingRecordStrategy,
                operation.RecordRetentionCount,
                operation.VersionMatchStrategy);

            this.Execute(delegatedOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TObject> operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public long? Execute(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOp = new PutWithIdAndReturnInternalRecordIdOp<NullIdentifier, TObject>(
                null,
                operation.ObjectToPut,
                operation.Tags,
                operation.ExistingRecordStrategy,
                operation.RecordRetentionCount,
                operation.VersionMatchStrategy);

            var result = this.delegatedWithIdProtocols.Execute(delegatedOp);

            return result;
        }

        /// <inheritdoc />
        public async Task<long?> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectOp<TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var record = this.stream.Execute(standardOp);

            // ReSharper disable once ArrangeDefaultValueWhenTypeNotEvident
            var result = record == null
                ? default(TObject)
                : record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

            return result;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectOp<TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public StreamRecord<TObject> Execute(
            GetLatestRecordOp<TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var record = this.stream.Execute(standardOp);

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
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectByTagsOp<TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var record = this.stream.Execute(standardOp);

            // ReSharper disable once ArrangeDefaultValueWhenTypeNotEvident
            var result = record == null
                ? default(TObject)
                : record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

            return result;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectByTagsOp<TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
