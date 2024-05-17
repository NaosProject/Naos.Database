// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
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

            return result.Any();
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public IReadOnlyList<StreamRecordWithId<TId>> Execute(
            GetAllRecordsByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var internalRecordIdsOp = new StandardGetInternalRecordIdsOp(
                new RecordFilter(
                    ids: new[]
                         {
                             new StringSerializedIdentifier(serializedObjectId, (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation()),
                         },
                    versionMatchStrategy: operation.VersionMatchStrategy,
                    deprecatedIdTypes: operation.DeprecatedIdTypes),
                operation.RecordNotFoundStrategy,
                locator);
            var internalRecordIds = this.stream.Execute(internalRecordIdsOp);

            var records = internalRecordIds
                         .Select(
                              _ =>
                                  this.stream.Execute(
                                      new StandardGetLatestRecordOp(
                                          new RecordFilter(
                                              internalRecordIds: new[]
                                                                 {
                                                                     _,
                                                                 }),
                                          operation.RecordNotFoundStrategy,
                                          StreamRecordItemsToInclude.MetadataAndPayload,
                                          locator)))
                         .ToList();

            List<StreamRecordWithId<TId>> result;

            StreamRecordWithId<TId> ProcessResultItem(
                StreamRecord inputStreamRecord)
            {
                var metadata = new StreamRecordMetadata<TId>(
                    operation.Id,
                    inputStreamRecord.Metadata.SerializerRepresentation,
                    inputStreamRecord.Metadata.TypeRepresentationOfId,
                    inputStreamRecord.Metadata.TypeRepresentationOfObject,
                    inputStreamRecord.Metadata.Tags,
                    inputStreamRecord.Metadata.TimestampUtc,
                    inputStreamRecord.Metadata.ObjectTimestampUtc);

                var resultItem = new StreamRecordWithId<TId>(inputStreamRecord.InternalRecordId, metadata, inputStreamRecord.Payload);

                return resultItem;
            }

            switch (operation.OrderRecordsBy)
            {
                case OrderRecordsBy.InternalRecordIdAscending:
                    result = records
                            .OrderBy(_ => _.InternalRecordId)
                            .Select(ProcessResultItem)
                            .ToList();
                    break;
                case OrderRecordsBy.InternalRecordIdDescending:
                    result = records
                            .OrderByDescending(_ => _.InternalRecordId)
                            .Select(ProcessResultItem)
                            .ToList();
                    break;
                case OrderRecordsBy.Random:
                    result = records
                            .OrderBy(_ => Guid.NewGuid())
                            .Select(ProcessResultItem)
                            .ToList();
                    break;
                default:
                    throw new NotSupportedException(Invariant($"Unsupported {nameof(OrderRecordsBy)}: {operation.OrderRecordsBy}."));
            }

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

            var recordWithOnlyMetadata = this.stream.Execute(standardOp);

            if (recordWithOnlyMetadata == null)
            {
                return null;
            }

            var result = new StreamRecordMetadata<TId>(
                operation.Id,
                recordWithOnlyMetadata.Metadata.SerializerRepresentation,
                recordWithOnlyMetadata.Metadata.TypeRepresentationOfId,
                recordWithOnlyMetadata.Metadata.TypeRepresentationOfObject,
                recordWithOnlyMetadata.Metadata.Tags,
                recordWithOnlyMetadata.Metadata.TimestampUtc,
                recordWithOnlyMetadata.Metadata.ObjectTimestampUtc);

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
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public IReadOnlyList<StreamRecordMetadata<TId>> Execute(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var internalRecordIdsOp = new StandardGetInternalRecordIdsOp(
                new RecordFilter(
                    ids: new[]
                         {
                             new StringSerializedIdentifier(serializedObjectId, (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation()),
                         },
                    versionMatchStrategy: operation.VersionMatchStrategy,
                    deprecatedIdTypes: operation.DeprecatedIdTypes),
                operation.RecordNotFoundStrategy,
                locator);
            var internalRecordIds = this.stream.Execute(internalRecordIdsOp);

            var records = internalRecordIds
                         .Select(
                              _ =>
                                  this.stream.Execute(
                                      new StandardGetLatestRecordOp(
                                          new RecordFilter(
                                              internalRecordIds: new[]
                                                                 {
                                                                     _,
                                                                 }),
                                          operation.RecordNotFoundStrategy,
                                          StreamRecordItemsToInclude.MetadataOnly,
                                          locator)))
                         .ToList();

            List<StreamRecordMetadata<TId>> result;

            StreamRecordMetadata<TId> ProcessResultItem(
                StreamRecord inputRecord)
            {
                var resultItem = new StreamRecordMetadata<TId>(
                    operation.Id,
                    inputRecord.Metadata.SerializerRepresentation,
                    inputRecord.Metadata.TypeRepresentationOfId,
                    inputRecord.Metadata.TypeRepresentationOfObject,
                    inputRecord.Metadata.Tags,
                    inputRecord.Metadata.TimestampUtc,
                    inputRecord.Metadata.ObjectTimestampUtc);

                return resultItem;
            }

            switch (operation.OrderRecordsBy)
            {
                case OrderRecordsBy.InternalRecordIdAscending:
                    result = records
                            .OrderBy(_ => _.InternalRecordId)
                            .Select(ProcessResultItem)
                            .ToList();
                    break;
                case OrderRecordsBy.InternalRecordIdDescending:
                    result = records
                            .OrderByDescending(_ => _.InternalRecordId)
                            .Select(ProcessResultItem)
                            .ToList();
                    break;
                case OrderRecordsBy.Random:
                    result = records
                            .OrderBy(_ => Guid.NewGuid())
                            .Select(ProcessResultItem)
                            .ToList();
                    break;
                default:
                    throw new NotSupportedException(
                        Invariant($"Unsupported {nameof(OrderRecordsBy)}: {operation.OrderRecordsBy}."));
            }

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
