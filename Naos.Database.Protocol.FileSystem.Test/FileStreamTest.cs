// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Protocol.FileSystem.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using FakeItEasy;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Database.Protocol.FileSystem;
    using Naos.Database.Serialization.Json;
    using Naos.Protocol.Domain;
    using Naos.Protocol.Serialization.Json;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using Xunit;
    using Xunit.Abstractions;
    using static System.FormattableString;

    /// <summary>
    /// Tests for <see cref="FileReadWriteStream"/>.
    /// </summary>
    public partial class FileStreamTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public FileStreamTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GetDistinctStringSerializedIdsRecordOp___Various_usages___Should_function()
        {
            var streamName = "FS_GetDistinctStringSerializedIdsRecordOp";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();
            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(SerializationKind.Json, configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var resourceLocatorForUniqueIdentifier = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "UniqueIdentifiers"));
            var resourceLocatorZero = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "Zero"));
            var resourceLocatorOne = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "One"));
            var resourceLocatorTwo = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "Two"));
            var resourceLocatorThree = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "Three"));

            IResourceLocator ResourceLocatorByIdProtocol(GetResourceLocatorByIdOp<string> operation)
            {
                if (operation.Id == null)
                {
                    return resourceLocatorZero;
                }
                else if (operation.Id.Contains("One"))
                {
                    return resourceLocatorOne;
                }
                else if (operation.Id.Contains("Two"))
                {
                    return resourceLocatorTwo;
                }
                else if (operation.Id.Contains("Three"))
                {
                    return resourceLocatorThree;
                }
                else
                {
                    return resourceLocatorZero;
                }
            }

            var allLocators = new[]
                              {
                                  resourceLocatorZero,
                                  resourceLocatorOne,
                                  resourceLocatorTwo,
                                  resourceLocatorThree,
                              }.ToList();

            var locatorProtocols = new PassThroughResourceLocatorProtocols<string>(
                allLocators,
                resourceLocatorForUniqueIdentifier,
                ResourceLocatorByIdProtocol);

            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                locatorProtocols);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Skip));
            var zeroObject = new MyObject(null, "Null Id");
            var firstObject = new MyObject("RecordOne", "One Id");
            var secondObject = new MyObject("RecordTwo", "Two Id");
            var thirdObjectId = "Three Id";
            var thirdObject = "RecordThree";

            var serializer = stream.SerializerFactory.BuildSerializer(stream.DefaultSerializerRepresentation);
            var zeroObjectStringSerializedId = serializer.SerializeToString(zeroObject.Id);
            var firstObjectStringSerializedId = serializer.SerializeToString(firstObject.Id);
            var secondObjectStringSerializedId = serializer.SerializeToString(secondObject.Id);
            var thirdObjectStringSerializedId = serializer.SerializeToString(thirdObjectId);

            for (int idx = 0;
                idx < 10;
                idx++)
            {
                var timestampUtc = DateTime.UtcNow;
                stream.Execute(
                    new PutRecordOp(
                        new StreamRecordMetadata(
                            zeroObjectStringSerializedId,
                            stream.DefaultSerializerRepresentation,
                            typeof(decimal?).ToRepresentation().ToWithAndWithoutVersion(),
                            zeroObject.GetType().ToRepresentation().ToWithAndWithoutVersion(),
                            new Dictionary<string, string>
                            {
                                { "tag", "zero" },
                            },
                            timestampUtc,
                            null),
                        zeroObject.ToDescribedSerializationUsingSpecificSerializer(serializer, SerializationFormat.String),
                        resourceLocatorZero));
                stream.PutWithId(
                    firstObject.Id,
                    firstObject,
                    new Dictionary<string, string>
                    {
                        { "tag", "one" },
                    });
                stream.PutWithId(
                    secondObject.Id,
                    secondObject,
                    new Dictionary<string, string>
                    {
                        { "tag", "two" },
                    });
                stream.PutWithId(
                    thirdObjectId,
                    thirdObject,
                    new Dictionary<string, string>
                    {
                        { "tag", "third" },
                    });
                var firstIdObject = stream.GetLatestObjectById<string, MyObject>(firstObject.Id);
                this.testOutputHelper.WriteLine(Invariant($"Key={firstIdObject.Id}, Field={firstIdObject.Field}"));
                firstIdObject.Id.MustForTest().BeEqualTo(firstObject.Id);
            }

            var anyDistinct = stream.Execute(
                new GetDistinctStringSerializedIdsOp());
            anyDistinct.ToList().MustForTest()
                       .BeEqualTo(
                            new List<string>
                            {
                                zeroObjectStringSerializedId,
                                firstObjectStringSerializedId,
                                secondObjectStringSerializedId,
                                thirdObjectStringSerializedId,
                            });

            var objectObjectDistinct = stream.Execute(
                new GetDistinctStringSerializedIdsOp(
                    null,
                    typeof(MyObject).ToRepresentation()));
            objectObjectDistinct.ToList().MustForTest()
                       .BeEqualTo(
                            new List<string>
                            {
                                zeroObjectStringSerializedId,
                                firstObjectStringSerializedId,
                                secondObjectStringSerializedId,
                            });

            var stringIdDistinct = stream.Execute(
                new GetDistinctStringSerializedIdsOp(
                    typeof(string).ToRepresentation()));
            stringIdDistinct.ToList().MustForTest()
                            .BeEqualTo(
                                 new List<string>
                                 {
                                     firstObjectStringSerializedId,
                                     secondObjectStringSerializedId,
                                     thirdObjectStringSerializedId,
                                 });

            var stringIdObjectObjectDistinct = stream.Execute(
                new GetDistinctStringSerializedIdsOp(
                    typeof(string).ToRepresentation(),
                    typeof(MyObject).ToRepresentation()));
            stringIdObjectObjectDistinct.ToList().MustForTest()
                            .BeEqualTo(
                                 new List<string>
                                 {
                                     firstObjectStringSerializedId,
                                     secondObjectStringSerializedId,
                                 });

            var tagDistinct = stream.Execute(
                new GetDistinctStringSerializedIdsOp(
                    null,
                    null,
                    TypeVersionMatchStrategy.Any,
                    new Dictionary<string, string>
                    {
                        { "tag", "one" },
                    }));

            tagDistinct.ToList().MustForTest()
                            .BeEqualTo(
                                 new List<string>
                                 {
                                     firstObjectStringSerializedId,
                                 });

            var tagDistinctWrongIdType = stream.Execute(
                new GetDistinctStringSerializedIdsOp(
                    typeof(decimal?).ToRepresentation(),
                    null,
                    TypeVersionMatchStrategy.Any,
                    new Dictionary<string, string>
                    {
                        { "tag", "one" },
                    }));

            tagDistinctWrongIdType.ToList()
                              .MustForTest()
                              .BeEmptyEnumerable();

            var tagDistinctWrongObjectType = stream.Execute(
                new GetDistinctStringSerializedIdsOp(
                    null,
                    typeof(short).ToRepresentation(),
                    TypeVersionMatchStrategy.Any,
                    new Dictionary<string, string>
                    {
                        { "tag", "one" },
                    }));

            tagDistinctWrongObjectType.ToList()
                              .MustForTest()
                              .BeEmptyEnumerable();

            var tagDistinctWrongTagValue = stream.Execute(
                new GetDistinctStringSerializedIdsOp(
                    null,
                    null,
                    TypeVersionMatchStrategy.Any,
                    new Dictionary<string, string>
                    {
                        { "tag", "monkey" },
                    }));

            tagDistinctWrongTagValue.ToList()
                                    .MustForTest()
                                    .BeEmptyEnumerable();

            var tagDistinctWrongTagName = stream.Execute(
                new GetDistinctStringSerializedIdsOp(
                    null,
                    null,
                    TypeVersionMatchStrategy.Any,
                    new Dictionary<string, string>
                    {
                        { "monkey", "one" },
                    }));

            tagDistinctWrongTagName.ToList()
                                    .MustForTest()
                                    .BeEmptyEnumerable();

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public void Create_Put_Get_Delete___Given_valid_data___Should_roundtrip_to_file_system()
        {
            var streamName = "FS_ReadWriteTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();
            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(SerializationKind.Json, configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var resourceLocatorForUniqueIdentifier = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "UniqueIdentifiers"));
            var resourceLocatorZero = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "Zero"));
            var resourceLocatorOne = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "One"));
            var resourceLocatorTwo = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "Two"));
            var resourceLocatorThree = new FileSystemDatabaseLocator(Path.Combine(testingFilePath, "Three"));

            IResourceLocator ResourceLocatorByIdProtocol(GetResourceLocatorByIdOp<string> operation)
            {
                if (operation.Id == null)
                {
                    return resourceLocatorZero;
                }
                else if (operation.Id.Contains("One"))
                {
                    return resourceLocatorOne;
                }
                else if (operation.Id.Contains("Two"))
                {
                    return resourceLocatorTwo;
                }
                else if (operation.Id.Contains("Three"))
                {
                    return resourceLocatorThree;
                }
                else
                {
                    return resourceLocatorZero;
                }
            }

            var allLocators = new[]
                              {
                                  resourceLocatorZero,
                                  resourceLocatorOne,
                                  resourceLocatorTwo,
                                  resourceLocatorThree,
                              }.ToList();

            var locatorProtocols = new PassThroughResourceLocatorProtocols<string>(
                allLocators,
                resourceLocatorForUniqueIdentifier,
                ResourceLocatorByIdProtocol);

            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                locatorProtocols);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Skip));
            var zeroObject = new MyObject(null, "Null Id");
            var firstObject = new MyObject("RecordOne", "One Id");
            var secondObject = new MyObject("RecordTwo", "Two Id");
            var thirdObject = new MyObject("RecordThree", "Three Id");

            var start = DateTime.UtcNow;
            for (int idx = 0;
                idx < 10;
                idx++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Reset();
                stopwatch.Start();
                stream.GetStreamWritingWithIdProtocols<string, MyObject>().Execute(new PutWithIdOp<string, MyObject>(zeroObject.Id, zeroObject));
                stopwatch.Stop();
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                stopwatch.Reset();
                stopwatch.Start();
                stream.GetStreamWritingWithIdProtocols<string, MyObject>().Execute(new PutWithIdOp<string, MyObject>(firstObject.Id, firstObject));
                stopwatch.Stop();
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                stopwatch.Reset();
                stopwatch.Start();
                stream.GetStreamWritingWithIdProtocols<string, MyObject>().Execute(new PutWithIdOp<string, MyObject>(secondObject.Id, secondObject));
                stopwatch.Stop();
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                stopwatch.Reset();
                stopwatch.Start();
                stream.GetStreamWritingWithIdProtocols<string, MyObject>().Execute(new PutWithIdOp<string, MyObject>(thirdObject.Id, thirdObject));
                stopwatch.Stop();
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                stopwatch.Reset();
                stopwatch.Start();
                var firstIdObject = stream.GetStreamReadingWithIdProtocols<string, MyObject>()
                                          .Execute(new GetLatestObjectByIdOp<string, MyObject>(firstObject.Id));
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Get: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Key={firstIdObject.Id}, Field={firstIdObject.Field}"));
                firstIdObject.Id.MustForTest().BeEqualTo(firstObject.Id);
            }

            var stop = DateTime.UtcNow;

            var pruneDate = start.AddMilliseconds((stop - start).TotalMilliseconds / 2);
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordDateOp(pruneDate, "Pruning by date.", _)));
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordIdOp(15, "Pruning by id.", _)));

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void PruneOnInsertTest()
        {
            var streamName = "FS_PruneOnInsertTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Throw));

            var key = Guid.NewGuid().ToString().ToUpperInvariant();
            var allRecords = stream.Execute(new GetAllRecordsByIdOp(key));
            allRecords.MustForTest().BeEmptyEnumerable();

            var itemCount = 10;
            for (var idx = 0;
                idx < itemCount;
                idx++)
            {
                stream.PutWithId(key, A.Dummy<string>());
            }

            var serializedKey = "\"" + key + "\"";
            allRecords = stream.Execute(new GetAllRecordsByIdOp(serializedKey));
            allRecords.MustForTest().HaveCount(itemCount);

            var retentionCount = 5;
            stream.PutWithId(key, A.Dummy<string>(), recordRetentionCount: retentionCount, existingRecordEncounteredStrategy: ExistingRecordEncounteredStrategy.PruneIfFoundById);

            allRecords = stream.Execute(new GetAllRecordsByIdOp(serializedKey));
            allRecords.MustForTest().HaveCount(retentionCount);

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification =  NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [Fact]
        public void Create_Put_Handle_Delete___Given_valid_data___Should_roundtrip_to_file_system()
        {
            var streamName = "FS_HandlingTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.GetStreamManagementProtocols().Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Skip));
            var start = DateTime.UtcNow;
            for (int idx = 0;
                idx < 10;
                idx++)
            {
                var key = Invariant($"{stream.Name}Key{idx}");

                var firstValue = "Testing again.";
                var firstObject = new MyObject(key, firstValue);
                var firstConcern = "CanceledPickedBackUpScenario";
                var firstTags = new Dictionary<string, string>()
                                {
                                    { "Run", Guid.NewGuid().ToString().ToUpper(CultureInfo.InvariantCulture) },
                                };

                stream.PutWithId(firstObject.Id, firstObject, firstObject.Tags);
                var first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.MustForTest().NotBeNull();
                var getFirstStatusByIdOp = new GetHandlingStatusOfRecordSetByTagOp(
                    firstConcern,
                    firstTags);

                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                var firstInternalRecordId = first.RecordToHandle.InternalRecordId;
                stream.Execute(
                    new CancelRunningHandleRecordExecutionOp(
                        firstInternalRecordId,
                        firstConcern,
                        "Resources unavailable; node out of disk space.",
                        tags: firstTags));
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.CanceledRunning);

                stream.Execute(new BlockRecordHandlingOp("Stop processing, fixing resource issue."));
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Blocked);
                first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.RecordToHandle.MustForTest().BeNull();
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Blocked);

                stream.Execute(new CancelBlockedRecordHandlingOp("Resume processing, fixed resource issue."));
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.CanceledRunning);

                first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.RecordToHandle.MustForTest().NotBeNull();
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new SelfCancelRunningHandleRecordExecutionOp(
                        firstInternalRecordId,
                        firstConcern,
                        "Processing not finished, check later.",
                        tags: firstTags));
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.SelfCanceledRunning);
                first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.RecordToHandle.MustForTest().NotBeNull();
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new CompleteRunningHandleRecordExecutionOp(
                        firstInternalRecordId,
                        firstConcern,
                        "Processing not finished, check later.",
                        tags: firstTags));
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Completed);
                first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.RecordToHandle.MustForTest().BeNull();
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Completed);

                var firstHistory = stream.Execute(new GetHandlingHistoryOfRecordOp(firstInternalRecordId, firstConcern));
                firstHistory.MustForTest().HaveCount(7);
                foreach (var history in firstHistory)
                {
                    this.testOutputHelper.WriteLine(
                        Invariant(
                            $"{history.Metadata.Concern}: {history.InternalHandlingEntryId}:{history.Metadata.InternalRecordId} - {history.Metadata.Status} - {history.Payload.DeserializePayloadUsingSpecificFactory<IHaveDetails>(stream.SerializerFactory).Details ?? "<no details specified>"}"));
                }

                var secondConcern = "FailedRetriedScenario";
                var second = stream.Execute(new TryHandleRecordOp(secondConcern));
                second.MustForTest().NotBeNull();
                var secondInternalRecordId = second.RecordToHandle.InternalRecordId;
                var getSecondStatusByIdOp = new GetHandlingStatusOfRecordsByIdOp(
                    secondConcern,
                    new[]
                    {
                        new StringSerializedIdentifier(second.RecordToHandle.Metadata.StringSerializedId, second.RecordToHandle.Metadata.TypeRepresentationOfId.WithVersion),
                    });

                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new FailRunningHandleRecordExecutionOp(
                        secondInternalRecordId,
                        secondConcern,
                        "NullReferenceException: Bot v1.0.1 doesn't work."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Failed);
                second = stream.Execute(new TryHandleRecordOp(secondConcern));
                second.RecordToHandle.MustForTest().BeNull();
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Failed);

                stream.Execute(
                    new RetryFailedHandleRecordExecutionOp(secondInternalRecordId, secondConcern, "Redeployed Bot v1.0.1-hotfix, re-run."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.RetryFailed);

                stream.Execute(new BlockRecordHandlingOp("Stop processing, need to confirm deployment."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Blocked);
                second = stream.Execute(new TryHandleRecordOp(secondConcern));
                second.RecordToHandle.MustForTest().BeNull();
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Blocked);

                stream.Execute(new CancelBlockedRecordHandlingOp("Resume processing, confirmed deployment."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.RetryFailed);

                second = stream.Execute(new TryHandleRecordOp(secondConcern));
                second.RecordToHandle.MustForTest().NotBeNull();
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new FailRunningHandleRecordExecutionOp(
                        secondInternalRecordId,
                        secondConcern,
                        "NullReferenceException: Bot v1.0.1-hotfix doesn't work."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Failed);

                stream.Execute(new CancelHandleRecordExecutionRequestOp(firstInternalRecordId, secondConcern, "Giving up."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Canceled);
                second = stream.Execute(new TryHandleRecordOp(secondConcern));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Canceled);
                second.RecordToHandle.MustForTest().BeNull();

                var secondHistory = stream.Execute(new GetHandlingHistoryOfRecordOp(secondInternalRecordId, secondConcern));
                secondHistory.MustForTest().HaveCount(7);

                foreach (var history in secondHistory)
                {
                    this.testOutputHelper.WriteLine(
                        Invariant(
                            $"{history.Metadata.Concern}: {history.InternalHandlingEntryId}:{history.Metadata.InternalRecordId} - {history.Metadata.Status} - {history.Payload.DeserializePayloadUsingSpecificFactory<IHaveDetails>(stream.SerializerFactory).Details ?? "<no details specified>"}"));
                }

                var blockingHistory = stream.Execute(new GetHandlingHistoryOfRecordOp(0, Concerns.RecordHandlingConcern));

                foreach (var history in blockingHistory)
                {
                    this.testOutputHelper.WriteLine(
                        Invariant(
                            $"{history.Metadata.Concern}: {history.InternalHandlingEntryId}:{history.Metadata.InternalRecordId} - {history.Metadata.Status} - {history.Payload.DeserializePayloadUsingSpecificFactory<IHaveDetails>(stream.SerializerFactory).Details ?? "<no details specified>"}"));
                }
            }

            var stop = DateTime.UtcNow;

            var allLocators = stream.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp()).ToList();
            var pruneDate = start.AddMilliseconds((stop - start).TotalMilliseconds / 2);
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordDateOp(pruneDate, "Pruning by date.", _)));
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordIdOp(7, "Pruning by id.", _)));

            stream.GetStreamManagementProtocols()
                  .Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void DoesNotExistTest()
        {
            var streamName = "FS_DoesNotExistTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Throw));

            var existsFirst = stream.DoesAnyExistById(1L);
            existsFirst.MustForTest().NotBeTrue();

            stream.PutWithId(1L, Guid.NewGuid());

            var existsSecond = stream.DoesAnyExistById(1L, typeof(string).ToRepresentation());
            existsSecond.MustForTest().NotBeTrue();

            var existsThird = stream.DoesAnyExistById(1L);
            existsThird.MustForTest().BeTrue();

            var existsFourth = stream.DoesAnyExistById(1L, typeof(Guid).ToRepresentation());
            existsFourth.MustForTest().BeTrue();

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void GetLatestRecordMetadataByIdTest()
        {
            var streamName = "FS_GetLatestRecordMetadataByIdTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Throw));

            var existsFirst = stream.GetLatestRecordMetadataById(1L);
            existsFirst.MustForTest().BeNull();

            stream.PutWithId(1L, Guid.NewGuid());

            var existsSecond = stream.GetLatestRecordMetadataById(1L, typeof(string).ToRepresentation());
            existsSecond.MustForTest().BeNull();

            var existsThird = stream.GetLatestRecordMetadataById(1L);
            existsThird.MustForTest().NotBeNull();

            var existsFourth = stream.GetLatestRecordMetadataById(1L, typeof(Guid).ToRepresentation());
            existsFourth.MustForTest().NotBeNull();

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void PutAndGetLatestRecordByInternalRecordIdTest()
        {
            var streamName = "FS_PutAndGetLatestRecordByInternalRecordIdTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Throw));

            var internalRecordId = 1L;
            var objectId = Guid.NewGuid();
            var objectPayload = A.Dummy<string>();

            var serializer = stream.SerializerFactory.BuildSerializer(stream.DefaultSerializerRepresentation);
            string serializedStringId = serializer.SerializeToString(objectId);

            var identifierTypeRep = objectId.GetType().ToRepresentation();
            var objectTypeRep = objectPayload.GetType().ToRepresentation();

            var payload = objectPayload.ToDescribedSerializationUsingSpecificFactory(
                stream.DefaultSerializerRepresentation,
                stream.SerializerFactory,
                stream.DefaultSerializationFormat);

            var metadata = new StreamRecordMetadata(
                serializedStringId,
                stream.DefaultSerializerRepresentation,
                identifierTypeRep.ToWithAndWithoutVersion(),
                objectTypeRep.ToWithAndWithoutVersion(),
                null,
                DateTime.UtcNow,
                null);

            var putRecordOp = new PutRecordOp(
                metadata,
                payload,
                null,
                ExistingRecordEncounteredStrategy.None,
                null,
                internalRecordId);

            var existsFirst = stream.GetLatestRecordMetadataById(objectId);
            existsFirst.MustForTest().BeNull();

            stream.Execute(putRecordOp);
            var exception = Record.Exception(() => stream.Execute(putRecordOp));
            exception.MustForTest().NotBeNull().And().BeOfType<InvalidOperationException>();
            exception.Message.MustForTest().BeEqualTo("Operation specified an InternalRecordId of 1 but that InternalRecordId is already present in the stream.");

            var foundRecord = stream.Execute(new GetRecordByInternalRecordIdOp(internalRecordId));
            foundRecord.MustForTest().NotBeNull();
            foundRecord.Metadata.MustForTest().BeEqualTo(metadata);
            foundRecord.Payload.MustForTest().BeEqualTo(payload);

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void NullIdentifierAndValueTest()
        {
            var streamName = "FS_NullIdentifierAndValueTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Throw));

            stream.PutWithId((string)null, (MyObject)null);
            var result = stream.GetLatestObjectById<string, MyObject>(null);
            result.MustForTest().BeNull();

            var concern = "NullIdentifierAndValueTest";
            var record = stream.Execute(new TryHandleRecordOp(concern));
            record?.RecordToHandle.MustForTest().NotBeNull();
            ((StringDescribedSerialization)record?.RecordToHandle.Payload)?.SerializedPayload.MustForTest().BeEqualTo("null");

            stream.Execute(new CompleteRunningHandleRecordExecutionOp(record.RecordToHandle.InternalRecordId, concern));

            var recordAgain = stream.Execute(new TryHandleRecordOp(concern));
            recordAgain.RecordToHandle.MustForTest().BeNull();

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void ExistingRecordEncounteredStrategyTest()
        {
            var streamName = "FS_ExistingRecordEncounteredStrategyTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Throw));

            var dummyOne = A.Dummy<MyObject>();
            var dummyTwo = A.Dummy<MyObject>();

            stream.PutWithId(1L, dummyOne);

            var exceptionById = Record.Exception(() => stream.PutWithId(1L, "otherType", null, ExistingRecordEncounteredStrategy.ThrowIfFoundById));
            exceptionById.MustForTest().NotBeNull();
            exceptionById.MustForTest().BeOfType<InvalidOperationException>();
            exceptionById?.ToString().MustForTest().ContainString(nameof(ExistingRecordEncounteredStrategy.ThrowIfFoundById));
            stream.PutWithId(1L, "otherType"); // should not throw

            var exceptionByIdAndType = Record.Exception(() => stream.PutWithId(1L, dummyTwo, null, ExistingRecordEncounteredStrategy.ThrowIfFoundByIdAndType));
            exceptionByIdAndType.MustForTest().NotBeNull();
            exceptionByIdAndType.MustForTest().BeOfType<InvalidOperationException>();
            exceptionByIdAndType?.ToString().MustForTest().ContainString(nameof(ExistingRecordEncounteredStrategy.ThrowIfFoundByIdAndType));
            stream.PutWithId(1L, dummyTwo); // should not throw

            var exceptionByIdAndTypeAndContent = Record.Exception(() => stream.PutWithId(1L, dummyTwo, null, ExistingRecordEncounteredStrategy.ThrowIfFoundByIdAndTypeAndContent));
            exceptionByIdAndTypeAndContent.MustForTest().NotBeNull();
            exceptionByIdAndTypeAndContent.MustForTest().BeOfType<InvalidOperationException>();
            exceptionByIdAndTypeAndContent?.ToString().MustForTest().ContainString(nameof(ExistingRecordEncounteredStrategy.ThrowIfFoundByIdAndTypeAndContent));
            stream.PutWithId(1L, dummyTwo); // should not throw

            stream.PutWithId(2L, "hello");
            stream.PutWithId(2L, dummyTwo, existingRecordEncounteredStrategy: ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById);
            stream.PutWithId(2L, "other", existingRecordEncounteredStrategy: ExistingRecordEncounteredStrategy.DoNotWriteIfFoundByIdAndType);
            stream.PutWithId(2L, "hello", existingRecordEncounteredStrategy: ExistingRecordEncounteredStrategy.DoNotWriteIfFoundByIdAndTypeAndContent);
            var indexTwoRecords = stream.GetAllRecordsById(2L);
            indexTwoRecords.MustForTest().HaveCount(1);

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void GetAllRecordsAndMetadataByIdTest()
        {
            var streamName = "FS_GetAllRecordsAndMetadataByIdTest";

            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Throw));

            var count = 5;

            var allRecordsFirst = stream.GetAllRecordsById(1L);
            allRecordsFirst.MustForTest().BeEmptyEnumerable();
            var allRecordsMetadataFirst = stream.GetAllRecordsMetadataById(1L);
            allRecordsMetadataFirst.MustForTest().BeEmptyEnumerable();

            for (int idx = 0;
                idx < count;
                idx++)
            {
                stream.PutWithId(1L, idx);
            }

            var allRecordsWrongId = stream.GetAllRecordsById(2L);
            allRecordsWrongId.MustForTest().BeEmptyEnumerable();
            var allRecordsMetadataWrongId = stream.GetAllRecordsMetadataById(2L);
            allRecordsMetadataWrongId.MustForTest().BeEmptyEnumerable();

            var allRecords = stream.GetAllRecordsById(1L);
            allRecords.MustForTest().NotBeEmptyEnumerable();
            var allRecordsMetadata = stream.GetAllRecordsMetadataById(1L);
            allRecordsMetadata.MustForTest().NotBeEmptyEnumerable();

            for (int idx = 0;
                idx < count;
                idx++)
            {
                ((StringDescribedSerialization)allRecords[idx].Payload).SerializedPayload.MustForTest().BeEqualTo(idx.ToString(CultureInfo.InvariantCulture));
                allRecordsMetadata[idx].MustForTest().BeEqualTo(allRecords[idx].Metadata);
            }

            var allRecordsReverse = stream.GetAllRecordsById(1L, orderRecordsStrategy: OrderRecordsStrategy.ByInternalRecordIdDescending);
            allRecordsReverse.MustForTest().NotBeEmptyEnumerable();
            var allRecordsMetadataReverse = stream.GetAllRecordsMetadataById(1L, orderRecordsStrategy: OrderRecordsStrategy.ByInternalRecordIdDescending);
            allRecordsMetadataReverse.MustForTest().NotBeEmptyEnumerable();

            for (int idx = 0;
                idx < count;
                idx++)
            {
                ((StringDescribedSerialization)allRecordsReverse[idx].Payload).SerializedPayload.MustForTest().BeEqualTo((count - 1 - idx).ToString(CultureInfo.InvariantCulture));
                allRecordsMetadataReverse[idx].MustForTest().BeEqualTo(allRecordsReverse[idx].Metadata);
            }

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void TagsCanBeNullTest()
        {
            var streamName = "FS_TagsCanBeNullTest";
            var testingFilePath = Path.Combine(Path.GetTempPath(), "Naos");
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Throw));

            var id = A.Dummy<string>();
            var putOpOne = new PutAndReturnInternalRecordIdOp<string>(A.Dummy<string>());
            var internalRecordIdOne = stream.GetStreamWritingProtocols<string>().Execute(putOpOne);
            var latestOne = stream.Execute(new GetLatestRecordOp());
            latestOne.InternalRecordId.MustForTest().BeEqualTo((long)internalRecordIdOne);
            latestOne.Metadata.Tags.MustForTest().BeNull();

            var putOpTwo = new PutWithIdAndReturnInternalRecordIdOp<string, string>(id, A.Dummy<string>());
            var internalRecordIdTwo = stream.GetStreamWritingWithIdProtocols<string, string>().Execute(putOpTwo);
            var latestTwo = stream.Execute(new GetLatestRecordByIdOp("\"" + id + "\""));
            latestTwo.InternalRecordId.MustForTest().BeEqualTo((long)internalRecordIdTwo);
            latestTwo.Metadata.Tags.MustForTest().BeNull();
        }
    }

    public class MyObject : IIdentifiableBy<string>, IHaveTags
    {
        public MyObject(
            string id,
            string field)
        {
            this.Id = id;
            this.Field = field;
        }

        public string Id { get; private set; }

        public string Field { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags => new Dictionary<string, string>();
    }
}