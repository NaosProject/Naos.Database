﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.171.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;

    using global::FakeItEasy;

    using global::Naos.Database.Domain;

    using global::OBeautifulCode.AutoFakeItEasy;
    using global::OBeautifulCode.Math.Recipes;
    using global::OBeautifulCode.Representation.System;
    using global::OBeautifulCode.Serialization;
    using global::OBeautifulCode.Type;

    /// <summary>
    /// The default (code generated) Dummy Factory.
    /// Derive from this class to add any overriding or custom registrations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("OBeautifulCode.CodeGen.ModelObject", "1.0.171.0")]
#if !NaosDatabaseSolution
    internal
#else
    public
#endif
    abstract class DefaultDatabaseDummyFactory : IDummyFactory
    {
        public DefaultDatabaseDummyFactory()
        {
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new BlockedRecordHandlingEvent(
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new BlockRecordHandlingOp(
                                 A.Dummy<string>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CancelBlockedRecordHandlingOp(
                                 A.Dummy<string>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CanceledBlockedRecordHandlingEvent(
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CanceledPruneRequestedEvent(
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CanceledRequestedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CanceledRunningHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CancelHandleRecordExecutionRequestOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CancelRunningHandleRecordExecutionOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CompletedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<DateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CompleteRunningHandleRecordExecutionOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardCreateStreamOp(
                                 A.Dummy<IStreamRepresentation>(),
                                 A.Dummy<ExistingStreamStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardCreateStreamResult(
                                 A.Dummy<bool>(),
                                 A.Dummy<bool>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var availableTypes = new[]
                    {
                        typeof(FileSystemDatabaseLocator),
                        typeof(MemoryDatabaseLocator),
                        typeof(NullDatabaseLocator)
                    };

                    var randomIndex = ThreadSafeRandom.Next(0, availableTypes.Length);

                    var randomType = availableTypes[randomIndex];

                    var result = (DatabaseLocatorBase)AD.ummy(randomType);

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardDeleteStreamOp(
                                 A.Dummy<IStreamRepresentation>(),
                                 A.Dummy<StreamNotFoundStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new DoesAnyExistByIdOp<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new FailedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new FailRunningHandleRecordExecutionOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new FileStreamRepresentation(
                                 A.Dummy<string>(),
                                 A.Dummy<IReadOnlyList<FileSystemDatabaseLocator>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new FileSystemDatabaseLocator(
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetAllRecordsByIdOp<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<OrderRecordsBy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetAllRecordsMetadataByIdOp<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<OrderRecordsBy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetAllResourceLocatorsOp());

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetHandlingHistoryOfRecordOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetHandlingStatusOfRecordByInternalRecordIdOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetHandlingStatusOfRecordsByIdOp(
                                 A.Dummy<string>(),
                                 A.Dummy<IReadOnlyCollection<StringSerializedIdentifier>>(),
                                 A.Dummy<HandlingStatusCompositionStrategy>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetHandlingStatusOfRecordsByIdOp<Version>(
                                 A.Dummy<string>(),
                                 A.Dummy<IReadOnlyCollection<Version>>(),
                                 A.Dummy<HandlingStatusCompositionStrategy>(),
                                 A.Dummy<VersionMatchStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetHandlingStatusOfRecordSetByTagOp(
                                 A.Dummy<string>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<HandlingStatusCompositionStrategy>(),
                                 A.Dummy<TagMatchStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetLatestObjectByIdOp<Version, Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetLatestObjectByTagOp<Version>(
                                 A.Dummy<NamedValue<string>>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetLatestObjectOp<Version>(
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetLatestRecordByIdOp<Version, Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetLatestRecordByIdOp<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetLatestRecordMetadataByIdOp<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetLatestRecordOp<Version>(
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetNextUniqueLongOp(
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetResourceLocatorByIdOp<Version>(
                                 A.Dummy<Version>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetResourceLocatorForUniqueIdentifierOp());

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetStreamFromRepresentationOp(
                                 A.Dummy<IStreamRepresentation>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new GetStreamFromRepresentationOp<FileStreamRepresentation, MemoryReadWriteStream>(
                                 A.Dummy<FileStreamRepresentation>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new HandleRecordOp(
                                 A.Dummy<StreamRecord>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new HandleRecordOp<Version>(
                                 A.Dummy<StreamRecord<Version>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new HandleRecordWithIdOp<Version, Version>(
                                 A.Dummy<StreamRecordWithId<Version, Version>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new HandleRecordWithIdOp<Version>(
                                 A.Dummy<StreamRecordWithId<Version>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new HandlingStatusCompositionStrategy(
                                 A.Dummy<bool>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new IdDeprecatedEvent<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<DateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new MemoryDatabaseLocator(
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new MemoryStreamRepresentation(
                                 A.Dummy<string>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new NamedResourceLocator(
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new NullDatabaseLocator());

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new NullResourceLocator());

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new NullStreamIdentifier());

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new NullStreamRepresentation());

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneBeforeInternalRecordDateOp(
                                 A.Dummy<DateTime>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneBeforeInternalRecordIdOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneOperationExecutedEvent(
                                 A.Dummy<IPruneOperation>(),
                                 A.Dummy<PruneSummary>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneOperationRequestedEvent(
                                 A.Dummy<IPruneOperation>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PruneSummary(
                                 A.Dummy<IReadOnlyList<long>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PutAndReturnInternalRecordIdOp<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<ExistingRecordStrategy>(),
                                 A.Dummy<int?>(),
                                 A.Dummy<VersionMatchStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PutOp<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<ExistingRecordStrategy>(),
                                 A.Dummy<int?>(),
                                 A.Dummy<VersionMatchStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PutRecordResult(
                                 A.Dummy<long?>(),
                                 A.Dummy<IReadOnlyCollection<long>>(),
                                 A.Dummy<IReadOnlyCollection<long>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<Version>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<ExistingRecordStrategy>(),
                                 A.Dummy<int?>(),
                                 A.Dummy<VersionMatchStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new PutWithIdOp<Version, Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<Version>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<ExistingRecordStrategy>(),
                                 A.Dummy<int?>(),
                                 A.Dummy<VersionMatchStrategy>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RequestedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<DateTime>(),
                                 A.Dummy<StreamRecord>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var availableTypes = new[]
                    {
                        typeof(FileSystemDatabaseLocator),
                        typeof(MemoryDatabaseLocator),
                        typeof(NamedResourceLocator),
                        typeof(NullDatabaseLocator),
                        typeof(NullResourceLocator)
                    };

                    var randomIndex = ThreadSafeRandom.Next(0, availableTypes.Length);

                    var randomType = availableTypes[randomIndex];

                    var result = (ResourceLocatorBase)AD.ummy(randomType);

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RetryFailedHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RetryFailedHandleRecordExecutionOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RunningHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<DateTime>(),
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new SelfCanceledRunningHandleRecordExecutionEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<DateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new SelfCancelRunningHandleRecordExecutionOp(
                                 A.Dummy<long>(),
                                 A.Dummy<string>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardDoesAnyExistByIdOp(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetAllRecordsByIdOp(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<OrderRecordsBy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetAllRecordsMetadataByIdOp(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<OrderRecordsBy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetDistinctStringSerializedIdsOp(
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<TagMatchStrategy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetLatestRecordByIdOp(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetLatestRecordByTagOp(
                                 A.Dummy<NamedValue<string>>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetLatestRecordMetadataByIdOp(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetLatestRecordOp(
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetNextUniqueLongOp(
                                 A.Dummy<string>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardGetRecordByInternalRecordIdOp(
                                 A.Dummy<long>(),
                                 A.Dummy<RecordNotFoundStrategy>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardPutRecordOp(
                                 A.Dummy<StreamRecordMetadata>(),
                                 A.Dummy<DescribedSerializationBase>(),
                                 A.Dummy<ExistingRecordStrategy>(),
                                 A.Dummy<int?>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<long?>(),
                                 A.Dummy<IResourceLocator>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecord(
                                 A.Dummy<long>(),
                                 A.Dummy<StreamRecordMetadata>(),
                                 A.Dummy<DescribedSerializationBase>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecord<Version>(
                                 A.Dummy<long>(),
                                 A.Dummy<StreamRecordMetadata>(),
                                 A.Dummy<Version>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordHandlingEntry(
                                 A.Dummy<long>(),
                                 A.Dummy<StreamRecordHandlingEntryMetadata>(),
                                 A.Dummy<DescribedSerializationBase>()));

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
                                 A.Dummy<DateTime>(),
                                 A.Dummy<DateTime?>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordMetadata(
                                 A.Dummy<string>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<DateTime>(),
                                 A.Dummy<DateTime?>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordMetadata<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<TypeRepresentationWithAndWithoutVersion>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<DateTime>(),
                                 A.Dummy<DateTime?>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordWithId<Version, Version>(
                                 A.Dummy<long>(),
                                 A.Dummy<StreamRecordMetadata<Version>>(),
                                 A.Dummy<Version>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordWithId<Version>(
                                 A.Dummy<long>(),
                                 A.Dummy<StreamRecordMetadata<Version>>(),
                                 A.Dummy<DescribedSerializationBase>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRepresentation(
                                 A.Dummy<string>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var availableTypes = new[]
                    {
                        typeof(FileStreamRepresentation),
                        typeof(MemoryStreamRepresentation),
                        typeof(StreamRepresentation)
                    };

                    var randomIndex = ThreadSafeRandom.Next(0, availableTypes.Length);

                    var randomType = availableTypes[randomIndex];

                    var result = (StreamRepresentationBase)AD.ummy(randomType);

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StringSerializedIdentifier(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new TagMatchStrategy(
                                 A.Dummy<TagMatchScope>(),
                                 A.Dummy<TagMatchScope>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new ThrowIfResourceUnavailableOp(
                                 A.Dummy<ResourceLocatorBase>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StandardTryHandleRecordOp(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<OrderRecordsBy>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<string>(),
                                 A.Dummy<long?>(),
                                 A.Dummy<bool>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new TryHandleRecordOp<Version>(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<OrderRecordsBy>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<string>(),
                                 A.Dummy<long?>(),
                                 A.Dummy<bool>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new TryHandleRecordResult(
                                 A.Dummy<StreamRecord>(),
                                 A.Dummy<bool>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new TryHandleRecordWithIdOp<Version, Version>(
                                 A.Dummy<string>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<OrderRecordsBy>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<string>(),
                                 A.Dummy<long?>(),
                                 A.Dummy<bool>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new TryHandleRecordWithIdOp<Version>(
                                 A.Dummy<string>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<VersionMatchStrategy>(),
                                 A.Dummy<OrderRecordsBy>(),
                                 A.Dummy<IResourceLocator>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>(),
                                 A.Dummy<string>(),
                                 A.Dummy<long?>(),
                                 A.Dummy<bool>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new TypeRepresentationWithAndWithoutVersion(
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<TypeRepresentation>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new UniqueLongIssuedEvent(
                                 A.Dummy<long>(),
                                 A.Dummy<DateTime>(),
                                 A.Dummy<string>()));
        }

        /// <inheritdoc />
        public Priority Priority => new FakeItEasy.Priority(1);

        /// <inheritdoc />
        public bool CanCreate(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public object Create(Type type)
        {
            return null;
        }
    }
}