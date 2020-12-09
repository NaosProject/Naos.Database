// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Enum.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IReadWriteStream"/>, it is thread resilient but not necessarily thread safe.
    /// Implements the <see cref="ReadWriteStreamBase" />.
    /// </summary>
    /// <seealso cref="ReadWriteStreamBase" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class FileReadWriteStream :
        StandardReadWriteStreamBase
    {
        private const string RecordHandlingTrackingDirectoryName = "_HandlingTracking";
        private const string RecordIdentifierTrackingFileName = "_InternalRecordIdentifierTracking.nfo";
        private const string RecordHandlingEntryIdentifierTrackingFileName = "_InternalRecordHandlingEntryIdentifierTracking.nfo";
        private const string NextUniqueLongTrackingFileName = "_NextUniqueLongTracking.nfo";
        private const string BinaryFileExtension = "bin";
        private const string MetadataFileExtension = "meta";

        private readonly object fileLock = new object();
        private readonly object handlingLock = new object();
        private readonly object singleLocatorLock = new object();

        private readonly object nextInternalRecordIdentifierLock = new object();
        private readonly object nextInternalRecordHandlingEntryIdentifierLock = new object();
        private readonly object nextUniqueLongLock = new object();
        private readonly ObcDateTimeStringSerializer dateTimeStringSerializer = new ObcDateTimeStringSerializer();
        private readonly ISerializer internalSerializer;
        private FileSystemDatabaseLocator singleLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReadWriteStream"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="defaultSerializerRepresentation">Default serializer description to use.</param>
        /// <param name="defaultSerializationFormat">Default serializer format.</param>
        /// <param name="serializerFactory">The factory to get a serializer to use for objects.</param>
        /// <param name="resourceLocatorProtocols">The protocols for getting locators.</param>
        public FileReadWriteStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            IResourceLocatorProtocols resourceLocatorProtocols)
        : base(name, resourceLocatorProtocols, serializerFactory, defaultSerializerRepresentation, defaultSerializationFormat)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();

            this.internalSerializer = serializerFactory.BuildSerializer(defaultSerializerRepresentation);
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation
        {
            get
            {
                var allFileSystemDatabaseLocators =
                    this.ResourceLocatorProtocols
                        .Execute(new GetAllResourceLocatorsOp())
                        .Cast<FileSystemDatabaseLocator>()
                        .ToList();

                var result = new FileStreamRepresentation(this.Name, allFileSystemDatabaseLocators);
                return result;
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CreateStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation)
                                        ?? throw new ArgumentException(FormattableString.Invariant($"Invalid implementation of {nameof(IStreamRepresentation)}, expected '{nameof(FileStreamRepresentation)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            lock (this.fileLock)
            {
                foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
                {
                    var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.Name);
                    var exists = Directory.Exists(directoryPath);
                    if (exists)
                    {
                        switch (operation.ExistingStreamEncounteredStrategy)
                        {
                            case ExistingStreamEncounteredStrategy.Overwrite:
                                DeleteDirectoryAndConfirm(directoryPath, true);
                                CreateDirectoryAndConfirm(directoryPath);
                                break;
                            case ExistingStreamEncounteredStrategy.Skip:
                                /* no-op */
                                break;
                            case ExistingStreamEncounteredStrategy.Throw:
                                throw new InvalidOperationException(
                                    FormattableString.Invariant(
                                        $"Path '{directoryPath}' already exists and {nameof(operation.ExistingStreamEncounteredStrategy)} on the operation is {operation.ExistingStreamEncounteredStrategy}."));
                            default:
                                throw new NotSupportedException(
                                    FormattableString.Invariant(
                                        $"{nameof(operation.ExistingStreamEncounteredStrategy)} value '{operation.ExistingStreamEncounteredStrategy}' is not supported."));
                        }
                    }
                    else
                    {
                        CreateDirectoryAndConfirm(directoryPath);
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            DeleteStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation)
                                        ?? throw new ArgumentException(Invariant($"Invalid implementation of {nameof(IStreamRepresentation)}, expected '{nameof(FileStreamRepresentation)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            lock (this.fileLock)
            {
                foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
                {
                    var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.Name);
                    var exists = Directory.Exists(directoryPath);
                    if (!exists)
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
                        Directory.Delete(directoryPath, true);
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            PruneBeforeInternalRecordDateOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(locator);

            lock (this.fileLock)
            {
                var filesToPotentiallyRemove
                    = Directory.GetFiles(
                        rootPath,
                        Invariant($"*___*"),
                        SearchOption.TopDirectoryOnly);
                foreach (var fileToConsiderRemoving in filesToPotentiallyRemove)
                {
                    var internalRecordDate = this.GetRootDateFromFilePath(fileToConsiderRemoving);
                    if (internalRecordDate < operation.InternalRecordDate)
                    {
                        File.Delete(fileToConsiderRemoving);
                    }
                }

                var concerns = Directory.GetDirectories(rootPath);
                foreach (var concern in concerns)
                {
                    var concernDirectory = this.GetHandlingConcernDirectory(locator, concern);
                    var handlingFilesToConsiderRemoving
                        = Directory.GetFiles(
                            concernDirectory,
                            Invariant($"*___*"),
                            SearchOption.TopDirectoryOnly);

                    foreach (var fileToConsiderRemoving in handlingFilesToConsiderRemoving)
                    {
                        var internalEntryDate = this.GetRootDateFromFilePath(fileToConsiderRemoving);
                        if (internalEntryDate < operation.InternalRecordDate)
                        {
                            File.Delete(fileToConsiderRemoving);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            PruneBeforeInternalRecordIdOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(locator);

            lock (this.fileLock)
            {
                var filesToPotentiallyRemove
                    = Directory.GetFiles(
                        rootPath,
                        Invariant($"*___*"),
                        SearchOption.TopDirectoryOnly);
                foreach (var fileToConsiderRemoving in filesToPotentiallyRemove)
                {
                    var internalRecordId = GetInternalRecordIdFromRecordFilePath(fileToConsiderRemoving);
                    if (internalRecordId < operation.InternalRecordId)
                    {
                        File.Delete(fileToConsiderRemoving);
                    }
                }

                var concerns = Directory.GetDirectories(rootPath);
                foreach (var concern in concerns)
                {
                    var concernDirectory = this.GetHandlingConcernDirectory(locator, concern);
                    var handlingFilesToConsiderRemoving
                        = Directory.GetFiles(
                            concernDirectory,
                            Invariant($"*___*"),
                            SearchOption.TopDirectoryOnly);

                    foreach (var fileToConsiderRemoving in handlingFilesToConsiderRemoving)
                    {
                        var internalRecordId = GetInternalRecordIdFromEntryFilePath(fileToConsiderRemoving);
                        if (internalRecordId < operation.InternalRecordId)
                        {
                            File.Delete(fileToConsiderRemoving);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public override long Execute(
            GetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var locator = this.ResourceLocatorProtocols.Execute(new GetResourceLocatorForUniqueIdentifierOp());
            var rootPath = this.GetRootPathFromLocator(locator);
            var trackingFilePath = Path.Combine(rootPath, NextUniqueLongTrackingFileName);

            long nextLong;

            lock (this.nextUniqueLongLock)
            {
                // open the file in locking mode to restrict a single thread changing the list of unique longs index at a time.
                using (var fileStream = new FileStream(
                    trackingFilePath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None))
                {
                    var reader = new StreamReader(fileStream);
                    var currentSerializedListText = reader.ReadToEnd();
                    var currentList = !string.IsNullOrWhiteSpace(currentSerializedListText)
                        ? this.internalSerializer.Deserialize<IList<UniqueLongIssuedEvent>>(currentSerializedListText)
                        : new List<UniqueLongIssuedEvent>();

                    nextLong = currentList.Any()
                        ? currentList.Max(_ => _.Id) + 1
                        : 1;

                    currentList.Add(new UniqueLongIssuedEvent(nextLong, DateTime.UtcNow, operation.Details));
                    var updatedSerializedListText = this.internalSerializer.SerializeToString(currentList);

                    fileStream.Position = 0;
                    var writer = new StreamWriter(fileStream);
                    writer.Write(updatedSerializedListText);

                    // necessary to flush buffer.
                    writer.Close();
                }
            }

            return nextLong;
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOfRecordOp operation)
        {
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
        public override HandlingStatus Execute(
            GetHandlingStatusOfRecordsByIdOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();

            if (this.IsMostRecentBlocked(locator))
            {
                return HandlingStatus.Blocked;
            }

            var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
            lock (this.handlingLock)
            {
                var files = operation.IdsToMatch.SelectMany(
                                          _ => Directory.GetFiles(
                                              concernDirectory,
                                              "*___ExtId-" + _.StringSerializedId.EncodeForFilePath() + "___*." + MetadataFileExtension,
                                              SearchOption.TopDirectoryOnly).Select(__ => new Tuple<StringSerializedIdentifier, string>(_, __)))
                                     .ToList();

                var recordIdToStatusAndEntryIdMap = new Dictionary<long, List<Tuple<HandlingStatus, long>>>();
                foreach (var file in files)
                {
                    var filePath = file.Item2;
                    var text = File.ReadAllText(filePath);
                    var metadata = this.internalSerializer.Deserialize<StreamRecordHandlingEntryMetadata>(text);

                    if (metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.TypeVersionMatchStrategy)
                            .EqualsAccordingToStrategy(file.Item1.IdentifierType, operation.TypeVersionMatchStrategy))
                    {
                        if (!recordIdToStatusAndEntryIdMap.ContainsKey(metadata.InternalRecordId))
                        {
                            recordIdToStatusAndEntryIdMap.Add(metadata.InternalRecordId, new List<Tuple<HandlingStatus, long>>());
                        }

                        var entryId = GetRootIdFromFilePath(filePath);
                        recordIdToStatusAndEntryIdMap[metadata.InternalRecordId].Add(new Tuple<HandlingStatus, long>(metadata.Status, entryId));
                    }
                }

                var statuses = recordIdToStatusAndEntryIdMap.Select(_ => _.Value.OrderByDescending(__ => __.Item2).First().Item1).ToList();
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
                    if (this.IsMostRecentBlocked(locator))
                    {
                        return HandlingStatus.Blocked;
                    }

                    var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
                    var files = Directory.GetFiles(
                                              concernDirectory,
                                              "*." + MetadataFileExtension,
                                              SearchOption.TopDirectoryOnly)
                                         .ToList();

                    var recordIdToStatusAndEntryIdMap = new Dictionary<long, List<Tuple<HandlingStatus, long>>>();
                    foreach (var file in files)
                    {
                        var text = File.ReadAllText(file);
                        var metadata = this.internalSerializer.Deserialize<StreamRecordHandlingEntryMetadata>(text);
                        if (operation.TagsToMatch.FuzzyMatchAccordingToStrategy(metadata.Tags, operation.TagMatchStrategy))
                        {
                            if (!recordIdToStatusAndEntryIdMap.ContainsKey(metadata.InternalRecordId))
                            {
                                recordIdToStatusAndEntryIdMap.Add(metadata.InternalRecordId, new List<Tuple<HandlingStatus, long>>());
                            }

                            var entryId = GetRootIdFromFilePath(file);
                            recordIdToStatusAndEntryIdMap[metadata.InternalRecordId].Add(new Tuple<HandlingStatus, long>(metadata.Status, entryId));
                        }
                    }

                    var locatorStatuses = recordIdToStatusAndEntryIdMap.Select(_ => _.Value.OrderByDescending(__ => __.Item2).First().Item1).ToList();
                    statuses.AddRange(locatorStatuses);
                }

                var result = statuses.ReduceToCompositeHandlingStatus(operation.HandlingStatusCompositionStrategy);
                return result;
            }
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override StreamRecord Execute(
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
                    var blocked = this.IsMostRecentBlocked(locator);
                    if (blocked)
                    {
                        return null;
                    }

                    lock (this.fileLock)
                    {
                        var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
                        var tupleOfIdsToHandleAndIdsToIgnore = GetIdsToHandleAndIdsToIgnore(concernDirectory);
                        var rootPath = this.GetRootPathFromLocator(locator);
                        var recordToHandleDetails =
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
                                     .FirstOrDefault(
                                          _ => _.Metadata.FuzzyMatchTypes(
                                              operation.IdentifierType,
                                              operation.ObjectType,
                                              operation.TypeVersionMatchStrategy));

                        if (recordToHandleDetails != null)
                        {
                            var recordToHandle = this.GetStreamRecordFromMetadataFile(recordToHandleDetails.Path, recordToHandleDetails.Metadata);
                            if (!tupleOfIdsToHandleAndIdsToIgnore.Item1.Contains(recordToHandle.InternalRecordId))
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

                                var requestedEntryId = this.GetNextRecordHandlingEntryId(locator);
                                this.PutRecordHandlingEntry(locator, operation.Concern, requestedEntryId, requestedMetadata, requestedPayload);
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

                            var runningEntryId = this.GetNextRecordHandlingEntryId(locator);
                            this.PutRecordHandlingEntry(locator, operation.Concern, runningEntryId, runningMetadata, runningPayload);

                            return recordToHandle;
                        }
                    }
                }

                return null;
            }
        }

        private string GetHandlingConcernDirectory(
            IResourceLocator locator,
            string concern)
        {
            var rootPath = this.GetRootPathFromLocator(locator);
            var handleDirectory = Path.Combine(rootPath, RecordHandlingTrackingDirectoryName);
            var handlingConcernDirectory = Path.Combine(handleDirectory, concern);
            if (!Directory.Exists(handlingConcernDirectory))
            {
                Directory.CreateDirectory(handlingConcernDirectory);
            }

            return handlingConcernDirectory;
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            GetLatestRecordOp operation)
        {
            var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(fileSystemLocator);
            lock (this.fileLock)
            {
                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var orderedDescendingByInternalRecordId = metadataPathsThatCouldMatch.OrderByDescending(Path.GetFileName).ToList();
                if (!orderedDescendingByInternalRecordId.Any())
                {
                    return default;
                }

                var latest = orderedDescendingByInternalRecordId.First();
                var result = this.GetStreamRecordFromMetadataFile(latest);
                return result;
            }
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            GetLatestRecordByIdOp operation)
        {
            lock (this.fileLock)
            {
                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*{operation.StringSerializedId.EncodeForFilePath()}*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var orderedDescendingByInternalRecordId = metadataPathsThatCouldMatch.OrderByDescending(Path.GetFileName).ToList();
                if (!orderedDescendingByInternalRecordId.Any())
                {
                    return default;
                }

                foreach (var metadataFilePathToTest in orderedDescendingByInternalRecordId)
                {
                    var fileText = File.ReadAllText(metadataFilePathToTest);
                    var metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(fileText);
                    if (metadata.FuzzyMatchTypesAndId(
                        operation.StringSerializedId,
                        operation.IdentifierType,
                        operation.ObjectType,
                        operation.TypeVersionMatchStrategy))
                    {
                        var result = this.GetStreamRecordFromMetadataFile(metadataFilePathToTest, metadata);
                        return result;
                    }
                }

                return default;
            }
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public override long Execute(
            PutRecordOp operation)
        {
            lock (this.fileLock)
            {
                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);
                var recordIdentifierTrackingFilePath = Path.Combine(rootPath, RecordIdentifierTrackingFileName);

                var timestampString = this.dateTimeStringSerializer.SerializeToString(operation.Metadata.TimestampUtc).Replace(":", "-");

                long newId;

                lock (this.nextInternalRecordIdentifierLock)
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

                var fileExtension = operation.Payload.SerializationFormat == SerializationFormat.Binary ? BinaryFileExtension :
                    operation.Payload.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture);
                var filePathIdentifier = operation.Metadata.StringSerializedId.EncodeForFilePath();
                var fileBaseName = Invariant($"{newId.PadWithLeadingZeros()}___{timestampString}___{filePathIdentifier}");
                var metadataFileName = Invariant($"{fileBaseName}.{MetadataFileExtension}");
                var payloadFileName = Invariant($"{fileBaseName}.{fileExtension}");
                var metadataFilePath = Path.Combine(rootPath, metadataFileName);
                var payloadFilePath = Path.Combine(rootPath, payloadFileName);

                var stringSerializedMetadata = this.internalSerializer.SerializeToString(operation.Metadata);
                File.WriteAllText(metadataFilePath, stringSerializedMetadata);
                if (fileExtension == BinaryFileExtension)
                {
                    var serializedBytes = Convert.FromBase64String(operation.Payload.SerializedPayload);

                    File.WriteAllBytes(payloadFilePath, serializedBytes);
                }
                else
                {
                    File.WriteAllText(payloadFilePath, operation.Payload.SerializedPayload);
                }

                return newId;
            }
        }

        /// <inheritdoc />
        public override void Execute(
            BlockRecordHandlingOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
            lock (this.handlingLock)
            {
                foreach (var locator in allLocators)
                {
                    var blocked = this.IsMostRecentBlocked(locator);

                    if (blocked)
                    {
                        throw new InvalidOperationException(Invariant($"Cannot block when a block already is in place without a cancel."));
                    }

                    var utcNow = DateTime.UtcNow;
                    var blockEvent = new BlockedRecordHandlingEvent(operation.Details, utcNow);
                    var payload =
                        blockEvent.ToDescribedSerializationUsingSpecificFactory(
                            this.DefaultSerializerRepresentation,
                            this.SerializerFactory,
                            this.DefaultSerializationFormat);

                    var metadata = new StreamRecordHandlingEntryMetadata(
                        Concerns.GlobalBlockingRecordId,
                        Concerns.RecordHandlingConcern,
                        HandlingStatus.Blocked,
                        null,
                        this.DefaultSerializerRepresentation,
                        NullStreamIdentifier.TypeRepresentation,
                        payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                        null,
                        utcNow,
                        blockEvent.TimestampUtc);

                    var entryId = this.GetNextRecordHandlingEntryId(locator);
                    this.PutRecordHandlingEntry(locator, Concerns.RecordHandlingConcern, entryId, metadata, payload);
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CancelBlockedRecordHandlingOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
            lock (this.handlingLock)
            {
                foreach (var locator in allLocators)
                {
                    var blocked = this.IsMostRecentBlocked(locator);

                    if (!blocked)
                    {
                        throw new InvalidOperationException(Invariant($"Cannot cancel a block that does not exist."));
                    }

                    var utcNow = DateTime.UtcNow;
                    var cancelBlockedEvent = new CanceledBlockedRecordHandlingEvent(operation.Details, utcNow);
                    var payload =
                        cancelBlockedEvent.ToDescribedSerializationUsingSpecificFactory(
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
                        cancelBlockedEvent.TimestampUtc);

                    var entryId = this.GetNextRecordHandlingEntryId(locator);
                    this.PutRecordHandlingEntry(locator, Concerns.RecordHandlingConcern, entryId, metadata, payload);
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CancelHandleRecordExecutionRequestOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            lock (this.handlingLock)
            {
                var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);

                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{operation.Id.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                if (mostRecentFilePath == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot cancel a requested {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                var mostRecent = this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

                if (mostRecent.Metadata.Status != HandlingStatus.Running && mostRecent.Metadata.Status != HandlingStatus.Failed)
                {
                    throw new InvalidOperationException(Invariant($"Cannot cancel an execution {nameof(HandleRecordOp)} unless it is {nameof(HandlingStatus.Requested)} or {nameof(HandlingStatus.Failed)}, the most recent status is {mostRecent.Metadata.Status}."));
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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CancelRunningHandleRecordExecutionOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);

            lock (this.handlingLock)
            {
                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{operation.Id.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                if (mostRecentFilePath == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot cancel a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                var mostRecent = this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CompleteRunningHandleRecordExecutionOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{operation.Id.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                if (mostRecentFilePath == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot complete a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                var mostRecent = this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            FailRunningHandleRecordExecutionOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{operation.Id.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                if (mostRecentFilePath == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot fail a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                var mostRecent = this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            SelfCancelRunningHandleRecordExecutionOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{operation.Id.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                if (mostRecentFilePath == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot self cancel a running {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                var mostRecent = this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        /// <inheritdoc />
        public override void Execute(
            RetryFailedHandleRecordExecutionOp operation)
        {
            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.handlingLock)
            {
                var concernDirectory = this.GetHandlingConcernDirectory(locator, operation.Concern);
                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___Id-{operation.Id.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var mostRecentFilePath = files.OrderByDescending(_ => _).FirstOrDefault();
                if (mostRecentFilePath == null)
                {
                    throw new InvalidOperationException(
                        Invariant(
                            $"Cannot retry a failed {nameof(HandleRecordOp)} execution as there is nothing in progress for concern {operation.Concern}."));
                }

                var mostRecent = this.GetStreamRecordHandlingEntryFromMetadataFile(mostRecentFilePath);

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

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, operation.Concern, entryId, metadata, payload);
            }
        }

        private FileSystemDatabaseLocator TryGetSingleLocator()
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

                    var result = allLocators.Single().ConfirmAndConvert<FileSystemDatabaseLocator>();

                    this.singleLocator = result;
                    return this.singleLocator;
                }
            }
        }

        private string GetRootPathFromLocator(
            IResourceLocator locator)
        {
            if (locator == null)
            {
                throw new ArgumentNullException(nameof(locator));
            }

            if (!(locator is FileSystemDatabaseLocator fileSystemLocator))
            {
                throw new ArgumentException(Invariant($"Only {nameof(FileSystemDatabaseLocator)}'s are supported; specified type: {locator.GetType().ToStringReadable()} - {locator.ToString()}"), nameof(locator));
            }

            var result = Path.Combine(fileSystemLocator.RootFolderPath, this.Name);
            return result;
        }

        private static void DeleteDirectoryAndConfirm(
            string directoryPath,
            bool recursive)
        {
            Directory.Delete(directoryPath, recursive);
            var timeoutTimeSpan = TimeSpan.FromSeconds(1);
            var timeout = DateTime.UtcNow.Add(timeoutTimeSpan);
            var directoryExists = true;
            while (directoryExists && DateTime.UtcNow < timeout)
            {
                directoryExists = Directory.Exists(directoryPath);
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }

            if (directoryExists)
            {
                throw new InvalidOperationException(Invariant($"Directory '{directoryPath}' was deleted but remains on disk after checking for '{timeoutTimeSpan.TotalSeconds}' seconds."));
            }
        }

        private static void CreateDirectoryAndConfirm(
            string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
            var timeoutTimeSpan = TimeSpan.FromSeconds(1);
            var timeout = DateTime.UtcNow.Add(timeoutTimeSpan);
            var directoryExists = false;
            while (!directoryExists && DateTime.UtcNow < timeout)
            {
                directoryExists = Directory.Exists(directoryPath);
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }

            if (!directoryExists)
            {
                throw new InvalidOperationException(Invariant($"Directory '{directoryPath}' was created but not found on disk after checking for '{timeoutTimeSpan.TotalSeconds}' seconds."));
            }
        }

        private StreamRecord GetStreamRecordFromMetadataFile(string metadataFilePath, StreamRecordMetadata metadata = null)
        {
            lock (this.fileLock)
            {
                if (metadata == null)
                {
                    var metadataFileText = File.ReadAllText(metadataFilePath);
                    metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(metadataFileText);
                }

                var filePathBase =
                    metadataFilePath.Substring(0, metadataFilePath.Length - MetadataFileExtension.Length - 1); // remove the '.' as well.
                var binaryFilePath = Invariant($"{filePathBase}.{BinaryFileExtension}");
                string stringPayload;
                SerializationFormat serializationFormat;
                if (File.Exists(binaryFilePath))
                {
                    var bytes = File.ReadAllBytes(binaryFilePath);
                    stringPayload = Convert.ToBase64String(bytes);
                    serializationFormat = SerializationFormat.Binary;
                }
                else
                {
                    var stringFilePath = Invariant(
                        $"{filePathBase}.{metadata.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture)}");
                    if (!File.Exists(stringFilePath))
                    {
                        throw new InvalidOperationException(
                            Invariant(
                                $"Expected payload file '{stringFilePath}' to exist to accompany metadata file '{metadataFilePath}' but was not found."));
                    }

                    stringPayload = File.ReadAllText(stringFilePath);
                    serializationFormat = SerializationFormat.String;
                }

                var internalRecordId = GetInternalRecordIdFromRecordFilePath(metadataFilePath);
                var payload = new DescribedSerialization(
                    metadata.TypeRepresentationOfObject.WithVersion,
                    stringPayload,
                    metadata.SerializerRepresentation,
                    serializationFormat);

                var result = new StreamRecord(internalRecordId, metadata, payload);
                return result;
            }
        }

        private StreamRecordHandlingEntry GetStreamRecordHandlingEntryFromMetadataFile(string metadataFilePath, StreamRecordHandlingEntryMetadata metadata = null)
        {
            lock (this.fileLock)
            {
                if (metadata == null)
                {
                    var metadataFileText = File.ReadAllText(metadataFilePath);
                    metadata = this.internalSerializer.Deserialize<StreamRecordHandlingEntryMetadata>(metadataFileText);
                }

                var filePathBase =
                    metadataFilePath.Substring(0, metadataFilePath.Length - MetadataFileExtension.Length - 1); // remove the '.' as well.
                var binaryFilePath = Invariant($"{filePathBase}.{BinaryFileExtension}");
                string stringPayload;
                SerializationFormat serializationFormat;
                if (File.Exists(binaryFilePath))
                {
                    var bytes = File.ReadAllBytes(binaryFilePath);
                    stringPayload = Convert.ToBase64String(bytes);
                    serializationFormat = SerializationFormat.Binary;
                }
                else
                {
                    var stringFilePath = Invariant(
                        $"{filePathBase}.{metadata.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture)}");
                    if (!File.Exists(stringFilePath))
                    {
                        throw new InvalidOperationException(
                            Invariant(
                                $"Expected payload file '{stringFilePath}' to exist to accompany metadata file '{metadataFilePath}' but was not found."));
                    }

                    stringPayload = File.ReadAllText(stringFilePath);
                    serializationFormat = SerializationFormat.String;
                }

                var internalRecordId = GetInternalRecordHandlingEntryIdFromEntryFilePath(metadataFilePath);

                var payload = new DescribedSerialization(
                    metadata.TypeRepresentationOfObject.WithVersion,
                    stringPayload,
                    metadata.SerializerRepresentation,
                    serializationFormat);

                var result = new StreamRecordHandlingEntry(internalRecordId, metadata, payload);
                return result;
            }
        }

        private static long GetInternalRecordIdFromRecordFilePath(
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is null or whitespace.", nameof(filePath));
            }

            var internalRecordIdString = Path.GetFileName(filePath)
                                            ?.Split(
                                                  new[]
                                                  {
                                                      "___",
                                                  },
                                                  StringSplitOptions.RemoveEmptyEntries)[0];

            if (string.IsNullOrWhiteSpace(internalRecordIdString))
            {
                throw new InvalidOperationException(Invariant($"Failed to extract internal record id from file path: '{filePath}'."));
            }

            var internalRecordId = long.Parse(internalRecordIdString, CultureInfo.InvariantCulture);
            return internalRecordId;
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

        private static HandlingStatus GetStatusFromEntryFilePath(string filePath)
        {
            var resultString = GetStringTokenFromFilePath(filePath, "Status");
            var result = resultString.ToEnum<HandlingStatus>();
            return result;
        }

        private static long GetInternalRecordIdFromEntryFilePath(string filePath)
        {
            var resultString = GetStringTokenFromFilePath(filePath, "Id");
            var result = long.Parse(resultString, CultureInfo.InvariantCulture);
            return result;
        }

        private static string GetStringTokenFromFilePath(
            string filePath,
            string tokenName)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is null or whitespace.", nameof(filePath));
            }

            var fileName = Path.GetFileName(filePath);
            var extensionWithLeadingDot = Path.GetExtension(filePath);
            var fileNameWithoutExtension = fileName.Substring(0, fileName.Length - extensionWithLeadingDot.Length);
            var tokens = fileNameWithoutExtension
                                            ?.Split(
                                                  new[]
                                                  {
                                                      "___",
                                                  },
                                                  StringSplitOptions.RemoveEmptyEntries);

            if (!tokens.Any())
            {
                throw new InvalidOperationException(Invariant($"Failed to extract tokens from file path: '{filePath}'."));
            }

            var token = tokens.SingleOrDefault(_ => _.StartsWith(tokenName + "-", StringComparison.Ordinal));
            if (token == null)
            {
                throw new InvalidOperationException(Invariant($"Failed to find token ({tokenName}) from file path: '{filePath}'."));
            }

            var tokenValue = token.Split('-')[1];

            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                throw new InvalidOperationException(Invariant($"Failed to extract token value ({tokenName}) from file path: '{filePath}'."));
            }

            return tokenValue;
        }

        private static long GetRootIdFromFilePath(
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is null or whitespace.", nameof(filePath));
            }

            var rootId = Path.GetFileName(filePath)
                                            ?.Split(
                                                  new[]
                                                  {
                                                      "___",
                                                  },
                                                  StringSplitOptions.RemoveEmptyEntries)[0];

            if (string.IsNullOrWhiteSpace(rootId))
            {
                throw new InvalidOperationException(Invariant($"Failed to extract root id from file path: '{filePath}'."));
            }

            var result = long.Parse(rootId, CultureInfo.InvariantCulture);
            return result;
        }

        private DateTime GetRootDateFromFilePath(
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is null or whitespace.", nameof(filePath));
            }

            var internalRecordDateString = Path.GetFileName(filePath)
                                            ?.Split(
                                                  new[]
                                                  {
                                                      "___",
                                                  },
                                                  StringSplitOptions.RemoveEmptyEntries)[1];

            if (string.IsNullOrWhiteSpace(internalRecordDateString))
            {
                throw new InvalidOperationException(Invariant($"Failed to extract internal record id from file path: '{filePath}'."));
            }

            var prepped = internalRecordDateString.Replace("-", ":");
            var result = this.dateTimeStringSerializer.Deserialize<DateTime>(prepped);
            return result;
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
                if (currentStatus.IsHandlingNeeded())
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
            var result = status == HandlingStatus.Blocked;
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
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
            DescribedSerialization payload)
        {
            var concernDirectory = this.GetHandlingConcernDirectory(locator, concern);
            var timestampString = this.dateTimeStringSerializer.SerializeToString(metadata.TimestampUtc).Replace(":", "-");
            var fileExtension = payload.SerializationFormat == SerializationFormat.Binary ? BinaryFileExtension :
                payload.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture);
            var fileBaseName = Invariant($"{entryId.PadWithLeadingZeros()}___{timestampString}___Id-{metadata.InternalRecordId.PadWithLeadingZeros()}___ExtId-{metadata.StringSerializedId?.EncodeForFilePath() ?? nameof(NullStreamIdentifier)}___Status-{metadata.Status}");
            var metadataFileName = Invariant($"{fileBaseName}.{MetadataFileExtension}");
            var payloadFileName = Invariant($"{fileBaseName}.{fileExtension}");
            var metadataFilePath = Path.Combine(concernDirectory, metadataFileName);
            var payloadFilePath = Path.Combine(concernDirectory, payloadFileName);

            var stringSerializedMetadata = this.internalSerializer.SerializeToString(metadata);
            File.WriteAllText(metadataFilePath, stringSerializedMetadata);
            if (fileExtension == BinaryFileExtension)
            {
                var serializedBytes = Convert.FromBase64String(payload.SerializedPayload);

                File.WriteAllBytes(payloadFilePath, serializedBytes);
            }
            else
            {
                File.WriteAllText(payloadFilePath, payload.SerializedPayload);
            }
        }
    }
}
