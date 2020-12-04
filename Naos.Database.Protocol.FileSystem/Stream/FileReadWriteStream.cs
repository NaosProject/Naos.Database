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
        private const string BinaryFileExtension = "bin";
        private const string MetadataFileExtension = "meta";

        private readonly object fileLock = new object();

        private readonly object nextInternalRecordIdentifierLock = new object();
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
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task ExecuteAsync(
            CreateStreamOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            DeleteStreamOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task ExecuteAsync(
            DeleteStreamOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordDateOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task ExecuteAsync(
            PruneBeforeInternalRecordDateOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordIdOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task ExecuteAsync(
            PruneBeforeInternalRecordIdOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
                    var recordSerializer = this.SerializerFactory.BuildSerializer(metadata.SerializerRepresentation);
                    var filePathBase = metadataFilePathToTest.Substring(0, metadataFilePathToTest.Length - MetadataFileExtension.Length - 1); // remove the '.' as well.
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
                            throw new InvalidOperationException(Invariant($"Expected payload file '{stringFilePath}' to exist to accompany metadata file '{metadataFilePathToTest}' but was not found."));
                        }

                        stringPayload = File.ReadAllText(stringFilePath);
                        serializationFormat = SerializationFormat.String;
                    }

                    var internalRecordIdString = Path.GetFileName(metadataFilePathToTest).Split(new[] { "___" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var internalRecordId = long.Parse(internalRecordIdString);
                    var payload = new DescribedSerialization(
                        metadata.TypeRepresentationOfObject.WithVersion,
                        stringPayload,
                        metadata.SerializerRepresentation,
                        serializationFormat);

                    var result = new StreamRecord(internalRecordId, metadata, payload);
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
    }
}
