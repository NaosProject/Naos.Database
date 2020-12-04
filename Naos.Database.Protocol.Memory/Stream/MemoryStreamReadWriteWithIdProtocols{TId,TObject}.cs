﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamReadWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Set of protocols to work with known identifier and/or object type.
    /// Implements the <see cref="IStreamReadWithIdProtocols{TId,TObject}" />
    /// Implements the <see cref="IStreamWriteWithIdProtocols{TId,TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the t identifier.</typeparam>
    /// <typeparam name="TObject">The type of the t object.</typeparam>
    /// <seealso cref="IStreamReadWithIdProtocols{TId,TObject}" />
    /// <seealso cref="IStreamWriteWithIdProtocols{TId,TObject}" />
    public partial class MemoryStreamReadWriteWithIdProtocols<TId, TObject> :
        IStreamReadWithIdProtocols<TId, TObject>,
        IStreamWriteWithIdProtocols<TId, TObject>
    {
        private readonly MemoryReadWriteStream stream;
        private readonly MemoryStreamReadWriteProtocols delegatedProtocols;
        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> locatorProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamReadWriteWithIdProtocols{TId,TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamReadWriteWithIdProtocols(
            MemoryReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();
            this.stream = stream;
            this.delegatedProtocols = new MemoryStreamReadWriteProtocols(stream);
            this.locatorProtocol = this.stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var serializedObjectId = serializer.SerializeToString(operation.Id);
            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var delegatedOperation = new GetLatestRecordByIdOp(
                locator,
                serializedObjectId,
                typeof(TId).ToRepresentation().ToWithAndWithoutVersion(),
                typeof(TObject).ToRepresentation().ToWithAndWithoutVersion(),
                operation.TypeVersionMatchStrategy);

            var record = this.delegatedProtocols.Execute(delegatedOperation);
            var result = record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);
            return result;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            PutWithIdOp<TId, TObject> operation)
        {
            var delegatedOperation = new PutWithIdAndReturnInternalRecordIdOp<TId, TObject>(operation.Id, operation.ObjectToPut, operation.Tags);
            this.Execute(delegatedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutWithIdOp<TId, TObject> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }

        /// <inheritdoc />
        public long Execute(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            string serializedStringId = serializer.SerializeToString(operation.Id);

            var identifierTypeRep = (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation();
            var objectTypeRep = (operation.ObjectToPut?.GetType() ?? typeof(TObject)).ToRepresentation();

            var describedSerialization = operation.ObjectToPut.ToDescribedSerializationUsingSpecificFactory(
                this.stream.DefaultSerializerRepresentation,
                this.stream.SerializerFactory,
                this.stream.DefaultSerializationFormat);

            var objectTimestamp = operation.ObjectToPut is IHaveTimestampUtc objectWithTimestamp
                ? objectWithTimestamp.TimestampUtc
                : (DateTime?)null;

            var metadata = new StreamRecordMetadata(
                serializedStringId,
                this.stream.DefaultSerializerRepresentation,
                identifierTypeRep.ToWithAndWithoutVersion(),
                objectTypeRep.ToWithAndWithoutVersion(),
                operation.Tags ?? new Dictionary<string, string>(),
                DateTime.UtcNow,
                objectTimestamp);

            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));
            var result = this.stream.Execute(new PutRecordOp(locator, metadata, describedSerialization));

            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var serializedObjectId = serializer.SerializeToString(operation.Id);
            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var delegatedOperation = new GetLatestRecordByIdOp(
                locator,
                serializedObjectId,
                typeof(TId).ToRepresentation().ToWithAndWithoutVersion(),
                typeof(TObject).ToRepresentation().ToWithAndWithoutVersion(),
                operation.TypeVersionMatchStrategy);

            var record = this.delegatedProtocols.Execute(delegatedOperation);

            var metadata = new StreamRecordMetadata<TId>(
                operation.Id,
                record.Metadata.SerializerRepresentation,
                record.Metadata.TypeRepresentationOfId,
                record.Metadata.TypeRepresentationOfObject,
                record.Metadata.Tags,
                record.Metadata.TimestampUtc,
                record.Metadata.ObjectTimestampUtc);

            var payload = record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

            var result = new StreamRecordWithId<TId, TObject>(record.InternalRecordId, metadata, payload);
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
