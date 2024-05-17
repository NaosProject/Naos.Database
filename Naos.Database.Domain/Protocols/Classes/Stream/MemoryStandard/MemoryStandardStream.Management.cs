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

            lock (this.streamLock)
            {
                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryStandardStream)} can only 'Create' a stream with the same representation."));
                }

                var alreadyExisted = this.created;
                var wasCreated = true;

                if (this.created)
                {
                    switch (operation.ExistingStreamStrategy)
                    {
                        case ExistingStreamStrategy.Throw:
                            throw new InvalidOperationException(Invariant($"Expected stream {operation.StreamRepresentation} to not exist, it does and the operation {nameof(operation.ExistingStreamStrategy)} is set to '{operation.ExistingStreamStrategy}'."));
                        case ExistingStreamStrategy.Overwrite:
                            this.InitializeBackingDataStructures();
                            break;
                        case ExistingStreamStrategy.Skip:
                            wasCreated = false;
                            break;
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(ExistingStreamStrategy)} '{operation.ExistingStreamStrategy}' is not supported."));
                    }
                }
                else
                {
                    this.InitializeBackingDataStructures();
                }

                this.created = true;

                var result = new CreateStreamResult(alreadyExisted, wasCreated);

                return result;
            }
        }

        /// <inheritdoc />
        public override void Execute(
            StandardDeleteStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            lock (this.streamLock)
            {
                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryStandardStream)} can only 'Delete' a stream with the same representation."));
                }

                if (this.created)
                {
                    this.InitializeBackingDataStructures();
                    this.created = false;
                }
                else
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
            }
        }

        /// <inheritdoc />
        public override void Execute(
            StandardPruneStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<MemoryDatabaseLocator>() ?? this.TryGetSingleLocator();

            lock (this.streamLock)
            {
                this.ThrowIfStreamNotCreated();

                if (this.locatorToRecordPartitionMap.TryGetValue(locator, out var partition))
                {
                    var recordsToKeep = partition
                        .Where(_ => !operation.ShouldPrune(_.InternalRecordId, _.Metadata.TimestampUtc))
                        .ToList();

                    partition.Clear();
                    partition.AddRange(recordsToKeep);

                    if (this.locatorToHandlingEntriesByConcernMap.TryGetValue(locator, out var handlingEntriesByConcernMap))
                    {
                        foreach (var concern in handlingEntriesByConcernMap.Keys)
                        {
                            var handlingEntries = handlingEntriesByConcernMap[concern];

                            var handlingEntriesToKeep = handlingEntries
                                .Where(_ => !operation.ShouldPrune(_.InternalRecordId, _.TimestampUtc))
                                .ToList();

                            handlingEntries.Clear();
                            handlingEntries.AddRange(handlingEntriesToKeep);
                        }
                    }
                }
            }
        }
    }
}
