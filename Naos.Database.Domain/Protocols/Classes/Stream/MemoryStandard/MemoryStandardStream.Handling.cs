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
                this.ThrowIfStreamNotCreated();

                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var recordBlockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingDisabledConcern);

                var result = new StreamRecordHandlingEntry[0]
                    .Concat(entries)
                    .Concat(recordBlockedEntries)
                    .Where(_ => _.InternalRecordId == operation.InternalRecordId)
                    .ToList();

                // We only inject the AvailableByDefault status if there has been any handling history at all
                // (i.e. the record has been try-handled at least one time).
                // If there is no handling history or there is no record with the specified InternalRecordId, then
                // the result is allowed to be empty.
                if (result.Any())
                {
                    result.Add(
                        new StreamRecordHandlingEntry(
                            Concerns.AvailableByDefaultHandlingEntryId,
                            operation.InternalRecordId,
                            operation.Concern,
                            HandlingStatus.AvailableByDefault,
                            null,
                            Concerns.AvailableByDefaultHandlingEntryDetails,
                            result.Min(_ => _.TimestampUtc)));
                }

                return result;
            }
        }

        /// <inheritdoc />
        public override IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                Dictionary<long, HandlingStatus> ApplyHandlingFilter(
                    Dictionary<long, HandlingStatus> handlingStatuses)
                {
                    if (operation.HandlingFilter.Tags != null)
                    {
                        throw new NotImplementedException(Invariant($"Filtering using {nameof(HandlingFilter)}.{nameof(HandlingFilter.Tags)} is not implemented."));
                    }

                    var applyFilterResult = ((operation.HandlingFilter.CurrentHandlingStatuses == null) ||
                                             (!operation.HandlingFilter.CurrentHandlingStatuses.Any()))
                        ? handlingStatuses
                        : handlingStatuses
                            .Where(_ => operation.HandlingFilter.CurrentHandlingStatuses.Contains(_.Value))
                            .ToDictionary(k => k.Key, v => v.Value);

                    return applyFilterResult;
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

                var filteredRecords = ApplyRecordFilterToPartition(
                    operation.RecordFilter,
                    records,
                    RecordsToFilterSelectionStrategy.All);

                // Get latest status for each record to consider.
                var unfilteredResult = filteredRecords
                    .Select(_ => _.InternalRecordId)
                    .ToDictionary(
                        k => k,
                        v => handlingEntries
                            .Concat(streamHandlingDisabledEntries) // this is needed to accomodate querying on Concerns.GlobalBlockingRecordId to check if AvailableByDefault
                            .Concat(recordHandlingDisabledEntries)
                            .Where(_ => _.InternalRecordId == v)
                            .OrderByDescending(_ => _.InternalHandlingEntryId)
                            .FirstOrDefault()
                           ?.Status
                          ?? HandlingStatus.AvailableByDefault);

                // Filter out records that don't match handling status filter.
                // Note that the filter is specifically applied AFTER and not in conjunction with
                // the above filter.  We are not looking for the most recent entry that passes the status filter
                // we are determining whether the last entry passes the status filter.
                var result = ApplyHandlingFilter(unfilteredResult);

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

            var allLocators = (operation.SpecifiedResourceLocator != null)
                ? new[]
                {
                    operation.SpecifiedResourceLocator,
                }
                : this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                foreach (var locator in allLocators)
                {
                    if (!(locator is MemoryDatabaseLocator memoryDatabaseLocator))
                    {
                        throw new NotSupportedException(Invariant($"{nameof(GetAllResourceLocatorsOp)} must only return locators of type {typeof(MemoryDatabaseLocator).ToStringReadable()}; found {locator?.GetType().ToStringReadable()}."));
                    }

                    var streamHandlingDisabledEntries = this.GetStreamRecordHandlingEntriesForConcern(locator, Concerns.StreamHandlingDisabledConcern);
                    var streamHandlingDisabledStatus = streamHandlingDisabledEntries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault()?.Status;
                    if (streamHandlingDisabledStatus == HandlingStatus.DisabledForStream)
                    {
                        return new TryHandleRecordResult(null, true);
                    }

                    var handlingEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern).ToList();
                    var recordBlockedEntries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, Concerns.RecordHandlingDisabledConcern).ToList();
                    var mergedEntries = new StreamRecordHandlingEntry[0]
                        .Concat(handlingEntries)
                        .Concat(recordBlockedEntries)
                        .ToList();

                    var existingInternalRecordIdsToIgnore = new List<long>();
                    foreach (var groupedByInternalRecordId in mergedEntries.GroupBy(_ => _.InternalRecordId))
                    {
                        var mostRecentEntry = groupedByInternalRecordId.OrderByDescending(_ => _.InternalHandlingEntryId).First();

                        if (!mostRecentEntry.Status.IsAvailable())
                        {
                            existingInternalRecordIdsToIgnore.Add(groupedByInternalRecordId.Key);
                        }
                    }

                    if (!this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var records))
                    {
                        return new TryHandleRecordResult(null);
                    }

                    var recordsToConsiderForHandling = records
                            .Where(_ => !existingInternalRecordIdsToIgnore.Contains(_.InternalRecordId))
                            .ToList();

                    var matchingRecords = ApplyRecordFilterToPartition(
                        operation.RecordFilter,
                        recordsToConsiderForHandling,
                        RecordsToFilterSelectionStrategy.All);

                    StreamRecord recordToHandle;
                    switch (operation.OrderRecordsBy)
                    {
                        case OrderRecordsBy.InternalRecordIdAscending:
                            recordToHandle = matchingRecords
                                .OrderBy(_ => _.InternalRecordId)
                                .FirstOrDefault(
                                    _ => (operation.MinimumInternalRecordId == null)
                                         || (_.InternalRecordId >= operation.MinimumInternalRecordId));
                            break;
                        case OrderRecordsBy.InternalRecordIdDescending:
                            recordToHandle = matchingRecords
                                .OrderByDescending(_ => _.InternalRecordId)
                                .FirstOrDefault(
                                    _ => (operation.MinimumInternalRecordId == null)
                                         || (_.InternalRecordId >= operation.MinimumInternalRecordId));
                            break;
                        case OrderRecordsBy.Random:
                            recordToHandle = matchingRecords
                                .OrderByDescending(_ => Guid.NewGuid())
                                .FirstOrDefault(
                                    _ => (operation.MinimumInternalRecordId == null)
                                         || (_.InternalRecordId >= operation.MinimumInternalRecordId));
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
                                recordToHandle = recordToHandle.DeepCloneWithPayload(
                                    new NullDescribedSerialization(
                                        recordToHandle.Payload.PayloadTypeRepresentation,
                                        recordToHandle.Payload.SerializerRepresentation));
                                break;
                            default:
                                throw new NotSupportedException(Invariant($"This {nameof(StreamRecordItemsToInclude)} is not supported: {operation.StreamRecordItemsToInclude}."));
                        }

                        var result = new TryHandleRecordResult(recordToHandle);

                        return result;
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
                this.ThrowIfStreamNotCreated();

                // ReSharper disable once SimplifyLinqExpressionUseAll - prefer the !Any for readability
                StreamRecord record = null;
                if (this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition))
                {
                    record = partition.SingleOrDefault(_ => _.InternalRecordId == operation.InternalRecordId);
                }

                if (record == null)
                {
                    throw new InvalidOperationException(Invariant($"There is no record with the specified {nameof(StandardUpdateHandlingStatusForRecordOp.InternalRecordId)}: {operation.InternalRecordId}."));
                }

                var entries = this.GetStreamRecordHandlingEntriesForConcern(memoryDatabaseLocator, operation.Concern);
                var mostRecent = entries
                    .OrderByDescending(_ => _.InternalHandlingEntryId)
                    .FirstOrDefault(_ => _.InternalRecordId == operation.InternalRecordId);

                var currentHandlingStatus = mostRecent?.Status ?? HandlingStatus.AvailableByDefault;
                if (!operation.AcceptableCurrentStatuses.Contains(currentHandlingStatus))
                {
                    var acceptableStatusesCsvString = string.Join(",", operation.AcceptableCurrentStatuses);

                    throw new InvalidOperationException(
                        Invariant($"Expected status to be one of [{acceptableStatusesCsvString}] but found '{currentHandlingStatus}'."));
                }

                var entryId = Interlocked.Increment(ref this.uniqueLongForInMemoryHandlingEntries);

                var tags = operation.InheritRecordTags
                    ? (operation.Tags ?? new List<NamedValue<string>>())
                        .Union(record.Metadata.Tags ?? new List<NamedValue<string>>())
                        .ToList()
                    : operation.Tags;

                var metadata = new StreamRecordHandlingEntry(
                    entryId,
                    operation.InternalRecordId,
                    operation.Concern,
                    operation.NewStatus,
                    tags,
                    operation.Details,
                    DateTime.UtcNow);

                // Note that we intentionally do not check whether the status transition is a valid one.
                // The standard op lets you change the handling status to any status.
                this.WriteHandlingEntryToMemoryMap(memoryDatabaseLocator, operation.Concern, metadata);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            StandardUpdateHandlingStatusForStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                var concern = Concerns.StreamHandlingDisabledConcern;
                var entries = this.GetStreamRecordHandlingEntriesForConcern(locator, concern);
                var mostRecentEntry = entries.OrderByDescending(_ => _.InternalHandlingEntryId).FirstOrDefault();
                var currentStatus = mostRecentEntry?.Status ?? HandlingStatus.AvailableByDefault;

                var expectedStatus = operation.NewStatus == HandlingStatus.DisabledForStream
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
                    operation.NewStatus,
                    operation.Tags,
                    operation.Details,
                    utcNow);

                this.WriteHandlingEntryToMemoryMap(locator, concern, metadata);
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
