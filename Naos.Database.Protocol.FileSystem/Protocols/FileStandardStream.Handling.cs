// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStandardStream.Handling.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Enum.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    public partial class FileStandardStream
    {
        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordHandlingEntry> Execute(
            StandardGetHandlingHistoryOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            lock (this.handlingLock)
            {
                var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{operation.InternalRecordId.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var result = new List<StreamRecordHandlingEntry>();
                foreach (var file in files)
                {
                    var item = this.GetStreamRecordHandlingEntryFromMetadataFile(file);
                    result.Add(item);
                }

                return result;
            }
        }

        /// <inheritdoc />
        public override IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);

            if (this.IsMostRecentBlocked(locator))
            {
                return new Dictionary<long, HandlingStatus>
                       {
                           { Concerns.GlobalBlockingRecordId, HandlingStatus.DisabledForStream },
                       };
            }

            var recordMetadataFiles = Directory.GetFiles(
                this.GetRootPathFromLocator(locator),
                Invariant($"*.{MetadataFileExtension}"),
                SearchOption.TopDirectoryOnly);

            bool HandlingEntryMatchingPredicate(
                long internalRecordId,
                StreamRecordMetadata localMetadata)
            {
                var matchResult = false;

                if (operation.RecordFilter.InternalRecordIds != null && operation.RecordFilter.InternalRecordIds.Any())
                {
                    matchResult = matchResult || operation.RecordFilter.InternalRecordIds.Contains(internalRecordId);
                }

                if (operation.RecordFilter.Ids != null && operation.RecordFilter.Ids.Any())
                {
                    matchResult = matchResult || operation.RecordFilter.Ids.Any(
                        __ => __.StringSerializedId.Equals(localMetadata.StringSerializedId)
                           && __.IdentifierType.EqualsAccordingToStrategy(
                                  localMetadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                                      operation.RecordFilter.VersionMatchStrategy),
                                  operation.RecordFilter.VersionMatchStrategy));
                }

                if (operation.RecordFilter.IdTypes != null && operation.RecordFilter.IdTypes.Any())
                {
                    matchResult = matchResult
                               || operation.RecordFilter.IdTypes.Contains(
                                      localMetadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                                          operation.RecordFilter.VersionMatchStrategy));
                }

                if (operation.RecordFilter.ObjectTypes != null && operation.RecordFilter.ObjectTypes.Any())
                {
                    matchResult = matchResult
                               || operation.RecordFilter.ObjectTypes.Contains(
                                      localMetadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(
                                          operation.RecordFilter.VersionMatchStrategy));
                }

                if (operation.RecordFilter.Tags != null && operation.RecordFilter.Tags.Any())
                {
                    matchResult = matchResult || localMetadata.Tags.FuzzyMatchTags(operation.RecordFilter.Tags, operation.RecordFilter.TagMatchStrategy);
                }

                return matchResult;
            }

            var internalRecordIdsToConsider = new HashSet<long>();
            foreach (var recordMetadataFilePath in recordMetadataFiles)
            {
                var recordMetadataFileText = File.ReadAllText(recordMetadataFilePath);
                var internalRecordId = GetInternalRecordIdFromRecordFilePath(recordMetadataFilePath);
                var recordMetadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(recordMetadataFileText);
                if (HandlingEntryMatchingPredicate(internalRecordId, recordMetadata))
                {
                    internalRecordIdsToConsider.Add(internalRecordId);
                }
            }

            var result = internalRecordIdsToConsider.ToDictionary(
                k => k,
                v =>
                {
                    var files = Directory.GetFiles(
                        concernDirectory,
                        Invariant($"*___Id-{v.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                        SearchOption.TopDirectoryOnly);

                    var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();

                    var mostRecent = this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

                    var currentHandlingStatus = mostRecent?.Status ?? HandlingStatus.AvailableByDefault;
                    return currentHandlingStatus;
                });

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
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
                    var blocked = this.IsMostRecentBlocked(locator);
                    if (blocked)
                    {
                        return new TryHandleRecordResult(null, true);
                    }

                    lock (this.fileLock)
                    {
                        var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
                        var tupleOfIdsToHandleAndIdsToIgnore = GetIdsToHandleAndIdsToIgnore(concernDirectory);
                        var rootPath = this.GetRootPathFromLocator(locator);

                        var predicate =
                            Directory.GetFiles(rootPath, "*." + MetadataFileExtension, SearchOption.TopDirectoryOnly)
                                     .Select(
                                          _ => new
                                          {
                                              Id = GetInternalRecordIdFromRecordFilePath(_),
                                              Path = _,
                                          })
                                     .Where(_ => !tupleOfIdsToHandleAndIdsToIgnore.Item2.Contains(_.Id))
                                     .Select(
                                          _ =>
                                          {
                                              var text = File.ReadAllText(_.Path);
                                              var metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(text);

                                              return new
                                              {
                                                  Id = _.Id,
                                                  Path = _.Path,
                                                  Metadata = metadata,
                                              };
                                          })
                                     .ToList();

                        if ((operation.RecordFilter.Tags != null) && operation.RecordFilter.Tags.Any())
                        {
                            predicate = predicate
                                .Where(_ => _.Metadata.Tags.FuzzyMatchTags(operation.RecordFilter.Tags, operation.RecordFilter.TagMatchStrategy))
                                .ToList();
                        }

                        string metadataFilePath;
                        StreamRecordMetadata recordMetadata;
                        switch (operation.OrderRecordsBy)
                        {
                            case OrderRecordsBy.InternalRecordIdAscending:
                                var ascItem = predicate.OrderBy(_ => _.Id)
                                                       .FirstOrDefault(
                                                            _ => _.Metadata.FuzzyMatchTypes(
                                                                     operation.RecordFilter.IdTypes,
                                                                     operation.RecordFilter.ObjectTypes,
                                                                     operation.RecordFilter.VersionMatchStrategy)
                                                              && (operation.MinimumInternalRecordId == null
                                                               || _.Id >= operation.MinimumInternalRecordId));
                                metadataFilePath = ascItem?.Path;
                                recordMetadata = ascItem?.Metadata;
                                break;
                            case OrderRecordsBy.InternalRecordIdDescending:
                                var descItem = predicate.OrderByDescending(_ => _.Id)
                                                       .FirstOrDefault(
                                                            _ => _.Metadata.FuzzyMatchTypes(
                                                                     operation.RecordFilter.IdTypes,
                                                                     operation.RecordFilter.ObjectTypes,
                                                                     operation.RecordFilter.VersionMatchStrategy)
                                                              && (operation.MinimumInternalRecordId == null
                                                               || _.Id >= operation.MinimumInternalRecordId));
                                metadataFilePath = descItem?.Path;
                                recordMetadata = descItem?.Metadata;
                                break;

                            case OrderRecordsBy.Random:
                                var randItem = predicate.OrderBy(_ => Guid.NewGuid())
                                                       .FirstOrDefault(
                                                            _ => _.Metadata.FuzzyMatchTypes(
                                                                     operation.RecordFilter.IdTypes,
                                                                     operation.RecordFilter.ObjectTypes,
                                                                     operation.RecordFilter.VersionMatchStrategy)
                                                              && (operation.MinimumInternalRecordId == null
                                                               || _.Id >= operation.MinimumInternalRecordId));
                                metadataFilePath = randItem?.Path;
                                recordMetadata = randItem?.Metadata;
                                break;
                            default:
                                throw new NotSupportedException(Invariant($"{nameof(OrderRecordsBy)} {operation.OrderRecordsBy} is not supported."));
                        }

                        if (!string.IsNullOrWhiteSpace(metadataFilePath) && recordMetadata != null)
                        {
                            var recordToHandle = this.GetStreamRecordFromMetadataFile(metadataFilePath, recordMetadata, operation.StreamRecordItemsToInclude);

                            var handlingTags = operation.InheritRecordTags
                                ? (operation.RecordFilter.Tags ?? new List<NamedValue<string>>())
                                 .Union(recordToHandle.Metadata.Tags ?? new List<NamedValue<string>>())
                                 .ToList()
                                : operation.RecordFilter.Tags;

                            if (!tupleOfIdsToHandleAndIdsToIgnore.Item1.Contains(recordToHandle.InternalRecordId))
                            {
                                // first time needs a requested record
                                var requestedTimestamp = DateTime.UtcNow;

                                var requestedEntryId = this.GetNextRecordHandlingEntryId(locator);
                                var requestedMetadata = new StreamRecordHandlingEntry(
                                    requestedEntryId,
                                    recordToHandle.InternalRecordId,
                                    operation.Concern,
                                    HandlingStatus.AvailableByDefault,
                                    handlingTags,
                                    operation.Details,
                                    requestedTimestamp);

                                this.PutRecordHandlingEntry(locator, operation.Concern, requestedMetadata);
                            }

                            var runningTimestamp = DateTime.UtcNow;

                            var runningEntryId = this.GetNextRecordHandlingEntryId(locator);
                            var runningMetadata = new StreamRecordHandlingEntry(
                                runningEntryId,
                                recordToHandle.InternalRecordId,
                                operation.Concern,
                                HandlingStatus.Running,
                                handlingTags,
                                operation.Details,
                                runningTimestamp);

                            this.PutRecordHandlingEntry(locator, operation.Concern, runningMetadata);

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

            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);

            lock (this.handlingLock)
            {
                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{operation.InternalRecordId.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                if (mostRecentFilePath == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot cancel a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                var mostRecent = this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

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
                        this.MakeHandlingDisabledForRecord(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterExternalCancellation:
                        this.MakeHandlingAvailableAfterExternalCancellation(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.Completed:
                        this.MakeHandlingCompleted(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.Failed:
                        this.MakeHandlingFailed(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterSelfCancellation:
                        this.MakeHandlingAvailableAfterSelfCancellation(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterFailure:
                        this.MakeHandlingAvailableAfterFailure(operation, locator, mostRecent);
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

            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            lock (this.handlingLock)
            {
                var newStatus = operation.NewStatus;
                var internalRecordId = Concerns.GlobalBlockingRecordId;
                var concern = Concerns.StreamHandlingDisabledConcern;
                var concernDirectory = this.GetHandlingConcernDirectory(locator, concern);

                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{internalRecordId.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                var mostRecent = mostRecentFilePath == null ? null : this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

                var currentStatus = mostRecent?.Status ?? HandlingStatus.AvailableByDefault;

                var expectedStatus = newStatus == HandlingStatus.DisabledForStream
                    ? HandlingStatus.AvailableByDefault
                    : HandlingStatus.DisabledForStream;

                if (currentStatus != expectedStatus)
                {
                    throw new InvalidOperationException(Invariant($"Cannot update status as expected status does not match; expected {expectedStatus} found {mostRecent?.Status.ToString() ?? "<null entry>"}."));
                }

                var utcNow = DateTime.UtcNow;
                var entryId = this.GetNextRecordHandlingEntryId(locator);
                var metadata = new StreamRecordHandlingEntry(
                    entryId,
                    internalRecordId,
                    concern,
                    newStatus,
                    operation.Tags,
                    operation.Details,
                    utcNow);

                this.PutRecordHandlingEntry(locator, concern, metadata);
            }
        }

        private void MakeHandlingDisabledForRecord(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator fileSystemDatabaseLocator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Status != HandlingStatus.Running && mostRecent.Status != HandlingStatus.Failed)
                {
                    throw new InvalidOperationException(Invariant($"Cannot cancel an execution {nameof(HandleRecordOp)} unless it is {nameof(HandlingStatus.AvailableByDefault)} or {nameof(HandlingStatus.Failed)}, the most recent status is {mostRecent.Status}."));
                }

                var timestamp = DateTime.UtcNow;

                var entryId = this.GetNextRecordHandlingEntryId(fileSystemDatabaseLocator);
                var metadata = new StreamRecordHandlingEntry(
                    entryId,
                    operation.InternalRecordId,
                    operation.Concern,
                    HandlingStatus.DisabledForRecord,
                    operation.Tags,
                    operation.Details,
                    timestamp);

                this.PutRecordHandlingEntry(fileSystemDatabaseLocator, operation.Concern, metadata);
            }
        }

        private void MakeHandlingAvailableAfterExternalCancellation(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot cancel a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Status}."));
                }

                var timestamp = DateTime.UtcNow;

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                var metadata = new StreamRecordHandlingEntry(
                    entryId,
                    operation.InternalRecordId,
                    operation.Concern,
                    HandlingStatus.AvailableAfterExternalCancellation,
                    operation.Tags,
                    operation.Details,
                    timestamp);

                this.PutRecordHandlingEntry(locator, operation.Concern, metadata);
            }
        }

        private void MakeHandlingCompleted(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot complete a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Status}."));
                }

                var timestamp = DateTime.UtcNow;

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                var metadata = new StreamRecordHandlingEntry(
                    entryId,
                    operation.InternalRecordId,
                    operation.Concern,
                    HandlingStatus.Completed,
                    operation.Tags,
                    operation.Details,
                    timestamp);

                this.PutRecordHandlingEntry(locator, operation.Concern, metadata);
            }
        }

        private void MakeHandlingFailed(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot fail a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Status}."));
                }

                var timestamp = DateTime.UtcNow;

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                var metadata = new StreamRecordHandlingEntry(
                    entryId,
                    operation.InternalRecordId,
                    operation.Concern,
                    HandlingStatus.Failed,
                    operation.Tags,
                    operation.Details,
                    timestamp);

                this.PutRecordHandlingEntry(locator, operation.Concern, metadata);
            }
        }

        private void MakeHandlingAvailableAfterSelfCancellation(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot self cancel a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Status}."));
                }

                var timestamp = DateTime.UtcNow;

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                var metadata = new StreamRecordHandlingEntry(
                    entryId,
                    operation.InternalRecordId,
                    operation.Concern,
                    HandlingStatus.AvailableAfterSelfCancellation,
                    operation.Tags,
                    operation.Details,
                    timestamp);

                this.PutRecordHandlingEntry(locator, operation.Concern, metadata);
            }
        }

        private void MakeHandlingAvailableAfterFailure(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Status != HandlingStatus.Failed)
                {
                    throw new InvalidOperationException(Invariant($"Cannot retry non-failed execution of {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Status}."));
                }

                var timestamp = DateTime.UtcNow;

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                var metadata = new StreamRecordHandlingEntry(
                    entryId,
                    operation.InternalRecordId,
                    operation.Concern,
                    HandlingStatus.AvailableAfterFailure,
                    operation.Tags,
                    operation.Details,
                    timestamp);

                this.PutRecordHandlingEntry(locator, operation.Concern, metadata);
            }
        }

        private static HandlingStatus GetStatusFromEntryFilePath(string filePath)
        {
            var resultString = GetStringTokenFromFilePath(filePath, "Status");
            var result = resultString.ToEnum<HandlingStatus>();
            return result;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for future use.")]
        private static long GetInternalRecordHandlingEntryIdFromEntryFilePath(
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is null or whitespace.", nameof(filePath));
            }

            var internalRecordHandlingIdString = Path.GetFileName(filePath)
                ?.Split(
                    new[]
                    {
                        "___",
                    },
                    StringSplitOptions.RemoveEmptyEntries)[0];

            if (string.IsNullOrWhiteSpace(internalRecordHandlingIdString))
            {
                throw new InvalidOperationException(Invariant($"Failed to extract internal record handling id from file path: '{filePath}'."));
            }

            var internalRecordId = long.Parse(internalRecordHandlingIdString, CultureInfo.InvariantCulture);
            return internalRecordId;
        }

        private StreamRecordHandlingEntry GetStreamRecordHandlingEntryFromMetadataFile(
            string metadataFilePath,
            StreamRecordHandlingEntry metadata = null)
        {
            lock (this.fileLock)
            {
                if (metadata == null)
                {
                    var metadataFileText = File.ReadAllText(metadataFilePath);
                    metadata = this.internalSerializer.Deserialize<StreamRecordHandlingEntry>(metadataFileText);
                }

                return metadata;
            }
        }

        private static Tuple<IReadOnlyCollection<long>, IReadOnlyCollection<long>> GetIdsToHandleAndIdsToIgnore(
            string concernDirectory)
        {
            var existingInternalRecordIdsToConsider = new List<long>();
            var existingInternalRecordIdsToIgnore = new List<long>();
            var files = Directory.GetFiles(
                concernDirectory,
                "*." + MetadataFileExtension,
                SearchOption.TopDirectoryOnly);

            if (!files.Any())
            {
                return new Tuple<IReadOnlyCollection<long>, IReadOnlyCollection<long>>(new List<long>(), new List<long>());
            }

            foreach (var groupedById in files.GroupBy(GetInternalRecordIdFromEntryFilePath))
            {
                var currentEntry = groupedById.OrderByDescending(_ => _).First();
                var currentStatus = GetStatusFromEntryFilePath(currentEntry);
                if (currentStatus.IsAvailable())
                {
                    existingInternalRecordIdsToConsider.Add(groupedById.Key);
                }
                else
                {
                    existingInternalRecordIdsToIgnore.Add(groupedById.Key);
                }
            }

            return new Tuple<IReadOnlyCollection<long>, IReadOnlyCollection<long>>(
                existingInternalRecordIdsToConsider,
                existingInternalRecordIdsToIgnore);
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        private long GetNextRecordHandlingEntryId(IResourceLocator locator)
        {
            if (locator == null)
            {
                throw new ArgumentNullException(nameof(locator));
            }

            if (!(locator is FileSystemDatabaseLocator fileSystemLocator))
            {
                throw new ArgumentException(Invariant($"Only {nameof(FileSystemDatabaseLocator)}'s are supported; specified type: {locator.GetType().ToStringReadable()} - {locator.ToString()}"), nameof(locator));
            }

            var rootPath = this.GetRootPathFromLocator(fileSystemLocator);
            var recordIdentifierTrackingFilePath = Path.Combine(rootPath, RecordHandlingEntryIdentifierTrackingFileName);

            long newId;

            lock (this.nextInternalRecordHandlingEntryIdentifierLock)
            {
                // open the file in locking mode to restrict a single thread changing the internal record identifier index at a time.
                using (var fileStream = new FileStream(
                    recordIdentifierTrackingFilePath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None))
                {
                    var reader = new StreamReader(fileStream);
                    var currentInternalRecordIdentifierString = reader.ReadToEnd();
                    currentInternalRecordIdentifierString = string.IsNullOrWhiteSpace(currentInternalRecordIdentifierString) ? 0.ToString(CultureInfo.InvariantCulture) : currentInternalRecordIdentifierString;
                    var currentId = long.Parse(currentInternalRecordIdentifierString, CultureInfo.InvariantCulture);
                    newId = currentId + 1;
                    fileStream.Position = 0;
                    var writer = new StreamWriter(fileStream);
                    writer.Write(newId.ToString(CultureInfo.InvariantCulture));

                    // necessary to flush buffer.
                    writer.Close();
                }
            }

            return newId;
        }

        private void PutRecordHandlingEntry(
            IResourceLocator locator,
            string concern,
            StreamRecordHandlingEntry metadata)
        {
            var concernDirectory = this.GetHandlingConcernDirectory(locator, concern);
            var timestampString = this.dateTimeStringSerializer.SerializeToString(metadata.TimestampUtc).Replace(":", "--");
            var fileBaseName = Invariant($"{metadata.InternalHandlingEntryId.PadWithLeadingZeros()}___{timestampString}___Id-{metadata.InternalRecordId.PadWithLeadingZeros()}___Status-{metadata.Status}");
            var metadataFileName = Invariant($"{fileBaseName}.{MetadataFileExtension}");
            var metadataFilePath = Path.Combine(concernDirectory, metadataFileName);

            var stringSerializedMetadata = this.internalSerializer.SerializeToString(metadata);
            File.WriteAllText(metadataFilePath, stringSerializedMetadata);
        }

        private bool IsMostRecentBlocked(
            IResourceLocator locator)
        {
            var concernDirectory = this.GetHandlingConcernDirectory(locator, Concerns.StreamHandlingDisabledConcern);

            var files = Directory.GetFiles(
                concernDirectory,
                "*." + MetadataFileExtension,
                SearchOption.TopDirectoryOnly);

            if (!files.Any())
            {
                return false;
            }

            var mostRecentFile = files.OrderByDescending(_ => _).First();
            var status = GetStatusFromEntryFilePath(mostRecentFile);
            var result = status == HandlingStatus.DisabledForStream;
            return result;
        }
    }
}