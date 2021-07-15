// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;
    using DomainExtensions = OBeautifulCode.Serialization.DomainExtensions;

    /// <summary>
    /// In memory implementation of <see cref="IReadWriteStream"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class MemoryReadWriteStream :
        StandardReadWriteStreamBase
    {
        private readonly object streamLock = new object();
        private readonly object handlingLock = new object();
        private readonly object singleLocatorLock = new object();

        private readonly Dictionary<MemoryDatabaseLocator, List<StreamRecord>> locatorToRecordPartitionMap = new Dictionary<MemoryDatabaseLocator, List<StreamRecord>>();
        private readonly Dictionary<MemoryDatabaseLocator, Dictionary<string, List<StreamRecordHandlingEntry>>> locatorToHandlingEntriesByConcernMap = new Dictionary<MemoryDatabaseLocator, Dictionary<string, List<StreamRecordHandlingEntry>>>();
        private bool created = false;
        private long uniqueLongForExternalProtocol = 0;
        private long uniqueLongForInMemoryRecords = 0;
        private long uniqueLongForInMemoryHandlingEntries = 0;
        private MemoryDatabaseLocator singleLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryReadWriteStream"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        /// <param name="resourceLocatorProtocols">The optional resource locator protocols; DEFAULT will be a single <see cref="MemoryDatabaseLocator"/> named 'Default'.</param>
        public MemoryReadWriteStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            IResourceLocatorProtocols resourceLocatorProtocols = null)
        : base(name, resourceLocatorProtocols ?? new SingleResourceLocatorProtocol(new MemoryDatabaseLocator("Default")), serializerFactory, defaultSerializerRepresentation, defaultSerializationFormat)
        {
            this.Id = Guid.NewGuid().ToString().ToUpperInvariant();
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation => new MemoryStreamRepresentation(this.Name, this.Id);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; private set; }

        /// <inheritdoc />
        public override CreateStreamResult Execute(
            CreateStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var alreadyExisted = this.created;
            var wasCreated = true;

            lock (this.streamLock)
            {
                if (operation == null)
                {
                    throw new ArgumentNullException(nameof(operation));
                }

                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryReadWriteStream)} can only 'create' a stream with the same representation."));
                }

                if (this.created)
                {
                    switch (operation.ExistingStreamEncounteredStrategy)
                    {
                        case ExistingStreamEncounteredStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {operation.StreamRepresentation} to not exist, it does and the operation {nameof(operation.ExistingStreamEncounteredStrategy)} is set to '{operation.ExistingStreamEncounteredStrategy}'."));
                        case ExistingStreamEncounteredStrategy.Overwrite:
                            this.locatorToRecordPartitionMap.Clear();
                            break;
                        case ExistingStreamEncounteredStrategy.Skip:
                            wasCreated = false;
                            break;
                        default:
                            throw new NotSupportedException(
                                Invariant(
                                    $"{nameof(ExistingStreamEncounteredStrategy)} '{operation.ExistingStreamEncounteredStrategy}' is not supported."));
                    }
                }

                this.created = true;
            }

            var result = new CreateStreamResult(alreadyExisted, wasCreated);
            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            DeleteStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            lock (this.streamLock)
            {
                if (operation == null)
                {
                    throw new ArgumentNullException(nameof(operation));
                }

                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryReadWriteStream)} can only 'Delete' a stream with the same representation."));
                }

                if (!this.created)
                {
                    switch (operation.ExistingStreamNotEncounteredStrategy)
                    {
                        case ExistingStreamNotEncounteredStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {operation.StreamRepresentation} to exist, it does not and the operation {nameof(operation.ExistingStreamNotEncounteredStrategy)} is '{operation.ExistingStreamNotEncounteredStrategy}'."));
                        case ExistingStreamNotEncounteredStrategy.Skip:
                            break;
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(ExistingStreamNotEncounteredStrategy)} {operation.ExistingStreamNotEncounteredStrategy} is not supported."));
                    }
                }
                else
                {
                    foreach (var partition in this.locatorToRecordPartitionMap)
                    {
                        partition.Value.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>System.Int64.</returns>
        public override long Execute(
            GetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var result = Interlocked.Increment(ref this.uniqueLongForExternalProtocol);
            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            PruneBeforeInternalRecordDateOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();
            lock (this.streamLock)
            {
                var newList = this.locatorToRecordPartitionMap[locator].Where(_ => _.Metadata.TimestampUtc >= operation.InternalRecordDate);
                this.locatorToRecordPartitionMap[locator].Clear();
                this.locatorToRecordPartitionMap[locator].AddRange(newList);

                lock (this.handlingLock)
                {
                    if (this.locatorToHandlingEntriesByConcernMap.ContainsKey(locator) && this.locatorToHandlingEntriesByConcernMap[locator].Any())
                    {
                        foreach (var concern in this.locatorToHandlingEntriesByConcernMap[locator].Keys)
                        {
                            var newHandlingList = this.locatorToHandlingEntriesByConcernMap[locator][concern]
                                                      .Where(_ => _.Metadata.TimestampUtc >= operation.InternalRecordDate);
                            this.locatorToHandlingEntriesByConcernMap[locator][concern].Clear();
                            this.locatorToHandlingEntriesByConcernMap[locator][concern].AddRange(newHandlingList);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            PruneBeforeInternalRecordIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                var newList = this.locatorToRecordPartitionMap[locator].Where(_ => _.InternalRecordId >= operation.InternalRecordId);
                this.locatorToRecordPartitionMap[locator].Clear();
                this.locatorToRecordPartitionMap[locator].AddRange(newList);

                lock (this.handlingLock)
                {
                    if (this.locatorToHandlingEntriesByConcernMap.ContainsKey(locator) && this.locatorToHandlingEntriesByConcernMap[locator].Any())
                    {
                        foreach (var concern in this.locatorToHandlingEntriesByConcernMap[locator].Keys)
                        {
                            var newHandlingList = this.locatorToHandlingEntriesByConcernMap[locator][concern]
                                                      .Where(_ => _.Metadata.InternalRecordId >= operation.InternalRecordId);
                            this.locatorToHandlingEntriesByConcernMap[locator][concern].Clear();
                            this.locatorToHandlingEntriesByConcernMap[locator][concern].AddRange(newHandlingList);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override bool Execute(
            DoesAnyExistByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);
                var result =
                    partition?.OrderByDescending(_ => _.InternalRecordId)
                                            .FirstOrDefault(
                                                 _ => _.Metadata.FuzzyMatchTypesAndId(
                                                     operation.StringSerializedId,
                                                     operation.IdentifierType,
                                                     operation.ObjectType,
                                                     operation.VersionMatchStrategy));

                return result != null;
            }
        }

        /// <inheritdoc />
        public override StreamRecordMetadata Execute(
            GetLatestRecordMetadataByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result =
                    partition?.OrderByDescending(_ => _.InternalRecordId)
                              .FirstOrDefault(
                                   _ => _.Metadata.FuzzyMatchTypesAndId(
                                       operation.StringSerializedId,
                                       operation.IdentifierType,
                                       operation.ObjectType,
                                       operation.VersionMatchStrategy))?.Metadata;

                if (result != null)
                {
                    return result;
                }

                switch (operation.ExistingRecordNotEncounteredStrategy)
                {
                    case ExistingRecordNotEncounteredStrategy.ReturnDefault:
                        return null;
                    case ExistingRecordNotEncounteredStrategy.Throw:
                        throw new InvalidOperationException(
                            Invariant(
                                $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.ExistingRecordNotEncounteredStrategy)} is '{operation.ExistingRecordNotEncounteredStrategy}'."));
                    default:
                        throw new NotSupportedException(Invariant($"{nameof(ExistingRecordNotEncounteredStrategy)} {operation.ExistingRecordNotEncounteredStrategy} is not supported."));
                }
            }
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecord> Execute(
            GetAllRecordsByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                IReadOnlyList<StreamRecord> ProcessDefaultReturn()
                {
                    switch (operation.ExistingRecordNotEncounteredStrategy)
                    {
                        case ExistingRecordNotEncounteredStrategy.ReturnDefault:
                            return new StreamRecord[0];
                        case ExistingRecordNotEncounteredStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.ExistingRecordNotEncounteredStrategy)} is '{operation.ExistingRecordNotEncounteredStrategy}'."));
                        default:
                            throw new NotSupportedException(
                                Invariant($"{nameof(ExistingRecordNotEncounteredStrategy)} {operation.ExistingRecordNotEncounteredStrategy} is not supported."));
                    }
                }

                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                if (partition == null)
                {
                    return ProcessDefaultReturn();
                }

                var result =
                    partition
                       .Where(
                            _ => _.Metadata.FuzzyMatchTypesAndId(
                                operation.StringSerializedId,
                                operation.IdentifierType,
                                operation.ObjectType,
                                operation.VersionMatchStrategy))
                       .ToList();

                if (result.Any())
                {
                    switch (operation.OrderRecordsStrategy)
                    {
                        case OrderRecordsStrategy.ByInternalRecordIdAscending:
                            return result.OrderBy(_ => _.InternalRecordId).ToList();
                        case OrderRecordsStrategy.ByInternalRecordIdDescending:
                            return result.OrderByDescending(_ => _.InternalRecordId).ToList();
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(OrderRecordsStrategy)} {operation.OrderRecordsStrategy} is not supported."));
                    }
                }

                return ProcessDefaultReturn();
            }
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordMetadata> Execute(
            GetAllRecordsMetadataByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                IReadOnlyList<StreamRecordMetadata> ProcessDefaultReturn()
                {
                    switch (operation.ExistingRecordNotEncounteredStrategy)
                    {
                        case ExistingRecordNotEncounteredStrategy.ReturnDefault:
                            return new StreamRecordMetadata[0];
                        case ExistingRecordNotEncounteredStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.ExistingRecordNotEncounteredStrategy)} is '{operation.ExistingRecordNotEncounteredStrategy}'."));
                        default:
                            throw new NotSupportedException(
                                Invariant($"{nameof(ExistingRecordNotEncounteredStrategy)} {operation.ExistingRecordNotEncounteredStrategy} is not supported."));
                    }
                }

                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                if (partition == null)
                {
                    return ProcessDefaultReturn();
                }

                var result =
                    partition
                       .Where(
                            _ => _.Metadata.FuzzyMatchTypesAndId(
                                operation.StringSerializedId,
                                operation.IdentifierType,
                                operation.ObjectType,
                                operation.VersionMatchStrategy))
                       .ToList();

                if (result.Any())
                {
                    switch (operation.OrderRecordsStrategy)
                    {
                        case OrderRecordsStrategy.ByInternalRecordIdAscending:
                            return result.OrderBy(_ => _.InternalRecordId).Select(_ => _.Metadata).ToList();
                        case OrderRecordsStrategy.ByInternalRecordIdDescending:
                            return result.OrderByDescending(_ => _.InternalRecordId).Select(_ => _.Metadata).ToList();
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(OrderRecordsStrategy)} {operation.OrderRecordsStrategy} is not supported."));
                    }
                }

                return ProcessDefaultReturn();
            }
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<string> Execute(
            GetDistinctStringSerializedIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new HashSet<string>();
            lock (this.streamLock)
            {
                var locators = new List<MemoryDatabaseLocator>();
                if (operation.SpecifiedResourceLocator != null)
                {
                    locators.Add(operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>());
                }
                else
                {
                    var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
                    foreach (var locator in allLocators)
                    {
                        locators.Add(locator.ConfirmAndConvert<MemoryDatabaseLocator>());
                    }
                }

                foreach (var memoryDatabaseLocator in locators)
                {
                    this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                    if (partition != null)
                    {
                        foreach (var streamRecord in partition)
                        {
                            if (streamRecord.Metadata.FuzzyMatchTypes(
                                    operation.IdentifierType,
                                    operation.ObjectType,
                                    operation.VersionMatchStrategy)
                             && ((!operation.TagsToMatch?.Any() ?? true)
                              || streamRecord.Metadata.Tags.FuzzyMatchAccordingToStrategy(operation.TagsToMatch, operation.TagMatchStrategy)))
                            {
                                result.Add(streamRecord.Metadata.StringSerializedId);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOfRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);

                var entriesForInternalRecordId = entries.Where(_ => _.Metadata.InternalRecordId == operation.InternalRecordId).ToList();
                return entriesForInternalRecordId;
            }
        }

        /// <inheritdoc />
        public override HandlingStatus Execute(
            GetHandlingStatusOfRecordsByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var blockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingConcern);
                if (blockedEntries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault()?.Metadata.Status == HandlingStatus.Blocked)
                {
                    return HandlingStatus.Blocked;
                }

                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var statuses = entries.Where(
                                           _ => operation.IdsToMatch.Any(
                                               __ => __.StringSerializedId.Equals(_.Metadata.StringSerializedId)
                                                  && __.IdentifierType.EqualsAccordingToStrategy(
                                                         _.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                                                             operation.VersionMatchStrategy),
                                                         operation.VersionMatchStrategy)))
                                      .GroupBy(_ => _.Metadata.InternalRecordId)
                                      .Select(_ => _.OrderByDescending(__ => __.InternalHandlingEntryId).First().Metadata.Status)
                                      .ToList();
                var result = statuses.ReduceToCompositeHandlingStatus(operation.HandlingStatusCompositionStrategy);
                return result;
            }
        }

        /// <inheritdoc />
        public override HandlingStatus Execute(
            GetHandlingStatusOfRecordSetByTagOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());

            lock (this.handlingLock)
            {
                var statuses = new List<HandlingStatus>();
                foreach (var locator in allLocators)
                {
                    var blockedEntries = this.GetStreamRecordHandlingEntriesForConcern(locator, Concerns.RecordHandlingConcern);
                    if (blockedEntries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault()?.Metadata.Status == HandlingStatus.Blocked)
                    {
                        return HandlingStatus.Blocked;
                    }

                    var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, operation.Concern);
                    var statusesForLocator = entries.Where(
                                                         _ => operation.TagsToMatch.FuzzyMatchAccordingToStrategy(
                                                             _.Metadata.Tags,
                                                             operation.TagMatchStrategy))
                                                    .GroupBy(_ => _.Metadata.InternalRecordId)
                                                    .Select(_ => _.OrderByDescending(__ => __.InternalHandlingEntryId).First().Metadata.Status)
                                                    .ToList();

                    statuses.AddRange(statusesForLocator);
                }

                var result = statuses.ReduceToCompositeHandlingStatus(operation.HandlingStatusCompositionStrategy);
                return result;
            }
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override TryHandleRecordResult Execute(
            TryHandleRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var allLocators = operation.SpecifiedResourceLocator != null
                ? new[]
                  {
                      operation.SpecifiedResourceLocator,
                  }
                : this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());

            lock (this.handlingLock)
            {
                foreach (var locator in allLocators)
                {
                    if (!(locator is MemoryDatabaseLocator memoryDatabaseLocator))
                    {
                        throw new NotSupportedException(
                            Invariant(
                                $"{nameof(GetAllResourceLocatorsOp)} must only return locators of type {typeof(MemoryDatabaseLocator).ToStringReadable()}; found {locator?.GetType().ToStringReadable()}."));
                    }

                    var handlingEntries = this.GetStreamRecordHandlingEntriesForConcern(locator, Concerns.RecordHandlingConcern);
                    var mostRecentEntry = handlingEntries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault();
                    if (mostRecentEntry != null && mostRecentEntry.Metadata.Status == HandlingStatus.Blocked)
                    {
                        return new TryHandleRecordResult(null, true);
                    }

                    var entries =
                        this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern)
                            .ToList();

                    var existingInternalRecordIdsToConsider = new List<long>();
                    var existingInternalRecordIdsToIgnore = new List<long>();
                    foreach (var groupedById in entries.GroupBy(_ => _.Metadata.InternalRecordId))
                    {
                        var mostRecent = groupedById.OrderByDescending(_ => _.InternalHandlingEntryId).First();
                        if (mostRecent.Metadata.Status.IsHandlingNeeded())
                        {
                            existingInternalRecordIdsToConsider.Add(groupedById.Key);
                        }
                        else
                        {
                            existingInternalRecordIdsToIgnore.Add(groupedById.Key);
                        }
                    }

                    lock (this.streamLock)
                    {
                        var recordsToConsiderForHandling =
                            this.locatorToRecordPartitionMap[memoryDatabaseLocator]
                                .Where(_ => !existingInternalRecordIdsToIgnore.Contains(_.InternalRecordId))
                                .ToList();

                        var matchingRecords = recordsToConsiderForHandling
                                             .Where(
                                                  _ => _.FuzzyMatch(
                                                      operation.IdentifierType,
                                                      operation.ObjectType,
                                                      operation.VersionMatchStrategy))
                                             .ToList();

                        StreamRecord recordToHandle;
                        switch (operation.OrderRecordsStrategy)
                        {
                            case OrderRecordsStrategy.ByInternalRecordIdAscending:
                                recordToHandle = matchingRecords
                                                .OrderBy(_ => _.InternalRecordId)
                                                .FirstOrDefault(
                                                     _ => operation.MinimumInternalRecordId == null
                                                       || _.InternalRecordId                >= operation.MinimumInternalRecordId);
                                break;
                            case OrderRecordsStrategy.ByInternalRecordIdDescending:
                                recordToHandle = matchingRecords
                                                .OrderByDescending(_ => _.InternalRecordId)
                                                .FirstOrDefault(
                                                     _ => operation.MinimumInternalRecordId == null
                                                       || _.InternalRecordId                >= operation.MinimumInternalRecordId);
                                break;
                            case OrderRecordsStrategy.Random:
                                recordToHandle = matchingRecords
                                                .OrderByDescending(_ => Guid.NewGuid())
                                                .FirstOrDefault(
                                                     _ => operation.MinimumInternalRecordId == null
                                                       || _.InternalRecordId                >= operation.MinimumInternalRecordId);
                                break;
                            default:
                                throw new NotSupportedException(Invariant($"{nameof(OrderRecordsStrategy)} {operation.OrderRecordsStrategy} is not supported."));
                        }

                        if (recordToHandle != null)
                        {
                            var handlingTags = operation.InheritRecordTags
                                ? (operation.Tags ?? new List<NamedValue<string>>())
                                 .Concat(recordToHandle.Metadata.Tags ?? new List<NamedValue<string>>())
                                 .Distinct()
                                 .ToList()
                                : operation.Tags;

                            if (!existingInternalRecordIdsToConsider.Contains(recordToHandle.InternalRecordId))
                            {
                                // first time needs a requested record
                                var requestedTimestamp = DateTime.UtcNow;
                                var requestedEvent = new RequestedHandleRecordExecutionEvent(
                                    recordToHandle.InternalRecordId,
                                    requestedTimestamp,
                                    recordToHandle);

                                var requestedPayload = requestedEvent.ToDescribedSerializationUsingSpecificFactory(
                                    this.DefaultSerializerRepresentation,
                                    this.SerializerFactory,
                                    this.DefaultSerializationFormat);

                                var requestedMetadata = new StreamRecordHandlingEntryMetadata(
                                    recordToHandle.InternalRecordId,
                                    operation.Concern,
                                    HandlingStatus.Requested,
                                    recordToHandle.Metadata.StringSerializedId,
                                    requestedPayload.SerializerRepresentation,
                                    recordToHandle.Metadata.TypeRepresentationOfId,
                                    requestedPayload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                                    handlingTags,
                                    requestedTimestamp,
                                    requestedEvent.TimestampUtc);

                                var requestedEntryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                                this.WriteHandlingEntryToMemoryMap(memoryDatabaseLocator, requestedEntryId, operation.Concern, requestedMetadata, requestedPayload);
                            }

                            var runningTimestamp = DateTime.UtcNow;

                            var runningEvent = new RunningHandleRecordExecutionEvent(recordToHandle.InternalRecordId, runningTimestamp);
                            var runningPayload = runningEvent.ToDescribedSerializationUsingSpecificFactory(
                                this.DefaultSerializerRepresentation,
                                this.SerializerFactory,
                                this.DefaultSerializationFormat);

                            var runningMetadata = new StreamRecordHandlingEntryMetadata(
                                recordToHandle.InternalRecordId,
                                operation.Concern,
                                HandlingStatus.Running,
                                recordToHandle.Metadata.StringSerializedId,
                                runningPayload.SerializerRepresentation,
                                recordToHandle.Metadata.TypeRepresentationOfId,
                                runningPayload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                                handlingTags,
                                runningTimestamp,
                                runningEvent.TimestampUtc);

                            var runningEntryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                            this.WriteHandlingEntryToMemoryMap(memoryDatabaseLocator, runningEntryId, operation.Concern, runningMetadata, runningPayload);

                            var result = new TryHandleRecordResult(recordToHandle);
                            return result;
                        }
                    }
                }

                return new TryHandleRecordResult(null);
            }
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            GetRecordByInternalRecordIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result = partition?.FirstOrDefault(_ => _.InternalRecordId == operation.InternalRecordId);

                if (result != null)
                {
                    return result;
                }

                switch (operation.ExistingRecordNotEncounteredStrategy)
                {
                    case ExistingRecordNotEncounteredStrategy.ReturnDefault:
                        return null;
                    case ExistingRecordNotEncounteredStrategy.Throw:
                        throw new InvalidOperationException(
                            Invariant(
                                $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.ExistingRecordNotEncounteredStrategy)} is '{operation.ExistingRecordNotEncounteredStrategy}'."));
                    default:
                        throw new NotSupportedException(Invariant($"{nameof(ExistingRecordNotEncounteredStrategy)} {operation.ExistingRecordNotEncounteredStrategy} is not supported."));
                }
            }
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            GetLatestRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result =
                    partition?.OrderByDescending(_ => _.InternalRecordId)
                              .FirstOrDefault(
                                   _ => _.Metadata.FuzzyMatchTypes(
                                       operation.IdentifierType,
                                       operation.ObjectType,
                                       operation.VersionMatchStrategy));

                if (result != null)
                {
                    return result;
                }

                switch (operation.ExistingRecordNotEncounteredStrategy)
                {
                    case ExistingRecordNotEncounteredStrategy.ReturnDefault:
                        return null;
                    case ExistingRecordNotEncounteredStrategy.Throw:
                        throw new InvalidOperationException(
                            Invariant(
                                $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.ExistingRecordNotEncounteredStrategy)} is '{operation.ExistingRecordNotEncounteredStrategy}'."));
                    default:
                        throw new NotSupportedException(Invariant($"{nameof(ExistingRecordNotEncounteredStrategy)} {operation.ExistingRecordNotEncounteredStrategy} is not supported."));
                }
            }
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            GetLatestRecordByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result =
                    partition?.OrderByDescending(_ => _.InternalRecordId)
                        .FirstOrDefault(
                             _ => _.Metadata.FuzzyMatchTypesAndId(
                                 operation.StringSerializedId,
                                 operation.IdentifierType,
                                 operation.ObjectType,
                                 operation.VersionMatchStrategy));

                if (result != null)
                {
                    return result;
                }

                switch (operation.ExistingRecordNotEncounteredStrategy)
                {
                    case ExistingRecordNotEncounteredStrategy.ReturnDefault:
                        return null;
                    case ExistingRecordNotEncounteredStrategy.Throw:
                        throw new InvalidOperationException(
                            Invariant(
                                $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.ExistingRecordNotEncounteredStrategy)} is '{operation.ExistingRecordNotEncounteredStrategy}'."));
                    default:
                        throw new NotSupportedException(Invariant($"{nameof(ExistingRecordNotEncounteredStrategy)} {operation.ExistingRecordNotEncounteredStrategy} is not supported."));
                }
            }
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        public override PutRecordResult Execute(
            PutRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                var existingRecordIds = new List<long>();
                var exists = this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var recordPartition);
                if (!exists)
                {
                    recordPartition = new List<StreamRecord>();
                    this.locatorToRecordPartitionMap.Add(memoryDatabaseLocator, recordPartition);
                }

                var matchesId =
                    operation.ExistingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById
                 || operation.ExistingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.ThrowIfFoundById
                 || operation.ExistingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.PruneIfFoundById
                        ? recordPartition.Where(
                                              _ => _.Metadata.FuzzyMatchTypesAndId(
                                                  operation.Metadata.StringSerializedId,
                                                  operation.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                                  null,
                                                  operation.VersionMatchStrategy))
                                         .ToList() : new List<StreamRecord>();

                var matchesIdAndObject =
                    operation.ExistingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.DoNotWriteIfFoundByIdAndTypeAndContent
                 || operation.ExistingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.DoNotWriteIfFoundByIdAndType
                 || operation.ExistingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.ThrowIfFoundByIdAndTypeAndContent
                 || operation.ExistingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.ThrowIfFoundByIdAndType
                 || operation.ExistingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType
                        ? recordPartition.Where(
                                              _ => _.Metadata.FuzzyMatchTypesAndId(
                                                  operation.Metadata.StringSerializedId,
                                                  operation.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                                  operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                                  operation.VersionMatchStrategy))
                                         .ToList() : new List<StreamRecord>();

                var recordIdsToPrune = new List<long>();
                switch (operation.ExistingRecordEncounteredStrategy)
                {
                    case ExistingRecordEncounteredStrategy.None:
                        /* no-op */
                        break;
                    case ExistingRecordEncounteredStrategy.ThrowIfFoundById:
                        if (matchesId.Any())
                        {
                            throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordEncounteredStrategy)} was {operation.ExistingRecordEncounteredStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' yet found {matchesId.Count}."));
                        }

                        break;
                    case ExistingRecordEncounteredStrategy.ThrowIfFoundByIdAndType:
                        if (matchesIdAndObject.Any())
                        {
                            throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordEncounteredStrategy)} was {operation.ExistingRecordEncounteredStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' and object type '{operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy)}' yet found {matchesIdAndObject.Count}."));
                        }

                        break;
                    case ExistingRecordEncounteredStrategy.ThrowIfFoundByIdAndTypeAndContent:
                        var matchesThrow =
                            matchesIdAndObject.Where(_ => _.Payload.Equals(operation.Payload)).ToList();

                        if (matchesThrow.Any())
                        {
                            throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordEncounteredStrategy)} was {operation.ExistingRecordEncounteredStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' and object type '{operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy)}' and contents '{operation.Payload}' yet found {matchesThrow.Count}."));
                        }

                        break;
                    case ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById:
                        if (matchesId.Any())
                        {
                            return new PutRecordResult(null, matchesId.Select(_ => _.InternalRecordId).ToList());
                        }

                        break;
                    case ExistingRecordEncounteredStrategy.DoNotWriteIfFoundByIdAndType:
                        if (matchesIdAndObject.Any())
                        {
                            return new PutRecordResult(null, matchesIdAndObject.Select(_ => _.InternalRecordId).ToList());
                        }

                        break;
                    case ExistingRecordEncounteredStrategy.DoNotWriteIfFoundByIdAndTypeAndContent:
                        var matchesDoNotWrite =
                            matchesIdAndObject.Where(_ => _.Payload.Equals(operation.Payload)).ToList();

                        if (matchesDoNotWrite.Any())
                        {
                            return new PutRecordResult(null, matchesDoNotWrite.Select(_ => _.InternalRecordId).ToList());
                        }

                        break;
                    case ExistingRecordEncounteredStrategy.PruneIfFoundById:
                        if (operation.RecordRetentionCount != null && matchesId.Count > operation.RecordRetentionCount - 1)
                        {
                            existingRecordIds.AddRange(
                                matchesId
                                   .Select(_ => _.InternalRecordId)
                                   .ToList());

                            var recordsToDeleteById =
                                matchesId.OrderByDescending(_ => _.InternalRecordId).Skip((int)operation.RecordRetentionCount - 1).ToList();
                            recordIdsToPrune.AddRange(recordsToDeleteById.Select(_ => _.InternalRecordId));
                        }

                        break;
                    case ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType:
                        if (operation.RecordRetentionCount != null && matchesIdAndObject.Count > operation.RecordRetentionCount - 1)
                        {
                            existingRecordIds.AddRange(
                                matchesIdAndObject
                                   .Select(_ => _.InternalRecordId)
                                   .ToList());

                            var recordsToDeleteById =
                                matchesIdAndObject.OrderByDescending(_ => _.InternalRecordId).Skip((int)operation.RecordRetentionCount - 1).ToList();
                            recordIdsToPrune.AddRange(recordsToDeleteById.Select(_ => _.InternalRecordId));
                        }

                        break;
                }

                long id;
                if (operation.InternalRecordId != null)
                {
                    if (recordPartition.Any(_ => _.InternalRecordId == operation.InternalRecordId))
                    {
                        throw new InvalidOperationException(Invariant($"Operation specified an {nameof(PutRecordOp.InternalRecordId)} of {operation.InternalRecordId} but that {nameof(PutRecordOp.InternalRecordId)} is already present in the stream."));
                    }
                    else
                    {
                        id = (long)operation.InternalRecordId;
                    }
                }
                else
                {
                    id = Interlocked.Increment(ref this.uniqueLongForInMemoryRecords);
                }

                var itemToAdd = new StreamRecord(id, operation.Metadata, operation.Payload);
                recordPartition.Add(itemToAdd);
                recordPartition.RemoveAll(_ => recordIdsToPrune.Contains(_.InternalRecordId));

                return new PutRecordResult(id, existingRecordIds, recordIdsToPrune);
            }
        }

        private MemoryDatabaseLocator TryGetSingleLocator()
        {
            if (this.singleLocator != null)
            {
                return this.singleLocator;
            }
            else
            {
                lock (this.singleLocatorLock)
                {
                    if (this.singleLocator != null)
                    {
                        return this.singleLocator;
                    }

                    var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
                    if (allLocators.Count != 1)
                    {
                        throw new NotSupportedException(Invariant($"The attempted operation cannot be performed because it expected a single {nameof(IResourceLocator)} to be available and there are: {allLocators.Count}."));
                    }

                    var result = allLocators.Single().ConfirmAndConvert<MemoryDatabaseLocator>();

                    this.singleLocator = result;
                    return this.singleLocator;
                }
            }
        }

        private List<StreamRecordHandlingEntry> GetStreamRecordHandlingEntriesForConcern(
            IResourceLocator locator,
            string concern)
        {
            if (locator == null)
            {
                throw new ArgumentNullException(nameof(locator));
            }

            if (!(locator is MemoryDatabaseLocator memoryDatabaseLocator))
            {
                throw new ArgumentException(Invariant($"Only {nameof(MemoryDatabaseLocator)}'s are supported; specified type: {locator.GetType().ToStringReadable()} - {locator.ToString()}"), nameof(locator));
            }

            lock (this.handlingLock)
            {
                if (!this.locatorToHandlingEntriesByConcernMap.ContainsKey(memoryDatabaseLocator))
                {
                    var newConcernToEntriesMap = new Dictionary<string, List<StreamRecordHandlingEntry>>();
                    this.locatorToHandlingEntriesByConcernMap.Add(memoryDatabaseLocator, newConcernToEntriesMap);
                }

                if (!this.locatorToHandlingEntriesByConcernMap[memoryDatabaseLocator].ContainsKey(concern))
                {
                    this.locatorToHandlingEntriesByConcernMap[memoryDatabaseLocator].Add(concern, new List<StreamRecordHandlingEntry>());
                }

                var entries = this.locatorToHandlingEntriesByConcernMap[memoryDatabaseLocator][concern];

                return entries;
            }
        }

        /// <inheritdoc />
        public override void Execute(
            BlockRecordHandlingOp operation)
        {
            var concern = Concerns.RecordHandlingConcern;
            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
            foreach (var locator in allLocators)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, concern);
                var mostRecentEntry = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault();
                if (mostRecentEntry != null && mostRecentEntry.Metadata.Status == HandlingStatus.Blocked)
                {
                    throw new InvalidOperationException(Invariant($"Cannot block when a block already is in place that does not have a cancel; most Recent Entry is: {mostRecentEntry?.ToString()}."));
                }

                var utcNow = DateTime.UtcNow;
                var blockEvent = new BlockedRecordHandlingEvent(operation.Details, utcNow);
                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                var payload =
                    blockEvent.ToDescribedSerializationUsingSpecificFactory(
                        this.DefaultSerializerRepresentation,
                        this.SerializerFactory,
                        this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    Concerns.GlobalBlockingRecordId,
                    concern,
                    HandlingStatus.Blocked,
                    null,
                    this.DefaultSerializerRepresentation,
                    NullStreamIdentifier.TypeRepresentation,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    null,
                    utcNow,
                    blockEvent.TimestampUtc);

                this.WriteHandlingEntryToMemoryMap(locator, entryId, concern, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CancelBlockedRecordHandlingOp operation)
        {
            var concern = Concerns.RecordHandlingConcern;

            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
            foreach (var locator in allLocators)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, Concerns.RecordHandlingConcern);
                var mostRecentEntry = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault();
                if (mostRecentEntry == null || mostRecentEntry.Metadata.Status != HandlingStatus.Blocked)
                {
                    throw new InvalidOperationException(Invariant($"Cannot cancel a block that does not exist; most Recent Entry is: {mostRecentEntry?.ToString() ?? "<null>"}."));
                }

                var utcNow = DateTime.UtcNow;
                var blockEvent = new CanceledBlockedRecordHandlingEvent(operation.Details, utcNow);
                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                var payload =
                    blockEvent.ToDescribedSerializationUsingSpecificFactory(
                        this.DefaultSerializerRepresentation,
                        this.SerializerFactory,
                        this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    Concerns.GlobalBlockingRecordId,
                    Concerns.RecordHandlingConcern,
                    HandlingStatus.Requested,
                    null,
                    this.DefaultSerializerRepresentation,
                    NullStreamIdentifier.TypeRepresentation,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    null,
                    utcNow,
                    blockEvent.TimestampUtc);

                this.WriteHandlingEntryToMemoryMap(locator, entryId, concern, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CancelHandleRecordExecutionRequestOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, operation.Concern);
                var mostRecent = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault(_ => _.Metadata.Status == HandlingStatus.Requested && _.Metadata.InternalRecordId == operation.Id);
                if (mostRecent == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot cancel a requested {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                var timestamp = DateTime.UtcNow;
                var newEvent = new CanceledRequestedHandleRecordExecutionEvent(operation.Id, operation.Details, timestamp);
                var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                    this.DefaultSerializerRepresentation,
                    this.SerializerFactory,
                    this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    operation.Id,
                    operation.Concern,
                    HandlingStatus.Canceled,
                    mostRecent.Metadata.StringSerializedId,
                    payload.SerializerRepresentation,
                    mostRecent.Metadata.TypeRepresentationOfId,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    timestamp,
                    newEvent.TimestampUtc);

                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CancelRunningHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, operation.Concern);
                var mostRecent = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault(_ => _.Metadata.InternalRecordId == operation.Id);
                if (mostRecent == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot cancel a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                if (mostRecent.Metadata.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot cancel a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

                var timestamp = DateTime.UtcNow;
                var newEvent = new CanceledRunningHandleRecordExecutionEvent(operation.Id, operation.Details, timestamp);
                var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                    this.DefaultSerializerRepresentation,
                    this.SerializerFactory,
                    this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    operation.Id,
                    operation.Concern,
                    HandlingStatus.CanceledRunning,
                    mostRecent.Metadata.StringSerializedId,
                    payload.SerializerRepresentation,
                    mostRecent.Metadata.TypeRepresentationOfId,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    timestamp,
                    newEvent.TimestampUtc);

                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CompleteRunningHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, operation.Concern);
                var mostRecent = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault(_ => _.Metadata.InternalRecordId == operation.Id);
                if (mostRecent == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot complete a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                if (mostRecent.Metadata.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot complete a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

                var timestamp = DateTime.UtcNow;
                var newEvent = new CompletedHandleRecordExecutionEvent(operation.Id, timestamp);
                var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                    this.DefaultSerializerRepresentation,
                    this.SerializerFactory,
                    this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    operation.Id,
                    operation.Concern,
                    HandlingStatus.Completed,
                    mostRecent.Metadata.StringSerializedId,
                    payload.SerializerRepresentation,
                    mostRecent.Metadata.TypeRepresentationOfId,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    timestamp,
                    newEvent.TimestampUtc);

                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            FailRunningHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, operation.Concern);
                var mostRecent = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault(_ => _.Metadata.InternalRecordId == operation.Id);
                if (mostRecent == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot fail a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                if (mostRecent.Metadata.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot fail a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

                var timestamp = DateTime.UtcNow;
                var newEvent = new FailedHandleRecordExecutionEvent(operation.Id, operation.Details, timestamp);
                var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                    this.DefaultSerializerRepresentation,
                    this.SerializerFactory,
                    this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    operation.Id,
                    operation.Concern,
                    HandlingStatus.Failed,
                    mostRecent.Metadata.StringSerializedId,
                    payload.SerializerRepresentation,
                    mostRecent.Metadata.TypeRepresentationOfId,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    timestamp,
                    newEvent.TimestampUtc);

                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            SelfCancelRunningHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, operation.Concern);
                var mostRecent = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault(_ => _.Metadata.InternalRecordId == operation.Id);
                if (mostRecent == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot self cancel a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                if (mostRecent.Metadata.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot self cancel a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

                var timestamp = DateTime.UtcNow;
                var newEvent = new SelfCanceledRunningHandleRecordExecutionEvent(operation.Id, operation.Details, timestamp);
                var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                    this.DefaultSerializerRepresentation,
                    this.SerializerFactory,
                    this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    operation.Id,
                    operation.Concern,
                    HandlingStatus.SelfCanceledRunning,
                    mostRecent.Metadata.StringSerializedId,
                    payload.SerializerRepresentation,
                    mostRecent.Metadata.TypeRepresentationOfId,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    timestamp,
                    newEvent.TimestampUtc);

                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            RetryFailedHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, operation.Concern);
                var mostRecent = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault(_ => _.Metadata.InternalRecordId == operation.Id);
                if (mostRecent == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot retry a failed {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                if (mostRecent.Metadata.Status != HandlingStatus.Failed)
                {
                    throw new InvalidOperationException(Invariant($"Cannot retry non-failed execution of {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

                var timestamp = DateTime.UtcNow;
                var newEvent = new RetryFailedHandleRecordExecutionEvent(operation.Id, operation.Details, timestamp);
                var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                    this.DefaultSerializerRepresentation,
                    this.SerializerFactory,
                    this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    operation.Id,
                    operation.Concern,
                    HandlingStatus.RetryFailed,
                    mostRecent.Metadata.StringSerializedId,
                    payload.SerializerRepresentation,
                    mostRecent.Metadata.TypeRepresentationOfId,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    timestamp,
                    newEvent.TimestampUtc);

                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
            }
        }

        private void WriteHandlingEntryToMemoryMap(
            IResourceLocator locator,
            long requestedEntryId,
            string concern,
            StreamRecordHandlingEntryMetadata requestedMetadata,
            DescribedSerializationBase requestedPayload)
        {
            lock (this.handlingLock)
            {
                // do not need this call but it has the confirm key path exists logic and i do not want to refactor yet another method for them to share...
                this.GetStreamRecordHandlingEntriesForConcern(locator, concern);

                // above will throw if this cast would...
                var memoryLocator = (MemoryDatabaseLocator)locator;

                // the reference would get broken in non-obvious ways when using variables so direct keying the map.
                this.locatorToHandlingEntriesByConcernMap[memoryLocator][concern]
                    .Add(new StreamRecordHandlingEntry(requestedEntryId, requestedMetadata, requestedPayload));
            }
        }
    }
}
