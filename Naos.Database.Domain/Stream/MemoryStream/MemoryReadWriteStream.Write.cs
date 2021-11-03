// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryReadWriteStream.Write.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;
    using DomainExtensions = OBeautifulCode.Serialization.DomainExtensions;

    /// <summary>
    /// In memory implementation of <see cref="IReadWriteStream"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class MemoryReadWriteStream :
        StandardStreamBase
    {
        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>System.Int64.</returns>
        public override long Execute(
            StandardGetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var result = Interlocked.Increment(ref this.uniqueLongForExternalProtocol);
            return result;
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        public override PutRecordResult Execute(
            StandardPutRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                var existingRecordIds = new List<long>();
                var exists = this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var recordPartition);
                if (!exists)
                {
                    recordPartition = new List<StreamRecord>();
                    this.locatorToRecordPartitionMap.Add(memoryDatabaseLocator, recordPartition);
                }

                var matchesId =
                    operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundById
                 || operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundById
                 || operation.ExistingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById
                        ? recordPartition.Where(
                                              _ => _.Metadata.FuzzyMatchTypesAndId(
                                                  operation.Metadata.StringSerializedId,
                                                  operation.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                                  null,
                                                  operation.VersionMatchStrategy))
                                         .ToList() : new List<StreamRecord>();

                var matchesIdAndObject =
                    operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundByIdAndTypeAndContent
                 || operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundByIdAndType
                 || operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundByIdAndTypeAndContent
                 || operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundByIdAndType
                 || operation.ExistingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType
                        ? recordPartition.Where(
                                              _ => _.Metadata.FuzzyMatchTypesAndId(
                                                  operation.Metadata.StringSerializedId,
                                                  operation.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                                  operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                                  operation.VersionMatchStrategy))
                                         .ToList() : new List<StreamRecord>();

                var recordIdsToPrune = new List<long>();
                switch (operation.ExistingRecordStrategy)
                {
                    case ExistingRecordStrategy.None:
                        /* no-op */
                        break;
                    case ExistingRecordStrategy.ThrowIfFoundById:
                        if (matchesId.Any())
                        {
                            throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordStrategy)} was {operation.ExistingRecordStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' yet found {matchesId.Count}."));
                        }

                        break;
                    case ExistingRecordStrategy.ThrowIfFoundByIdAndType:
                        if (matchesIdAndObject.Any())
                        {
                            throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordStrategy)} was {operation.ExistingRecordStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' and object type '{operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy)}' yet found {matchesIdAndObject.Count}."));
                        }

                        break;
                    case ExistingRecordStrategy.ThrowIfFoundByIdAndTypeAndContent:
                        var matchesThrow =
                            matchesIdAndObject.Where(_ => _.Payload.Equals(operation.Payload)).ToList();

                        if (matchesThrow.Any())
                        {
                            throw new InvalidOperationException(Invariant($"Operation {nameof(ExistingRecordStrategy)} was {operation.ExistingRecordStrategy}; expected to not find a record by identifier '{operation.Metadata.StringSerializedId}' and object type '{operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy)}' and contents '{operation.Payload}' yet found {matchesThrow.Count}."));
                        }

                        break;
                    case ExistingRecordStrategy.DoNotWriteIfFoundById:
                        if (matchesId.Any())
                        {
                            return new PutRecordResult(null, matchesId.Select(_ => _.InternalRecordId).ToList());
                        }

                        break;
                    case ExistingRecordStrategy.DoNotWriteIfFoundByIdAndType:
                        if (matchesIdAndObject.Any())
                        {
                            return new PutRecordResult(null, matchesIdAndObject.Select(_ => _.InternalRecordId).ToList());
                        }

                        break;
                    case ExistingRecordStrategy.DoNotWriteIfFoundByIdAndTypeAndContent:
                        var matchesDoNotWrite =
                            matchesIdAndObject.Where(_ => _.Payload.Equals(operation.Payload)).ToList();

                        if (matchesDoNotWrite.Any())
                        {
                            return new PutRecordResult(null, matchesDoNotWrite.Select(_ => _.InternalRecordId).ToList());
                        }

                        break;
                    case ExistingRecordStrategy.PruneIfFoundById:
                        if (operation.RecordRetentionCount != null && matchesId.Count > operation.RecordRetentionCount - 1)
                        {
                            existingRecordIds.AddRange(
                                matchesId
                                   .Select(_ => _.InternalRecordId)
                                   .ToList());

                            var recordsToDeleteById =
                                matchesId.OrderByDescending(_ => _.InternalRecordId).Skip((int)operation.RecordRetentionCount - 1).ToList();
                            recordIdsToPrune.AddRange(recordsToDeleteById.Select(_ => _.InternalRecordId));
                        }

                        break;
                    case ExistingRecordStrategy.PruneIfFoundByIdAndType:
                        if (operation.RecordRetentionCount != null && matchesIdAndObject.Count > operation.RecordRetentionCount - 1)
                        {
                            existingRecordIds.AddRange(
                                matchesIdAndObject
                                   .Select(_ => _.InternalRecordId)
                                   .ToList());

                            var recordsToDeleteById =
                                matchesIdAndObject.OrderByDescending(_ => _.InternalRecordId).Skip((int)operation.RecordRetentionCount - 1).ToList();
                            recordIdsToPrune.AddRange(recordsToDeleteById.Select(_ => _.InternalRecordId));
                        }

                        break;
                }

                long id;
                if (operation.InternalRecordId != null)
                {
                    if (recordPartition.Any(_ => _.InternalRecordId == operation.InternalRecordId))
                    {
                        throw new InvalidOperationException(Invariant($"Operation specified an {nameof(StandardPutRecordOp.InternalRecordId)} of {operation.InternalRecordId} but that {nameof(StandardPutRecordOp.InternalRecordId)} is already present in the stream."));
                    }
                    else
                    {
                        id = (long)operation.InternalRecordId;
                    }
                }
                else
                {
                    id = Interlocked.Increment(ref this.uniqueLongForInMemoryRecords);
                }

                var itemToAdd = new StreamRecord(id, operation.Metadata, operation.Payload);
                recordPartition.Add(itemToAdd);
                recordPartition.RemoveAll(_ => recordIdsToPrune.Contains(_.InternalRecordId));

                return new PutRecordResult(id, existingRecordIds, recordIdsToPrune);
            }
        }
    }
}
