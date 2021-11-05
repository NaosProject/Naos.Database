// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.cs" company="Naos Project">
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
    /// In memory implementation of <see cref="StandardStreamBase"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class MemoryStandardStream :
        StandardStreamBase
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
        /// Initializes a new instance of the <see cref="MemoryStandardStream"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        /// <param name="resourceLocatorProtocols">The optional resource locator protocols; DEFAULT will be a single <see cref="MemoryDatabaseLocator"/> named 'Default'.</param>
        public MemoryStandardStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            IResourceLocatorProtocols resourceLocatorProtocols = null)
        : base(name, serializerFactory, defaultSerializerRepresentation, defaultSerializationFormat, resourceLocatorProtocols ?? new SingleResourceLocatorProtocols(new MemoryDatabaseLocator("Default")))
        {
            this.Id = Guid.NewGuid().ToString().ToUpperInvariant();
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation => new MemoryStreamRepresentation(this.Name, this.Id);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id { get; private set; }

        /// <inheritdoc />
        public override void Execute(
            StandardUpdateHandlingStatusForStreamOp operation)
        {
            var newStatus = operation.NewStatus;
            var concern = Concerns.RecordHandlingConcern;
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();
            var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, concern);
            var mostRecentEntry = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault();
            var currentStatus = mostRecentEntry?.Metadata.Status ?? HandlingStatus.AvailableByDefault;

            var expectedStatus = newStatus == HandlingStatus.DisabledForStream
                ? HandlingStatus.AvailableByDefault
                : HandlingStatus.DisabledForStream;

            if (currentStatus != expectedStatus)
            {
                throw new InvalidOperationException(Invariant($"Cannot update status as expected status does not match; expected {expectedStatus} found {mostRecentEntry?.Metadata.Status.ToString() ?? "<null entry>"}."));
            }

            var utcNow = DateTime.UtcNow;
            IEvent statusEvent;
            switch (newStatus)
            {
                case HandlingStatus.DisabledForStream:
                    statusEvent = new HandlingForStreamDisabledEvent(utcNow, operation.Details);
                    break;
                case HandlingStatus.AvailableByDefault:
                    statusEvent = new HandlingForStreamEnabledEvent(utcNow, operation.Details);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(HandlingStatus)} {newStatus} is not supported."));
            }

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var payload =
                statusEvent.ToDescribedSerializationUsingSpecificFactory(
                    this.DefaultSerializerRepresentation,
                    this.SerializerFactory,
                    this.DefaultSerializationFormat);

            var metadata = new StreamRecordHandlingEntryMetadata(
                Concerns.GlobalBlockingRecordId,
                concern,
                newStatus,
                null,
                this.DefaultSerializerRepresentation,
                NullStreamIdentifier.TypeRepresentation,
                payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                utcNow,
                statusEvent.TimestampUtc);

            this.WriteHandlingEntryToMemoryMap(locator, entryId, concern, metadata, payload);
        }

        /// <inheritdoc />
        public override StandardCreateStreamResult Execute(
            StandardCreateStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var alreadyExisted = this.created;
            var wasCreated = true;

            lock (this.streamLock)
            {
                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryStandardStream)} can only 'create' a stream with the same representation."));
                }

                if (this.created)
                {
                    switch (operation.ExistingStreamStrategy)
                    {
                        case ExistingStreamStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {operation.StreamRepresentation} to not exist, it does and the operation {nameof(operation.ExistingStreamStrategy)} is set to '{operation.ExistingStreamStrategy}'."));
                        case ExistingStreamStrategy.Overwrite:
                            this.locatorToRecordPartitionMap.Clear();
                            break;
                        case ExistingStreamStrategy.Skip:
                            wasCreated = false;
                            break;
                        default:
                            throw new NotSupportedException(
                                Invariant(
                                    $"{nameof(ExistingStreamStrategy)} '{operation.ExistingStreamStrategy}' is not supported."));
                    }
                }

                this.created = true;
            }

            var result = new StandardCreateStreamResult(alreadyExisted, wasCreated);
            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            StandardDeleteStreamOp operation)
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
                    throw new ArgumentException(Invariant($"This {nameof(MemoryStandardStream)} can only 'Delete' a stream with the same representation."));
                }

                if (!this.created)
                {
                    switch (operation.StreamNotFoundStrategy)
                    {
                        case StreamNotFoundStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {operation.StreamRepresentation} to exist, it does not and the operation {nameof(operation.StreamNotFoundStrategy)} is '{operation.StreamNotFoundStrategy}'."));
                        case StreamNotFoundStrategy.Skip:
                            break;
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(StreamNotFoundStrategy)} {operation.StreamNotFoundStrategy} is not supported."));
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
        public override void Execute(
            StandardPruneStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            bool RecordPredicate(
                StreamRecord record) => operation.ShouldInclude(record.InternalRecordId, record.Metadata.TimestampUtc);

            bool HandlingPredicate(
                StreamRecordHandlingEntry handlingEntry) => operation.ShouldInclude(
                handlingEntry.Metadata.InternalRecordId,
                handlingEntry.Metadata.TimestampUtc);

            lock (this.streamLock)
            {
                var newList = this.locatorToRecordPartitionMap[locator].Where(RecordPredicate);
                this.locatorToRecordPartitionMap[locator].Clear();
                this.locatorToRecordPartitionMap[locator].AddRange(newList);

                lock (this.handlingLock)
                {
                    if (this.locatorToHandlingEntriesByConcernMap.ContainsKey(locator) && this.locatorToHandlingEntriesByConcernMap[locator].Any())
                    {
                        foreach (var concern in this.locatorToHandlingEntriesByConcernMap[locator].Keys)
                        {
                            var newHandlingList =
                                this.locatorToHandlingEntriesByConcernMap[locator][concern]
                                    .Where(HandlingPredicate);
                            this.locatorToHandlingEntriesByConcernMap[locator][concern].Clear();
                            this.locatorToHandlingEntriesByConcernMap[locator][concern].AddRange(newHandlingList);
                        }
                    }
                }
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
