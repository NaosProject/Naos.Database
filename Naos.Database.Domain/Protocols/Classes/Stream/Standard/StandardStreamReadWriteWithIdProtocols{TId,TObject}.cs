// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
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
    /// with a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
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

            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(this.stream, locator);

            var record = this.stream.Execute(standardOp);

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
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public IReadOnlyList<TObject> Execute(
            GetLatestObjectsByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            // We are supporting OrderRecordsBy to future-proof the introduction of OrderRecordsBy.RecordTimestamp.
            // It doesn't make sense to order by internal record id because multiple locators may be used and ids
            // across locators can't really be compared.
            // The end user might think they are ordering by time when they are not.
            if (operation.OrderRecordsBy != OrderRecordsBy.Random)
            {
                throw new NotSupportedException(Invariant($"{nameof(OrderRecordsBy)} {operation.OrderRecordsBy} is not supported."));
            }

            var result = new List<TObject>();

            if ((operation.Ids != null) && operation.Ids.Any())
            {
                // We have tested and implementations of ResourceLocatorBase will group properly, even though they do
                // not implement IEquatable<IResourceLocator>.  In practice, its most likely that the same type of
                // resource locator will be used.  And if folks create a new implementation of IResourceLocator, they
                // can make it IEquatable<NewImplementation> and the below will work fine.  In the worst case, if they
                // do not make their implementation IEquatable, then the foreach below will iterate through each id
                // which is equivalent to calling GetLatestObjectByIdOp<TId, TObject> in a loop.
                var locatorToIdsMap = operation.Ids
                    .Select(_ => new { Id = _, Locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(_)) })
                    .GroupBy(_ => _.Locator)
                    .ToDictionary(_ => _.Key, _ => _.Select(i => i.Id).ToList());

                foreach (var locator in locatorToIdsMap.Keys)
                {
                    var ids = locatorToIdsMap[locator];

                    var stringSerializedIdentifiers = ids
                        .Select(_ => this.stream.GetStringSerializedIdentifier(_, operation.TypeSelectionStrategy))
                        .ToList();

                    var internalRecordIdsOp = new StandardGetInternalRecordIdsOp(
                        new RecordFilter(
                            ids: stringSerializedIdentifiers,
                            idTypes: new[] { typeof(TId).ToRepresentation() },
                            objectTypes: new[] { typeof(TObject).ToRepresentation() },
                            versionMatchStrategy: operation.VersionMatchStrategy,
                            tags: operation.TagsToMatch,
                            tagMatchStrategy: operation.TagMatchStrategy,
                            deprecatedIdTypes: operation.DeprecatedIdTypes),
                        RecordNotFoundStrategy.ReturnDefault, // Here we are hard coding to ReturnDefault because we cannot evaluate the strategy until we've pulled records from all locators
                        new RecordsToFilterCriteria(
                            RecordsToFilterSelectionStrategy.LatestByIdAndObjectType,
                            operation.VersionMatchStrategy),
                        locator);

                    var internalRecordIds = this.stream.Execute(internalRecordIdsOp);

                    if (internalRecordIds.Any())
                    {
                        // ReSharper disable once RedundantArgumentDefaultValue - want to be explicit about RecordNotFoundStrategy and also add a comment
                        var thisLocatorRecords = internalRecordIds
                            .Select(_ => this.stream.Execute(
                                new StandardGetLatestRecordOp(
                                    new RecordFilter(
                                        internalRecordIds: new[]
                                        {
                                        _,
                                        }),
                                    RecordNotFoundStrategy.ReturnDefault, // See comment above.  We specifically do NOT want to chain-thru operation.RecordNotFoundStrategy here.  If that strategy is Throw and all records disappear in-between the call above and this call, then effectively those records don't exist and we should continue looping thru locators.
                                    specifiedResourceLocator: locator)))
                            .ToList();

                        var thisLocatorObjects = thisLocatorRecords
                            .Select(_ => _.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory))
                            .ToList();

                        result.AddRange(thisLocatorObjects);
                    }
                }

                switch (operation.RecordNotFoundStrategy)
                {
                    case RecordNotFoundStrategy.ReturnDefault:
                        break;  // result list starts off empty, so if no records found then the result is correct.
                    case RecordNotFoundStrategy.Throw:
                        if (!result.Any())
                        {
                            throw new InvalidOperationException(Invariant($"Expected stream {this.stream.StreamRepresentation} to contain a matching record(s) for {operation}, none were found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        }

                        break;
                    default:
                        throw new NotSupportedException(Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                }
            }
            else
            {
                var allLocators = this.stream.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());

                foreach (var locator in allLocators)
                {
                    var internalRecordIdsOp = new StandardGetInternalRecordIdsOp(
                        new RecordFilter(
                            objectTypes: new[] { typeof(TObject).ToRepresentation() },
                            idTypes: new[] { typeof(TId).ToRepresentation() },
                            versionMatchStrategy: operation.VersionMatchStrategy,
                            tags: operation.TagsToMatch,
                            tagMatchStrategy: operation.TagMatchStrategy,
                            deprecatedIdTypes: operation.DeprecatedIdTypes),
                        operation.RecordNotFoundStrategy,
                        new RecordsToFilterCriteria(
                            RecordsToFilterSelectionStrategy.LatestByIdAndObjectType,
                            operation.VersionMatchStrategy),
                        specifiedResourceLocator: locator);

                    var internalRecordIds = this.stream.Execute(internalRecordIdsOp);

                    if (internalRecordIds.Any())
                    {
                        var records = internalRecordIds
                            .Select(_ => this.stream.Execute(
                                new StandardGetLatestRecordOp(
                                    new RecordFilter(
                                        internalRecordIds: new[]
                                        {
                                            _,
                                        }),
                                    operation.RecordNotFoundStrategy,
                                    specifiedResourceLocator: locator)))
                            .ToList();

                        var objects = records
                            .Select(_ => _.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory))
                            .ToList();

                        result.AddRange(objects);
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetLatestObjectsByIdOp<TId, TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(this.stream, locator);

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

            var locator = this.locatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var standardOp = operation.Standardize(this.stream, locator);

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
                operation.TagsToMatch,
                operation.TagMatchStrategy,
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
