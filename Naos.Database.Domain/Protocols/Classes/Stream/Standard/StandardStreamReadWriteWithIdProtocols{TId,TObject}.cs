// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Set of protocols to execute read and write operations on a stream,
    /// with a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class StandardStreamReadWriteWithIdProtocols<TId, TObject> :
        IStreamReadWithIdProtocols<TId, TObject>,
        IStreamWriteWithIdProtocols<TId, TObject>
    {
        private readonly IStandardStream stream;

        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> locatorProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamReadWriteWithIdProtocols{TId,TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamReadWriteWithIdProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.locatorProtocol = this.stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
        }

        /// <inheritdoc />
        public void Execute(
            PutWithIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOp = new PutWithIdAndReturnInternalRecordIdOp<TId, TObject>(
                operation.Id,
                operation.ObjectToPut,
                operation.Tags,
                operation.ExistingRecordStrategy,
                operation.RecordRetentionCount,
                operation.VersionMatchStrategy,
                operation.TypeSelectionStrategy);

            this.Execute(delegatedOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutWithIdOp<TId, TObject> operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public long? Execute(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(this.stream, locator);

            var result = this.stream.Execute(standardOp);

            return result.InternalRecordIdOfPutRecord;
        }

        /// <inheritdoc />
        public async Task<long?> ExecuteAsync(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(serializer, locator);

            var record = this.stream.Execute(standardOp);

            if ((record == null) && (operation.RecordNotFoundStrategy != RecordNotFoundStrategy.ReturnDefault))
            {
                throw new NotSupportedException(Invariant($"record is null but {nameof(RecordNotFoundStrategy)} is not {nameof(RecordNotFoundStrategy.ReturnDefault)}"));
            }

            // ReSharper disable once ArrangeDefaultValueWhenTypeNotEvident
            var result = record == null
                ? default(TObject)
                : record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

            return result;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(serializer, locator);

            var record = this.stream.Execute(standardOp);

            if (record == null)
            {
                return null;
            }

            var metadata = record.Metadata.ToStreamRecordMetadata(operation.Id);

            var payload = record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

            var result = new StreamRecordWithId<TId, TObject>(record.InternalRecordId, metadata, payload);

            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public bool Execute(
            DoesAnyExistByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(serializer, locator);

            var result = this.stream.Execute(standardOp);

            return result.Any();
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp<TId, TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<TObject> Execute(
            GetAllObjectsByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOperation = new GetAllRecordsByIdOp<TId>(
                operation.Id,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                operation.OrderRecordsBy,
                operation.DeprecatedIdTypes,
                operation.TypeSelectionStrategy);

            var records = this.stream.GetStreamReadingWithIdProtocols<TId>().Execute(delegatedOperation);

            // records cannot contain a null element.
            // A record payload may be null, but it's the serializer's responsibility to deal with that.
            var result = records
                .Select(_ => _.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory))
                .ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetAllObjectsByIdOp<TId, TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
