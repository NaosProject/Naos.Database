// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.Handling.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    public partial class MemoryStandardStream
    {
        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordHandlingEntry> Execute(
            StandardGetHandlingHistoryOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var recordBlockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingDisabledConcern);

                var entriesForInternalRecordId =
                    (entries ?? new List<StreamRecordHandlingEntry>())
                   .Concat(recordBlockedEntries ?? new List<StreamRecordHandlingEntry>())
                   .Where(_ => _.Metadata.InternalRecordId == operation.InternalRecordId)
                   .ToList();
                return entriesForInternalRecordId;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public override IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();
            lock (this.handlingLock)
            {
                var blockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.StreamHandlingDisabledConcern);
                if (blockedEntries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault()?.Metadata.Status == HandlingStatus.DisabledForStream)
                {
                    return new Dictionary<long, HandlingStatus>
                           {
                               { Concerns.GlobalBlockingRecordId, HandlingStatus.DisabledForStream },
                           };
                }

                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var recordBlockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingDisabledConcern);

                bool HandlingEntryMatchingPredicate(
                    StreamRecordHandlingEntry entry)
                {
                    var matchResult = false;

                    if (operation.RecordFilter.InternalRecordIds != null && operation.RecordFilter.InternalRecordIds.Any())
                    {
                        matchResult = matchResult || operation.RecordFilter.InternalRecordIds.Contains(entry.Metadata.InternalRecordId);
                    }

                    if (operation.RecordFilter.Ids != null && operation.RecordFilter.Ids.Any())
                    {
                        matchResult = matchResult || operation.RecordFilter.Ids.Any(
                            __ => __.StringSerializedId.Equals(entry.Metadata.StringSerializedId)
                               && __.IdentifierType.EqualsAccordingToStrategy(
                                      entry.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                                          operation.RecordFilter.VersionMatchStrategy),
                                      operation.RecordFilter.VersionMatchStrategy));
                    }

                    if (operation.RecordFilter.IdTypes != null && operation.RecordFilter.IdTypes.Any())
                    {
                        matchResult = matchResult
                                   || operation.RecordFilter.IdTypes.Contains(
                                          entry.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                                              operation.RecordFilter.VersionMatchStrategy));
                    }

                    if (operation.RecordFilter.ObjectTypes != null && operation.RecordFilter.ObjectTypes.Any())
                    {
                        matchResult = matchResult
                                   || operation.RecordFilter.ObjectTypes.Contains(
                                          entry.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(
                                              operation.RecordFilter.VersionMatchStrategy));
                    }

                    if (operation.RecordFilter.Tags != null && operation.RecordFilter.Tags.Any())
                    {
                        matchResult = matchResult || entry.Metadata.Tags.FuzzyMatchTags(operation.RecordFilter.Tags, operation.RecordFilter.TagMatchStrategy);
                    }

                    return matchResult;
                }

                var result =
                    (entries ?? new List<StreamRecordHandlingEntry>())
                   .Concat(recordBlockedEntries ?? new List<StreamRecordHandlingEntry>())
                   .Where(HandlingEntryMatchingPredicate)
                   .GroupBy(_ => _.Metadata.InternalRecordId)
                   .Select(_ => new Tuple<long, HandlingStatus>(_.Key, _.OrderByDescending(__ => __.InternalHandlingEntryId).First().Metadata.Status))
                   .ToDictionary(k => k.Item1, v => v.Item2);

                return result;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public override TryHandleRecordResult Execute(
            StandardTryHandleRecordOp operation)
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

                    var handlingEntries = this.GetStreamRecordHandlingEntriesForConcern(locator, Concerns.StreamHandlingDisabledConcern);
                    var mostRecentEntry = handlingEntries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault();
                    if (mostRecentEntry != null && mostRecentEntry.Metadata.Status == HandlingStatus.DisabledForStream)
                    {
                        return new TryHandleRecordResult(null, true);
                    }

                    var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern).ToList();
                    var recordBlockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingDisabledConcern).ToList();

                    var existingInternalRecordIdsToConsider = new List<long>();
                    var existingInternalRecordIdsToIgnore = new List<long>();
                    var mergedEntries = entries.Concat(recordBlockedEntries);
                    foreach (var groupedById in mergedEntries.GroupBy(_ => _.Metadata.InternalRecordId))
                    {
                        var mostRecent = groupedById.OrderByDescending(_ => _.InternalHandlingEntryId).First();

                        if (mostRecent.Metadata.Status.IsAvailable())
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
                                                  _ => _.Metadata.FuzzyMatchTypes(
                                                      operation.IdentifierType,
                                                      operation.ObjectType,
                                                      operation.VersionMatchStrategy))
                                             .ToList();

                        if ((operation.TagsToMatch != null) && operation.TagsToMatch.Any())
                        {
                            matchingRecords = matchingRecords
                                .Where(_ => _.Metadata.Tags.FuzzyMatchTags(operation.TagsToMatch, operation.TagMatchStrategy))
                                .ToList();
                        }

                        StreamRecord recordToHandle;
                        switch (operation.OrderRecordsBy)
                        {
                            case OrderRecordsBy.InternalRecordIdAscending:
                                recordToHandle = matchingRecords
                                                .OrderBy(_ => _.InternalRecordId)
                                                .FirstOrDefault(
                                                     _ => operation.MinimumInternalRecordId == null
                                                       || _.InternalRecordId >= operation.MinimumInternalRecordId);
                                break;
                            case OrderRecordsBy.InternalRecordIdDescending:
                                recordToHandle = matchingRecords
                                                .OrderByDescending(_ => _.InternalRecordId)
                                                .FirstOrDefault(
                                                     _ => operation.MinimumInternalRecordId == null
                                                       || _.InternalRecordId >= operation.MinimumInternalRecordId);
                                break;
                            case OrderRecordsBy.Random:
                                recordToHandle = matchingRecords
                                                .OrderByDescending(_ => Guid.NewGuid())
                                                .FirstOrDefault(
                                                     _ => operation.MinimumInternalRecordId == null
                                                       || _.InternalRecordId >= operation.MinimumInternalRecordId);
                                break;
                            default:
                                throw new NotSupportedException(Invariant($"{nameof(OrderRecordsBy)} {operation.OrderRecordsBy} is not supported."));
                        }

                        if (recordToHandle != null)
                        {
                            var handlingTags = operation.InheritRecordTags
                                ? (operation.Tags ?? new List<NamedValue<string>>())
                                 .Union(recordToHandle.Metadata.Tags ?? new List<NamedValue<string>>())
                                 .ToList()
                                : operation.Tags;

                            if (!existingInternalRecordIdsToConsider.Contains(recordToHandle.InternalRecordId))
                            {
                                // first time needs a requested record
                                var requestedTimestamp = DateTime.UtcNow;
                                var requestedEvent = new RecordHandlingAvailableEvent(
                                    recordToHandle.InternalRecordId,
                                    operation.Concern,
                                    recordToHandle,
                                    requestedTimestamp);

                                var requestedPayload = requestedEvent.ToDescribedSerializationUsingSpecificFactory(
                                    this.DefaultSerializerRepresentation,
                                    this.SerializerFactory,
                                    this.DefaultSerializationFormat);

                                var requestedMetadata = new StreamRecordHandlingEntryMetadata(
                                    recordToHandle.InternalRecordId,
                                    operation.Concern,
                                    HandlingStatus.AvailableByDefault,
                                    recordToHandle.Metadata.StringSerializedId,
                                    requestedPayload.SerializerRepresentation,
                                    recordToHandle.Metadata.TypeRepresentationOfId,
                                    requestedPayload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                                    handlingTags,
                                    requestedTimestamp);

                                var requestedEntryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                                this.WriteHandlingEntryToMemoryMap(memoryDatabaseLocator, requestedEntryId, operation.Concern, requestedMetadata, requestedPayload);
                            }

                            var runningTimestamp = DateTime.UtcNow;

                            var runningEvent = new RecordHandlingRunningEvent(
                                recordToHandle.InternalRecordId,
                                operation.Concern,
                                runningTimestamp);

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
                                runningTimestamp);

                            var runningEntryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);

                            this.WriteHandlingEntryToMemoryMap(memoryDatabaseLocator, runningEntryId, operation.Concern, runningMetadata, runningPayload);

                            switch (operation.StreamRecordItemsToInclude)
                            {
                                case StreamRecordItemsToInclude.MetadataAndPayload:
                                    break;
                                case StreamRecordItemsToInclude.MetadataOnly:
                                    recordToHandle = recordToHandle.DeepCloneWithPayload(new NullDescribedSerialization(recordToHandle.Payload.PayloadTypeRepresentation, recordToHandle.Payload.SerializerRepresentation));
                                    break;
                                default:
                                    throw new NotSupportedException(Invariant($"This {nameof(StreamRecordItemsToInclude)} is not supported: {operation.StreamRecordItemsToInclude}."));
                            }

                            var result = new TryHandleRecordResult(recordToHandle);

                            return result;
                        }
                    }
                }

                return new TryHandleRecordResult(null);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();
            lock (this.handlingLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var mostRecent = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault(_ => _.Metadata.InternalRecordId == operation.InternalRecordId);

                var currentHandlingStatus = mostRecent?.Metadata.Status ?? HandlingStatus.AvailableByDefault;
                if (!operation.AcceptableCurrentStatuses.Contains(currentHandlingStatus))
                {
                    var acceptableStatusesCsvString = string.Join(",", operation.AcceptableCurrentStatuses);
                    throw new InvalidOperationException(
                        Invariant($"Expected status to be one of [{acceptableStatusesCsvString}] but found '{currentHandlingStatus}'."));
                }

                switch (operation.NewStatus)
                {
                    case HandlingStatus.DisabledForRecord:
                        this.MakeHandlingDisabledForRecord(operation, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterExternalCancellation:
                        this.MakeHandlingAvailableAfterExternalCancellation(operation, mostRecent);
                        break;
                    case HandlingStatus.Completed:
                        this.MakeHandlingCompleted(operation, mostRecent);
                        break;
                    case HandlingStatus.Failed:
                        this.MakeHandlingFailed(operation, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterSelfCancellation:
                        this.MakeHandlingAvailableAfterSelfCancellation(operation, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterFailure:
                        this.MakeHandlingAvailableAfterFailure(operation, mostRecent);
                        break;
                    default:
                        throw new NotSupportedException(Invariant($"Unsupported {nameof(HandlingStatus)} '{operation.NewStatus}' to update handling of {nameof(operation.InternalRecordId)}: {operation.InternalRecordId}."));
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            StandardUpdateHandlingStatusForStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var newStatus = operation.NewStatus;
            var concern = Concerns.StreamHandlingDisabledConcern;
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
                null, // Since we need a type, we are using NullIdentifier, however we are passing a null NullIdentifier instead of constructing one to reduce runtime complexity and payload size
                this.DefaultSerializerRepresentation,
                NullIdentifier.TypeRepresentation,
                payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                utcNow);

            this.WriteHandlingEntryToMemoryMap(locator, entryId, concern, metadata, payload);
        }

        private void MakeHandlingAvailableAfterFailure(
            StandardUpdateHandlingStatusForRecordOp operation,
            StreamRecordHandlingEntry mostRecent)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;
            var newEvent = new RecordHandlingFailureResetEvent(
                operation.InternalRecordId,
                operation.Concern,
                timestamp,
                operation.Details);

            var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                this.DefaultSerializerRepresentation,
                this.SerializerFactory,
                this.DefaultSerializationFormat);

            var metadata = new StreamRecordHandlingEntryMetadata(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterFailure,
                mostRecent.Metadata.StringSerializedId,
                payload.SerializerRepresentation,
                mostRecent.Metadata.TypeRepresentationOfId,
                payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                timestamp);

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
        }

        private void MakeHandlingAvailableAfterSelfCancellation(
            StandardUpdateHandlingStatusForRecordOp operation,
            StreamRecordHandlingEntry mostRecent)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var newEvent = new RecordHandlingSelfCanceledEvent(
                operation.InternalRecordId,
                operation.Concern,
                timestamp,
                operation.Details);

            var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                this.DefaultSerializerRepresentation,
                this.SerializerFactory,
                this.DefaultSerializationFormat);

            var metadata = new StreamRecordHandlingEntryMetadata(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterSelfCancellation,
                mostRecent.Metadata.StringSerializedId,
                payload.SerializerRepresentation,
                mostRecent.Metadata.TypeRepresentationOfId,
                payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                timestamp);

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
        }

        private void MakeHandlingFailed(
            StandardUpdateHandlingStatusForRecordOp operation,
            StreamRecordHandlingEntry mostRecent)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var newEvent = new RecordHandlingFailedEvent(
                operation.InternalRecordId,
                operation.Concern,
                timestamp,
                operation.Details);

            var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                this.DefaultSerializerRepresentation,
                this.SerializerFactory,
                this.DefaultSerializationFormat);

            var metadata = new StreamRecordHandlingEntryMetadata(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.Failed,
                mostRecent.Metadata.StringSerializedId,
                payload.SerializerRepresentation,
                mostRecent.Metadata.TypeRepresentationOfId,
                payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                timestamp);

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
        }

        private void MakeHandlingCompleted(
            StandardUpdateHandlingStatusForRecordOp operation,
            StreamRecordHandlingEntry mostRecent)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var newEvent = new RecordHandlingCompletedEvent(
                operation.InternalRecordId,
                operation.Concern,
                timestamp);

            var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                this.DefaultSerializerRepresentation,
                this.SerializerFactory,
                this.DefaultSerializationFormat);

            var metadata = new StreamRecordHandlingEntryMetadata(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.Completed,
                mostRecent.Metadata.StringSerializedId,
                payload.SerializerRepresentation,
                mostRecent.Metadata.TypeRepresentationOfId,
                payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                timestamp);

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
        }

        private void MakeHandlingAvailableAfterExternalCancellation(
            StandardUpdateHandlingStatusForRecordOp operation,
            StreamRecordHandlingEntry mostRecent)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var newEvent = new RecordHandlingCanceledEvent(
                operation.InternalRecordId,
                operation.Concern,
                timestamp,
                operation.Details);

            var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                this.DefaultSerializerRepresentation,
                this.SerializerFactory,
                this.DefaultSerializationFormat);

            var metadata = new StreamRecordHandlingEntryMetadata(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterExternalCancellation,
                mostRecent.Metadata.StringSerializedId,
                payload.SerializerRepresentation,
                mostRecent.Metadata.TypeRepresentationOfId,
                payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                timestamp);

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
        }

        private void MakeHandlingDisabledForRecord(
            StandardUpdateHandlingStatusForRecordOp operation,
            StreamRecordHandlingEntry mostRecent)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var newEvent = new HandlingForRecordDisabledEvent(
                operation.InternalRecordId,
                operation.Concern,
                timestamp,
                operation.Details);

            var payload = newEvent.ToDescribedSerializationUsingSpecificFactory(
                this.DefaultSerializerRepresentation,
                this.SerializerFactory,
                this.DefaultSerializationFormat);

            string stringSerializedId;
            TypeRepresentationWithAndWithoutVersion identifierTypeRepresentationWithAndWithoutVersion;

            if (mostRecent != null)
            {
                stringSerializedId = mostRecent.Metadata.StringSerializedId;
                identifierTypeRepresentationWithAndWithoutVersion = mostRecent.Metadata.TypeRepresentationOfId;
            }
            else
            {
                var record = this.Execute(new StandardGetRecordByInternalRecordIdOp(operation.InternalRecordId));
                if (record == null)
                {
                    throw new ArgumentException(Invariant($"Specified {nameof(operation.InternalRecordId)}: {operation.InternalRecordId} does not exist in stream: {this.StreamRepresentation}."));
                }

                stringSerializedId = record.Metadata.StringSerializedId;
                identifierTypeRepresentationWithAndWithoutVersion = record.Metadata.TypeRepresentationOfId;
            }

            var metadata = new StreamRecordHandlingEntryMetadata(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.DisabledForRecord,
                stringSerializedId,
                payload.SerializerRepresentation,
                identifierTypeRepresentationWithAndWithoutVersion,
                payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                timestamp);

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            this.WriteHandlingEntryToMemoryMap(locator, entryId, operation.Concern, metadata, payload);
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
    }
}
