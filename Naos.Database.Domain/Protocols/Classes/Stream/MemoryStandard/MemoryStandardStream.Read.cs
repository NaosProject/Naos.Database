// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.Read.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    public partial class MemoryStandardStream
    {
        /// <inheritdoc />
        public override IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                var matchingRecords = this.GetMatchingRecords(operation);

                if (!matchingRecords.Any())
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            return Array.Empty<long>();
                        case RecordNotFoundStrategy.Throw:
                            throw new InvalidOperationException(Invariant($"No records were found."));
                        default:
                            throw new NotSupportedException(Invariant($"This {nameof(RecordNotFoundStrategy)} is not supported: {operation.RecordNotFoundStrategy}."));
                    }
                }

                var result = matchingRecords.Select(_ => _.InternalRecordId).ToList();

                return result;
            }
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new HashSet<StringSerializedIdentifier>();

            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                foreach (var locator in allLocators)
                {
                    var operationWithLocator = operation.DeepCloneWithSpecifiedResourceLocator(locator);
                    var matchingRecords = this.GetMatchingRecords(operationWithLocator);

                    var stringSerializedIds = matchingRecords
                        .Select(_ => new StringSerializedIdentifier(_.Metadata.StringSerializedId, _.Metadata.TypeRepresentationOfId.WithVersion))
                        .ToList();

                    result.AddRange(stringSerializedIds);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                var matchingRecords = this.GetMatchingRecords(operation);

                if ((matchingRecords != null) && matchingRecords.Any())
                {
                    var result = matchingRecords.OrderByDescending(_ => _.InternalRecordId).First();
                    switch (operation.StreamRecordItemsToInclude)
                    {
                        case StreamRecordItemsToInclude.MetadataAndPayload:
                            return result;
                        case StreamRecordItemsToInclude.MetadataOnly:
                            var resultWithoutPayload = result.DeepCloneWithPayload(
                                new NullDescribedSerialization(
                                    result.Payload.PayloadTypeRepresentation,
                                    result.Payload.SerializerRepresentation));
                            return resultWithoutPayload;
                        default:
                            throw new NotSupportedException(Invariant($"Unsupported {nameof(StreamRecordItemsToInclude)}: {operation.StreamRecordItemsToInclude}."));
                    }
                }

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
                // delegatedOp will honor the RecordNotFoundStrategy, so no need to check here.
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
                    throw new InvalidOperationException(Invariant($"record {nameof(SerializationFormat)} not {SerializationFormat.String}, it is {record.Payload.GetSerializationFormat()}."));
                }
            }

            return result;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private IReadOnlyList<StreamRecord> GetMatchingRecords<TOperation>(
            TOperation operation)
            where TOperation : ISpecifyResourceLocator, ISpecifyRecordFilter
        {
            lock (this.streamLock)
            {
                var recordFilter = operation.RecordFilter;
                var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);
                if (partition == null)
                {
                    return new List<StreamRecord>();
                }

                // ReSharper disable once RedundantArgumentDefaultValue
                var recordsToFilterCriteria = operation is ISpecifyRecordsToFilterCriteria recordsToFilterCriteriaSpecified
                    ? recordsToFilterCriteriaSpecified.RecordsToFilterCriteria
                    : new RecordsToFilterCriteria(RecordsToFilterSelectionStrategy.All);

                var result = ApplyRecordFilterToPartition(recordFilter, partition, recordsToFilterCriteria);

                return result;
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private static List<StreamRecord> ApplyRecordFilterToPartition(
            RecordFilter recordFilter,
            List<StreamRecord> partition,
            RecordsToFilterCriteria recordsToFilterCriteria)
        {
            List<StreamRecord> result;
            bool filterApplied;

            var versionMatchStrategy = recordsToFilterCriteria.VersionMatchStrategy;
            new { versionMatchStrategy }.AsOp().Must().BeElementIn(new[] { VersionMatchStrategy.Any, VersionMatchStrategy.SpecifiedVersion });

            if (recordsToFilterCriteria.RecordsToFilterSelectionStrategy == RecordsToFilterSelectionStrategy.All)
            {
                result = new List<StreamRecord>();

                filterApplied = false;
            }
            else if (recordsToFilterCriteria.RecordsToFilterSelectionStrategy == RecordsToFilterSelectionStrategy.LatestById)
            {
                result = partition
                    .GroupBy(_ => _.Metadata.StringSerializedId)
                    .Select(_ => _.OrderByDescending(streamRecord => streamRecord.InternalRecordId).First())
                    .ToList();

                filterApplied = true;
            }
            else if (recordsToFilterCriteria.RecordsToFilterSelectionStrategy == RecordsToFilterSelectionStrategy.LatestByIdAndObjectType)
            {
                result = partition
                    .GroupBy(_ =>
                    {
                        var objectType = versionMatchStrategy == VersionMatchStrategy.Any
                            ? _.Metadata.TypeRepresentationOfObject.WithoutVersion
                            : _.Metadata.TypeRepresentationOfObject.WithVersion;

                        return new { _.Metadata.StringSerializedId, objectType };
                    })
                    .Select(_ => _.OrderByDescending(streamRecord => streamRecord.InternalRecordId).First())
                    .ToList();

                filterApplied = true;
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(RecordsToFilterSelectionStrategy)} is not supported: {recordsToFilterCriteria.RecordsToFilterSelectionStrategy}."));
            }

            // Internal Record Identifier
            if ((recordFilter.InternalRecordIds != null) && recordFilter.InternalRecordIds.Any())
            {
                if (filterApplied)
                {
                    result.RemoveAll(_ => !recordFilter.InternalRecordIds.Contains(_.InternalRecordId));
                }
                else
                {
                    result.AddRange(partition.Where(_ => recordFilter.InternalRecordIds.Contains(_.InternalRecordId)));
                    filterApplied = true;
                }
            }

            // String Serialized Identifier
            if ((recordFilter.Ids != null) && recordFilter.Ids.Any())
            {
                // Why are we not just filtering on the Ids themselves?  Why does the type and version matching strategy matter?
                // Technically Ids can be objects and objects can serialize differently depending on their version or properties
                // can have different meaning dependent on the version.  If you are running using VersionMatchStrategy.SpecifiedVersion
                // and not confirming that the version is consistent with the object used as the identifier, you could have an invalid match.
                var recordsMatchingById = recordFilter.Ids.SelectMany(
                        i => partition.Where(
                            _ => _.Metadata.FuzzyMatchTypesAndId(
                                i.StringSerializedId,
                                i.IdentifierType,
                                null,
                                recordFilter.VersionMatchStrategy)))
                    .ToList();

                if (filterApplied)
                {
                    // ReSharper disable once SimplifyLinqExpressionUseAll - prefer the !Any for readability
                    result.RemoveAll(_ => !recordsMatchingById.Any(__ => _.InternalRecordId == __.InternalRecordId));
                }
                else
                {
                    result.AddRange(recordsMatchingById);
                    filterApplied = true;
                }
            }

            // Identifier and Object Type
            if (((recordFilter.IdTypes != null) && recordFilter.IdTypes.Any()) ||
                (recordFilter.ObjectTypes != null && recordFilter.ObjectTypes.Any()))
            {
                var recordsMatchingByType = partition.Where(
                        _ => _.Metadata.FuzzyMatchTypes(
                            recordFilter.IdTypes,
                            recordFilter.ObjectTypes,
                            recordFilter.VersionMatchStrategy))
                    .ToList();

                if (filterApplied)
                {
                    // ReSharper disable once SimplifyLinqExpressionUseAll - prefer the !Any for readability
                    result.RemoveAll(_ => !recordsMatchingByType.Any(__ => __.InternalRecordId == _.InternalRecordId));
                }
                else
                {
                    result.AddRange(recordsMatchingByType);
                    filterApplied = true;
                }
            }

            // Tag
            if ((recordFilter.Tags != null) && recordFilter.Tags.Any())
            {
                var recordsMatchingByTag = partition
                    .Where(_ => _.Metadata.Tags.FuzzyMatchTags(recordFilter.Tags, recordFilter.TagMatchStrategy))
                    .ToList();
                if (filterApplied)
                {
                    // ReSharper disable once SimplifyLinqExpressionUseAll - prefer the !Any for readability
                    result.RemoveAll(_ => !recordsMatchingByTag.Any(__ => _.InternalRecordId == __.InternalRecordId));
                }
                else
                {
                    result.AddRange(recordsMatchingByTag);
                    filterApplied = true;
                }
            }

            if (!filterApplied)
            {
                result.AddRange(partition);
            }

            if ((recordFilter.DeprecatedIdTypes != null) && recordFilter.DeprecatedIdTypes.Any())
            {
                var internalRecordIdsToRemove = new List<long>();
                foreach (var streamRecord in result)
                {
                    // Is the stream record's object type in the set of DeprecatedIdTypes?
                    // If so, then it deprecates itself and should be removed.
                    if (recordFilter.DeprecatedIdTypes.Any(_ =>
                            streamRecord.Metadata.TypeRepresentationOfObject.WithVersion.EqualsAccordingToStrategy(
                                _,
                                recordFilter.VersionMatchStrategy)))
                    {
                        internalRecordIdsToRemove.Add(streamRecord.InternalRecordId);

                        continue;
                    }

                    foreach (var depreciatedIdType in recordFilter.DeprecatedIdTypes)
                    {
                        // Get all records having the same id as streamRecord but with a greater internal record id.
                        var moreRecentRecordsMatchingId = partition
                            .OrderByDescending(_ => _.InternalRecordId)
                            .Where(
                                _ => _.Metadata.FuzzyMatchTypesAndId(
                                    streamRecord.Metadata.StringSerializedId,
                                    streamRecord.Metadata.TypeRepresentationOfId.WithVersion,
                                    null,
                                    recordFilter.VersionMatchStrategy))
                            .Where(_ => _.InternalRecordId > streamRecord.InternalRecordId)
                            .ToList();

                        // If any of those records are of the same type as depreciatedIdType, then the record is deprecated
                        if (moreRecentRecordsMatchingId.Any(
                                _ => _.Metadata.TypeRepresentationOfObject.WithVersion.EqualsAccordingToStrategy(
                                        depreciatedIdType,
                                        recordFilter.VersionMatchStrategy)))
                        {
                            internalRecordIdsToRemove.Add(streamRecord.InternalRecordId);
                            break;
                        }
                    }
                }

                result.RemoveAll(_ => internalRecordIdsToRemove.Contains(_.InternalRecordId));
            }

            return result;
        }
    }
}
