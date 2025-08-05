// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

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

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(this.stream, locator);

            var record = this.stream.Execute(standardOp);

            if (record == null)
            {
                return null;
            }

            var metadata = record.Metadata.ToStreamRecordMetadata(operation.Id);

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

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(this.stream, locator);

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

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var stringSerializedIdentifier = this.stream.GetStringSerializedIdentifier(
                operation.Id,
                operation.TypeSelectionStrategy);

            var internalRecordIdsOp = new StandardGetInternalRecordIdsOp(
                new RecordFilter(
                    ids: new[]
                    {
                        stringSerializedIdentifier,
                    },
                    idTypes: new[]
                    {
                        typeof(TId).ToRepresentation(),
                    },
                    objectTypes: (operation.ObjectType == null)
                        ? null
                        : new[] { operation.ObjectType },
                    versionMatchStrategy: operation.VersionMatchStrategy,
                    tags: operation.TagsToMatch,
                    tagMatchStrategy: operation.TagMatchStrategy,
                    deprecatedIdTypes: operation.DeprecatedIdTypes),
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator: locator);
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

            StreamRecordWithId<TId> ProcessResultItem(
                StreamRecord inputStreamRecord)
            {
                var metadata = inputStreamRecord.Metadata.ToStreamRecordMetadata(operation.Id);

                var resultItem = new StreamRecordWithId<TId>(inputStreamRecord.InternalRecordId, metadata, inputStreamRecord.Payload);

                return resultItem;
            }

            var result = StandardStreamReadWriteProtocols.OrderAndConvertToTypedStreamRecords(
                records,
                operation.OrderRecordsBy,
                ProcessResultItem);

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

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(this.stream, locator);

            var recordWithOnlyMetadata = this.stream.Execute(standardOp);

            if (recordWithOnlyMetadata == null)
            {
                return null;
            }

            var result = recordWithOnlyMetadata.Metadata.ToStreamRecordMetadata(operation.Id);

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

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var serializedIdentifier = this.stream.GetStringSerializedIdentifier(
                operation.Id,
                operation.TypeSelectionStrategy);

            var internalRecordIdsOp = new StandardGetInternalRecordIdsOp(
                new RecordFilter(
                    ids: new[]
                    {
                        serializedIdentifier,
                    },
                    idTypes: new[]
                    {
                        typeof(TId).ToRepresentation(),
                    },
                    objectTypes: (operation.ObjectType == null)
                        ? null
                        : new[] { operation.ObjectType },
                    versionMatchStrategy: operation.VersionMatchStrategy,
                    tags: operation.TagsToMatch,
                    tagMatchStrategy: operation.TagMatchStrategy,
                    deprecatedIdTypes: operation.DeprecatedIdTypes),
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator: locator);
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

            StreamRecordMetadata<TId> ProcessResultItem(
                StreamRecord inputRecord)
            {
                var resultItem = inputRecord.Metadata.ToStreamRecordMetadata(operation.Id);

                return resultItem;
            }

            var result = StandardStreamReadWriteProtocols.OrderAndConvertToTypedStreamRecords(
                records,
                operation.OrderRecordsBy,
                ProcessResultItem);

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

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(this.stream, locator);

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

            var standardOp = operation.Standardize();

            var standardResult = this.stream.Execute(standardOp);

            var result = standardResult.Select(_ => this.stream.IdSerializer.Deserialize<TId>(_.StringSerializedId)).ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<TId>> ExecuteAsync(
            GetDistinctIdsOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata<TId>> Execute(
            GetAllRecordsMetadataOp<TId> operation)
        {
            var records = new List<StreamRecord>();

            var allLocators = this.stream.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());

            foreach (var locator in allLocators)
            {
                var internalRecordIdsOp = new StandardGetInternalRecordIdsOp(
                    new RecordFilter(
                        idTypes: new[] { typeof(TId).ToRepresentation() },
                        objectTypes: (operation.ObjectType == null)
                            ? null
                            : new[] { operation.ObjectType },
                        versionMatchStrategy: operation.VersionMatchStrategy,
                        deprecatedIdTypes: operation.DeprecatedIdTypes,
                        tags: operation.TagsToMatch,
                        tagMatchStrategy: operation.TagMatchStrategy),
                    operation.RecordNotFoundStrategy,
                    specifiedResourceLocator: locator);

                var internalRecordIds = this.stream.Execute(internalRecordIdsOp);

                var thisLocatorRecords = internalRecordIds
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
                                    specifiedResourceLocator: locator)))
                    .ToList();

                records.AddRange(thisLocatorRecords);
            }

            StreamRecordMetadata<TId> ProcessResultItem(
                StreamRecord inputRecord)
            {
                var resultItem = inputRecord.Metadata.ToStreamRecordMetadata<TId>(this.stream.IdSerializer);

                return resultItem;
            }

            var result = StandardStreamReadWriteProtocols.OrderAndConvertToTypedStreamRecords(
                records,
                operation.OrderRecordsBy,
                ProcessResultItem);

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordMetadata<TId>>> ExecuteAsync(
            GetAllRecordsMetadataOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
