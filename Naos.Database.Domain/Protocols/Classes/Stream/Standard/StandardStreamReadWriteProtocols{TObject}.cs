﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
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
                operation.VersionMatchStrategy,
                operation.TypeSelectionStrategy);

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
                operation.VersionMatchStrategy,
                operation.TypeSelectionStrategy);

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
                : record.GetDescribedSerialization().DeserializePayloadUsingSpecificFactory<TObject>(
                    this.stream.SerializerFactory);

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

            var payload = record.GetDescribedSerialization().DeserializePayloadUsingSpecificFactory<TObject>(
                this.stream.SerializerFactory);

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
        public IReadOnlyList<StreamRecord<TObject>> Execute(
            GetAllRecordsOp<TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var internalRecordIdsOp = new StandardGetInternalRecordIdsOp(
                new RecordFilter(
                    idTypes: (operation.IdentifierType == null)
                        ? null
                        : new[] { operation.IdentifierType },
                    objectTypes: new[] { typeof(TObject).ToRepresentation() },
                    versionMatchStrategy: operation.VersionMatchStrategy,
                    tags: operation.TagsToMatch,
                    tagMatchStrategy: operation.TagMatchStrategy,
                    deprecatedIdTypes: operation.DeprecatedIdTypes),
                operation.RecordNotFoundStrategy);

            var internalRecordIds = this.stream.Execute(internalRecordIdsOp);

            var records = internalRecordIds
                .Select(_ =>
                    this.stream.Execute(
                        new StandardGetLatestRecordOp(
                            new RecordFilter(
                                internalRecordIds: new[]
                                {
                                    _,
                                }),
                            operation.RecordNotFoundStrategy,
                            streamRecordItemsToInclude: StreamRecordItemsToInclude.MetadataAndPayload)))
                .ToList();

            StreamRecord<TObject> ProcessResultItem(
                StreamRecord inputStreamRecord)
            {
                var payload = inputStreamRecord
                    .GetDescribedSerialization()
                    .DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

                var resultItem = new StreamRecord<TObject>(
                    inputStreamRecord.InternalRecordId,
                    inputStreamRecord.Metadata,
                    payload);

                return resultItem;
            }

            var result = StandardStreamReadWriteProtocols.OrderAndConvertToTypedStreamRecords(
                records,
                operation.OrderRecordsBy,
                ProcessResultItem);

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecord<TObject>>> ExecuteAsync(
            GetAllRecordsOp<TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<TObject> Execute(
            GetAllObjectsOp<TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOperation = new GetAllRecordsOp<TObject>(
                operation.IdentifierType,
                operation.VersionMatchStrategy,
                operation.TagsToMatch,
                operation.TagMatchStrategy,
                operation.RecordNotFoundStrategy,
                operation.OrderRecordsBy,
                operation.DeprecatedIdTypes);

            var records = this.stream.GetStreamReadingProtocols<TObject>().Execute(delegatedOperation);

            var result = records
                .Select(_ => _.Payload)
                .ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetAllObjectsOp<TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
