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
        public override IReadOnlyCollection<HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            // var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();

            throw new NotImplementedException();
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
                                          });

                        string metadataFilePath;
                        StreamRecordMetadata recordMetadata;
                        switch (operation.OrderRecordsBy)
                        {
                            case OrderRecordsBy.InternalRecordIdAscending:
                                var ascItem = predicate.OrderBy(_ => _.Id)
                                                       .FirstOrDefault(
                                                            _ => _.Metadata.FuzzyMatchTypes(
                                                                     operation.IdentifierType,
                                                                     operation.ObjectType,
                                                                     operation.VersionMatchStrategy)
                                                              && (operation.MinimumInternalRecordId == null
                                                               || _.Id >= operation.MinimumInternalRecordId));
                                metadataFilePath = ascItem?.Path;
                                recordMetadata = ascItem?.Metadata;
                                break;
                            case OrderRecordsBy.InternalRecordIdDescending:
                                var descItem = predicate.OrderByDescending(_ => _.Id)
                                                       .FirstOrDefault(
                                                            _ => _.Metadata.FuzzyMatchTypes(
                                                                     operation.IdentifierType,
                                                                     operation.ObjectType,
                                                                     operation.VersionMatchStrategy)
                                                              && (operation.MinimumInternalRecordId == null
                                                               || _.Id >= operation.MinimumInternalRecordId));
                                metadataFilePath = descItem?.Path;
                                recordMetadata = descItem?.Metadata;
                                break;

                            case OrderRecordsBy.Random:
                                var randItem = predicate.OrderBy(_ => Guid.NewGuid())
                                                       .FirstOrDefault(
                                                            _ => _.Metadata.FuzzyMatchTypes(
                                                                     operation.IdentifierType,
                                                                     operation.ObjectType,
                                                                     operation.VersionMatchStrategy)
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
                                ? (operation.Tags ?? new List<NamedValue<string>>())
                                 .Union(recordToHandle.Metadata.Tags ?? new List<NamedValue<string>>())
                                 .ToList()
                                : operation.Tags;

                            if (!tupleOfIdsToHandleAndIdsToIgnore.Item1.Contains(recordToHandle.InternalRecordId))
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

                                var requestedEntryId = this.GetNextRecordHandlingEntryId(locator);
                                this.PutRecordHandlingEntry(locator, operation.Concern, requestedEntryId, requestedMetadata, requestedPayload);
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

                            var runningEntryId = this.GetNextRecordHandlingEntryId(locator);
                            this.PutRecordHandlingEntry(locator, operation.Concern, runningEntryId, runningMetadata, runningPayload);

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

            operation.InternalRecordId.MustForOp(nameof(operation.InternalRecordId)).NotBeNull();

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
                var concern = Concerns.RecordHandlingConcern;
                var concernDirectory = this.GetHandlingConcernDirectory(locator, concern);

                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{internalRecordId.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                var mostRecent = mostRecentFilePath == null ? null : this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

                var currentStatus = mostRecent?.Metadata.Status ?? HandlingStatus.AvailableByDefault;

                var expectedStatus = newStatus == HandlingStatus.DisabledForStream
                    ? HandlingStatus.AvailableByDefault
                    : HandlingStatus.DisabledForStream;

                if (currentStatus != expectedStatus)
                {
                    throw new InvalidOperationException(Invariant($"Cannot update status as expected status does not match; expected {expectedStatus} found {mostRecent?.Metadata.Status.ToString() ?? "<null entry>"}."));
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

                var payload = statusEvent.ToDescribedSerializationUsingSpecificFactory(
                        this.DefaultSerializerRepresentation,
                        this.SerializerFactory,
                        this.DefaultSerializationFormat);

                var metadata = new StreamRecordHandlingEntryMetadata(
                    internalRecordId,
                    concern,
                    HandlingStatus.AvailableByDefault,
                    null, // Since we need a type, we are using NullIdentifier, however we are passing a null NullIdentifier instead of constructing one to reduce runtime complexity and payload size
                    this.DefaultSerializerRepresentation,
                    NullIdentifier.TypeRepresentation,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    utcNow);

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, concern, entryId, metadata, payload);
            }
        }

        private void MakeHandlingDisabledForRecord(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator fileSystemDatabaseLocator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Metadata.Status != HandlingStatus.Running && mostRecent.Metadata.Status != HandlingStatus.Failed)
                {
                    throw new InvalidOperationException(Invariant($"Cannot cancel an execution {nameof(HandleRecordOp)} unless it is {nameof(HandlingStatus.AvailableByDefault)} or {nameof(HandlingStatus.Failed)}, the most recent status is {mostRecent.Metadata.Status}."));
                }

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

                var metadata = new StreamRecordHandlingEntryMetadata(
                    operation.InternalRecordId,
                    operation.Concern,
                    HandlingStatus.DisabledForRecord,
                    mostRecent.Metadata.StringSerializedId,
                    payload.SerializerRepresentation,
                    mostRecent.Metadata.TypeRepresentationOfId,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    timestamp);

                var entryId = this.GetNextRecordHandlingEntryId(fileSystemDatabaseLocator);
                this.PutRecordHandlingEntry(fileSystemDatabaseLocator, operation.Concern, entryId, metadata, payload);
            }
        }

        private void MakeHandlingAvailableAfterExternalCancellation(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Metadata.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot cancel a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        private void MakeHandlingCompleted(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Metadata.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot complete a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        private void MakeHandlingFailed(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Metadata.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot fail a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        private void MakeHandlingAvailableAfterSelfCancellation(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Metadata.Status != HandlingStatus.Running)
                {
                    throw new InvalidOperationException(Invariant($"Cannot self cancel a running {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        private void MakeHandlingAvailableAfterFailure(
            StandardUpdateHandlingStatusForRecordOp operation,
            FileSystemDatabaseLocator locator,
            StreamRecordHandlingEntry mostRecent)
        {
            lock (this.handlingLock)
            {
                if (mostRecent.Metadata.Status != HandlingStatus.Failed)
                {
                    throw new InvalidOperationException(Invariant($"Cannot retry non-failed execution of {nameof(HandleRecordOp)} because the most recent status is {mostRecent.Metadata.Status}."));
                }

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        private static HandlingStatus GetStatusFromEntryFilePath(string filePath)
        {
            var resultString = GetStringTokenFromFilePath(filePath, "Status");
            var result = resultString.ToEnum<HandlingStatus>();
            return result;
        }

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
            StreamRecordHandlingEntryMetadata metadata = null)
        {
            lock (this.fileLock)
            {
                if (metadata == null)
                {
                    var metadataFileText = File.ReadAllText(metadataFilePath);
                    metadata = this.internalSerializer.Deserialize<StreamRecordHandlingEntryMetadata>(metadataFileText);
                }

                DescribedSerializationBase payload;

                var filePathBase =
                    metadataFilePath.Substring(0, metadataFilePath.Length - MetadataFileExtension.Length - 1); // remove the '.' as well.
                var binaryFilePath = Invariant($"{filePathBase}.{BinaryFileExtension}");

                if (File.Exists(binaryFilePath))
                {
                    var bytes = File.ReadAllBytes(binaryFilePath);
                    payload = new BinaryDescribedSerialization(
                        metadata.TypeRepresentationOfObject.WithVersion,
                        metadata.SerializerRepresentation,
                        bytes);
                }
                else
                {
                    var stringFilePath = Invariant($"{filePathBase}.{metadata.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture)}");

                    if (!File.Exists(stringFilePath))
                    {
                        throw new InvalidOperationException(Invariant($"Expected payload file '{stringFilePath}' to exist to accompany metadata file '{metadataFilePath}' but was not found."));
                    }

                    var stringPayload = File.ReadAllText(stringFilePath);

                    payload = new StringDescribedSerialization(
                        metadata.TypeRepresentationOfObject.WithVersion,
                        metadata.SerializerRepresentation,
                        stringPayload);
                }

                var internalRecordId = GetInternalRecordHandlingEntryIdFromEntryFilePath(metadataFilePath);

                var result = new StreamRecordHandlingEntry(internalRecordId, metadata, payload);
                return result;
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
            long entryId,
            StreamRecordHandlingEntryMetadata metadata,
            DescribedSerializationBase payload)
        {
            var concernDirectory = this.GetHandlingConcernDirectory(locator, concern);
            var timestampString = this.dateTimeStringSerializer.SerializeToString(metadata.TimestampUtc).Replace(":", "--");
            var fileExtension = payload.GetSerializationFormat() == SerializationFormat.Binary ? BinaryFileExtension :
                payload.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture);
            var fileBaseName = Invariant($"{entryId.PadWithLeadingZeros()}___{timestampString}___Id-{metadata.InternalRecordId.PadWithLeadingZeros()}___ExtId-{metadata.StringSerializedId?.EncodeForFilePath() ?? NullToken}___Status-{metadata.Status}");
            var metadataFileName = Invariant($"{fileBaseName}.{MetadataFileExtension}");
            var payloadFileName = Invariant($"{fileBaseName}.{fileExtension}");
            var metadataFilePath = Path.Combine(concernDirectory, metadataFileName);
            var payloadFilePath = Path.Combine(concernDirectory, payloadFileName);

            var stringSerializedMetadata = this.internalSerializer.SerializeToString(metadata);
            File.WriteAllText(metadataFilePath, stringSerializedMetadata);
            if (fileExtension == BinaryFileExtension)
            {
                var serializedBytes = payload.GetSerializedPayloadAsEncodedBytes();

                File.WriteAllBytes(payloadFilePath, serializedBytes);
            }
            else
            {
                var serializedString = payload.GetSerializedPayloadAsEncodedString();

                File.WriteAllText(payloadFilePath, serializedString);
            }
        }

        private bool IsMostRecentBlocked(
            IResourceLocator locator)
        {
            var concernDirectory = this.GetHandlingConcernDirectory(locator, Concerns.RecordHandlingConcern);

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