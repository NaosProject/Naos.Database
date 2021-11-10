// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.Management.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    public partial class MemoryStandardStream
    {
        /// <inheritdoc />
        public override CreateStreamResult Execute(
            StandardCreateStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var alreadyExisted = this.created;
            var wasCreated = true;

            lock (this.streamLock)
            {
                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryStandardStream)} can only 'create' a stream with the same representation."));
                }

                if (this.created)
                {
                    switch (operation.ExistingStreamStrategy)
                    {
                        case ExistingStreamStrategy.Throw:
                            throw new InvalidOperationException(Invariant($"Expected stream {operation.StreamRepresentation} to not exist, it does and the operation {nameof(operation.ExistingStreamStrategy)} is set to '{operation.ExistingStreamStrategy}'."));
                        case ExistingStreamStrategy.Overwrite:
                            this.locatorToRecordPartitionMap.Clear();
                            break;
                        case ExistingStreamStrategy.Skip:
                            wasCreated = false;
                            break;
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(ExistingStreamStrategy)} '{operation.ExistingStreamStrategy}' is not supported."));
                    }
                }

                this.created = true;
            }

            var result = new CreateStreamResult(alreadyExisted, wasCreated);

            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            StandardDeleteStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            lock (this.streamLock)
            {
                if (operation == null)
                {
                    throw new ArgumentNullException(nameof(operation));
                }

                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryStandardStream)} can only 'Delete' a stream with the same representation."));
                }

                if (!this.created)
                {
                    switch (operation.StreamNotFoundStrategy)
                    {
                        case StreamNotFoundStrategy.Throw:
                            throw new InvalidOperationException(Invariant($"Expected stream {operation.StreamRepresentation} to exist, it does not and the operation {nameof(operation.StreamNotFoundStrategy)} is '{operation.StreamNotFoundStrategy}'."));
                        case StreamNotFoundStrategy.Skip:
                            break;
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(StreamNotFoundStrategy)} {operation.StreamNotFoundStrategy} is not supported."));
                    }
                }
                else
                {
                    foreach (var partition in this.locatorToRecordPartitionMap)
                    {
                        partition.Value.Clear();
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            StandardPruneStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            bool RecordPredicate(
                StreamRecord record) => operation.ShouldPrune(record.InternalRecordId, record.Metadata.TimestampUtc);

            bool HandlingPredicate(
                StreamRecordHandlingEntry handlingEntry) => operation.ShouldPrune(
                handlingEntry.Metadata.InternalRecordId,
                handlingEntry.Metadata.TimestampUtc);

            lock (this.streamLock)
            {
                var newList = this.locatorToRecordPartitionMap[locator].Where(RecordPredicate);
                this.locatorToRecordPartitionMap[locator].Clear();
                this.locatorToRecordPartitionMap[locator].AddRange(newList);

                lock (this.handlingLock)
                {
                    if (this.locatorToHandlingEntriesByConcernMap.ContainsKey(locator) && this.locatorToHandlingEntriesByConcernMap[locator].Any())
                    {
                        foreach (var concern in this.locatorToHandlingEntriesByConcernMap[locator].Keys)
                        {
                            var newHandlingList = this.locatorToHandlingEntriesByConcernMap[locator][concern].Where(HandlingPredicate);
                            this.locatorToHandlingEntriesByConcernMap[locator][concern].Clear();
                            this.locatorToHandlingEntriesByConcernMap[locator][concern].AddRange(newHandlingList);
                        }
                    }
                }
            }
        }
    }
}
