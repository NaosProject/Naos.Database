// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
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
    /// with a typed identifier and without a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public class StandardStreamReadWriteWithIdProtocols<TId> :
        IStreamReadWithIdProtocols<TId>,
        IStreamWriteWithIdProtocols<TId>
    {
        private readonly IStandardStream stream;

        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> locatorProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamReadWriteWithIdProtocols{TId}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamReadWriteWithIdProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.locatorProtocols = stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            GetLatestRecordByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(serializer, locator);

            var record = this.stream.Execute(standardOp);

            if (record == null)
            {
                return null;
            }

            var metadata = new StreamRecordMetadata<TId>(
                operation.Id,
                record.Metadata.SerializerRepresentation,
                record.Metadata.TypeRepresentationOfId,
                record.Metadata.TypeRepresentationOfObject,
                record.Metadata.Tags,
                record.Metadata.TimestampUtc,
                record.Metadata.ObjectTimestampUtc);

            var result = new StreamRecordWithId<TId>(record.InternalRecordId, metadata, record.Payload);

            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId>> ExecuteAsync(
            GetLatestRecordByIdOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public bool Execute(
            DoesAnyExistByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(serializer, locator);

            var result = this.stream.Execute(standardOp);

            return result;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordWithId<TId>> Execute(
            GetAllRecordsByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var internalRecordIdsOp = new StandardGetRecordIdsOp(
                new RecordFilter(
                    ids: new[]
                         {
                             new StringSerializedIdentifier(serializedObjectId, (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation()),
                         },
                    versionMatchStrategy: operation.VersionMatchStrategy),
                operation.RecordNotFoundStrategy,
                locator);
            var internalRecordIds = this.stream.Execute(internalRecordIdsOp);

            var records = internalRecordIds
                         .Select(
                              _ =>
                                  this.stream.Execute(
                                      new StandardGetRecordByInternalRecordIdOp(
                                          _,
                                          operation.RecordNotFoundStrategy,
                                          StreamRecordItemsToInclude.MetadataAndPayload,
                                          locator)))
                         .ToList();

            var result = records?.Select(
                    _ =>
                    {
                        var metadata = new StreamRecordMetadata<TId>(
                            operation.Id,
                            _.Metadata.SerializerRepresentation,
                            _.Metadata.TypeRepresentationOfId,
                            _.Metadata.TypeRepresentationOfObject,
                            _.Metadata.Tags,
                            _.Metadata.TimestampUtc,
                            _.Metadata.ObjectTimestampUtc);

                        var streamRecord = new StreamRecordWithId<TId>(_.InternalRecordId, metadata, _.Payload);

                        return streamRecord;
                    })
                .ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordWithId<TId>>> ExecuteAsync(
            GetAllRecordsByIdOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public StreamRecordMetadata<TId> Execute(
            GetLatestRecordMetadataByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(serializer, locator);

            var metadata = this.stream.Execute(standardOp);

            if (metadata == null)
            {
                return null;
            }

            var result = new StreamRecordMetadata<TId>(
                operation.Id,
                metadata.SerializerRepresentation,
                metadata.TypeRepresentationOfId,
                metadata.TypeRepresentationOfObject,
                metadata.Tags,
                metadata.TimestampUtc,
                metadata.ObjectTimestampUtc);

            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordMetadata<TId>> ExecuteAsync(
            GetLatestRecordMetadataByIdOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata<TId>> Execute(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var internalRecordIdsOp = new StandardGetRecordIdsOp(
                new RecordFilter(
                    ids: new[]
                         {
                             new StringSerializedIdentifier(serializedObjectId, (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation()),
                         },
                    versionMatchStrategy: operation.VersionMatchStrategy),
                operation.RecordNotFoundStrategy,
                locator);
            var internalRecordIds = this.stream.Execute(internalRecordIdsOp);

            var records = internalRecordIds
                         .Select(
                              _ =>
                                  this.stream.Execute(
                                      new StandardGetRecordByInternalRecordIdOp(
                                          _,
                                          operation.RecordNotFoundStrategy,
                                          StreamRecordItemsToInclude.MetadataOnly,
                                          locator)))
                         .ToList();

            var result = records?.Select(
                    _ =>
                    {
                        var metadata = new StreamRecordMetadata<TId>(
                            operation.Id,
                            _.Metadata.SerializerRepresentation,
                            _.Metadata.TypeRepresentationOfId,
                            _.Metadata.TypeRepresentationOfObject,
                            _.Metadata.Tags,
                            _.Metadata.TimestampUtc,
                            _.Metadata.ObjectTimestampUtc);

                        return metadata;
                    })
                .ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordMetadata<TId>>> ExecuteAsync(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public string Execute(
            GetLatestStringSerializedObjectByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(serializer, locator);

            var result = this.stream.Execute(standardOp);

            return result;
        }

        /// <inheritdoc />
        public async Task<string> ExecuteAsync(
            GetLatestStringSerializedObjectByIdOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<TId> Execute(
            GetDistinctIdsOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var standardOp = operation.Standardize();

            var standardResult = this.stream.Execute(standardOp);

            var result = standardResult.Select(_ => serializer.Deserialize<TId>(_.StringSerializedId)).ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<TId>> ExecuteAsync(
            GetDistinctIdsOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
