// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IReadWriteStream"/>, it is thread resilient but not necessarily thread safe.
    /// Implements the <see cref="StandardStreamBase" />.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class FileStandardStream : StandardStreamBase
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

        private StreamRecord GetStreamRecordFromMetadataFile(
            string metadataFilePath,
            StreamRecordMetadata metadata = null)
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

        private static long GetInternalRecordIdFromEntryFilePath(
            string filePath)
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
    }
}