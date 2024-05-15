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
    using OBeautifulCode.Serialization;
    using static System.FormattableString;

    public partial class MemoryStandardStream
    {
        /// <inheritdoc />
        public override IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            var matchingRecords = this.GetMatchingRecords(operation);
            var result = matchingRecords.Select(_ => _.InternalRecordId).ToList();
            return result;
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new HashSet<StringSerializedIdentifier>();
            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
            foreach (var locator in allLocators)
            {
                var operationWithLocator = operation.DeepCloneWithSpecifiedResourceLocator(locator);
                var matchingRecords = this.GetMatchingRecords(operationWithLocator);
                matchingRecords.ToList()
                               .ForEach(
                                    _ => result.Add(
                                        new StringSerializedIdentifier(_.Metadata.StringSerializedId, _.Metadata.TypeRepresentationOfId.WithVersion)));
            }

            return result;
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var matchingRecords = this.GetMatchingRecords(operation);

            if (matchingRecords != null && matchingRecords.Any())
            {
                var result = matchingRecords.OrderBy(_ => _.InternalRecordId).Last();
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

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private IReadOnlyList<StreamRecord> GetMatchingRecords<TOperation>(
            TOperation operation)
            where TOperation : ISpecifyResourceLocator, ISpecifyRecordFilter
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            lock (this.streamLock)
            {
                var recordFilter = operation.RecordFilter;
                var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);
                if (partition == null)
                {
                    return new List<StreamRecord>();
                }

                var result = ApplyRecordFilterToPartition(recordFilter, partition);

                return result;
            }
        }

        private static List<StreamRecord> ApplyRecordFilterToPartition(
            RecordFilter recordFilter,
            List<StreamRecord> partition)
        {
            // Short-circuit empty record filter to return all records.
            if (recordFilter.IsEmptyRecordFilter())
            {
                return partition;
            }

            var result = new List<StreamRecord>();
            var resultInitialized = false;

            // Internal Record Identifier
            if ((recordFilter.InternalRecordIds != null) && recordFilter.InternalRecordIds.Any())
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (resultInitialized)
                {
                    result.RemoveAll(_ => !recordFilter.InternalRecordIds.Contains(_.InternalRecordId));
                }
                else
                {
                    result.AddRange(partition.Where(_ => recordFilter.InternalRecordIds.Contains(_.InternalRecordId)));
                    resultInitialized = true;
                }
            }

            // String Serialized Identifier
            if (recordFilter.Ids != null && recordFilter.Ids.Any())
            {
                var recordsMatchingById = recordFilter.Ids.SelectMany(
                        i => partition.Where(
                            _ => _.Metadata.FuzzyMatchTypesAndId(
                                i.StringSerializedId,
                                i.IdentifierType,
                                null,
                                recordFilter.VersionMatchStrategy)))
                    .ToList();

                if (resultInitialized)
                {
                    result.RemoveAll(_ => recordsMatchingById.Any(__ => _.InternalRecordId != __.InternalRecordId));
                }
                else
                {
                    result.AddRange(recordsMatchingById);
                    resultInitialized = true;
                }
            }

            // Identifier and Object Type
            if ((recordFilter.IdTypes != null && recordFilter.IdTypes.Any()) ||
                (recordFilter.ObjectTypes != null && recordFilter.ObjectTypes.Any()))
            {
                var recordsMatchingByType = partition.Where(
                        _ => _.Metadata.FuzzyMatchTypes(
                            recordFilter.IdTypes,
                            recordFilter.ObjectTypes,
                            recordFilter.VersionMatchStrategy))
                    .ToList();

                if (resultInitialized)
                {
                    result.RemoveAll(_ => !recordsMatchingByType.Any(__ => __.InternalRecordId == _.InternalRecordId));
                }
                else
                {
                    result.AddRange(recordsMatchingByType);
                    resultInitialized = true;
                }
            }

            // Tag
            if (recordFilter.Tags != null && recordFilter.Tags.Any())
            {
                var recordsMatchingByTag = partition
                    .Where(_ => _.Metadata.Tags.FuzzyMatchTags(recordFilter.Tags, recordFilter.TagMatchStrategy))
                    .ToList();
                if (resultInitialized)
                {
                    result.RemoveAll(_ => recordsMatchingByTag.Any(__ => _.InternalRecordId != __.InternalRecordId));
                }
                else
                {
                    result.AddRange(recordsMatchingByTag);
                    resultInitialized = true;
                }
            }

            if (!resultInitialized)
            {
                result.AddRange(partition);
            }

            if ((recordFilter.DeprecatedIdTypes != null) && recordFilter.DeprecatedIdTypes.Any())
            {
                var internalRecordIdsToRemove = new List<long>();
                foreach (var streamRecord in result)
                {
                    // todo: What if the id is un-deprecated?
                    if (
                        recordFilter.DeprecatedIdTypes.Any(
                            d =>
                                partition
                                    .OrderBy(_ => _.InternalRecordId)
                                    .Last(
                                        _ => _.Metadata.FuzzyMatchTypesAndId(
                                            streamRecord.Metadata.StringSerializedId,
                                            streamRecord.Metadata.TypeRepresentationOfId.WithVersion,
                                            null,
                                            recordFilter.VersionMatchStrategy))
                                    .Metadata.TypeRepresentationOfId.WithVersion.EqualsAccordingToStrategy(
                                        d,
                                        recordFilter.VersionMatchStrategy)))
                    {
                        internalRecordIdsToRemove.Add(streamRecord.InternalRecordId);
                    }
                }

                result.RemoveAll(_ => internalRecordIdsToRemove.Contains(_.InternalRecordId));
            }

            return result;
        }
    }
}
