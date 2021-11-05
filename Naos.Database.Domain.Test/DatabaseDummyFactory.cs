// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseDummyFactory.cs" company="Naos Project">
//     Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FakeItEasy;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// A Dummy Factory for types in <see cref="Domain" />.
    /// Implements the <see cref="DefaultDatabaseDummyFactory" />.
    /// </summary>
    /// <seealso cref="DefaultDatabaseDummyFactory" />
#if !NaosDatabaseSolution
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Database.Domain.Test", "See package version number")]
    internal
#else
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public
#endif
    class DatabaseDummyFactory : DefaultDatabaseDummyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDummyFactory" /> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        public DatabaseDummyFactory()
        {
            // ------------------------------- ENUMS --------------------------------------
            AutoFixtureBackedDummyFactory.ConstrainDummyToBeOneOf(VersionMatchStrategy.Any, VersionMatchStrategy.SpecifiedVersion);
            AutoFixtureBackedDummyFactory.ConstrainDummyToExclude(HandlingStatus.Unknown);

            // ------------------------------- EVENTS -------------------------------------
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new IdDeprecatedEvent<Version>(A.Dummy<Version>(), A.Dummy<UtcDateTime>(), A.Dummy<string>());

                    return result;
                });

            // ------------------------------- OPERATIONS -------------------------------------
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var existingRecordStrategy = A.Dummy<ExistingRecordStrategy>();
                    return new PutAndReturnInternalRecordIdOp<Version>(
                        A.Dummy<Version>(),
                        A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                        existingRecordStrategy,
                        existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById
                        || existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType
                            ? (int?)A.Dummy<ZeroOrPositiveInteger>()
                            : null,
                        A.Dummy<VersionMatchStrategy>());
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var existingRecordStrategy = A.Dummy<ExistingRecordStrategy>();
                    return new PutOp<Version>(
                        A.Dummy<Version>(),
                        A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                        existingRecordStrategy,
                        existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById
                     || existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType
                            ? (int?)A.Dummy<ZeroOrPositiveInteger>()
                            : null,
                        A.Dummy<VersionMatchStrategy>());
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var existingRecordStrategy = A.Dummy<ExistingRecordStrategy>();
                    return new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                        A.Dummy<Version>(),
                        A.Dummy<Version>(),
                        A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                        existingRecordStrategy,
                        existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById
                        || existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType
                            ? (int?)A.Dummy<ZeroOrPositiveInteger>()
                            : null,
                        A.Dummy<VersionMatchStrategy>());
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var existingRecordStrategy = A.Dummy<ExistingRecordStrategy>();
                    return new PutWithIdOp<Version, Version>(
                        A.Dummy<Version>(),
                        A.Dummy<Version>(),
                        A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                        existingRecordStrategy,
                        existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById
                     || existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType
                            ? (int?)A.Dummy<ZeroOrPositiveInteger>()
                            : null,
                        A.Dummy<VersionMatchStrategy>());
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var existingRecordStrategy = A.Dummy<ExistingRecordStrategy>();
                    return new StandardPutRecordOp(
                        A.Dummy<StreamRecordMetadata>(),
                        A.Dummy<DescribedSerializationBase>(),
                        existingRecordStrategy,
                        existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById
                        || existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType
                            ? (int?)A.Dummy<ZeroOrPositiveInteger>()
                            : null,
                        A.Dummy<VersionMatchStrategy>(),
                        A.Dummy<long?>(),
                        A.Dummy<IResourceLocator>());
                });

            // ------------------------------- TO CLEANUP -------------------------------------
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    // make sure IStreamRepresentation has a correct option instead of a proxy
                    var availableTypes = new[]
                                         {
                                             typeof(FileStreamRepresentation),
                                             typeof(MemoryStreamRepresentation),
                                         };

                    var randomIndex = ThreadSafeRandom.Next(0, availableTypes.Length);

                    var randomType = availableTypes[randomIndex];

                    var result = (IStreamRepresentation)AD.ummy(randomType);

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    // make sure IPruneOperation has a correct option instead of a proxy
                    var availableTypes = new[]
                                         {
                                             typeof(PruneBeforeInternalRecordDateOp),
                                             typeof(PruneBeforeInternalRecordIdOp),
                                         };

                    var randomIndex = ThreadSafeRandom.Next(0, availableTypes.Length);

                    var randomType = availableTypes[randomIndex];

                    var result = (IPruneOp)AD.ummy(randomType);

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    // make sure IPruneOperation has a correct option instead of a proxy
                    var availableTypes = new[]
                                         {
                                             typeof(NullDatabaseLocator),
                                             typeof(MemoryDatabaseLocator),
                                             typeof(FileSystemDatabaseLocator),
                                         };

                    var randomIndex = ThreadSafeRandom.Next(0, availableTypes.Length);

                    var randomType = availableTypes[randomIndex];

                    var result = (IResourceLocator)AD.ummy(randomType);

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var streamRecord = A.Dummy<StreamRecord>();

                    return new RecordHandlingAvailableEvent(
                        streamRecord.InternalRecordId,
                        A.Dummy<string>(),
                        streamRecord,
                        A.Dummy<UtcDateTime>());
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new HandlingForRecordDisabledEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RecordHandlingCancelledEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RecordHandlingCompletedEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RecordHandlingFailureResetEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var record = A.Dummy<StreamRecord>();
                    return new RecordHandlingAvailableEvent(
                        record.InternalRecordId,
                        A.Dummy<string>(),
                        record,
                        A.Dummy<UtcDateTime>(),
                        A.Dummy<string>());
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RecordHandlingRunningEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RecordHandlingSelfCancelledEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new HandlingForStreamDisabledEvent(
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new HandlingForStreamEnabledEvent(
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RecordHandlingFailedEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordHandlingEntryMetadata(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<HandlingStatus>(),
                                 A.Dummy<string>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<UtcDateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneRequestCancelledEvent(
                                 A.Dummy<string>(),
                                 A.Dummy<UtcDateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneBeforeInternalRecordDateOp(
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneOperationExecutedEvent(
                                 A.Dummy<IPruneOp>(),
                                 A.Dummy<PruneSummary>(),
                                 A.Dummy<UtcDateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneOperationRequestedEvent(
                                 A.Dummy<IPruneOp>(),
                                 A.Dummy<UtcDateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordMetadata(
                                 A.Dummy<string>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<DateTime?>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordMetadata<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<DateTime?>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new UniqueLongIssuedEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<UtcDateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var scenario = ThreadSafeRandom.Next(1, 4);
                    switch (scenario)
                    {
                        case 1:
                            return new CreateStreamResult(false, true);
                        case 2:
                            return new CreateStreamResult(true, false);
                        case 3:
                            return new CreateStreamResult(true, true);
                        default:
                            throw new NotSupportedException(
                                FormattableString.Invariant($"Invalid scenario {scenario} for creating a dummy {nameof(CreateStreamResult)}."));
                    }
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var scenario = ThreadSafeRandom.Next(1, 5);
                    switch (scenario)
                    {
                        case 1:
                            return new PutRecordResult(A.Dummy<long>(), null);
                        case 2:
                            return new PutRecordResult(null, Some.ReadOnlyDummies<long>().ToList());
                        case 3:
                            return new PutRecordResult(null, Some.ReadOnlyDummies<long>().ToList(), Some.ReadOnlyDummies<long>().ToList());
                        case 4:
                            return new PutRecordResult(A.Dummy<long>(), Some.ReadOnlyDummies<long>().ToList(), Some.ReadOnlyDummies<long>().ToList());
                        default:
                            throw new NotSupportedException(
                                FormattableString.Invariant($"Invalid scenario {scenario} for creating a dummy {nameof(PutRecordResult)}."));
                    }
                });
        }
    }

    public static class DummyExtensions
    {
        public static DateTime? ToUniversalTime(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            var intermediate = (DateTime)dateTime;
            var result = intermediate.ToUniversalTime();
            return result;
        }
    }
}
