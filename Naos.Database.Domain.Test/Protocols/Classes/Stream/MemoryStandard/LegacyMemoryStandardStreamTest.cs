﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegacyMemoryStandardStreamTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test.MemoryStream
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using FakeItEasy;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Database.Serialization.Json;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using Xunit;
    using Xunit.Abstractions;
    using static System.FormattableString;

    public class LegacyMemoryStandardStreamTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public LegacyMemoryStandardStreamTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GetDistinctStringSerializedIdsRecordOp___Various_usages___Should_function()
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

            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                locatorProtocols);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Skip));
            var zeroObject = new MyObject(null, "Null Id");
            var firstObject = new MyObject("RecordOne", "One Id");
            var secondObject = new MyObject("RecordTwo", "Two Id");
            var thirdObjectId = "Three Id";
            var thirdObject = "RecordThree";

            for (int idx = 0;
                idx < 10;
                idx++)
            {
                var timestampUtc = DateTime.UtcNow;
                stream.Execute(
                    new StandardPutRecordOp(
                        new StreamRecordMetadata(
                            zeroObject.Id,
                            stream.DefaultSerializerRepresentation,
                            typeof(decimal?).ToRepresentation().ToWithAndWithoutVersion(),
                            zeroObject.GetType().ToRepresentation().ToWithAndWithoutVersion(),
                            new List<NamedValue<string>>
                            {
                                new NamedValue<string>("tag", "zero"),
                            },
                            timestampUtc,
                            null),
                        zeroObject.ToDescribedSerializationUsingSpecificFactory(
                            stream.DefaultSerializerRepresentation,
                            SerializerRepresentationSelectionStrategy.UseSpecifiedRepresentation,
                            stream.SerializerFactory,
                            SerializationFormat.String)
                            .ToStreamRecordPayload(),
                        specifiedResourceLocator: resourceLocatorZero));
                stream.PutWithId(
                    firstObject.Id,
                    firstObject,
                    new List<NamedValue<string>>
                    {
                        new NamedValue<string>("tag", "one"),
                    });
                stream.PutWithId(
                    secondObject.Id,
                    secondObject,
                    new List<NamedValue<string>>
                    {
                        new NamedValue<string>("tag", "two"),
                    });
                stream.PutWithId(
                    thirdObjectId,
                    thirdObject,
                    new List<NamedValue<string>>
                    {
                        new NamedValue<string>("tag", "third"),
                    });
                var firstIdObject = stream.GetLatestObjectById<string, MyObject>(firstObject.Id);
                this.testOutputHelper.WriteLine(Invariant($"Key={firstIdObject.Id}, Field={firstIdObject.Field}"));
                firstIdObject.Id.MustForTest().BeEqualTo(firstObject.Id);
            }

            var anyDistinct = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(new RecordFilter()));
            anyDistinct.Select(_ => _.StringSerializedId).ToList().MustForTest()
                       .BeEqualTo(
                            new List<string>
                            {
                                zeroObject.Id,
                                firstObject.Id,
                                secondObject.Id,
                                thirdObjectId,
                            });

            var objectObjectDistinct = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(
                    new RecordFilter(
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     })));

            objectObjectDistinct.Select(_ => _.StringSerializedId).ToList().MustForTest()
                       .BeEqualTo(
                            new List<string>
                            {
                                zeroObject.Id,
                                firstObject.Id,
                                secondObject.Id,
                            });

            var stringIdDistinct = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 })));

            stringIdDistinct.Select(_ => _.StringSerializedId).ToList().MustForTest()
                            .BeEqualTo(
                                 new List<string>
                                 {
                                     firstObject.Id,
                                     secondObject.Id,
                                     thirdObjectId,
                                 });

            var stringIdObjectObjectDistinct = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 },
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     })));
            stringIdObjectObjectDistinct.Select(_ => _.StringSerializedId).ToList().MustForTest()
                            .BeEqualTo(
                                 new List<string>
                                 {
                                     firstObject.Id,
                                     secondObject.Id,
                                 });

            var tagDistinct = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(
                    new RecordFilter(
                        tags: new List<NamedValue<string>>
                              {
                                  new NamedValue<string>("tag", "one"),
                              })));

            tagDistinct.Select(_ => _.StringSerializedId).ToList().MustForTest()
                            .BeEqualTo(
                                 new List<string>
                                 {
                                     firstObject.Id,
                                 });

            var tagDistinctWrongIdType = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(short?).ToRepresentation(),
                                 },
                        versionMatchStrategy: VersionMatchStrategy.Any,
                        tags: new List<NamedValue<string>>
                              {
                                  new NamedValue<string>("tag", "one"),
                              })));

            tagDistinctWrongIdType.Select(_ => _.StringSerializedId).ToList()
                              .MustForTest()
                              .BeEmptyEnumerable();

            var tagDistinctWrongObjectType = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(
                    new RecordFilter(
                        objectTypes: new[]
                                     {
                                         typeof(short).ToRepresentation(),
                                     },
                        versionMatchStrategy: VersionMatchStrategy.Any,
                        tags: new List<NamedValue<string>>
                              {
                                  new NamedValue<string>("tag", "one"),
                              })));

            tagDistinctWrongObjectType.Select(_ => _.StringSerializedId).ToList()
                              .MustForTest()
                              .BeEmptyEnumerable();

            var tagDistinctWrongTagValue = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(
                    new RecordFilter(
                        tags: new List<NamedValue<string>>
                              {
                                  new NamedValue<string>("tag", "monkey"),
                              })));

            tagDistinctWrongTagValue.Select(_ => _.StringSerializedId).ToList()
                                    .MustForTest()
                                    .BeEmptyEnumerable();

            var tagDistinctWrongTagName = stream.Execute(
                new StandardGetDistinctStringSerializedIdsOp(
                    new RecordFilter(
                        tags:
                        new List<NamedValue<string>>
                        {
                            new NamedValue<string>("monkey", "one"),
                        })));

            tagDistinctWrongTagName.Select(_ => _.StringSerializedId).ToList()
                                    .MustForTest()
                                    .BeEmptyEnumerable();

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
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

            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                locatorProtocols);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Skip));
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
                stream.PutWithId(zeroObject.Id, zeroObject, typeSelectionStrategy: TypeSelectionStrategy.UseDeclaredType);
                stopwatch.Stop();
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                stopwatch.Reset();
                stopwatch.Start();
                stream.PutWithId(firstObject.Id, firstObject);
                stopwatch.Stop();
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                stopwatch.Reset();
                stopwatch.Start();
                stream.PutWithId(secondObject.Id, secondObject);
                stopwatch.Stop();
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                stopwatch.Reset();
                stopwatch.Start();
                stream.PutWithId(thirdObject.Id, thirdObject);
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
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordDateOp(pruneDate, "Pruning by date.").Standardize(_)));
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordIdOp(25, "Pruning by id.").Standardize(_)));

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
        }

        /// <summary>
        /// Defines the test method HandlingTests.
        /// </summary>
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = NaosSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [Fact]
        public void HandlingTests()
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
                                  //resourceLocatorOne,
                                  //resourceLocatorTwo,
                                  //resourceLocatorThree,
                              }.ToList();

            var locatorProtocols = new PassThroughResourceLocatorProtocols<string>(
                allLocators,
                resourceLocatorForUniqueIdentifier,
                ResourceLocatorByIdProtocol);

            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                locatorProtocols);
            var x = stream.GetNextUniqueLong();
            x.MustForTest().BeGreaterThan(0L);

            var start = DateTime.UtcNow;
            for (int idx = 0;
                idx < 10;
                idx++)
            {
                var key = Invariant($"{stream.Name}Key{idx}");

                var firstValue = "Testing again.";
                var firstObject = new MyObject(key, firstValue);
                var firstConcern = "CanceledPickedBackUpScenario";
                var firstTags = new List<NamedValue<string>>
                                {
                                    new NamedValue<string>("Record", Guid.NewGuid().ToString().ToUpper(CultureInfo.InvariantCulture)),
                                };

                var handleTags = new List<NamedValue<string>>
                                {
                                    new NamedValue<string>("Handle", Guid.NewGuid().ToString().ToUpper(CultureInfo.InvariantCulture)),
                                };

                var firstRecordAndHandleTags = firstTags.Concat(handleTags).ToList();

                stream.PutWithId(firstObject.Id, firstObject, firstTags);
                var metadata = stream.GetLatestRecordMetadataById("monkeyAintThere");
                metadata.MustForTest().BeNull();

                var handlingStatusAvailableByDefault = stream.Execute(
                    new StandardGetHandlingStatusOp(
                        firstConcern,
                        new RecordFilter(tags: firstTags),
                        new HandlingFilter(
                            new[]
                            {
                                HandlingStatus.AvailableByDefault,
                            })));
                handlingStatusAvailableByDefault.MustForTest().NotBeEmptyDictionary();
                handlingStatusAvailableByDefault.Single().Value.MustForTest().BeEqualTo(HandlingStatus.AvailableByDefault);
                var first = stream.Execute(
                    new StandardTryHandleRecordOp(
                        firstConcern,
                        new RecordFilter(
                            idTypes: new[]
                                     {
                                         typeof(string).ToRepresentation(),
                                     },
                            objectTypes: new[]
                                         {
                                             typeof(MyObject).ToRepresentation(),
                                         }),
                        inheritRecordTags: true,
                        tags: handleTags));
                first.RecordToHandle.MustForTest().NotBeNull();

                var getFirstStatusByTagsOp = new StandardGetHandlingStatusOp(
                    firstConcern,
                    new RecordFilter(tags: firstTags),
                    new HandlingFilter());

                stream.Execute(getFirstStatusByTagsOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Running);

                var firstInternalRecordId = first.RecordToHandle.InternalRecordId;
                stream.Execute(
                    new CancelRunningHandleRecordOp(
                        firstInternalRecordId,
                        firstConcern,
                        "Resources unavailable; node out of disk space.",
                        tags: firstRecordAndHandleTags).Standardize());
                stream.Execute(getFirstStatusByTagsOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.AvailableAfterExternalCancellation);

                stream.Execute(new DisableHandlingForStreamOp("Stop processing, fixing resource issue.").Standardize());
                first = stream.Execute(new StandardTryHandleRecordOp(
                    firstConcern,
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 },
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     }),
                    inheritRecordTags: true,
                    tags: firstTags));
                first.RecordToHandle.MustForTest().BeNull();
                first.IsBlocked.MustForTest().BeTrue();
                var runningStatuses = stream.Execute(
                    new StandardGetHandlingStatusOp(
                        firstConcern,
                        getFirstStatusByTagsOp.RecordFilter,
                        new HandlingFilter(
                            new[]
                            {
                                HandlingStatus.Running,
                            })));
                runningStatuses.MustForTest().BeEmptyDictionary();

                stream.Execute(new EnableHandlingForStreamOp("Resume processing, fixed resource issue.").Standardize());
                stream.Execute(getFirstStatusByTagsOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.AvailableAfterExternalCancellation);

                first = stream.Execute(new StandardTryHandleRecordOp(
                    firstConcern,
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 },
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     }),
                    inheritRecordTags: true,
                    tags: handleTags));
                first.RecordToHandle.MustForTest().NotBeNull();
                stream.Execute(getFirstStatusByTagsOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new SelfCancelRunningHandleRecordOp(
                        firstInternalRecordId,
                        firstConcern,
                        "Processing not finished, check later.",
                        tags: firstRecordAndHandleTags).Standardize());
                stream.Execute(getFirstStatusByTagsOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.AvailableAfterSelfCancellation);
                first = stream.Execute(new StandardTryHandleRecordOp(
                    firstConcern,
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 },
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     }),
                    inheritRecordTags: true,
                    tags: handleTags));
                first.RecordToHandle.MustForTest().NotBeNull();
                stream.Execute(getFirstStatusByTagsOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new CompleteRunningHandleRecordOp(
                        firstInternalRecordId,
                        firstConcern,
                        "Processing not finished, check later.",
                        tags: firstRecordAndHandleTags).Standardize());
                stream.Execute(getFirstStatusByTagsOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Completed);
                first = stream.Execute(new StandardTryHandleRecordOp(
                    firstConcern,
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 },
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     }),
                    inheritRecordTags: true,
                    tags: handleTags));
                first.RecordToHandle.MustForTest().BeNull();
                stream.Execute(getFirstStatusByTagsOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Completed);

                var firstHistory = stream.Execute(new StandardGetHandlingHistoryOp(firstInternalRecordId, firstConcern));
                firstHistory.MustForTest().HaveCount(7);
                foreach (var history in firstHistory)
                {
                    this.testOutputHelper.WriteLine(
                        Invariant(
                            $"{history.Concern}: {history.InternalHandlingEntryId}:{history.InternalRecordId} - {history.Status} - {history.Details ?? "<no details specified>"}"));
                }

                var secondConcern = "FailedRetriedScenario";
                var second = stream.Execute(
                    new StandardTryHandleRecordOp(
                        secondConcern,
                        new RecordFilter(
                            idTypes: new[]
                                     {
                                         typeof(string).ToRepresentation(),
                                     },
                            objectTypes: new[]
                                         {
                                             typeof(MyObject).ToRepresentation(),
                                         }),
                        inheritRecordTags: true,
                        tags: handleTags));
                second.RecordToHandle.MustForTest().NotBeNull();
                var secondInternalRecordId = second.RecordToHandle.InternalRecordId;
                var getSecondStatusByIdOp = new StandardGetHandlingStatusOp(
                    secondConcern,
                    new RecordFilter(
                        ids:
                        new[]
                        {
                            new StringSerializedIdentifier(
                                second.RecordToHandle.Metadata.StringSerializedId,
                                second.RecordToHandle.Metadata.TypeRepresentationOfId.WithVersion),
                        }),
                    new HandlingFilter());

                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new FailRunningHandleRecordOp(
                        secondInternalRecordId,
                        secondConcern,
                        "NullReferenceException: Bot v1.0.1 doesn't work.",
                        tags: firstRecordAndHandleTags).Standardize());

                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Failed);
                second = stream.Execute(new StandardTryHandleRecordOp(
                    secondConcern,
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 },
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     }),
                    inheritRecordTags: true,
                    tags: handleTags));
                second.RecordToHandle.MustForTest().BeNull();
                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Failed);

                stream.Execute(
                    new ResetFailedHandleRecordOp(secondInternalRecordId, secondConcern, "Redeployed Bot v1.0.1-hotfix, re-run.", tags: firstRecordAndHandleTags).Standardize());
                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.AvailableAfterFailure);

                stream.GetStreamRecordHandlingProtocols().Execute(new DisableHandlingForStreamOp("Stop processing, need to confirm deployment."));
                second = stream.Execute(
                    new StandardTryHandleRecordOp(
                        secondConcern,
                        new RecordFilter(
                            idTypes: new[]
                                     {
                                         typeof(string).ToRepresentation(),
                                     },
                            objectTypes: new[]
                                         {
                                             typeof(MyObject).ToRepresentation(),
                                         })));
                second.RecordToHandle.MustForTest().BeNull();

                stream.GetStreamRecordHandlingProtocols().Execute(new EnableHandlingForStreamOp("Resume processing, confirmed deployment."));

                var secondConcernEmptyStatusCheck = stream.Execute(new StandardGetHandlingStatusOp(secondConcern, new RecordFilter(), new HandlingFilter(), null));
                secondConcernEmptyStatusCheck.MustForTest().NotBeEmptyDictionary();

                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.AvailableAfterFailure);

                second = stream.Execute(new StandardTryHandleRecordOp(
                    secondConcern,
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 },
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     }),
                    inheritRecordTags: true,
                    tags: handleTags));
                second.RecordToHandle.MustForTest().NotBeNull();
                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Running);

                stream.Execute(
                    new FailRunningHandleRecordOp(
                        secondInternalRecordId,
                        secondConcern,
                        "NullReferenceException: Bot v1.0.1-hotfix doesn't work.",
                        tags: firstRecordAndHandleTags).Standardize());
                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.Failed);

                stream.Execute(
                    new ArchiveFailureToHandleRecordOp(
                        secondInternalRecordId,
                        secondConcern,
                        "NullReferenceException: Bot v1.0.1-hotfix won't work.",
                        tags: firstRecordAndHandleTags).Standardize());
                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.ArchivedAfterFailure);

                stream.Execute(new DisableHandlingForRecordOp(firstInternalRecordId, "Giving up.", tags: firstRecordAndHandleTags).Standardize());

                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.DisabledForRecord);
                second = stream.Execute(new StandardTryHandleRecordOp(
                    secondConcern,
                    new RecordFilter(
                        idTypes: new[]
                                 {
                                     typeof(string).ToRepresentation(),
                                 },
                        objectTypes: new[]
                                     {
                                         typeof(MyObject).ToRepresentation(),
                                     }),
                    inheritRecordTags: true,
                    tags: handleTags));
                stream.Execute(getSecondStatusByIdOp).OrderByDescending(_ => _.Key).First().Value.MustForTest().BeEqualTo(HandlingStatus.DisabledForRecord);
                second.RecordToHandle.MustForTest().BeNull();

                var secondHistory = stream.Execute(new StandardGetHandlingHistoryOp(secondInternalRecordId, secondConcern));
                secondHistory.MustForTest().HaveCount(8);

                foreach (var history in secondHistory)
                {
                    this.testOutputHelper.WriteLine(
                        Invariant(
                            $"{history.Concern}: {history.InternalHandlingEntryId}:{history.InternalRecordId} - {history.Status} - {history.Details ?? "<no details specified>"}"));
                }

                var blockingHistory = stream.Execute(new StandardGetHandlingHistoryOp(0, Concerns.StreamHandlingDisabledConcern));

                foreach (var history in blockingHistory)
                {
                    this.testOutputHelper.WriteLine(
                        Invariant(
                            $"{history.Concern}: {history.InternalHandlingEntryId}:{history.InternalRecordId} - {history.Status} - {history.Details ?? "<no details specified>"}"));
                }
            }

            var stop = DateTime.UtcNow;
            this.testOutputHelper.WriteLine(Invariant($"TotalSeconds: {(stop - start).TotalSeconds}."));

            /*
            var allLocators = stream.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp()).ToList();
            var pruneDate = start.AddMilliseconds((stop - start).TotalMilliseconds / 2);
            allLocators.ForEach(_ => stream.Execute(new StandardPruneStreamOp(pruneDate, "Pruning by date.", _)));
            allLocators.ForEach(_ => stream.Execute(new PruneBeforeInternalRecordIdOp(7, "Pruning by id.", _)));
            */
        }

        [Fact]
        public static void PruneOnInsertTest()
        {
            var streamName = "MS_PruneOnInsertTest";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                createStreamOnConstruction: false);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Throw));

            /*
            var key = Guid.NewGuid().ToString().ToUpperInvariant();
            var allRecords = stream.Execute(new StandardGetAllRecordsByIdOp(key));
            allRecords.MustForTest().BeEmptyEnumerable();

            var itemCount = 10;
            for (var idx = 0;
                idx < itemCount;
                idx++)
            {
                stream.PutWithId(key, A.Dummy<string>());
            }

            var serializedKey = "\"" + key + "\"";
            allRecords = stream.Execute(new StandardGetAllRecordsByIdOp(serializedKey));
            allRecords.MustForTest().HaveCount(itemCount);

            var retentionCount = 5;
            stream.PutWithId(key, A.Dummy<string>(), recordRetentionCount: retentionCount, existingRecordStrategy: ExistingRecordStrategy.PruneIfFoundById);

            allRecords = stream.Execute(new StandardGetAllRecordsByIdOp(serializedKey));
            allRecords.MustForTest().HaveCount(retentionCount);
            */

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
        }

        [Fact]
        public static void TagsCanBeNullTest()
        {
            var streamName = "MS_TagsCanBeNullTest";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                createStreamOnConstruction: false);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Throw));

            var id = A.Dummy<string>();
            var putOpOne = new PutAndReturnInternalRecordIdOp<string>(A.Dummy<string>());
            var internalRecordIdOne = stream.GetStreamWritingProtocols<string>().Execute(putOpOne);
            var latestOne = stream.Execute(
                new StandardGetLatestRecordOp(
                    new RecordFilter(
                        new[]
                        {
                            (long)internalRecordIdOne,
                        })));
            latestOne.InternalRecordId.MustForTest().BeEqualTo((long)internalRecordIdOne);
            latestOne.Metadata.Tags.MustForTest().BeNull();

            var putOpTwo = new PutWithIdAndReturnInternalRecordIdOp<string, string>(id, A.Dummy<string>());
            var internalRecordIdTwo = stream.GetStreamWritingWithIdProtocols<string, string>().Execute(putOpTwo);
            var latestTwo = stream.Execute(
                new StandardGetLatestRecordOp(
                    new RecordFilter(
                        new[]
                        {
                            (long)internalRecordIdTwo,
                        })));
            latestTwo.InternalRecordId.MustForTest().BeEqualTo((long)internalRecordIdTwo);
            latestTwo.Metadata.Tags.MustForTest().BeNull();
        }

        [Fact]
        public static void PutAndGetLatestRecordByInternalRecordIdTest()
        {
            var streamName = "MS_PutAndGetLatestRecordByInternalRecordIdTest";

            var resourceLocatorProtocol = new MemoryDatabaseLocator(streamName).ToResourceLocatorProtocols();

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol,
                createStreamOnConstruction: false);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Throw));

            var internalRecordId = 1L;
            var objectId = Guid.NewGuid();
            var objectPayload = A.Dummy<string>();

            var serializedStringId = stream.IdSerializer.SerializeToString(objectId);

            var identifierTypeRep = objectId.GetType().ToRepresentation();
            var objectTypeRep = objectPayload.GetType().ToRepresentation();

            var payload = objectPayload.ToDescribedSerializationUsingSpecificFactory(
                    stream.DefaultSerializerRepresentation,
                    SerializerRepresentationSelectionStrategy.UseSpecifiedRepresentation,
                    stream.SerializerFactory,
                    stream.DefaultSerializationFormat)
                .ToStreamRecordPayload();

            var metadata = new StreamRecordMetadata(
                serializedStringId,
                stream.DefaultSerializerRepresentation,
                identifierTypeRep.ToWithAndWithoutVersion(),
                objectTypeRep.ToWithAndWithoutVersion(),
                null,
                DateTime.UtcNow,
                null);

            var putRecordOp = new StandardPutRecordOp(
                metadata,
                payload,
                ExistingRecordStrategy.None,
                internalRecordId: internalRecordId);

            var existsFirst = stream.GetLatestRecordMetadataById(objectId);
            existsFirst.MustForTest().BeNull();

            stream.Execute(putRecordOp);
            var exception = Record.Exception(() => stream.Execute(putRecordOp));
            exception.MustForTest().NotBeNull().And().BeOfType<InvalidOperationException>();
            exception.Message.MustForTest().BeEqualTo("Operation specified an InternalRecordId of 1 but that InternalRecordId is already present in the stream.");

            var foundRecord = stream.Execute(
                new StandardGetLatestRecordOp(
                    new RecordFilter(
                        new[]
                        {
                            (long)internalRecordId,
                        })));
            foundRecord.MustForTest().NotBeNull();
            foundRecord.Metadata.MustForTest().BeEqualTo(metadata);
            foundRecord.Payload.MustForTest().BeEqualTo(payload);

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
        }

        [Fact]
        public static void DoesNotExistTest()
        {
            var streamName = "MS_DoesNotExistTest";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                createStreamOnConstruction: false);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Throw));

            var existsFirst = stream.DoesAnyExistById(1L);
            existsFirst.MustForTest().NotBeTrue();

            stream.PutWithId(1L, Guid.NewGuid());

            var existsSecond = stream.DoesAnyExistById(1L, typeof(string).ToRepresentation());
            existsSecond.MustForTest().NotBeTrue();

            var existsThird = stream.DoesAnyExistById(1L);
            existsThird.MustForTest().BeTrue();

            var existsFourth = stream.DoesAnyExistById(1L, typeof(Guid).ToRepresentation());
            existsFourth.MustForTest().BeTrue();

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
        }

        [Fact]
        public static void NullIdentifierAndValueTest()
        {
            var streamName = "MS_NullIdentifierAndValueTest";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                createStreamOnConstruction: false);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Throw));

            stream.PutWithId((string)null, (MyObject)null, typeSelectionStrategy: TypeSelectionStrategy.UseDeclaredType);
            var result = stream.GetLatestObjectById<string, MyObject>(null);
            result.MustForTest().BeNull();

            var concern = "NullIdentifierAndValueTest";
            var record = stream.Execute(new StandardTryHandleRecordOp(concern, new RecordFilter()));
            record?.RecordToHandle.MustForTest().NotBeNull();
            ((StringDescribedSerialization)record?.RecordToHandle.GetDescribedSerialization())?.SerializedPayload.MustForTest().BeEqualTo("null");

            stream.GetStreamRecordHandlingProtocols().Execute(new CompleteRunningHandleRecordOp(record.RecordToHandle.InternalRecordId, concern));

            var recordAgain = stream.Execute(new StandardTryHandleRecordOp(concern, new RecordFilter()));
            recordAgain.RecordToHandle.MustForTest().BeNull();

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
        }

        [Fact]
        public static void ExistingRecordStrategyTest()
        {
            var streamName = "MS_ExistingRecordStrategyTest";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                createStreamOnConstruction: false);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Throw));

            var dummyOne = A.Dummy<MyObject>();
            var dummyTwo = A.Dummy<MyObject>();

            stream.PutWithId(1L, dummyOne);

            var exceptionById = Record.Exception(() => stream.PutWithId(1L, "otherType", null, ExistingRecordStrategy.ThrowIfFoundById));
            exceptionById.MustForTest().NotBeNull();
            exceptionById.MustForTest().BeOfType<InvalidOperationException>();
            exceptionById?.ToString().MustForTest().ContainString(nameof(ExistingRecordStrategy.ThrowIfFoundById));
            stream.PutWithId(1L, "otherType"); // should not throw

            var exceptionByIdAndType = Record.Exception(() => stream.PutWithId(1L, dummyTwo, null, ExistingRecordStrategy.ThrowIfFoundByIdAndType));
            exceptionByIdAndType.MustForTest().NotBeNull();
            exceptionByIdAndType.MustForTest().BeOfType<InvalidOperationException>();
            exceptionByIdAndType?.ToString().MustForTest().ContainString(nameof(ExistingRecordStrategy.ThrowIfFoundByIdAndType));
            stream.PutWithId(1L, dummyTwo); // should not throw

            var exceptionByIdAndTypeAndContent = Record.Exception(() => stream.PutWithId(1L, dummyTwo, null, ExistingRecordStrategy.ThrowIfFoundByIdAndTypeAndContent));
            exceptionByIdAndTypeAndContent.MustForTest().NotBeNull();
            exceptionByIdAndTypeAndContent.MustForTest().BeOfType<InvalidOperationException>();
            exceptionByIdAndTypeAndContent?.ToString().MustForTest().ContainString(nameof(ExistingRecordStrategy.ThrowIfFoundByIdAndTypeAndContent));
            stream.PutWithId(1L, dummyTwo); // should not throw

            stream.PutWithId(2L, "hello");
            stream.PutWithId(2L, dummyTwo, existingRecordStrategy: ExistingRecordStrategy.DoNotWriteIfFoundById);
            stream.PutWithId(2L, "other", existingRecordStrategy: ExistingRecordStrategy.DoNotWriteIfFoundByIdAndType);
            stream.PutWithId(2L, "hello", existingRecordStrategy: ExistingRecordStrategy.DoNotWriteIfFoundByIdAndTypeAndContent);
            var indexTwoRecords = stream.GetAllRecordsById(2L);
            indexTwoRecords.MustForTest().HaveCount(1);

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
        }

        [Fact]
        public static void GetLatestRecordMetadataByIdTest()
        {
            var streamName = "MS_GetLatestRecordMetadataByIdTest";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                createStreamOnConstruction: false);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Throw));

            var existsFirst = stream.GetLatestRecordMetadataById(1L);
            existsFirst.MustForTest().BeNull();

            stream.PutWithId(1L, Guid.NewGuid());

            var existsSecond = stream.GetLatestRecordMetadataById(1L, typeof(string).ToRepresentation());
            existsSecond.MustForTest().BeNull();

            var existsThird = stream.GetLatestRecordMetadataById(1L);
            existsThird.MustForTest().NotBeNull();

            var existsFourth = stream.GetLatestRecordMetadataById(1L, typeof(Guid).ToRepresentation());
            existsFourth.MustForTest().NotBeNull();

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
        }

        [Fact]
        public static void GetAllRecordsAndMetadataByIdTest()
        {
            var streamName = "MS_GetAllRecordsAndMetadataByIdTest";

            var configurationTypeRepresentation =
                typeof(DependencyOnlyJsonSerializationConfiguration<
                    TypesToRegisterJsonSerializationConfiguration<MyObject>,
                    DatabaseJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;
            var stream = new MemoryStandardStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                createStreamOnConstruction: false);

            stream.Execute(new StandardCreateStreamOp(stream.StreamRepresentation, ExistingStreamStrategy.Throw));

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
                ((StringDescribedSerialization)allRecords[idx].GetDescribedSerialization()).SerializedPayload.MustForTest().BeEqualTo(idx.ToString(CultureInfo.InvariantCulture));
                allRecordsMetadata[idx].MustForTest().BeEqualTo(allRecords[idx].Metadata);
            }

            var allRecordsReverse = stream.GetAllRecordsById(1L, orderRecordsBy: OrderRecordsBy.InternalRecordIdDescending);
            allRecordsReverse.MustForTest().NotBeEmptyEnumerable();
            var allRecordsMetadataReverse = stream.GetAllRecordsMetadataById(1L, orderRecordsBy: OrderRecordsBy.InternalRecordIdDescending);
            allRecordsMetadataReverse.MustForTest().NotBeEmptyEnumerable();

            for (int idx = 0;
                idx < count;
                idx++)
            {
                ((StringDescribedSerialization)allRecordsReverse[idx].GetDescribedSerialization()).SerializedPayload.MustForTest().BeEqualTo((count - 1 - idx).ToString(CultureInfo.InvariantCulture));
                allRecordsMetadataReverse[idx].MustForTest().BeEqualTo(allRecordsReverse[idx].Metadata);
            }

            stream.Execute(new StandardDeleteStreamOp(stream.StreamRepresentation, StreamNotFoundStrategy.Throw));
        }

        private class MyObject : IHaveId<string>, IHaveTags
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
            public IReadOnlyCollection<NamedValue<string>> Tags => new List<NamedValue<string>>();
        }
    }
}
