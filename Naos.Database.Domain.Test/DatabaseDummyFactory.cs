// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseDummyFactory.cs" company="Naos Project">
//     Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using Naos.Protocol.Domain;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A Dummy Factory for types in <see cref="Naos.Database.Domain" />.
    /// Implements the <see cref="Naos.Database.Domain.Test.DefaultDatabaseDummyFactory" />.
    /// </summary>
    /// <seealso cref="Naos.Database.Domain.Test.DefaultDatabaseDummyFactory" />
#if !NaosDatabaseSolution
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Database.Domain.Test", "See package version number")]
    internal
#else
    public
#endif
    class DatabaseDummyFactory : DefaultDatabaseDummyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDummyFactory" /> class.
        /// </summary>
        public DatabaseDummyFactory()
        {
            /* Add any overriding or custom registrations here. */

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

                    var result = (IPruneOperation)AD.ummy(randomType);

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
                    return new RequestedHandleRecordExecutionEvent(
                        streamRecord.InternalRecordId,
                        A.Dummy<DateTime>(),
                        streamRecord);
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CanceledRequestedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CanceledRunningHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CompletedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<DateTime>().ToUniversalTime(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RetryFailedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var record = A.Dummy<StreamRecord>();
                    return new RequestedHandleRecordExecutionEvent(
                        record.InternalRecordId,
                        A.Dummy<DateTime>().ToUniversalTime(),
                        record,
                        A.Dummy<string>());
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RunningHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<DateTime>().ToUniversalTime(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new SelfCanceledRunningHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new BlockedRecordHandlingEvent(
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CanceledBlockedRecordHandlingEvent(
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new FailedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordHandlingEntryMetadata(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<HandlingStatus>(),
                                 A.Dummy<string>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<IReadOnlyDictionary<string, string>>(),
                                 A.Dummy<DateTime>().ToUniversalTime(),
                                 A.Dummy<DateTime?>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CanceledPruneRequestedEvent(
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneBeforeInternalRecordDateOp(
                                 A.Dummy<DateTime>().ToUniversalTime(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneOperationExecutedEvent(
                                 A.Dummy<IPruneOperation>(),
                                 A.Dummy<PruneSummary>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneOperationRequestedEvent(
                                 A.Dummy<IPruneOperation>(),
                                 A.Dummy<DateTime>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordMetadata(
                                 A.Dummy<string>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<IReadOnlyDictionary<string, string>>(),
                                 A.Dummy<DateTime>().ToUniversalTime(),
                                 A.Dummy<DateTime?>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordMetadata<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<IReadOnlyDictionary<string, string>>(),
                                 A.Dummy<DateTime>().ToUniversalTime(),
                                 A.Dummy<DateTime?>().ToUniversalTime()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new UniqueLongIssuedEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<DateTime>().ToUniversalTime(),
                                 A.Dummy<string>()));
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
