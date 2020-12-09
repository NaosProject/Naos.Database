// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test.MemoryStream
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Naos.Database.Domain;
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
    /// Tests for <see cref="MemoryReadWriteStream"/>.
    /// </summary>
    public partial class MemoryStreamTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public MemoryStreamTests(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Create_Put_Get_Delete___Given_valid_data___Should_roundtrip_to_through_memory()
        {
            var streamName = "MemoryStreamName";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();
            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(SerializationKind.Json, configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var resourceLocatorForUniqueIdentifier = new MemoryDatabaseLocator("UniqueIdentifiers");
            var resourceLocatorZero = new MemoryDatabaseLocator("Zero");
            var resourceLocatorOne = new MemoryDatabaseLocator("One");
            var resourceLocatorTwo = new MemoryDatabaseLocator("Two");
            var resourceLocatorThree = new MemoryDatabaseLocator("Three");

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

            var stream = new MemoryReadWriteStream(
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
                var firstIdObject = stream.GetStreamReadingWithIdProtocols<string, MyObject>().Execute(new GetLatestObjectByIdOp<string, MyObject>(firstObject.Id));
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Get: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Key={firstIdObject.Id}, Field={firstIdObject.Field}"));
                firstIdObject.Id.MustForTest().BeEqualTo(firstObject.Id);
            }

            var stop = DateTime.UtcNow;

            var pruneDate = start.AddMilliseconds((stop - start).TotalMilliseconds / 2);
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordDateOp(pruneDate, "Pruning by date.", _)));
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordIdOp(25, "Pruning by id.", _)));

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public void Create_Put_Handle___Given_valid_data___Should_roundtrip_to_through_memory()
        {
            var streamName = "MemoryStreamHandlingName";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;

            var stream = new MemoryReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory());

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

                stream.GetStreamWritingWithIdProtocols<string, MyObject>()
                      .Execute(new PutWithIdOp<string, MyObject>(firstObject.Id, firstObject, firstObject.Tags));
                var first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.MustForTest().NotBeNull();
                var getFirstStatusByIdOp = new GetHandlingStatusOfRecordSetByTagOp(
                    firstConcern,
                    firstTags);

                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                var firstInternalRecordId = first.InternalRecordId;
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
                first.MustForTest().BeNull();
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Blocked);

                stream.Execute(new CancelBlockedRecordHandlingOp("Resume processing, fixed resource issue."));
                first.MustForTest().BeNull();
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.CanceledRunning);

                first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.MustForTest().NotBeNull();
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new SelfCancelRunningHandleRecordExecutionOp(
                        firstInternalRecordId,
                        firstConcern,
                        "Processing not finished, check later.",
                        tags: firstTags));
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.SelfCanceledRunning);
                first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.MustForTest().NotBeNull();
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new CompleteRunningHandleRecordExecutionOp(
                        firstInternalRecordId,
                        firstConcern,
                        "Processing not finished, check later.",
                        tags: firstTags));
                stream.Execute(getFirstStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Completed);
                first = stream.Execute(new TryHandleRecordOp(firstConcern, tags: firstTags));
                first.MustForTest().BeNull();
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
                var secondInternalRecordId = second.InternalRecordId;
                var getSecondStatusByIdOp = new GetHandlingStatusOfRecordsByIdOp(
                    secondConcern,
                    new[]
                    {
                        new StringSerializedIdentifier(second.Metadata.StringSerializedId, second.Metadata.TypeRepresentationOfId.WithVersion),
                    });

                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new FailRunningHandleRecordExecutionOp(
                        secondInternalRecordId,
                        secondConcern,
                        "NullReferenceException: Bot v1.0.1 doesn't work."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Failed);
                second = stream.Execute(new TryHandleRecordOp(secondConcern));
                second.MustForTest().BeNull();
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Failed);

                stream.Execute(
                    new RetryFailedHandleRecordExecutionOp(secondInternalRecordId, secondConcern, "Redeployed Bot v1.0.1-hotfix, re-run."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.RetryFailed);

                stream.Execute(new BlockRecordHandlingOp("Stop processing, need to confirm deployment."));
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Blocked);
                second = stream.Execute(new TryHandleRecordOp(secondConcern));
                second.MustForTest().BeNull();
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.Blocked);

                stream.Execute(new CancelBlockedRecordHandlingOp("Resume processing, confirmed deployment."));
                second.MustForTest().BeNull();
                stream.Execute(getSecondStatusByIdOp).MustForTest().BeEqualTo(HandlingStatus.RetryFailed);

                second = stream.Execute(new TryHandleRecordOp(secondConcern));
                second.MustForTest().NotBeNull();
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
                second.MustForTest().BeNull();

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

            stream.GetStreamManagementProtocols().Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
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
