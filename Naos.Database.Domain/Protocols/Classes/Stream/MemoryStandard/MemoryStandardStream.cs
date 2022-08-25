// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.cs" company="Naos Project">
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
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// In-memory implementation of a <see cref="StandardStreamBase"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public partial class MemoryStandardStream : StandardStreamBase, IHaveStringId
    {
        private readonly object streamLock = new object();
        private readonly object handlingLock = new object();
        private readonly object singleLocatorLock = new object();

        private readonly Dictionary<MemoryDatabaseLocator, List<StreamRecord>> locatorToRecordPartitionMap = new Dictionary<MemoryDatabaseLocator, List<StreamRecord>>();
        private readonly Dictionary<MemoryDatabaseLocator, Dictionary<string, List<StreamRecordHandlingEntry>>> locatorToHandlingEntriesByConcernMap = new Dictionary<MemoryDatabaseLocator, Dictionary<string, List<StreamRecordHandlingEntry>>>();
        private bool created = false;
        private long uniqueLongForExternalProtocol = 0;
        private long uniqueLongForInMemoryRecords = 0;
        private long uniqueLongForInMemoryHandlingEntries = 0;
        private MemoryDatabaseLocator singleLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStandardStream"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        /// <param name="resourceLocatorProtocols">OPTIONAL resource locator protocols.  DEFAULT is to use a single <see cref="MemoryDatabaseLocator"/> named 'Default'.</param>
        public MemoryStandardStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            IResourceLocatorProtocols resourceLocatorProtocols = null)
            : base(name, serializerFactory, defaultSerializerRepresentation, defaultSerializationFormat, resourceLocatorProtocols ?? new SingleResourceLocatorProtocols(new MemoryDatabaseLocator("Default")))
        {
            this.Id = Guid.NewGuid().ToString().ToUpperInvariant();
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation => new MemoryStreamRepresentation(this.Name, this.Id);

        /// <inheritdoc />
        public string Id { get; private set; }

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
                var result = new List<StreamRecord>();
                var resultInitialized = false;
                this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var partition);
                if (partition == null)
                {
                    return result;
                }

                // Internal Record Identifier
                if (recordFilter.InternalRecordIds != null && recordFilter.InternalRecordIds.Any())
                {
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
                if ((recordFilter.IdTypes != null && recordFilter.IdTypes.Any()) || (recordFilter.ObjectTypes != null && recordFilter.ObjectTypes.Any()))
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

                if (recordFilter.DeprecatedIdTypes != null && recordFilter.DeprecatedIdTypes.Any())
                {
                    var internalRecordIdsToRemove = new List<long>();
                    foreach (var streamRecord in result)
                    {
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

        private MemoryDatabaseLocator TryGetSingleLocator()
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

                    var result = allLocators.Single().ConfirmAndConvert<MemoryDatabaseLocator>();

                    this.singleLocator = result;

                    return this.singleLocator;
                }
            }
        }

        private void WriteHandlingEntryToMemoryMap(
            IResourceLocator locator,
            string concern,
            StreamRecordHandlingEntry requestedEntry)
        {
            lock (this.handlingLock)
            {
                // Do not need this call but it has the confirm key path exists logic and I do not want to refactor yet another method for them to share...
                this.GetStreamRecordHandlingEntriesForConcern(locator, concern);

                // Above will throw if this cast is not possible.
                var memoryLocator = (MemoryDatabaseLocator)locator;

                // The reference would get broken in non-obvious ways when using variables so direct keying the map.
                this.locatorToHandlingEntriesByConcernMap[memoryLocator][concern]
                    .Add(requestedEntry);
            }
        }
    }
}
