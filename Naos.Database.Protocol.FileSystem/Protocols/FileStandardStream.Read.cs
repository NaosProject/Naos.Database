﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStandardStream.Read.cs" company="Naos Project">
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
    using OBeautifulCode.Serialization;
    using static System.FormattableString;

    public partial class FileStandardStream
    {
        /// <inheritdoc />
        public override IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            throw new NotImplementedException();

            /*
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
            */
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new HashSet<StringSerializedIdentifier>();
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
                                operation.RecordFilter.IdTypes,
                                operation.RecordFilter.ObjectTypes,
                                operation.RecordFilter.VersionMatchStrategy)
                         && ((!operation.RecordFilter.Tags?.Any() ?? true)
                          || metadata.Tags.FuzzyMatchTags(operation.RecordFilter.Tags, operation.RecordFilter.TagMatchStrategy)))
                        {
                            result.Add(
                                new StringSerializedIdentifier(
                                    metadata.StringSerializedId,
                                    metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.RecordFilter.VersionMatchStrategy)));
                        }
                    }
                }
            }

            return result;
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
            StandardGetLatestStringSerializedObjectOp operation)
        {
            throw new NotImplementedException();

            /*
            operation.MustForArg(nameof(operation)).NotBeNull();

            var stringSerializedIdentifier = operation.RecordFilter.Ids.Single();
            var delegatedOp = new StandardGetLatestRecordByIdOp(
                stringSerializedIdentifier.StringSerializedId,
                stringSerializedIdentifier.IdentifierType,
                operation.RecordFilter.ObjectTypes.Single(),
                operation.RecordFilter.VersionMatchStrategy,
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
            */
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for potential use.")]
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