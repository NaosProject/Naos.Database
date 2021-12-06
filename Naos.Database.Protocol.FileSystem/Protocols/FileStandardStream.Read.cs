// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStandardStream.Read.cs" company="Naos Project">
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
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using static System.FormattableString;

    public partial class FileStandardStream
    {
        /// <inheritdoc />
        public override bool Execute(
            StandardDoesAnyExistByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

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
        public override IReadOnlyList<StreamRecord> Execute(
            StandardGetAllRecordsByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

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
            operation.MustForArg(nameof(operation)).NotBeNull();

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
            operation.MustForArg(nameof(operation)).NotBeNull();

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
                          || metadata.Tags.FuzzyMatchTags(operation.TagsToMatch, operation.TagMatchStrategy)))
                        {
                            result.Add(metadata.StringSerializedId);
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            lock (this.fileLock)
            {
                StreamRecord ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return null;
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(Invariant($"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
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
            operation.MustForArg(nameof(operation)).NotBeNull();

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override StreamRecordMetadata Execute(
            StandardGetLatestRecordMetadataByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

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
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

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
        public override string Execute(
            StandardGetLatestStringSerializedObjectByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOp = new StandardGetLatestRecordByIdOp(
                operation.StringSerializedId,
                operation.IdentifierType,
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                operation.SpecifiedResourceLocator);

            var record = this.Execute(delegatedOp);

            string result;

            if (record == null)
            {
                result = null;
            }
            else
            {
                if (record.Payload is StringDescribedSerialization stringDescribedSerialization)
                {
                    result = stringDescribedSerialization.SerializedPayload;
                }
                else
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            result = null;
                            break;
                        case RecordNotFoundStrategy.Throw:
                            throw new NotSupportedException(Invariant($"record {nameof(SerializationFormat)} not {SerializationFormat.String}, it is {record.Payload.GetSerializationFormat()}, but {nameof(RecordNotFoundStrategy)} is not {nameof(RecordNotFoundStrategy.ReturnDefault)}"));
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetRecordByInternalRecordIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

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
    }
}