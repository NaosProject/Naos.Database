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
        IReturningProtocol<GetHandlingStatusOfRecordSetByIdOp, HandlingStatus>,
        IReturningProtocol<GetHandlingStatusOfRecordSetByTagOp, HandlingStatus>,
        IReturningProtocol<TryHandleRecordOp, StreamRecord>,
        IReturningProtocol<PutRecordOp, long>
    {
        private const string RecordIdentifierTrackingFileName = "_InternalRecordIdentifierTracking.nfo";
        private const string NextUniqueLongTrackingFileName = "_NextUniqueLongTracking.nfo";
        private const string BinaryFileExtension = "bin";
        private const string MetadataFileExtension = "meta";

        private readonly object fileLock = new object();

        private readonly object nextInternalRecordIdentifierLock = new object();
        private readonly object nextUniqueLongLock = new object();
        private readonly ObcDateTimeStringSerializer dateTimeStringSerializer = new ObcDateTimeStringSerializer();
        private readonly ISerializer metadataSerializer;

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

            this.metadataSerializer = serializerFactory.BuildSerializer(defaultSerializerRepresentation);

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
                            throw new InvalidOperationException(FormattableString.Invariant($"Path '{directoryPath}' already exists and {nameof(operation.ExistingStreamEncounteredStrategy)} on the operation is {operation.ExistingStreamEncounteredStrategy}."));
                        default:
                            throw new NotSupportedException(FormattableString.Invariant($"{nameof(operation.ExistingStreamEncounteredStrategy)} value '{operation.ExistingStreamEncounteredStrategy}' is not supported."));
                    }
                }
                else
                {
                    CreateDirectoryAndConfirm(directoryPath);
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordIdOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var resourceLocator = this.ResourceLocatorProtocols.Execute(new GetResourceLocatorForUniqueIdentifierOp());
            var fileLocator = resourceLocator as FileSystemDatabaseLocator
                           ?? throw new InvalidOperationException(
                                  Invariant(
                                      $"Resource locator was expected to be a {nameof(FileSystemDatabaseLocator)} but was a '{resourceLocator?.GetType()?.ToStringReadable() ?? "<null>"}'."));
            var rootPath = Path.Combine(fileLocator.RootFolderPath, this.Name);
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
                        ? this.metadataSerializer.Deserialize<IList<UniqueLongIssuedEvent>>(currentSerializedListText)
                        : new List<UniqueLongIssuedEvent>();

                    nextLong = currentList.Any()
                        ? currentList.Max(_ => _.Id) + 1
                        : 1;

                    currentList.Add(new UniqueLongIssuedEvent(nextLong, DateTime.UtcNow, operation.Details));
                    var updatedSerializedListText = this.metadataSerializer.SerializeToString(currentList);

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
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOfRecordSetByIdOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOfRecordSetByTagOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            TryHandleRecordOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            GetLatestRecordOp operation)
        {
            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
            if (allLocators.Count != 1)
            {
                throw new NotSupportedException(Invariant($"Cannot execute '{nameof(GetLatestRecordOp)}' with more than one locator as there is no way to know which one to target; there are {allLocators.Count}."));
            }

            var singleFileLocator = allLocators.Single();
            var rootPath = this.GetRootPathFromLocator(singleFileLocator);

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

        /// <inheritdoc />
        public StreamRecord Execute(
            GetLatestRecordByIdOp operation)
        {
            var rootPath = this.GetRootPathFromLocator(operation.Locator);

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
                var metadata = this.metadataSerializer.Deserialize<StreamRecordMetadata>(fileText);
                if (metadata.FuzzyMatchTypesAndId(operation.StringSerializedId, operation.IdentifierType, operation.ObjectType, operation.TypeVersionMatchStrategy))
                {
                    var result = this.GetStreamRecordFromMetadataFile(metadataFilePathToTest, metadata);
                }
            }

            return default;
        }

        /// <inheritdoc />
        public long Execute(
            PutRecordOp operation)
        {
            var rootPath = this.GetRootPathFromLocator(operation.Locator);
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
                    string currentIdString;
                    var reader = new StreamReader(fileStream);
                    currentIdString = reader.ReadToEnd();
                    currentIdString = string.IsNullOrWhiteSpace(currentIdString) ? 0.ToString(CultureInfo.InvariantCulture) : currentIdString;
                    var currentId = long.Parse(currentIdString, CultureInfo.InvariantCulture);
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

            var stringSerializedMetadata = this.metadataSerializer.SerializeToString(operation.Metadata);
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

        private string GetRootPathFromLocator(
            IResourceLocator resourceLocator)
        {
            var fileLocator = resourceLocator as FileSystemDatabaseLocator ?? throw new InvalidOperationException(Invariant($"Resource locator was expected to be a {nameof(FileSystemDatabaseLocator)} but was a '{resourceLocator?.GetType()?.ToStringReadable() ?? "<null>"}'."));
            var result = Path.Combine(fileLocator.RootFolderPath, this.Name);
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
            if (metadata == null)
            {
                var metadataFileText = File.ReadAllText(metadataFilePath);
                metadata = this.metadataSerializer.Deserialize<StreamRecordMetadata>(metadataFileText);
            }

            var filePathBase = metadataFilePath.Substring(0, metadataFilePath.Length - MetadataFileExtension.Length - 1); // remove the '.' as well.
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
                var stringFilePath = Invariant($"{filePathBase}.{metadata.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture)}");
                if (!File.Exists(stringFilePath))
                {
                    throw new InvalidOperationException(Invariant($"Expected payload file '{stringFilePath}' to exist to accompany metadata file '{metadataFilePath}' but was not found."));
                }

                stringPayload = File.ReadAllText(stringFilePath);
                serializationFormat = SerializationFormat.String;
            }

            var internalRecordIdString = Path.GetFileName(metadataFilePath).Split(new[] { "___" }, StringSplitOptions.RemoveEmptyEntries)[0];
            var internalRecordId = long.Parse(internalRecordIdString);
            var payload = new DescribedSerialization(
                metadata.TypeRepresentationOfObject.WithVersion,
                stringPayload,
                metadata.SerializerRepresentation,
                serializationFormat);

            var result = new StreamRecord(internalRecordId, metadata, payload);
            return result;
        }
    }
}
