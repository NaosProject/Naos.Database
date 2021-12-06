// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.Read.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using static System.FormattableString;

    public partial class MemoryStandardStream
    {
        /// <inheritdoc />
        public override bool Execute(
            StandardDoesAnyExistByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result = partition?
                    .OrderByDescending(_ => _.InternalRecordId)
                    .FirstOrDefault(
                        _ => _.Metadata.FuzzyMatchTypesAndId(
                            operation.StringSerializedId,
                            operation.IdentifierType,
                            operation.ObjectType,
                            operation.VersionMatchStrategy));

                return result != null;
            }
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecord> Execute(
            StandardGetAllRecordsByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                IReadOnlyList<StreamRecord> ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return new StreamRecord[0];
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(Invariant($"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }

                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                if (partition == null)
                {
                    return ProcessDefaultReturn();
                }

                var result = partition
                    .Where(
                        _ => _.Metadata.FuzzyMatchTypesAndId(
                            operation.StringSerializedId,
                            operation.IdentifierType,
                            operation.ObjectType,
                            operation.VersionMatchStrategy))
                    .ToList();

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

                return ProcessDefaultReturn();
            }
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordMetadata> Execute(
            StandardGetAllRecordsMetadataByIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                IReadOnlyList<StreamRecordMetadata> ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return new StreamRecordMetadata[0];
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(Invariant($"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }

                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                if (partition == null)
                {
                    return ProcessDefaultReturn();
                }

                var result = partition
                    .Where(
                        _ => _.Metadata.FuzzyMatchTypesAndId(
                            operation.StringSerializedId,
                            operation.IdentifierType,
                            operation.ObjectType,
                            operation.VersionMatchStrategy))
                    .ToList();

                if (result.Any())
                {
                    switch (operation.OrderRecordsBy)
                    {
                        case OrderRecordsBy.InternalRecordIdAscending:
                            return result.OrderBy(_ => _.InternalRecordId).Select(_ => _.Metadata).ToList();
                        case OrderRecordsBy.InternalRecordIdDescending:
                            return result.OrderByDescending(_ => _.InternalRecordId).Select(_ => _.Metadata).ToList();
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(OrderRecordsBy)} {operation.OrderRecordsBy} is not supported."));
                    }
                }

                return ProcessDefaultReturn();
            }
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<string> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new HashSet<string>();
            lock (this.streamLock)
            {
                var locators = new List<MemoryDatabaseLocator>();
                if (operation.SpecifiedResourceLocator != null)
                {
                    locators.Add(operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>());
                }
                else
                {
                    var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());

                    foreach (var locator in allLocators)
                    {
                        locators.Add(locator.ConfirmAndConvert<MemoryDatabaseLocator>());
                    }
                }

                foreach (var memoryDatabaseLocator in locators)
                {
                    this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                    if (partition != null)
                    {
                        foreach (var streamRecord in partition)
                        {
                            if (streamRecord.Metadata.FuzzyMatchTypes(
                                    operation.IdentifierType,
                                    operation.ObjectType,
                                    operation.VersionMatchStrategy)
                             && ((!operation.TagsToMatch?.Any() ?? true)
                              || streamRecord.Metadata.Tags.FuzzyMatchTags(operation.TagsToMatch, operation.TagMatchStrategy)))
                            {
                                result.Add(streamRecord.Metadata.StringSerializedId);
                            }
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

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result = partition?
                    .OrderByDescending(_ => _.InternalRecordId)
                    .FirstOrDefault(
                        _ => _.Metadata.FuzzyMatchTypesAndId(
                            operation.StringSerializedId,
                            operation.IdentifierType,
                            operation.ObjectType,
                            operation.VersionMatchStrategy));

                if (result != null)
                {
                    return result;
                }

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

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result = partition?
                    .OrderByDescending(_ => _.InternalRecordId)
                    .FirstOrDefault(
                        _ => _.Metadata.FuzzyMatchTypesAndId(
                            operation.StringSerializedId,
                            operation.IdentifierType,
                            operation.ObjectType,
                            operation.VersionMatchStrategy))?.Metadata;

                if (result != null)
                {
                    return result;
                }

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
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result = partition?
                    .OrderByDescending(_ => _.InternalRecordId)
                    .FirstOrDefault(
                        _ => _.Metadata.FuzzyMatchTypes(
                            operation.IdentifierType,
                            operation.ObjectType,
                            operation.VersionMatchStrategy));

                if (result != null)
                {
                    return result;
                }

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

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result = partition?.FirstOrDefault(_ => _.InternalRecordId == operation.InternalRecordId);

                if (result != null)
                {
                    return result;
                }

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
        }
    }
}
