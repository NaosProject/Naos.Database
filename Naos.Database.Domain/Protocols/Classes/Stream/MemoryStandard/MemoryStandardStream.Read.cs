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
        public override IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            throw new NotImplementedException();

            /*
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                IReadOnlyCollection<long> ProcessDefaultReturn()
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return new long[0];
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
                                 _ => _.Metadata.FuzzyMatchTypes(
                                     operation.RecordFilter.IdTypes,
                                     operation.RecordFilter.ObjectTypes,
                                     operation.RecordFilter.VersionMatchStrategy))
                            .Select(_ => _.InternalRecordId)
                            .Union(
                                 operation.RecordFilter.Ids.Any()
                                 partition
                                    .Where(
                                         _ => _.Metadata.FuzzyMatchTypesAndId(
                                             operation.StringSerializedId,
                                             operation.IdentifierType,
                                             operation.ObjectType,
                                             operation.VersionMatchStrategy))
                                    .Select(_ => _.InternalRecordId))
                            .ToList();

                if (result.Any())
                {
                    return result;
                }
                else
                {
                    return ProcessDefaultReturn();
                }
            }
            */
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new HashSet<StringSerializedIdentifier>();
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
                                    operation.RecordFilter.IdTypes,
                                    operation.RecordFilter.ObjectTypes,
                                    operation.RecordFilter.VersionMatchStrategy)
                             && ((!operation.RecordFilter.Tags?.Any() ?? true)
                              || streamRecord.Metadata.Tags.FuzzyMatchTags(operation.RecordFilter.Tags, operation.RecordFilter.TagMatchStrategy)))
                            {
                                result.Add(
                                    new StringSerializedIdentifier(
                                        streamRecord.Metadata.StringSerializedId,
                                        streamRecord.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(
                                            operation.RecordFilter.VersionMatchStrategy)));
                            }
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
            throw new NotImplementedException();

            /*
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);

                var result = partition?
                            .OrderByDescending(_ => _.InternalRecordId)
                            .FirstOrDefault(
                                 _ => _.Metadata.FuzzyMatchTypes(
                                     operation.IdentifierType == null
                                         ? null
                                         : new[]
                                           {
                                               operation.IdentifierType,
                                           },
                                     operation.ObjectType == null
                                         ? null
                                         : new[]
                                           {
                                               operation.ObjectType,
                                           },
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
            */
        }

        /// <inheritdoc />
        public override string Execute(
            StandardGetLatestStringSerializedObjectOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOp = new StandardGetLatestRecordOp(
                operation.RecordFilter,
                operation.RecordNotFoundStrategy,
                StreamRecordItemsToInclude.MetadataAndPayload,
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
    }
}
