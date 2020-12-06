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
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
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
        private const string RecordHandlingTrackingDirectoryName = "_HandlingTracking";
        private const string RecordIdentifierTrackingFileName = "_InternalRecordIdentifierTracking.nfo";
        private const string NextUniqueLongTrackingFileName = "_NextUniqueLongTracking.nfo";
        private const string BinaryFileExtension = "bin";
        private const string MetadataFileExtension = "meta";

        private readonly object fileLock = new object();
        private readonly object handlingLock = new object();
        private readonly object singleLocatorLock = new object();

        private readonly object nextInternalRecordIdentifierLock = new object();
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
        : base(name, resourceLocatorProtocols)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            defaultSerializerRepresentation.MustForArg(nameof(defaultSerializerRepresentation)).NotBeNull();
            serializerFactory.MustForArg(nameof(serializerFactory)).NotBeNull();
            resourceLocatorProtocols.MustForArg(nameof(resourceLocatorProtocols)).NotBeNull();

            this.internalSerializer = serializerFactory.BuildSerializer(defaultSerializerRepresentation);

            this.DefaultSerializerRepresentation = defaultSerializerRepresentation;
            this.SerializerFactory = serializerFactory;
            this.DefaultSerializationFormat = defaultSerializationFormat;
        }

        /// <summary>
        /// Gets the default serializer description.
        /// </summary>
        /// <value>The default serializer description.</value>
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <summary>
        /// Gets the default serialization format.
        /// </summary>
        /// <value>The default serialization format.</value>
        public SerializationFormat DefaultSerializationFormat { get; private set; }

        /// <summary>
        /// Gets the serializer factory.
        /// </summary>
        /// <value>The serializer factory.</value>
        public ISerializerFactory SerializerFactory { get; private set; }

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
        public override IStreamReadProtocols GetStreamReadingProtocols() => new FileStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>() => new FileStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public override IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>() => new FileStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public override IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>() => new FileStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols GetStreamWritingProtocols() => new FileStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>() => new FileStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>() => new FileStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public override IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>() => new FileStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols() => new FileStreamManagementProtocols(this);

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols() => new FileStreamRecordHandlingProtocols(this);

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>() => new FileStreamRecordHandlingProtocols<TObject>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>() => new FileStreamRecordWithIdHandlingProtocols<TId>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>() => new FileStreamRecordWithIdHandlingProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public void Execute(
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
        public async Task ExecuteAsync(
            CreateStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public void Execute(
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
        public async Task ExecuteAsync(
            DeleteStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordDateOp operation)
        {
            var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

            lock (this.fileLock)
            {
                var filesToPotentiallyRemove
                    = Directory.GetFiles(
                        rootPath,
                        Invariant($"*___*"),
                        SearchOption.TopDirectoryOnly);
                foreach (var fileToConsiderRemoving in filesToPotentiallyRemove)
                {
                    var internalRecordDate = this.GetInternalRecordDateFromFilePath(fileToConsiderRemoving);
                    if (internalRecordDate < operation.MaxInternalRecordDate)
                    {
                        File.Delete(fileToConsiderRemoving);
                    }
                }
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
            var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

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
                    if (internalRecordId < operation.MaxInternalRecordId)
                    {
                        File.Delete(fileToConsiderRemoving);
                    }
                }
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public long Execute(
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
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOfRecordOp operation)
        {
            var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(fileSystemLocator);
            var handleDirectory = Path.Combine(rootPath, RecordHandlingTrackingDirectoryName);
            var concernDirectory = Path.Combine(handleDirectory, operation.Concern);
            lock (this.handlingLock)
            {
                var files = Directory.GetFiles(
                    concernDirectory,
                    Invariant($"*___RecordId-{operation.InternalRecordId}___*.{MetadataFileExtension}"),
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
        public HandlingStatus Execute(
            GetHandlingStatusOfRecordsByIdOp operation)
        {
            var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(fileSystemLocator);
            var handleDirectory = Path.Combine(rootPath, RecordHandlingTrackingDirectoryName);
            var concernDirectory = Path.Combine(handleDirectory, operation.Concern);
            lock (this.handlingLock)
            {
                var files = operation.IdsToMatch.SelectMany(
                                          _ => Directory.GetFiles(
                                              concernDirectory,
                                              "*___Id-" + _.StringSerializedId.EncodeForFilePath() + "___*." + MetadataFileExtension,
                                              SearchOption.TopDirectoryOnly).Select(__ => new Tuple<StringSerializedIdentifier, string>(_, __)))
                                     .ToList();

                var statuses = new List<HandlingStatus>();
                foreach (var file in files)
                {
                    var text = File.ReadAllText(file.Item2);
                    var item = this.internalSerializer.Deserialize<StreamRecordHandlingEntry>(text);

                    if (item.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.TypeVersionMatchStrategy)
                            .EqualsAccordingToStrategy(file.Item1.IdentifierType, operation.TypeVersionMatchStrategy))
                    {
                        statuses.Add(item.Metadata.Status);
                    }
                }

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
                    var rootPath = this.GetRootPathFromLocator(locator);
                    var handleDirectory = Path.Combine(rootPath, RecordHandlingTrackingDirectoryName);
                    var concernDirectory = Path.Combine(handleDirectory, operation.Concern);

                    var files = Directory.GetFiles(
                                              concernDirectory,
                                              "*." + MetadataFileExtension,
                                              SearchOption.TopDirectoryOnly)
                                         .ToList();

                    foreach (var file in files)
                    {
                        var text = File.ReadAllText(file);
                        var metadata = this.internalSerializer.Deserialize<StreamRecordHandlingEntryMetadata>(text);
                        if (operation.TagsToMatch.FuzzyMatchAccordingToStrategy(metadata.Tags, operation.TagMatchStrategy))
                        {
                            statuses.Add(metadata.Status);
                        }
                    }
                }

                var result = statuses.ReduceToCompositeHandlingStatus(operation.HandlingStatusCompositionStrategy);
                return result;
            }
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            TryHandleRecordOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecord Execute(
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
        public StreamRecord Execute(
            GetLatestRecordByIdOp operation)
        {
            lock (this.fileLock)
            {
                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*{operation.StringSerializedId}*.{MetadataFileExtension}"),
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
        public long Execute(
            PutRecordOp operation)
        {
            lock (this.fileLock)
            {
                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                var timestampString = this.dateTimeStringSerializer.SerializeToString(operation.Metadata.TimestampUtc).Replace(":", "-");
                var recordIdentifierTrackingFilePath = Path.Combine(rootPath, RecordIdentifierTrackingFileName);

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
                        string currentInternalRecordIdentifierString;
                        var reader = new StreamReader(fileStream);
                        currentInternalRecordIdentifierString = reader.ReadToEnd();
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
                var fileBaseName = Invariant($"{newId}___{timestampString}___{filePathIdentifier}");
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

        private DateTime GetInternalRecordDateFromFilePath(
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

        /// <inheritdoc />
        public void Execute(
            BlockRecordHandlingOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            CancelBlockedRecordHandlingOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            CancelHandleRecordExecutionRequestOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            CancelRunningHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            CompleteRunningHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            FailRunningHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            SelfCancelRunningHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException();
        }
    }
}
