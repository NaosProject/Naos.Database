// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.Write.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using static System.FormattableString;

    public partial class MemoryStandardStream
    {
        /// <inheritdoc />
        public override long Execute(
            StandardGetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                var result = Interlocked.Increment(ref this.uniqueLongForExternalProtocol);

                return result;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        public override PutRecordResult Execute(
            StandardPutRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var memoryDatabaseLocator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                var existingRecordIds = new List<long>();

                if (!this.locatorToRecordPartitionMap.TryGetValue(memoryDatabaseLocator, out var recordPartition))
                {
                    recordPartition = new List<StreamRecord>();

                    this.locatorToRecordPartitionMap.Add(memoryDatabaseLocator, recordPartition);
                }

                var matchesId =
                    (operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundById)
                 || (operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundById)
                 || (operation.ExistingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById)
                        ? recordPartition.Where(
                                _ => _.Metadata.FuzzyMatchTypesAndId(
                                    operation.Metadata.StringSerializedId,
                                    operation.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                    null,
                                    operation.VersionMatchStrategy))
                            .ToList()
                        : new List<StreamRecord>();

                var matchesIdAndObject =
                    (operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundByIdAndTypeAndContent)
                 || (operation.ExistingRecordStrategy == ExistingRecordStrategy.DoNotWriteIfFoundByIdAndType)
                 || (operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundByIdAndTypeAndContent)
                 || (operation.ExistingRecordStrategy == ExistingRecordStrategy.ThrowIfFoundByIdAndType)
                 || (operation.ExistingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType)
                        ? recordPartition.Where(
                                _ => _.Metadata.FuzzyMatchTypesAndId(
                                    operation.Metadata.StringSerializedId,
                                    operation.Metadata.TypeRepresentationOfId.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                    operation.Metadata.TypeRepresentationOfObject.GetTypeRepresentationByStrategy(operation.VersionMatchStrategy),
                                    operation.VersionMatchStrategy))
                            .ToList()
                        : new List<StreamRecord>();

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
                        var matchesThrow = matchesIdAndObject.Where(_ => _.Payload.Equals(operation.Payload)).ToList();

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
                        var matchesDoNotWrite = matchesIdAndObject.Where(_ => _.Payload.Equals(operation.Payload)).ToList();

                        if (matchesDoNotWrite.Any())
                        {
                            return new PutRecordResult(null, matchesDoNotWrite.Select(_ => _.InternalRecordId).ToList());
                        }

                        break;
                    case ExistingRecordStrategy.PruneIfFoundById:
                        // ReSharper disable once PossibleInvalidOperationException - constructor ensures RecordRetentionCount can't be null when ExistingRecordStrategy is PruneIfFoundById
                        if (matchesId.Count > (int)operation.RecordRetentionCount)
                        {
                            existingRecordIds.AddRange(matchesId.Select(_ => _.InternalRecordId).ToList());

                            var recordsToDeleteById = matchesId.OrderByDescending(_ => _.InternalRecordId).Skip((int)operation.RecordRetentionCount).ToList();

                            recordIdsToPrune.AddRange(recordsToDeleteById.Select(_ => _.InternalRecordId));
                        }

                        break;
                    case ExistingRecordStrategy.PruneIfFoundByIdAndType:
                        // ReSharper disable once PossibleInvalidOperationException - constructor ensures RecordRetentionCount can't be null when ExistingRecordStrategy is PruneIfFoundByIdAndType
                        if (matchesIdAndObject.Count > (int)operation.RecordRetentionCount)
                        {
                            existingRecordIds.AddRange(matchesIdAndObject.Select(_ => _.InternalRecordId).ToList());

                            var recordsToDeleteById = matchesIdAndObject.OrderByDescending(_ => _.InternalRecordId).Skip((int)operation.RecordRetentionCount).ToList();

                            recordIdsToPrune.AddRange(recordsToDeleteById.Select(_ => _.InternalRecordId));
                        }

                        break;
                    default:
                        throw new NotSupportedException(Invariant($"This {nameof(ExistingRecordStrategy)} is not supported: {operation.ExistingRecordStrategy}."));
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

                        if ((long)operation.InternalRecordId > this.uniqueLongForInMemoryRecords)
                        {
                            Interlocked.Exchange(ref this.uniqueLongForInMemoryRecords, id);
                        }
                    }
                }
                else
                {
                    id = Interlocked.Increment(ref this.uniqueLongForInMemoryRecords);
                }

                var itemToAdd = new StreamRecord(id, operation.Metadata, operation.Payload);

                recordPartition.Add(itemToAdd);

                recordPartition.RemoveAll(_ => recordIdsToPrune.Contains(_.InternalRecordId));

                var result = new PutRecordResult(id, existingRecordIds, recordIdsToPrune);

                return result;
            }
        }
    }
}
