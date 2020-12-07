// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// In memory implementation of <see cref="IReadWriteStream"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class MemoryReadWriteStream :
        ReadWriteStreamBase,
        IStreamManagementProtocolFactory,
        IStreamRecordHandlingProtocolFactory,
        IStreamManagementProtocols,
        IReturningProtocol<GetNextUniqueLongOp, long>,
        IReturningProtocol<GetLatestRecordOp, StreamRecord>,
        IReturningProtocol<GetLatestRecordByIdOp, StreamRecord>,
        IReturningProtocol<GetHandlingHistoryOfRecordOp, IReadOnlyList<StreamRecordHandlingEntry>>,
        IReturningProtocol<GetHandlingStatusOfRecordsByIdOp, HandlingStatus>,
        IReturningProtocol<GetHandlingStatusOfRecordSetByTagOp, HandlingStatus>,
        IReturningProtocol<TryHandleRecordOp, StreamRecord>,
        IReturningProtocol<PutRecordOp, long>,
        IVoidProtocol<BlockRecordHandlingOp>,
        IVoidProtocol<CancelBlockedRecordHandlingOp>,
        IVoidProtocol<CancelHandleRecordExecutionRequestOp>,
        IVoidProtocol<CancelRunningHandleRecordExecutionOp>,
        IVoidProtocol<CompleteRunningHandleRecordExecutionOp>,
        IVoidProtocol<FailRunningHandleRecordExecutionOp>,
        IVoidProtocol<SelfCancelRunningHandleRecordExecutionOp>
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
        public MemoryReadWriteStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory)
        : base(name, new SingleResourceLocatorProtocol(new MemoryDatabaseLocator(name)))
        {
            this.DefaultSerializerRepresentation = defaultSerializerRepresentation ?? throw new ArgumentNullException(nameof(defaultSerializerRepresentation));

            if (defaultSerializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentException(Invariant($"Cannot specify a {nameof(SerializationFormat)} of {SerializationFormat.Invalid}."));
            }

            this.DefaultSerializationFormat = defaultSerializationFormat;
            this.SerializerFactory = serializerFactory ?? throw new ArgumentNullException(nameof(serializerFactory));
            this.Id = Guid.NewGuid().ToString().ToUpperInvariant();
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation => new MemoryStreamRepresentation(this.Name, this.Id);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the serializer factory.
        /// </summary>
        /// <value>The serializer factory.</value>
        public ISerializerFactory SerializerFactory { get; private set; }

        /// <summary>
        /// Gets the default serializer representation.
        /// </summary>
        /// <value>The default serializer representation.</value>
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <summary>
        /// Gets the default serialization format.
        /// </summary>
        /// <value>The default serialization format.</value>
        public SerializationFormat DefaultSerializationFormat { get; private set; }

        /// <inheritdoc />
        public void Execute(
            CreateStreamOp operation)
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
                            break;
                        default:
                            throw new NotSupportedException(
                                Invariant(
                                    $"Operation {nameof(operation.ExistingStreamEncounteredStrategy)} of '{operation.ExistingStreamEncounteredStrategy}' is not supported."));
                    }
                }

                this.created = true;
            }
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CreateStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        /// <inheritdoc />
        public void Execute(
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

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DeleteStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        /// <inheritdoc />
        public override IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>() => new MemoryStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public override IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>() => new MemoryStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols GetStreamWritingProtocols() => new MemoryStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamReadProtocols GetStreamReadingProtocols() => new MemoryStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>() => new MemoryStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>() => new MemoryStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public override IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>() => new MemoryStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            var result = new MemoryStreamReadWriteProtocols<TObject>(this);
            return result;
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>System.Int64.</returns>
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var result = Interlocked.Increment(ref this.uniqueLongForExternalProtocol);
            return result;
        }

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols() => this;

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols() => new MemoryStreamRecordHandlingProtocols(this);

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>() => new MemoryStreamRecordHandlingProtocols<TObject>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>() => new MemoryStreamRecordWithIdHandlingProtocols<TId>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>() => new MemoryStreamRecordWithIdHandlingProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordDateOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                var newList = this.locatorToRecordPartitionMap[memoryDatabaseLocator].Where(_ => _.Metadata.TimestampUtc >= operation.MaxInternalRecordDate);
                this.locatorToRecordPartitionMap[memoryDatabaseLocator].Clear();
                this.locatorToRecordPartitionMap[memoryDatabaseLocator].AddRange(newList);
            }
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordDateOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                var newList = this.locatorToRecordPartitionMap[memoryDatabaseLocator].Where(_ => _.InternalRecordId >= operation.MaxInternalRecordId);
                this.locatorToRecordPartitionMap[memoryDatabaseLocator].Clear();
                this.locatorToRecordPartitionMap[memoryDatabaseLocator].AddRange(newList);
            }
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordIdOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(
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
        public HandlingStatus Execute(
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
                                                             operation.TypeVersionMatchStrategy),
                                                         operation.TypeVersionMatchStrategy)))
                                      .Select(_ => _.Metadata.Status)
                                      .ToList();
                var result = statuses.ReduceToCompositeHandlingStatus(operation.HandlingStatusCompositionStrategy);
                return result;
            }
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
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
                                               _ => operation.TagsToMatch.FuzzyMatchAccordingToStrategy(_.Metadata.Tags, operation.TagMatchStrategy))
                                          .Select(_ => _.Metadata.Status)
                                          .ToList();

                    statuses.AddRange(statusesForLocator);
                }

                var result = statuses.ReduceToCompositeHandlingStatus(operation.HandlingStatusCompositionStrategy);
                return result;
            }
        }

        /// <inheritdoc />
        public StreamRecord Execute(
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
                        return null;
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
                                                      operation.TypeVersionMatchStrategy))
                                             .ToList();
                        var recordToHandle = matchingRecords.FirstOrDefault();
                        if (recordToHandle != null)
                        {
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
                                    operation.Tags,
                                    requestedTimestamp,
                                    requestedEvent.TimestampUtc);

                                var requestedEntryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                                entries.Add(new StreamRecordHandlingEntry(requestedEntryId, requestedMetadata, requestedPayload));
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
                                operation.Tags,
                                runningTimestamp,
                                runningEvent.TimestampUtc);

                            var runningEntryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                            entries.Add(new StreamRecordHandlingEntry(runningEntryId, runningMetadata, runningPayload));

                            return recordToHandle;
                        }
                    }
                }

                return null;
            }
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            GetLatestRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                var result =
                    this.locatorToRecordPartitionMap[memoryDatabaseLocator].OrderByDescending(_ => _.InternalRecordId)
                           .FirstOrDefault(
                                _ => _.Metadata.FuzzyMatchTypes(
                                    operation.IdentifierType,
                                    operation.ObjectType,
                                    operation.TypeVersionMatchStrategy));
                return result;
            }
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            GetLatestRecordByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                var result =
                    this.locatorToRecordPartitionMap[memoryDatabaseLocator].OrderByDescending(_ => _.InternalRecordId)
                           .FirstOrDefault(
                                _ => _.Metadata.FuzzyMatchTypesAndId(
                                    operation.StringSerializedId,
                                    operation.IdentifierType,
                                    operation.ObjectType,
                                    operation.TypeVersionMatchStrategy));

                return result;
            }
        }

        /// <inheritdoc />
        public long Execute(
            PutRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                var exists = this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var recordPartition);
                if (!exists)
                {
                    recordPartition = new List<StreamRecord>();
                    this.locatorToRecordPartitionMap.Add(memoryDatabaseLocator, recordPartition);
                }

                var id = Interlocked.Increment(ref this.uniqueLongForInMemoryRecords);
                var itemToAdd = new StreamRecord(id, operation.Metadata, operation.Payload);
                recordPartition.Add(itemToAdd);
                return id;
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

            var foundLocator = this.locatorToHandlingEntriesByConcernMap.TryGetValue(memoryDatabaseLocator, out var concernToEntriesMap);
            if (!foundLocator)
            {
                concernToEntriesMap = new Dictionary<string, List<StreamRecordHandlingEntry>>();
                this.locatorToHandlingEntriesByConcernMap.Add(memoryDatabaseLocator, concernToEntriesMap);
            }

            var foundConcern = concernToEntriesMap.TryGetValue(concern, out var entries);
            if (!foundConcern)
            {
                entries = new List<StreamRecordHandlingEntry>();
                concernToEntriesMap.Add(concern, entries);
            }

            return entries;
        }

        /// <inheritdoc />
        public void Execute(
            BlockRecordHandlingOp operation)
        {
            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
            foreach (var locator in allLocators)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, Concerns.RecordHandlingConcern);
                var mostRecentEntry = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault();
                if (mostRecentEntry != null && mostRecentEntry.Metadata.Status == HandlingStatus.Blocked)
                {
                    throw new InvalidOperationException(Invariant($"Cannot block when a block already is in place that does not have a cancel; most Recent Entry is: {mostRecentEntry?.ToString()}."));
                }

                var utcNow = DateTime.UtcNow;
                var blockEvent = new BlockedRecordHandlingEvent(operation.Details, utcNow);
                var id = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                var blockedPayload =
                    blockEvent.ToDescribedSerializationUsingSpecificFactory(
                        this.DefaultSerializerRepresentation,
                        this.SerializerFactory,
                        this.DefaultSerializationFormat);

                var blockedMetadata = new StreamRecordHandlingEntryMetadata(
                    0,
                    Concerns.RecordHandlingConcern,
                    HandlingStatus.Blocked,
                    null,
                    this.DefaultSerializerRepresentation,
                    NullStreamIdentifier.TypeRepresentation,
                    blockedPayload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    null,
                    utcNow,
                    blockEvent.TimestampUtc);

                var blockEntry = new StreamRecordHandlingEntry(id, blockedMetadata, blockedPayload);
                entries.Add(blockEntry);
            }
        }

        /// <inheritdoc />
        public void Execute(
            CancelBlockedRecordHandlingOp operation)
        {
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
                var id = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                var blockedPayload =
                    blockEvent.ToDescribedSerializationUsingSpecificFactory(
                        this.DefaultSerializerRepresentation,
                        this.SerializerFactory,
                        this.DefaultSerializationFormat);

                var blockedMetadata = new StreamRecordHandlingEntryMetadata(
                    0,
                    Concerns.RecordHandlingConcern,
                    HandlingStatus.Requested,
                    null,
                    this.DefaultSerializerRepresentation,
                    NullStreamIdentifier.TypeRepresentation,
                    blockedPayload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    null,
                    utcNow,
                    blockEvent.TimestampUtc);

                var blockEntry = new StreamRecordHandlingEntry(id, blockedMetadata, blockedPayload);
                entries.Add(blockEntry);
            }
        }

        /// <inheritdoc />
        public void Execute(
            CancelHandleRecordExecutionRequestOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
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
                entries.Add(new StreamRecordHandlingEntry(entryId, metadata, payload));
            }
        }

        /// <inheritdoc />
        public void Execute(
            CancelRunningHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
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
                entries.Add(new StreamRecordHandlingEntry(entryId, metadata, payload));
            }
        }

        /// <inheritdoc />
        public void Execute(
            CompleteRunningHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
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
                entries.Add(new StreamRecordHandlingEntry(entryId, metadata, payload));
            }
        }

        /// <inheritdoc />
        public void Execute(
            FailRunningHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
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
                entries.Add(new StreamRecordHandlingEntry(entryId, metadata, payload));
            }
        }

        /// <inheritdoc />
        public void Execute(
            SelfCancelRunningHandleRecordExecutionOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
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
                entries.Add(new StreamRecordHandlingEntry(entryId, metadata, payload));
            }
        }
    }
}
