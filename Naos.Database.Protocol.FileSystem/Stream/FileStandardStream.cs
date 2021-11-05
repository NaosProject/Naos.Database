// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStandardStream.cs" company="Naos Project">
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
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Enum.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;
    using DomainExtensions = OBeautifulCode.Serialization.DomainExtensions;

    /// <summary>
    /// File system implementation of <see cref="IReadWriteStream"/>, it is thread resilient but not necessarily thread safe.
    /// Implements the <see cref="StandardStreamBase" />.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class FileStandardStream :
        StandardStreamBase
    {
        private const string NullToken = "null";
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
        /// Initializes a new instance of the <see cref="FileStandardStream"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="defaultSerializerRepresentation">Default serializer description to use.</param>
        /// <param name="defaultSerializationFormat">Default serializer format.</param>
        /// <param name="serializerFactory">The factory to get a serializer to use for objects.</param>
        /// <param name="resourceLocatorProtocols">The protocols for getting locators.</param>
        public FileStandardStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            IResourceLocatorProtocols resourceLocatorProtocols)
        : base(name, serializerFactory, defaultSerializerRepresentation, defaultSerializationFormat, resourceLocatorProtocols)
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
        public override CreateStreamResult Execute(
            StandardCreateStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation)
                                        ?? throw new ArgumentException(FormattableString.Invariant($"Invalid implementation of {nameof(IStreamRepresentation)}, expected '{nameof(FileStreamRepresentation)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            var alreadyExists = false;
            var wasCreated = true;
            lock (this.fileLock)
            {
                foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
                {
                    var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.Name);
                    var exists = Directory.Exists(directoryPath);
                    if (exists)
                    {
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse - this is not true as it's iterating the potential locators...
                        alreadyExists = alreadyExists || exists;
                        switch (operation.ExistingStreamStrategy)
                        {
                            case ExistingStreamStrategy.Overwrite:
                                DeleteDirectoryAndConfirm(directoryPath, true);
                                CreateDirectoryAndConfirm(directoryPath);
                                break;
                            case ExistingStreamStrategy.Skip:
                                wasCreated = false;
                                break;
                            case ExistingStreamStrategy.Throw:
                                throw new InvalidOperationException(
                                    FormattableString.Invariant(
                                        $"Path '{directoryPath}' already exists and {nameof(operation.ExistingStreamStrategy)} on the operation is {operation.ExistingStreamStrategy}."));
                            default:
                                throw new NotSupportedException(
                                    FormattableString.Invariant(
                                        $"{nameof(operation.ExistingStreamStrategy)} value '{operation.ExistingStreamStrategy}' is not supported."));
                        }
                    }
                    else
                    {
                        CreateDirectoryAndConfirm(directoryPath);
                    }
                }
            }

            var result = new CreateStreamResult(alreadyExists, wasCreated);
            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            StandardDeleteStreamOp operation)
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
                        switch (operation.StreamNotFoundStrategy)
                        {
                            case StreamNotFoundStrategy.Throw:
                                throw new InvalidOperationException(
                                    Invariant(
                                        $"Expected stream {operation.StreamRepresentation} to exist, it does not and the operation {nameof(operation.StreamNotFoundStrategy)} is '{operation.StreamNotFoundStrategy}'."));
                            case StreamNotFoundStrategy.Skip:
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
            StandardPruneStreamOp operation)
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
                    var internalRecordDate = this.GetRootDateFromFilePath(fileToConsiderRemoving);
                    if (operation.ShouldInclude(internalRecordId, internalRecordDate))
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
                        var internalEntryDate = this.GetRootDateFromFilePath(fileToConsiderRemoving);
                        if (operation.ShouldInclude(internalRecordId, internalEntryDate))
                        {
                            File.Delete(fileToConsiderRemoving);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override bool Execute(
            StandardDoesAnyExistByIdOp operation)
        {
            lock (this.fileLock)
            {
                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*___{operation.StringSerializedId?.EncodeForFilePath() ?? NullToken}.{MetadataFileExtension}"),
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
                        operation.VersionMatchStrategy))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <inheritdoc />
        public override StreamRecordMetadata Execute(
            StandardGetLatestRecordMetadataByIdOp operation)
        {
            lock (this.fileLock)
            {
                StreamRecordMetadata ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return null;
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(
                                Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }

                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*___{operation.StringSerializedId?.EncodeForFilePath() ?? NullToken}.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var orderedDescendingByInternalRecordId = metadataPathsThatCouldMatch.OrderByDescending(Path.GetFileName).ToList();
                if (!orderedDescendingByInternalRecordId.Any())
                {
                    return ProcessDefaultReturn();
                }

                foreach (var metadataFilePathToTest in orderedDescendingByInternalRecordId)
                {
                    var fileText = File.ReadAllText(metadataFilePathToTest);
                    var metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(fileText);
                    if (metadata.FuzzyMatchTypesAndId(
                        operation.StringSerializedId,
                        operation.IdentifierType,
                        operation.ObjectType,
                        operation.VersionMatchStrategy))
                    {
                        return metadata;
                    }
                }

                return ProcessDefaultReturn();
            }
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecord> Execute(
            StandardGetAllRecordsByIdOp operation)
        {
            lock (this.fileLock)
            {
                IReadOnlyList<StreamRecord> ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return new StreamRecord[0];
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(
                                Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }

                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*___{operation.StringSerializedId?.EncodeForFilePath() ?? NullToken}.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var orderedDescendingByInternalRecordId = metadataPathsThatCouldMatch.OrderByDescending(Path.GetFileName).ToList();
                if (!orderedDescendingByInternalRecordId.Any())
                {
                    return ProcessDefaultReturn();
                }

                var result = new List<StreamRecord>();
                foreach (var metadataFilePathToTest in orderedDescendingByInternalRecordId)
                {
                    var fileText = File.ReadAllText(metadataFilePathToTest);
                    var metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(fileText);
                    if (metadata.FuzzyMatchTypesAndId(
                        operation.StringSerializedId,
                        operation.IdentifierType,
                        operation.ObjectType,
                        operation.VersionMatchStrategy))
                    {
                        var record = this.GetStreamRecordFromMetadataFile(metadataFilePathToTest);
                        result.Add(record);
                    }
                }

                if (result.Any())
                {
                    switch (operation.OrderRecordsBy)
                    {
                        case OrderRecordsBy.InternalRecordIdAscending:
                            return result.OrderBy(_ => _.InternalRecordId).ToList();
                        case OrderRecordsBy.InternalRecordIdDescending:
                            return result.OrderByDescending(_ => _.InternalRecordId).ToList();
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(OrderRecordsBy)} {operation.OrderRecordsBy} is not supported."));
                    }
                }
                else
                {
                    return ProcessDefaultReturn();
                }
            }
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordMetadata> Execute(
            StandardGetAllRecordsMetadataByIdOp operation)
        {
            lock (this.fileLock)
            {
                IReadOnlyList<StreamRecordMetadata> ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return new StreamRecordMetadata[0];
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(
                                Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }

                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*___{operation.StringSerializedId?.EncodeForFilePath() ?? NullToken}.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var orderedDescendingByInternalRecordId = metadataPathsThatCouldMatch.OrderByDescending(Path.GetFileName).ToList();
                if (!orderedDescendingByInternalRecordId.Any())
                {
                    return ProcessDefaultReturn();
                }

                var result = new List<Tuple<long, StreamRecordMetadata>>();
                foreach (var metadataFilePathToTest in orderedDescendingByInternalRecordId)
                {
                    var fileText = File.ReadAllText(metadataFilePathToTest);
                    var metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(fileText);
                    if (metadata.FuzzyMatchTypesAndId(
                        operation.StringSerializedId,
                        operation.IdentifierType,
                        operation.ObjectType,
                        operation.VersionMatchStrategy))
                    {
                        var internalRecordId = GetRootIdFromFilePath(metadataFilePathToTest);
                        result.Add(new Tuple<long, StreamRecordMetadata>(internalRecordId, metadata));
                    }
                }

                if (result.Any())
                {
                    switch (operation.OrderRecordsBy)
                    {
                        case OrderRecordsBy.InternalRecordIdAscending:
                            return result.OrderBy(_ => _.Item1).Select(_ => _.Item2).ToList();
                        case OrderRecordsBy.InternalRecordIdDescending:
                            return result.OrderByDescending(_ => _.Item1).Select(_ => _.Item2).ToList();
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(OrderRecordsBy)} {operation.OrderRecordsBy} is not supported."));
                    }
                }
                else
                {
                    return ProcessDefaultReturn();
                }
            }
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<string> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            var result = new HashSet<string>();
            lock (this.fileLock)
            {
                var locators = new List<FileSystemDatabaseLocator>();
                if (operation.SpecifiedResourceLocator != null)
                {
                    locators.Add(operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>());
                }
                else
                {
                    var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
                    foreach (var locator in allLocators)
                    {
                        locators.Add(locator.ConfirmAndConvert<FileSystemDatabaseLocator>());
                    }
                }

                foreach (var fileSystemLocator in locators)
                {
                    var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                    var metadataPaths = Directory.GetFiles(
                        rootPath,
                        Invariant($"*.{MetadataFileExtension}"),
                        SearchOption.TopDirectoryOnly);

                    foreach (var metadataFilePath in metadataPaths)
                    {
                        var fileText = File.ReadAllText(metadataFilePath);
                        var metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(fileText);

                        if (metadata.FuzzyMatchTypes(
                                operation.IdentifierType,
                                operation.ObjectType,
                                operation.VersionMatchStrategy)
                         && ((!operation.TagsToMatch?.Any() ?? true)
                          || metadata.Tags.FuzzyMatchAccordingToStrategy(operation.TagsToMatch, operation.TagMatchStrategy)))
                        {
                            result.Add(metadata.StringSerializedId);
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public override long Execute(
            StandardGetNextUniqueLongOp operation)
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
            StandardGetHandlingHistoryOp operation)
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
        public override IReadOnlyCollection<HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
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
                                                               || _.Id                              >= operation.MinimumInternalRecordId));
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
                                                               || _.Id                              >= operation.MinimumInternalRecordId));
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
                                                               || _.Id                              >= operation.MinimumInternalRecordId));
                                metadataFilePath = randItem?.Path;
                                recordMetadata = randItem?.Metadata;
                                break;
                            default:
                                throw new NotSupportedException(Invariant($"{nameof(OrderRecordsBy)} {operation.OrderRecordsBy} is not supported."));
                        }

                        if (!string.IsNullOrWhiteSpace(metadataFilePath) && recordMetadata != null)
                        {
                            var recordToHandle = this.GetStreamRecordFromMetadataFile(metadataFilePath, recordMetadata);

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
            StandardGetRecordByInternalRecordIdOp operation)
        {
            var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(fileSystemLocator);
            lock (this.fileLock)
            {
                StreamRecord ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return null;
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(
                                Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                StreamRecord result = null;

                var matchingIdFile = metadataPathsThatCouldMatch.FirstOrDefault(_ => _ == null ? throw new InvalidOperationException("This should not have happened, a null path was returned for Directory . Get Files for " + rootPath) : Path.GetFileName(_).StartsWith(Invariant($"{((long)operation.InternalRecordId).PadWithLeadingZeros()}"), StringComparison.Ordinal));

                if (matchingIdFile == null)
                {
                    // could not find a matching file but a direct ID match was expected.
                    ProcessDefaultReturn();
                }
                else
                {
                    result = this.GetStreamRecordFromMetadataFile(matchingIdFile);
                }

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return ProcessDefaultReturn();
                }
            }
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(fileSystemLocator);
            lock (this.fileLock)
            {
                StreamRecord ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return null;
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(
                                Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var orderedDescendingByInternalRecordId = metadataPathsThatCouldMatch.OrderByDescending(Path.GetFileName).ToList();
                if (!orderedDescendingByInternalRecordId.Any())
                {
                    return ProcessDefaultReturn();
                }

                var latest = orderedDescendingByInternalRecordId.First();

                var result = this.GetStreamRecordFromMetadataFile(latest);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return ProcessDefaultReturn();
                }
            }
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordByIdOp operation)
        {
            lock (this.fileLock)
            {
                StreamRecord ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return null;
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(
                                Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }

                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);

                var metadataPathsThatCouldMatch = Directory.GetFiles(
                    rootPath,
                    Invariant($"*___{operation.StringSerializedId?.EncodeForFilePath() ?? NullToken}.{MetadataFileExtension}"),
                    SearchOption.TopDirectoryOnly);

                var orderedDescendingByInternalRecordId = metadataPathsThatCouldMatch.OrderByDescending(Path.GetFileName).ToList();
                if (!orderedDescendingByInternalRecordId.Any())
                {
                    return ProcessDefaultReturn();
                }

                foreach (var metadataFilePathToTest in orderedDescendingByInternalRecordId)
                {
                    var fileText = File.ReadAllText(metadataFilePathToTest);
                    var metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(fileText);
                    if (metadata.FuzzyMatchTypesAndId(
                        operation.StringSerializedId,
                        operation.IdentifierType,
                        operation.ObjectType,
                        operation.VersionMatchStrategy))
                    {
                        var result = this.GetStreamRecordFromMetadataFile(metadataFilePathToTest, metadata);
                        return result;
                    }
                }

                return ProcessDefaultReturn();
            }
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordByTagsOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public override PutRecordResult Execute(
            StandardPutRecordOp operation)
        {
            lock (this.fileLock)
            {
                var fileSystemLocator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
                var rootPath = this.GetRootPathFromLocator(fileSystemLocator);
                var recordIdentifierTrackingFilePath = Path.Combine(rootPath, RecordIdentifierTrackingFileName);

                var timestampString = this.dateTimeStringSerializer.SerializeToString(operation.Metadata.TimestampUtc).Replace(":", "--");

                long newId;

                var recordFilePathsToPrune = new List<string>();
                var existingRecordIds = new List<long>();
                lock (this.nextInternalRecordIdentifierLock)
                {
                    // no need to waste the cycles if it the logic is disabled
                    var metadataPathsThatCouldMatch = operation.ExistingRecordStrategy != ExistingRecordStrategy.None
                        ? Directory.GetFiles(
                            rootPath,
                            Invariant($"*___{operation.Metadata.StringSerializedId?.EncodeForFilePath() ?? NullToken}.{MetadataFileExtension}"),
                            SearchOption.TopDirectoryOnly)
                        : null;

                    var metadataThatCouldMatch =
                        (operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundById
                      || operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundById
                      || operation.ExistingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById)
                            ? metadataPathsThatCouldMatch?.Select(
                                                               _ => new
                                                                    {
                                                                        Path = _,
                                                                        Text = File.ReadAllText(_),
                                                                    })
                                                          .Select(
                                                               _ => new
                                                                    {
                                                                        MetadataPath = _.Path,
                                                                        BinaryDataPath = Path.ChangeExtension(_.Path, BinaryFileExtension),
                                                                        StringDataPath = Path.ChangeExtension(
                                                                            _.Path,
                                                                            this.DefaultSerializerRepresentation.SerializationKind.ToString()
                                                                                .ToLowerFirstCharacter(CultureInfo.InvariantCulture)),
                                                                        Metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(_.Text),
                                                                    })
                                                          .Where(
                                                               _ => _.Metadata.FuzzyMatchTypesAndId(
                                                                   operation.Metadata.StringSerializedId,
                                                                   operation.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                                                                       operation.VersionMatchStrategy),
                                                                   null,
                                                                   operation.VersionMatchStrategy))
                                                          .ToList()
                            : (operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundByIdAndTypeAndContent
                            || operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundByIdAndType
                            || operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundByIdAndTypeAndContent
                            || operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundByIdAndType
                            || operation.ExistingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType
                                ? metadataPathsThatCouldMatch?.Select(
                                                                   _ => new
                                                                        {
                                                                            Path = _,
                                                                            Text = File.ReadAllText(_),
                                                                        })
                                                              .Select(
                                                                   _ => new
                                                                        {
                                                                            MetadataPath = _.Path,
                                                                            BinaryDataPath = Path.ChangeExtension(_.Path, BinaryFileExtension),
                                                                            StringDataPath = Path.ChangeExtension(
                                                                                _.Path,
                                                                                this.DefaultSerializerRepresentation.SerializationKind.ToString()
                                                                                    .ToLowerFirstCharacter(CultureInfo.InvariantCulture)),
                                                                            Metadata = this.internalSerializer.Deserialize<StreamRecordMetadata>(_.Text),
                                                                        })
                                                              .Where(
                                                                   _ => _.Metadata.FuzzyMatchTypesAndId(
                                                                       operation.Metadata.StringSerializedId,
                                                                       operation.Metadata.TypeRepresentationOfId
                                                                                .GetTypeRepresentationByStrategy(
                                                                                     operation.VersionMatchStrategy),
                                                                       operation.Metadata.TypeRepresentationOfObject
                                                                                .GetTypeRepresentationByStrategy(
                                                                                     operation.VersionMatchStrategy),
                                                                       operation.VersionMatchStrategy))
                                                              .ToList()
                                : null);

                    switch (operation.ExistingRecordStrategy)
                    {
                        case ExistingRecordStrategy.None:
                            /* no-op */
                            break;
                        case ExistingRecordStrategy.ThrowIfFoundById:
                            if (metadataThatCouldMatch?.Any() ?? throw new InvalidOperationException(Invariant($"This should be unreachable as {nameof(metadataPathsThatCouldMatch)} should not be null.")))
                            {
                                throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordStrategy)} was {operation.ExistingRecordStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' yet found {metadataPathsThatCouldMatch.Length}."));
                            }

                            break;
                        case ExistingRecordStrategy.ThrowIfFoundByIdAndType:
                            if (metadataThatCouldMatch?.Any() ?? throw new InvalidOperationException(Invariant($"This should be unreachable as {nameof(metadataPathsThatCouldMatch)} should not be null.")))
                            {
                                throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordStrategy)} was {operation.ExistingRecordStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' and object type '{operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy)}' yet found {metadataThatCouldMatch.Count}."));
                            }

                            break;
                        case ExistingRecordStrategy.ThrowIfFoundByIdAndTypeAndContent:
                            var matchesThrow =
                                metadataThatCouldMatch?
                                   .Where(_ =>
                                          {
                                              var binaryFileExists = File.Exists(_.BinaryDataPath);
                                              var stringFileExists = File.Exists(_.StringDataPath);
                                              if (binaryFileExists && stringFileExists)
                                              {
                                                  throw new NotSupportedException(Invariant($"Found a file for the same metadata but in both string and binary formats, this is not supported: '{_.BinaryDataPath}' and '{_.StringDataPath}'."));
                                              }

                                              switch (operation.Payload.GetSerializationFormat())
                                              {
                                                  case SerializationFormat.String:
                                                      if (binaryFileExists)
                                                      {
                                                          throw new NotSupportedException(Invariant($"Found a file for the id and type in binary when a string payload is being put, this is not supported: '{_.BinaryDataPath}'."));
                                                      }

                                                      var stringPayload = ((StringDescribedSerialization)operation.Payload).SerializedPayload;
                                                      var fileStringPayload = File.ReadAllText(_.StringDataPath);
                                                      return fileStringPayload.Equals(stringPayload ?? NullToken);
                                                  case SerializationFormat.Binary:
                                                      if (binaryFileExists)
                                                      {
                                                          throw new NotSupportedException(Invariant($"Found a file for the id and type in binary when a Binary payload is being put, this is not supported: '{_.BinaryDataPath}'."));
                                                      }

                                                      var binaryPayload = ((BinaryDescribedSerialization)operation.Payload).SerializedPayload;
                                                      var fileBinaryPayload = File.ReadAllBytes(_.BinaryDataPath);
                                                      return fileBinaryPayload.Equals(binaryPayload);
                                                  default:
                                                      throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} {operation.Payload.GetSerializationFormat()} is not supported."));
                                              }
                                          })
                                   .ToList()
                             ?? throw new InvalidOperationException(Invariant($"This should be unreachable as {nameof(metadataThatCouldMatch)} should not be null."));

                            if (matchesThrow.Any())
                            {
                                throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordStrategy)} was {operation.ExistingRecordStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' and object type '{operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy)}' and contents '{operation.Payload}' yet found {matchesThrow.Count}."));
                            }

                            break;
                        case ExistingRecordStrategy.DoNotWriteIfFoundById:
                            if (metadataPathsThatCouldMatch?.Any() ?? throw new InvalidOperationException(Invariant($"This should be unreachable as {nameof(metadataPathsThatCouldMatch)} should not be null.")))
                            {
                                var matchingIds = metadataPathsThatCouldMatch.Select(GetInternalRecordIdFromRecordFilePath).ToList();
                                return new PutRecordResult(null, matchingIds);
                            }

                            break;
                        case ExistingRecordStrategy.DoNotWriteIfFoundByIdAndType:
                            if (metadataThatCouldMatch?.Any() ?? throw new InvalidOperationException(Invariant($"This should be unreachable as {nameof(metadataPathsThatCouldMatch)} should not be null.")))
                            {
                                var matchingIds = metadataPathsThatCouldMatch.Select(GetInternalRecordIdFromRecordFilePath).ToList();
                                return new PutRecordResult(null, matchingIds);
                            }

                            break;
                        case ExistingRecordStrategy.DoNotWriteIfFoundByIdAndTypeAndContent:
                            var matchesDoNotWrite =
                                metadataThatCouldMatch?
                                   .Where(_ =>
                                   {
                                       var binaryFileExists = File.Exists(_.BinaryDataPath);
                                       var stringFileExists = File.Exists(_.StringDataPath);
                                       if (binaryFileExists && stringFileExists)
                                       {
                                           throw new NotSupportedException(Invariant($"Found a file for the same metadata but in both string and binary formats, this is not supported: '{_.BinaryDataPath}' and '{_.StringDataPath}'."));
                                       }

                                       switch (operation.Payload.GetSerializationFormat())
                                       {
                                           case SerializationFormat.String:
                                               if (binaryFileExists)
                                               {
                                                   throw new NotSupportedException(Invariant($"Found a file for the id and type in binary when a string payload is being put, this is not supported: '{_.BinaryDataPath}'."));
                                               }

                                               var stringPayload = ((StringDescribedSerialization)operation.Payload).SerializedPayload;
                                               var fileStringPayload = File.ReadAllText(_.StringDataPath);
                                               return fileStringPayload.Equals(stringPayload ?? NullToken);
                                           case SerializationFormat.Binary:
                                               if (binaryFileExists)
                                               {
                                                   throw new NotSupportedException(Invariant($"Found a file for the id and type in binary when a Binary payload is being put, this is not supported: '{_.BinaryDataPath}'."));
                                               }

                                               var binaryPayload = ((BinaryDescribedSerialization)operation.Payload).SerializedPayload;
                                               var fileBinaryPayload = File.ReadAllBytes(_.BinaryDataPath);
                                               return fileBinaryPayload.Equals(binaryPayload);
                                           default:
                                               throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} {operation.Payload.GetSerializationFormat()} is not supported."));
                                       }
                                   })
                                   .ToList()
                             ?? throw new InvalidOperationException(Invariant($"This should be unreachable as {nameof(metadataThatCouldMatch)} should not be null."));

                            if (matchesDoNotWrite.Any())
                            {
                                return new PutRecordResult(null, matchesDoNotWrite.Select(_ => GetInternalRecordIdFromRecordFilePath(_.MetadataPath)).ToList());
                            }

                            break;
                        case ExistingRecordStrategy.PruneIfFoundById:
                            if (metadataThatCouldMatch != null && operation.RecordRetentionCount != null && metadataPathsThatCouldMatch.Length >= operation.RecordRetentionCount - 1)
                            {
                                existingRecordIds.AddRange(
                                    metadataThatCouldMatch
                                       .Select(_ => GetInternalRecordIdFromRecordFilePath(_.MetadataPath))
                                       .ToList());
                                var recordsToDeleteById =
                                    metadataThatCouldMatch.OrderByDescending(_ => _.MetadataPath).Skip((int)operation.RecordRetentionCount - 1).ToList();
                                recordFilePathsToPrune.AddRange(
                                    recordsToDeleteById.SelectMany(
                                        _ => new[]
                                             {
                                                 _.MetadataPath,
                                                 _.BinaryDataPath,
                                                 _.StringDataPath,
                                             }));
                            }

                            break;
                        case ExistingRecordStrategy.PruneIfFoundByIdAndType:
                            if (metadataThatCouldMatch != null && operation.RecordRetentionCount != null && metadataPathsThatCouldMatch.Length >= operation.RecordRetentionCount - 1)
                            {
                                existingRecordIds.AddRange(
                                    metadataThatCouldMatch
                                       .Select(_ => GetInternalRecordIdFromRecordFilePath(_.MetadataPath))
                                       .ToList());
                                var recordsToDeleteById =
                                    metadataThatCouldMatch.OrderByDescending(_ => _.MetadataPath).Skip((int)operation.RecordRetentionCount - 1).ToList();
                                recordFilePathsToPrune.AddRange(
                                    recordsToDeleteById.SelectMany(
                                        _ => new[]
                                             {
                                                 _.MetadataPath,
                                                 _.BinaryDataPath,
                                                 _.StringDataPath,
                                             }));
                            }

                            break;
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(ExistingRecordStrategy)} {operation.ExistingRecordStrategy} is not supported."));
                    }

                    if (operation.InternalRecordId == null)
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
                            currentInternalRecordIdentifierString = string.IsNullOrWhiteSpace(currentInternalRecordIdentifierString)
                                ? 0.ToString(CultureInfo.InvariantCulture)
                                : currentInternalRecordIdentifierString;
                            var currentId = long.Parse(currentInternalRecordIdentifierString, CultureInfo.InvariantCulture);
                            newId = currentId + 1;
                            fileStream.Position = 0;
                            var writer = new StreamWriter(fileStream);
                            writer.Write(newId.ToString(CultureInfo.InvariantCulture));

                            // necessary to flush buffer.
                            writer.Close();
                        }
                    }
                    else
                    {
                        var operationInternalRecordId = (long)operation.InternalRecordId;
                        if (Directory.GetFiles(
                                          rootPath,
                                          Invariant($"{operationInternalRecordId.PadWithLeadingZeros()}___*.{MetadataFileExtension}"),
                                          SearchOption.TopDirectoryOnly)
                                     .Any())
                        {
                            throw new InvalidOperationException(Invariant($"Operation specified an {nameof(StandardPutRecordOp.InternalRecordId)} of {operation.InternalRecordId} but that {nameof(StandardPutRecordOp.InternalRecordId)} is already present in the stream."));
                        }

                        newId = operationInternalRecordId;
                    }
                }

                var fileExtension = operation.Payload.GetSerializationFormat() == SerializationFormat.Binary ? BinaryFileExtension :
                    operation.Payload.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture);
                var filePathIdentifier = operation.Metadata.StringSerializedId?.EncodeForFilePath() ?? NullToken;
                var fileBaseName = Invariant($"{newId.PadWithLeadingZeros()}___{timestampString}___{filePathIdentifier}");
                var metadataFileName = Invariant($"{fileBaseName}.{MetadataFileExtension}");
                var payloadFileName = Invariant($"{fileBaseName}.{fileExtension}");
                var metadataFilePath = Path.Combine(rootPath, metadataFileName);
                var payloadFilePath = Path.Combine(rootPath, payloadFileName);

                var stringSerializedMetadata = this.internalSerializer.SerializeToString(operation.Metadata);
                File.WriteAllText(metadataFilePath, stringSerializedMetadata);
                if (fileExtension == BinaryFileExtension)
                {
                    var serializedBytes = ((BinaryDescribedSerialization)operation.Payload).SerializedPayload;

                    File.WriteAllBytes(payloadFilePath, serializedBytes.ToArray());
                }
                else
                {
                    var serializedString = ((StringDescribedSerialization)operation.Payload).SerializedPayload;

                    File.WriteAllText(payloadFilePath, serializedString ?? NullToken);
                }

                recordFilePathsToPrune.ForEach(File.Delete);
                var prunedRecordIds = recordFilePathsToPrune.Select(GetInternalRecordIdFromRecordFilePath).Distinct().ToList();

                var result = new PutRecordResult(newId, existingRecordIds, prunedRecordIds);
                return result;
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
                    null,
                    this.DefaultSerializerRepresentation,
                    NullStreamIdentifier.TypeRepresentation,
                    payload.PayloadTypeRepresentation.ToWithAndWithoutVersion(),
                    operation.Tags,
                    utcNow);

                var entryId = this.GetNextRecordHandlingEntryId(locator);
                this.PutRecordHandlingEntry(locator, concern, entryId, metadata, payload);
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
                        this.CancelHandleRecordExecutionRequest(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterExternalCancellation:
                        this.CancelRunningHandleRecordExecution(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.Completed:
                        this.CompleteRunningHandleRecordExecution(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.Failed:
                        this.FailRunningHandleRecordExecution(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterSelfCancellation:
                        this.SelfCancelRunningHandleRecordExecution(operation, locator, mostRecent);
                        break;
                    case HandlingStatus.AvailableAfterFailure:
                        this.RetryFailedHandleRecordExecution(operation, locator, mostRecent);
                        break;
                    default:
                        throw new NotSupportedException(Invariant($"Unsupported {nameof(HandlingStatus)} '{operation.NewStatus}' to update handling of {nameof(operation.InternalRecordId)}: {operation.InternalRecordId}."));
                }
            }
        }

        private void CancelHandleRecordExecutionRequest(
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

        private void CancelRunningHandleRecordExecution(
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

                var newEvent = new RecordHandlingCancelledEvent(
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

        private void CompleteRunningHandleRecordExecution(
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

        private void FailRunningHandleRecordExecution(
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

        private void SelfCancelRunningHandleRecordExecution(
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

                var newEvent = new RecordHandlingSelfCancelledEvent(
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

        private void RetryFailedHandleRecordExecution(
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
                DescribedSerializationBase payload;
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
                    var stringFilePath = Invariant(
                        $"{filePathBase}.{metadata.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture)}");
                    if (!File.Exists(stringFilePath))
                    {
                        throw new InvalidOperationException(
                            Invariant(
                                $"Expected payload file '{stringFilePath}' to exist to accompany metadata file '{metadataFilePath}' but was not found."));
                    }

                    var stringPayload = File.ReadAllText(stringFilePath);

                    payload = new StringDescribedSerialization(
                        metadata.TypeRepresentationOfObject.WithVersion,
                        metadata.SerializerRepresentation,
                        stringPayload);
                }

                var internalRecordId = GetInternalRecordIdFromRecordFilePath(metadataFilePath);

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

            var prepped = internalRecordDateString.Replace("--", ":");
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
    }
}
