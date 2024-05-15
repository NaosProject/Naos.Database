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

            lock (this.streamLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var recordBlockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingDisabledConcern);

                var result = new StreamRecordHandlingEntry[0]
                    .Concat(entries)
                    .Concat(recordBlockedEntries)
                    .Where(_ => _.InternalRecordId == operation.InternalRecordId)
                    .ToList();

                return result;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public override IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                Dictionary<long, HandlingStatus> ApplyHandlingFilter(
                    Dictionary<long, HandlingStatus> handlingStatuses)
                {
                    if (operation.HandlingFilter.Tags != null)
                    {
                        throw new NotImplementedException(Invariant($"Filtering using {nameof(HandlingFilter)}.{nameof(HandlingFilter.Tags)} is not implemented."));
                    }

                    var dictionary = ((operation.HandlingFilter?.CurrentHandlingStatuses == null) ||
                                      (!operation.HandlingFilter.CurrentHandlingStatuses.Any()))
                        ? handlingStatuses
                        : handlingStatuses
                            .Where(_ => operation.HandlingFilter.CurrentHandlingStatuses.Contains(_.Value))
                            .ToDictionary(k => k.Key, v => v.Value);

                    return dictionary;
                }

                // If DisabledForStream is the most recent status for the StreamHandlingDisabledConcern concern
                // then the stream is disabled for handling; it doesn't matter what the handling status is for any
                // record.  At that point, all records are blocked from being handled and are considered to have a status of DisabledForStream.
                // It's not possible to handle any record until the AvailableByDefault status is set for the StreamHandlingDisabledConcern concern.
                var streamHandlingDisabledEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.StreamHandlingDisabledConcern);
                if (streamHandlingDisabledEntries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault()?.Status == HandlingStatus.DisabledForStream)
                {
                    return ApplyHandlingFilter(
                        new Dictionary<long, HandlingStatus>
                        {
                            { Concerns.GlobalBlockingRecordId, HandlingStatus.DisabledForStream },
                        });
                }

                // Short-circuit case where there are no records.
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var records);
                if (records == null)
                {
                    return new Dictionary<long, HandlingStatus>();
                }

                var handlingEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var recordHandlingDisabledEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingDisabledConcern);

                var filteredRecords = ApplyRecordFilterToPartition(operation.RecordFilter, records);
                var internalRecordIdsToConsider = new HashSet<long>(filteredRecords.Select(_ => _.InternalRecordId));

                ////var internalRecordIdsToConsider = new HashSet<long>();
                ////foreach (var record in records)
                ////{
                ////    if (operation.RecordFilter.IsEmptyRecordFilter() || HandlingEntryMatchingPredicate(record.InternalRecordId, record.Metadata, operation.RecordFilter))
                ////    {
                ////        internalRecordIdsToConsider.Add(record.InternalRecordId);
                ////    }
                ////}

                // todo: Filtering by handling status should be done in conjunction with filtering by record filter
                // (e.g. FirstOrDefault return some status that is subsequently filtered out, but the second status
                // would pass the filter).  What do we do in SQL?
                var unfilteredResult = internalRecordIdsToConsider
                   .ToDictionary(
                        k => k,
                        v => handlingEntries
                            .Concat(streamHandlingDisabledEntries) // this is needed to accomodate querying on Concerns.GlobalBlockingRecordId to check if AvailableByDefault
                            .Concat(recordHandlingDisabledEntries)
                            .Where(_ => _.InternalRecordId == v)
                            .OrderByDescending(__ => __.InternalHandlingEntryId)
                            .FirstOrDefault()
                           ?.Status
                          ?? HandlingStatus.AvailableByDefault);

                var result = ApplyHandlingFilter(unfilteredResult);

                return result;
            }
        }

        // todo: why isn't this just using the front door?
        private bool HandlingEntryMatchingPredicate(
            long internalRecordId,
            StreamRecordMetadata localMetadata,
            RecordFilter operationRecordFilter)
        {
            var matchResult = false;

            if ((operationRecordFilter.InternalRecordIds != null) && operationRecordFilter.InternalRecordIds.Any())
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                matchResult = matchResult || operationRecordFilter.InternalRecordIds.Contains(internalRecordId);
            }

            if ((operationRecordFilter.Ids != null) && operationRecordFilter.Ids.Any())
            {
                // todo: discuss.  Do we need everything after the &&?  Why are we considering
                // VersionMatchStrategy when comparing Ids? Shouldn't any version suffice?
                // VersionMatchStrategy should only apply to IdTypes and ObjectTypes.  What do we do in SQL?
                matchResult = matchResult || operationRecordFilter.Ids.Any(
                    __ => __.StringSerializedId.Equals(localMetadata.StringSerializedId)
                          && __.IdentifierType.EqualsAccordingToStrategy(
                              localMetadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                                  operationRecordFilter.VersionMatchStrategy),
                              operationRecordFilter.VersionMatchStrategy));
            }

            if ((operationRecordFilter.IdTypes != null) && operationRecordFilter.IdTypes.Any())
            {
                matchResult = matchResult || operationRecordFilter.IdTypes.Contains(
                    localMetadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                        operationRecordFilter.VersionMatchStrategy));
            }

            if ((operationRecordFilter.ObjectTypes != null) && operationRecordFilter.ObjectTypes.Any())
            {
                matchResult = matchResult || operationRecordFilter.ObjectTypes.Contains(
                                  localMetadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(
                                      operationRecordFilter.VersionMatchStrategy));
            }

            if (operationRecordFilter.Tags != null && operationRecordFilter.Tags.Any())
            {
                matchResult = matchResult || localMetadata.Tags.FuzzyMatchTags(operationRecordFilter.Tags, operationRecordFilter.TagMatchStrategy);
            }

            // todo: What does DeprecatedIdTypes mean in the context of getting handling status?  Is this supported in SQL?
            if (operationRecordFilter.DeprecatedIdTypes != null)
            {
                throw new NotImplementedException(Invariant($"Filtering using {nameof(RecordFilter)}.{nameof(RecordFilter.DeprecatedIdTypes)} is not implemented."));
            }

            return matchResult;
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

            lock (this.streamLock)
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
                    if (mostRecentEntry != null && mostRecentEntry.Status == HandlingStatus.DisabledForStream)
                    {
                        return new TryHandleRecordResult(null, true);
                    }

                    var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern).ToList();
                    var recordBlockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingDisabledConcern).ToList();

                    var existingInternalRecordIdsToConsider = new List<long>();
                    var existingInternalRecordIdsToIgnore = new List<long>();
                    var mergedEntries = entries.Concat(recordBlockedEntries);
                    foreach (var groupedById in mergedEntries.GroupBy(_ => _.InternalRecordId))
                    {
                        var mostRecent = groupedById.OrderByDescending(_ => _.InternalHandlingEntryId).First();

                        if (mostRecent.Status.IsAvailable())
                        {
                            existingInternalRecordIdsToConsider.Add(groupedById.Key);
                        }
                        else
                        {
                            existingInternalRecordIdsToIgnore.Add(groupedById.Key);
                        }
                    }

                    {
                        var recordsToConsiderForHandling =
                            this.locatorToRecordPartitionMap[memoryDatabaseLocator]
                                .Where(_ => !existingInternalRecordIdsToIgnore.Contains(_.InternalRecordId))
                                .ToList();

                        var matchingRecords = recordsToConsiderForHandling
                                             .Where(
                                                  _ => _.Metadata.FuzzyMatchTypes(
                                                      operation.RecordFilter.IdTypes,
                                                      operation.RecordFilter.ObjectTypes,
                                                      operation.RecordFilter.VersionMatchStrategy))
                                             .ToList();

                        if ((operation.RecordFilter.Tags != null) && operation.RecordFilter.Tags.Any())
                        {
                            matchingRecords = matchingRecords
                                .Where(_ => _.Metadata.Tags.FuzzyMatchTags(operation.RecordFilter.Tags, operation.RecordFilter.TagMatchStrategy))
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
                                ? (operation.RecordFilter.Tags ?? new List<NamedValue<string>>())
                                 .Union(recordToHandle.Metadata.Tags ?? new List<NamedValue<string>>())
                                 .ToList()
                                : operation.RecordFilter.Tags;

                            if (!existingInternalRecordIdsToConsider.Contains(recordToHandle.InternalRecordId))
                            {
                                // first time needs a requested record
                                var requestedTimestamp = DateTime.UtcNow;
                                var requestedEntryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                                var requestedMetadata = new StreamRecordHandlingEntry(
                                    requestedEntryId,
                                    recordToHandle.InternalRecordId,
                                    operation.Concern,
                                    HandlingStatus.AvailableByDefault,
                                    handlingTags,
                                    operation.Details,
                                    requestedTimestamp);

                                this.WriteHandlingEntryToMemoryMap(memoryDatabaseLocator, operation.Concern, requestedMetadata);
                            }

                            var runningTimestamp = DateTime.UtcNow;

                            var runningEntryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
                            var runningMetadata = new StreamRecordHandlingEntry(
                                runningEntryId,
                                recordToHandle.InternalRecordId,
                                operation.Concern,
                                HandlingStatus.Running,
                                handlingTags,
                                operation.Details,
                                runningTimestamp);

                            this.WriteHandlingEntryToMemoryMap(memoryDatabaseLocator, operation.Concern, runningMetadata);

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
            lock (this.streamLock)
            {
                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var mostRecent = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault(_ => _.InternalRecordId == operation.InternalRecordId);

                var currentHandlingStatus = mostRecent?.Status ?? HandlingStatus.AvailableByDefault;
                if (!operation.AcceptableCurrentStatuses.Contains(currentHandlingStatus))
                {
                    var acceptableStatusesCsvString = string.Join(",", operation.AcceptableCurrentStatuses);
                    throw new InvalidOperationException(
                        Invariant($"Expected status to be one of [{acceptableStatusesCsvString}] but found '{currentHandlingStatus}'."));
                }

                switch (operation.NewStatus)
                {
                    case HandlingStatus.DisabledForRecord:
                        this.MakeHandlingDisabledForRecord(operation);
                        break;
                    case HandlingStatus.AvailableAfterExternalCancellation:
                        this.MakeHandlingAvailableAfterExternalCancellation(operation);
                        break;
                    case HandlingStatus.Completed:
                        this.MakeHandlingCompleted(operation);
                        break;
                    case HandlingStatus.Failed:
                        this.MakeHandlingFailed(operation);
                        break;
                    case HandlingStatus.AvailableAfterSelfCancellation:
                        this.MakeHandlingAvailableAfterSelfCancellation(operation);
                        break;
                    case HandlingStatus.AvailableAfterFailure:
                        this.MakeHandlingAvailableAfterFailure(operation);
                        break;
                    case HandlingStatus.ArchivedAfterFailure:
                        this.MakeHandlingArchivedAfterFailure(operation);
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
            var currentStatus = mostRecentEntry?.Status ?? HandlingStatus.AvailableByDefault;

            var expectedStatus = newStatus == HandlingStatus.DisabledForStream
                ? HandlingStatus.AvailableByDefault
                : HandlingStatus.DisabledForStream;

            if (currentStatus != expectedStatus)
            {
                throw new InvalidOperationException(Invariant($"Cannot update status as expected status does not match; expected {expectedStatus} found {mostRecentEntry?.Status.ToString() ?? "<null entry>"}."));
            }

            var utcNow = DateTime.UtcNow;

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var metadata = new StreamRecordHandlingEntry(
                entryId,
                Concerns.GlobalBlockingRecordId,
                concern,
                newStatus,
                operation.Tags,
                operation.Details,
                utcNow);

            this.WriteHandlingEntryToMemoryMap(locator, concern, metadata);
        }

        private void MakeHandlingArchivedAfterFailure(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var metadata = new StreamRecordHandlingEntry(
                entryId,
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.ArchivedAfterFailure,
                operation.Tags,
                operation.Details,
                timestamp);

            this.WriteHandlingEntryToMemoryMap(locator, operation.Concern, metadata);
        }

        private void MakeHandlingAvailableAfterFailure(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var metadata = new StreamRecordHandlingEntry(
                entryId,
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterFailure,
                operation.Tags,
                operation.Details,
                timestamp);

            this.WriteHandlingEntryToMemoryMap(locator, operation.Concern, metadata);
        }

        private void MakeHandlingAvailableAfterSelfCancellation(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var metadata = new StreamRecordHandlingEntry(
                entryId,
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterSelfCancellation,
                operation.Tags,
                operation.Details,
                timestamp);

            this.WriteHandlingEntryToMemoryMap(locator, operation.Concern, metadata);
        }

        private void MakeHandlingFailed(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var metadata = new StreamRecordHandlingEntry(
                entryId,
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.Failed,
                operation.Tags,
                operation.Details,
                timestamp);

            this.WriteHandlingEntryToMemoryMap(locator, operation.Concern, metadata);
        }

        private void MakeHandlingCompleted(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var metadata = new StreamRecordHandlingEntry(
                entryId,
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.Completed,
                operation.Tags,
                operation.Details,
                timestamp);

            this.WriteHandlingEntryToMemoryMap(locator, operation.Concern, metadata);
        }

        private void MakeHandlingAvailableAfterExternalCancellation(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var metadata = new StreamRecordHandlingEntry(
                entryId,
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterExternalCancellation,
                operation.Tags,
                operation.Details,
                timestamp);

            this.WriteHandlingEntryToMemoryMap(locator, operation.Concern, metadata);
        }

        private void MakeHandlingDisabledForRecord(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            var timestamp = DateTime.UtcNow;

            var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);
            var metadata = new StreamRecordHandlingEntry(
                entryId,
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.DisabledForRecord,
                operation.Tags,
                operation.Details,
                timestamp);

            this.WriteHandlingEntryToMemoryMap(locator, operation.Concern, metadata);
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
                throw new ArgumentException(Invariant($"Only {nameof(MemoryDatabaseLocator)}'s are supported; specified type: {locator.GetType().ToStringReadable()} - {locator}"), nameof(locator));
            }

            lock (this.streamLock)
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

        private void WriteHandlingEntryToMemoryMap(
            IResourceLocator locator,
            string concern,
            StreamRecordHandlingEntry requestedEntry)
        {
            lock (this.streamLock)
            {
                // Do not need this call but it has the confirm key path exists logic and I do not want to refactor yet another method for them to share...
                this.GetStreamRecordHandlingEntriesForConcern(locator, concern);

                // Above will throw if this cast is not possible.
                var memoryLocator = (MemoryDatabaseLocator)locator;

                // The reference would get broken in non-obvious ways when using variables so direct keying the map.
                this.locatorToHandlingEntriesByConcernMap[memoryLocator][concern]
                    .Add(requestedEntry);
            }
        }
    }
}
