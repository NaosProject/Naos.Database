// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStandardStream.Write.cs" company="Naos Project">
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
    using OBeautifulCode.String.Recipes;
    using static System.FormattableString;

    public partial class FileStandardStream
    {
        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
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
                        ? currentList.Max(_ => _.UniqueLong) + 1
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
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public override PutRecordResult Execute(
            StandardPutRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

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

                                               var stringPayload = ((StringStreamRecordPayload)operation.Payload).SerializedPayload;
                                               var fileStringPayload = File.ReadAllText(_.StringDataPath);
                                               return fileStringPayload.Equals(stringPayload ?? NullToken);
                                           case SerializationFormat.Binary:
                                               if (binaryFileExists)
                                               {
                                                   throw new NotSupportedException(Invariant($"Found a file for the id and type in binary when a Binary payload is being put, this is not supported: '{_.BinaryDataPath}'."));
                                               }

                                               var binaryPayload = ((BinaryStreamRecordPayload)operation.Payload).SerializedPayload;
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

                                               var stringPayload = ((StringStreamRecordPayload)operation.Payload).SerializedPayload;
                                               var fileStringPayload = File.ReadAllText(_.StringDataPath);
                                               return fileStringPayload.Equals(stringPayload ?? NullToken);
                                           case SerializationFormat.Binary:
                                               if (binaryFileExists)
                                               {
                                                   throw new NotSupportedException(Invariant($"Found a file for the id and type in binary when a Binary payload is being put, this is not supported: '{_.BinaryDataPath}'."));
                                               }

                                               var binaryPayload = ((BinaryStreamRecordPayload)operation.Payload).SerializedPayload;
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

                var fileExtension = operation.Payload.GetSerializationFormat() == SerializationFormat.Binary
                    ? BinaryFileExtension
                    : operation.Metadata.SerializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter(CultureInfo.InvariantCulture);
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
                    var serializedBytes = ((BinaryStreamRecordPayload)operation.Payload).SerializedPayload;

                    File.WriteAllBytes(payloadFilePath, serializedBytes.ToArray());
                }
                else
                {
                    var serializedString = ((StringStreamRecordPayload)operation.Payload).SerializedPayload;

                    File.WriteAllText(payloadFilePath, serializedString ?? NullToken);
                }

                recordFilePathsToPrune.ForEach(File.Delete);
                var prunedRecordIds = recordFilePathsToPrune.Select(GetInternalRecordIdFromRecordFilePath).Distinct().ToList();

                var result = new PutRecordResult(newId, existingRecordIds, prunedRecordIds);
                return result;
            }
        }
    }
}