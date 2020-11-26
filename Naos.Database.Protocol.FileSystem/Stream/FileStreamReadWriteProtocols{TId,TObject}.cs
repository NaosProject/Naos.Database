// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IStreamReadProtocols{TId,TObject}"/>
    /// and <see cref="IStreamWriteProtocols{TId,TObject}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class FileStreamReadWriteProtocols<TId, TObject>
        : IStreamReadProtocols<TId, TObject>,
          IStreamWriteProtocols<TId, TObject>
    {
        private const string RecordIdentifierTrackingFileName = "_InternalRecordIdentifierTracking.nfo";
        private const string BinaryFileExtension = "bin";
        private const string MetadataFileExtension = "meta";

        private readonly object nextInternalRecordIdentifierLock = new object();
        private readonly ObcDateTimeStringSerializer dateTimeStringSerializer = new ObcDateTimeStringSerializer();
        private readonly FileReadWriteStream stream;
        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> resourceLocatorUsingIdProtocols;
        private readonly ISerializer serializer;
        private readonly SerializerRepresentation serializerRepresentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamReadWriteProtocols{TId,TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamReadWriteProtocols(
            FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;

            this.resourceLocatorUsingIdProtocols = this.stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
            this.serializerRepresentation = this.stream.DefaultSerializerRepresentation;
            this.serializer = this.stream.SerializerFactory.BuildSerializer(this.serializerRepresentation);
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public TObject Execute(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var stringSerializedId = this.ConvertIdToString(operation.Id);
            var resourceLocator = this.resourceLocatorUsingIdProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));
            var rootPath = this.GetRootPathFromLocator(resourceLocator);

            var metadataPathsThatCouldMatch = Directory.GetFiles(
                rootPath,
                Invariant($"*{stringSerializedId}*.{MetadataFileExtension}"),
                SearchOption.TopDirectoryOnly);

            var orderedDescendingByInternalRecordId = metadataPathsThatCouldMatch.OrderByDescending(Path.GetFileName).ToList();
            if (!orderedDescendingByInternalRecordId.Any())
            {
                return default;
            }

            var resultTypeRepresentation = typeof(TObject).ToRepresentation().ToWithAndWithoutVersion();
            var typeRepresentationToCheck = operation.TypeVersionMatchStrategy == TypeVersionMatchStrategy.Specific
                ? resultTypeRepresentation.WithVersion
                : resultTypeRepresentation.WithoutVersion;
            foreach (var metadataFilePathToTest in orderedDescendingByInternalRecordId)
            {
                var fileText = File.ReadAllText(metadataFilePathToTest);
                var metadata = this.serializer.Deserialize<StreamRecordMetadata>(fileText);
                if (metadata.TypeRepresentationOfObject.WithoutVersion == typeRepresentationToCheck)
                {
                    var recordSerializer = this.stream.SerializerFactory.BuildSerializer(metadata.SerializerRepresentation);
                    var filePathBase = metadataFilePathToTest.Substring(0, metadataFilePathToTest.Length - MetadataFileExtension.Length - 1); // remove the '.' as well.
                    var binaryFilePath = Invariant($"{filePathBase}.{BinaryFileExtension}");
                    TObject result;
                    if (File.Exists(binaryFilePath))
                    {
                        var bytes = File.ReadAllBytes(binaryFilePath);
                        result = recordSerializer.Deserialize<TObject>(bytes);
                    }
                    else
                    {
                        var stringFilePath = Invariant($"{filePathBase}.{metadata.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture)}");
                        if (!File.Exists(stringFilePath))
                        {
                            throw new InvalidOperationException(Invariant($"Expected payload file '{stringFilePath}' to exist to accompany metadata file '{metadataFilePathToTest}' but was not found."));
                        }

                        var text = File.ReadAllText(stringFilePath);
                        result = recordSerializer.Deserialize<TObject>(text);
                    }

                    return result;
                }
            }

            return default;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "fileStream is only disposed once.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public long Execute(
            PutAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var resourceLocator = this.resourceLocatorUsingIdProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));
            var rootPath = this.GetRootPathFromLocator(resourceLocator);

            var identifierTypeRepresentation = (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation();
            var objectTypeRepresentation = (operation.ObjectToPut?.GetType() ?? typeof(TObject)).ToRepresentation();

            var stringSerializedId = this.ConvertIdToString(operation.Id);
            var streamRecordMetadata = new StreamRecordMetadata(
                stringSerializedId,
                this.serializerRepresentation,
                identifierTypeRepresentation.ToWithAndWithoutVersion(),
                objectTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                DateTime.UtcNow);

            var stringSerializedMetadata = this.serializer.SerializeToString(streamRecordMetadata);
            byte[] serializedBytes = null;
            string serializedString = null;
            string fileExtension;
            switch (this.stream.DefaultSerializationFormat)
            {
                case SerializationFormat.String:
                    serializedString = this.serializer.SerializeToString(operation.ObjectToPut);
                    fileExtension = this.serializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture);
                    break;
                case SerializationFormat.Binary:
                    serializedBytes = this.serializer.SerializeToBytes(operation.ObjectToPut);
                    fileExtension = BinaryFileExtension;
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} from {nameof(this.stream)} of '{this.stream.DefaultSerializationFormat}' is not supported."));
            }

            var timestampString = this.dateTimeStringSerializer.SerializeToString(streamRecordMetadata.TimestampUtc).Replace(":", "-");
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

            var filePathIdentifier = stringSerializedId.EncodeForFilePath();
            var fileBaseName = Invariant($"{newId}___{timestampString}___{filePathIdentifier}");
            var metadataFileName = Invariant($"{fileBaseName}.{MetadataFileExtension}");
            var payloadFileName = Invariant($"{fileBaseName}.{fileExtension}");
            var metadataFilePath = Path.Combine(rootPath, metadataFileName);
            var payloadFilePath = Path.Combine(rootPath, payloadFileName);

            File.WriteAllText(metadataFilePath, stringSerializedMetadata);
            if (fileExtension == BinaryFileExtension)
            {
                serializedBytes.MustForOp(nameof(serializedBytes)).NotBeNull(payloadFilePath);

                // ReSharper disable once AssignNullToNotNullAttribute -- checked above with must
                File.WriteAllBytes(payloadFilePath, serializedBytes);
            }
            else
            {
                File.WriteAllText(payloadFilePath, serializedString);
            }

            return newId;
        }

        private string ConvertIdToString(
            TId identifier)
        {
            var identifierType = identifier?.GetType() ?? typeof(TId);
            if (identifierType == typeof(string)
             || identifierType == typeof(long)
             || identifierType == typeof(int)
             || identifierType == typeof(short))
            {
                return identifier?.ToString();
            }
            else
            {
                var result = this.serializer.SerializeToString(identifier);
                return result;
            }
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TId, TObject> operation)
        {
            var delegatedOperation = new PutAndReturnInternalRecordIdOp<TId, TObject>(operation.Id, operation.ObjectToPut, operation.Tags);
            this.Execute(delegatedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TId, TObject> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true);
        }

        private string GetRootPathFromLocator(
            IResourceLocator resourceLocator)
        {
            var fileLocator = resourceLocator as FileSystemDatabaseLocator ?? throw new InvalidOperationException(Invariant($"Resource locator was expected to be a {nameof(FileSystemDatabaseLocator)} but was a '{resourceLocator?.GetType()?.ToStringReadable() ?? "<null>"}'."));
            var result = Path.Combine(fileLocator.RootFolderPath, this.stream.Name);
            return result;
        }
    }
}
