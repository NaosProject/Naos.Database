// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStreamTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test.MemoryStream
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
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

    /// <summary>
    /// README: These tests should be kept in-sync with those in Naos.SqlServer.Protocol.Client.Test.SqlStreamTest.
    /// Any test on a <see cref="MemoryStandardStream"/> should also work for a SQL Stream.
    /// In keeping this suite of tests in-sync we ensure standardized test coverage.
    /// </summary>
    public static class MemoryStandardStreamTest
    {
        [Fact]
        public static void StandardGetInternalRecordIds___Should_return_internal_records_ids_of_not_deprecated_records___When_object_has_been_deprecated_and_then_put_again_with_same_id()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var objectToPut1 = A.Dummy<MyObject>();
            var objectToPut2 = new MyObject(objectToPut1.Id, A.Dummy<string>());

            stream.PutWithId(objectToPut1.Id, objectToPut1);
            stream.PutWithId(objectToPut1.Id, new IdDeprecatedEvent<MyObject>(DateTime.UtcNow));
            stream.PutWithId(objectToPut1.Id, objectToPut2);

            // Act
            var actual = stream.Execute(
                    new StandardGetInternalRecordIdsOp(
                        new RecordFilter(
                            objectTypes: new[] { typeof(MyObject).ToRepresentation() },
                            deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<MyObject>).ToRepresentation() })))
                .ToArray();

            // Assert
            actual.AsTest().Must().BeEqualTo(new long[] { 3 });
        }

        [Fact]
        public static void StandardGetInternalRecordIds___Should_return_ids___When_called()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var item1 = new MyObject("1", "my-obj-1");
            var item2 = new MyObject("2", "my-obj-2");
            var item3 = new MyObject2("1", "my-obj-2");
            var item4 = new MyObject2("2", "my-obj-2");
            var item5 = new MyObject("3", "my-obj-1");
            var item6 = new MyObject2("4", "my-obj-2");

            stream.PutWithId(item1.Id, item1);
            stream.PutWithId(item2.Id, item2);
            stream.PutWithId(item3.Id, item3);
            stream.PutWithId(item4.Id, item4);
            stream.PutWithId(item5.Id, item5);
            stream.PutWithId(item6.Id, item6);

            var operation = new StandardGetInternalRecordIdsOp(new RecordFilter());

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(new long[] { 1, 2, 3, 4, 5, 6 });
        }

        [Fact]
        public static void StandardGetInternalRecordIds___Should_return_empty_collection___When_no_records_found_and_RecordNotFoundStrategy_is_ReturnDefault()
        {
            // Arrange
            var stream = BuildCreatedStream();

            // ReSharper disable once RedundantArgumentDefaultValue - specifically testing this value
            var operation = new StandardGetInternalRecordIdsOp(new RecordFilter(), RecordNotFoundStrategy.ReturnDefault);

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeEmptyEnumerable();
        }

        [Fact]
        public static void StandardGetInternalRecordIds___Should_throw_InvalidOperationException___When_no_records_found_and_RecordNotFoundStrategy_is_Throw()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var operation = new StandardGetInternalRecordIdsOp(new RecordFilter(), RecordNotFoundStrategy.Throw);

            // Act
            var actual = Record.Exception(() => stream.Execute(operation));

            // Assert
            actual.AsTest().Must().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public static void StandardGetInternalRecordIds___Should_not_return_id_of_DeprecatedIdTypes___When_record_filter_specifies_DeprecatedIdsTypes()
        {
            // Arrange
            var stream = BuildCreatedStream();

            stream.PutWithId("id-1", new MyObject(A.Dummy<string>(), A.Dummy<string>()));
            stream.PutWithId("id-2", new IdDeprecatedEvent<MyObject>(DateTime.UtcNow));

            var operation = new StandardGetInternalRecordIdsOp(
                new RecordFilter(
                    ids: new[]
                    {
                        new StringSerializedIdentifier("id-1", typeof(string).ToRepresentation()),
                        new StringSerializedIdentifier("id-2", typeof(string).ToRepresentation()),
                    },
                    deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<MyObject>).ToRepresentation() }));

            var expected = new[] { 1L };

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void StandardGetInternalRecordIds___Should_return_ids_that_survive_filtering___When_RecordsToFilterSelectionStrategy_LatestById_used_with_tag_based_RecordFilter()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var id1 = "id-1";
            var id2 = "id-2";
            var id3 = "id-3";
            var id4 = "id-4";

            var tags = new[] { A.Dummy<NamedValue<string>>() };

            stream.PutWithId(id1, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id2, A.Dummy<NamedResourceLocator>());
            stream.PutWithId(id3, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id4, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id1, A.Dummy<MyObject>());
            stream.PutWithId(id4, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            stream.PutWithId(id2, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow), tags: tags);

            var operation = new StandardGetInternalRecordIdsOp(
                new RecordFilter(
                    tags: tags,
                    deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() }),
                recordsToFilterCriteria: new RecordsToFilterCriteria(RecordsToFilterSelectionStrategy.LatestById));

            var expected = new[] { 3L };

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void StandardGetInternalRecordIds___Should_return_latest_ids_that_survive_filtering___When_RecordsToFilterSelectionStrategy_LatestByIdAndObjectType_used_with_tag_based_RecordFilter()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var id1 = "id-1";
            var id2 = "id-2";
            var id3 = "id-3";
            var id4 = "id-4";

            var tags = new[] { A.Dummy<NamedValue<string>>() };

            stream.PutWithId(id1, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id2, A.Dummy<NamedResourceLocator>());
            stream.PutWithId(id3, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id4, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id1, A.Dummy<MyObject>());
            stream.PutWithId(id4, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            stream.PutWithId(id2, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow), tags: tags);

            var operation = new StandardGetInternalRecordIdsOp(
                new RecordFilter(
                    tags: tags,
                    deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() }),
                recordsToFilterCriteria: new RecordsToFilterCriteria(RecordsToFilterSelectionStrategy.LatestByIdAndObjectType));

            var expected = new[] { 1L, 3L };

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void StandardGetDistinctStringSerializedIds___Should_not_return_id_of_DeprecatedIdTypes___When_record_filter_specifies_DeprecatedIdsTypes()
        {
            // Arrange
            var stream = BuildCreatedStream();

            stream.PutWithId("id-1", new MyObject(A.Dummy<string>(), A.Dummy<string>()));
            stream.PutWithId("id-2", new IdDeprecatedEvent<MyObject>(DateTime.UtcNow));

            var operation = new StandardGetDistinctStringSerializedIdsOp(
                new RecordFilter(
                    ids: new[]
                    {
                        new StringSerializedIdentifier("id-1", typeof(string).ToRepresentation()),
                        new StringSerializedIdentifier("id-2", typeof(string).ToRepresentation()),
                    },
                    deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<MyObject>).ToRepresentation() }));

            var expected = new[] { new StringSerializedIdentifier("id-1", typeof(string).ToRepresentation()) };

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void StandardGetDistinctStringSerializedIds___Should_return_latest_ids_that_survive_filtering___When_RecordsToFilterSelectionStrategy_LatestById_used_with_tag_based_RecordFilter()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var id1 = "id-1";
            var id2 = "id-2";
            var id3 = "id-3";
            var id4 = "id-4";

            var tags = new[] { A.Dummy<NamedValue<string>>() };

            stream.PutWithId(id1, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id2, A.Dummy<NamedResourceLocator>());
            stream.PutWithId(id3, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id4, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id1, A.Dummy<MyObject>());
            stream.PutWithId(id4, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            stream.PutWithId(id2, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow), tags: tags);

            var expected = new[]
            {
                new StringSerializedIdentifier(id3, typeof(string).ToRepresentation()),
            };

            var operation = new StandardGetDistinctStringSerializedIdsOp(
                new RecordFilter(
                    tags: tags,
                    deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() }),
                new RecordsToFilterCriteria(RecordsToFilterSelectionStrategy.LatestById));

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void StandardGetDistinctStringSerializedIds___Should_return_latest_ids_that_survive_filtering___When_RecordsToFilterSelectionStrategy_LatestByIdAndObjectType_used_with_tag_based_RecordFilter()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var id1 = "id-1";
            var id2 = "id-2";
            var id3 = "id-3";
            var id4 = "id-4";

            var tags = new[] { A.Dummy<NamedValue<string>>() };

            stream.PutWithId(id1, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id2, A.Dummy<NamedResourceLocator>());
            stream.PutWithId(id3, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id4, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id1, A.Dummy<MyObject>());
            stream.PutWithId(id4, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            stream.PutWithId(id2, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow), tags: tags);

            var expected = new[]
            {
                new StringSerializedIdentifier(id1, typeof(string).ToRepresentation()),
                new StringSerializedIdentifier(id3, typeof(string).ToRepresentation()),
            };

            var operation = new StandardGetDistinctStringSerializedIdsOp(
                new RecordFilter(
                    tags: tags,
                    deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() }),
                new RecordsToFilterCriteria(RecordsToFilterSelectionStrategy.LatestByIdAndObjectType));

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void StandardGetDistinctStringSerializedIds___Should_return_latest_ids_that_survive_filtering___When_id_and_object_and_deprecated_id_types_specified()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var expected1 = Guid.NewGuid().ToString().Replace("-", ",");
            var expected2 = Guid.NewGuid().ToString().Replace("-", ",");

            IReadOnlyCollection<string> GetActual() => stream
                .Execute(
                    new StandardGetDistinctStringSerializedIdsOp(
                        new RecordFilter(
                            idTypes: new[] { typeof(string).ToRepresentation() },
                            objectTypes: new[] { typeof(string).ToRepresentation(), },
                            deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent).ToRepresentation() })))
                .Select(_ => stream.IdSerializer.Deserialize<string>(_.StringSerializedId))
                .ToList();

            IReadOnlyCollection<string> GetActualWrongObjectType() => stream
                .Execute(
                    new StandardGetDistinctStringSerializedIdsOp(
                        new RecordFilter(
                            idTypes: new[] { typeof(string).ToRepresentation() },
                            objectTypes: new[] { typeof(long).ToRepresentation(), },
                            deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent).ToRepresentation() })))
                .Select(_ => stream.IdSerializer.Deserialize<string>(_.StringSerializedId))
                .ToList();

            IReadOnlyCollection<string> GetActualWrongIdType() => stream
                .Execute(
                    new StandardGetDistinctStringSerializedIdsOp(
                        new RecordFilter(
                            idTypes: new[] { typeof(long).ToRepresentation() },
                            objectTypes: new[] { typeof(string).ToRepresentation(), },
                            deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent).ToRepresentation() })))
                .Select(_ => stream.IdSerializer.Deserialize<string>(_.StringSerializedId))
                .ToList();

            // Act, Assert
            stream.PutWithIdAndReturnInternalRecordId(expected1, A.Dummy<string>());
            var actual = GetActual();
            actual.MustForTest().BeUnorderedEqualTo(new[] { expected1 });

            stream.PutWithIdAndReturnInternalRecordId(expected1, A.Dummy<string>());
            actual = GetActual();
            actual.MustForTest().BeUnorderedEqualTo(new[] { expected1 });

            stream.PutWithIdAndReturnInternalRecordId(expected2, A.Dummy<short>());
            actual = GetActual();
            actual.MustForTest().BeUnorderedEqualTo(new[] { expected1 });

            stream.PutWithIdAndReturnInternalRecordId(expected2, A.Dummy<string>());
            actual = GetActual();
            actual.MustForTest().BeUnorderedEqualTo(new[] { expected1, expected2 });

            stream.PutWithIdAndReturnInternalRecordId(expected2, new IdDeprecatedEvent(DateTime.UtcNow));
            actual = GetActual();
            actual.MustForTest().BeUnorderedEqualTo(new[] { expected1 });

            stream.PutWithIdAndReturnInternalRecordId(expected2, A.Dummy<string>());
            actual = GetActual();
            actual.MustForTest().BeUnorderedEqualTo(new[] { expected1, expected2 });

            actual = GetActualWrongIdType();
            actual.MustForTest().BeEmptyEnumerable();

            actual = GetActualWrongObjectType();
            actual.MustForTest().BeEmptyEnumerable();
        }

        [Fact]
        public static void StandardGetLatestRecord___Should_return_latest_object___When_some_objects_in_stream_have_been_deprecated()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var objectToPut1 = A.Dummy<MyObject>();
            var objectToPut2 = A.Dummy<MyObject>();

            stream.PutWithId(objectToPut2.Id, objectToPut2);
            stream.PutWithId(objectToPut1.Id, objectToPut1);
            stream.PutWithId(objectToPut1.Id, new IdDeprecatedEvent<MyObject>(DateTime.UtcNow));

            // Act
            var actual = stream.Execute(
                new StandardGetLatestRecordOp(
                    new RecordFilter(
                        objectTypes: new[] { typeof(MyObject).ToRepresentation() },
                        deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<MyObject>).ToRepresentation() })));

            // Assert
            actual.AsTest().Must().NotBeNull();
            actual.Payload.DeserializePayloadUsingSpecificFactory<MyObject>(stream.SerializerFactory).Id.Must().BeEqualTo(objectToPut2.Id);
        }

        [Fact]
        public static void StandardGetLatestRecord___Should_return_latest_record_whose_object_type_is_not_in_DepreciatedIdTypes___When_object_type_of_latest_record_in_stream_is_contained_within_record_filter_DeprecatedIdsTypes()
        {
            // Arrange
            var stream = BuildCreatedStream();

            stream.PutWithId("id-1", new MyObject(A.Dummy<string>(), A.Dummy<string>()));
            stream.PutWithId("id-2", new IdDeprecatedEvent<MyObject>(DateTime.UtcNow));

            var operation = new StandardGetLatestRecordOp(
                new RecordFilter(
                    deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<MyObject>).ToRepresentation() }));

            var expectedStreamRecord = stream.GetLatestRecordById("id-1");
            var expectedMetadata = expectedStreamRecord.Metadata;

            var expected = new StreamRecord(
                expectedStreamRecord.InternalRecordId,
                new StreamRecordMetadata(
                    stream.IdSerializer.SerializeToString(expectedMetadata.Id),
                    stream.DefaultSerializerRepresentation,
                    expectedMetadata.TypeRepresentationOfId,
                    expectedMetadata.TypeRepresentationOfObject,
                    expectedMetadata.Tags,
                    expectedMetadata.TimestampUtc,
                    expectedMetadata.ObjectTimestampUtc),
                expectedStreamRecord.Payload);

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void StandardGetLatestStringSerializedObject___Should_return_latest_string_serialized_object_whose_type_is_not_in_DepreciatedIdTypes___When_object_type_of_latest_record_in_stream_is_contained_within_record_filter_DeprecatedIdsTypes()
        {
            // Arrange
            var stream = BuildCreatedStream();

            stream.PutWithId("id-1", new MyObject(A.Dummy<string>(), A.Dummy<string>()));
            stream.PutWithId("id-2", new IdDeprecatedEvent<MyObject>(DateTime.UtcNow));

            var operation = new StandardGetLatestStringSerializedObjectOp(
                new RecordFilter(
                    deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<MyObject>).ToRepresentation() }));

            var expected = stream.GetLatestStringSerializedObjectById("id-1");

            // Act
            var actual = stream.Execute(operation);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetAllObjects_TObject___Should_return_all_object_of_the_specified_type___When_called()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var object1 = A.Dummy<NamedResourceLocator>();
            var object2 = A.Dummy<NamedResourceLocator>();

            stream.Put(object1);
            stream.Put(object2);
            stream.Put(A.Dummy<string>());
            stream.Put(A.Dummy<NullResourceLocator>());

            var expected = new[] { object2, object1 };

            // Act
            var actual = stream.GetAllObjects<NamedResourceLocator>(orderRecordsBy: OrderRecordsBy.InternalRecordIdDescending);

            // Assert
            actual.AsTest().Must().BeSequenceEqualTo(expected);
        }

        [Fact]
        public static void GetAllObjectsById_TId_TObject___Should_return_all_object_of_the_specified_type_having_the_specified_id___When_called()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var id1 = A.Dummy<string>();
            var object1 = A.Dummy<NamedResourceLocator>();
            var object2 = A.Dummy<NamedResourceLocator>();

            stream.PutWithId(id1, object1);
            stream.PutWithId(A.Dummy<string>(), A.Dummy<NamedResourceLocator>());
            stream.PutWithId(id1, object2);
            stream.PutWithId(A.Dummy<string>(), A.Dummy<string>());
            stream.PutWithId(id1, A.Dummy<NullResourceLocator>());
            stream.PutWithId(A.Dummy<string>(), object1);

            var expected = new[] { object2, object1 };

            // Act
            var actual = stream.GetAllObjectsById<string, NamedResourceLocator>(id1, orderRecordsBy: OrderRecordsBy.InternalRecordIdDescending);

            // Assert
            actual.AsTest().Must().BeSequenceEqualTo(expected);
        }

        [Fact]
        public static void GetAllObjectsById_TId_TObject___Should_get_all_matching_objects___When_TObject_is_base_class_type_and_TypeSelectionStrategy_is_UseDeclaredType()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var id = A.Dummy<string>();

            var expected = A.Dummy<MyDerivedClass2>();

            stream.PutWithId<string, MyBaseClass>(id, A.Dummy<MyDerivedClass1>(), existingRecordStrategy: ExistingRecordStrategy.PruneIfFoundByIdAndType, recordRetentionCount: 0, typeSelectionStrategy: TypeSelectionStrategy.UseDeclaredType);
            stream.PutWithId<string, MyBaseClass>(id, expected, existingRecordStrategy: ExistingRecordStrategy.PruneIfFoundByIdAndType, recordRetentionCount: 0, typeSelectionStrategy: TypeSelectionStrategy.UseDeclaredType);

            // Act
            var actual = stream.GetAllObjectsById<string, MyBaseClass>(id);

            // Assert
            actual.AsTest().Must().HaveCount(1);
            var actualBaseClass = actual.Single();
            actualBaseClass.AsTest().Must().BeOfType<MyDerivedClass2>();
            var actualDerivedClass = (MyDerivedClass2)actualBaseClass;
            actualDerivedClass.Id.Must().BeEqualTo(expected.Id);
            actualDerivedClass.Name.Must().BeEqualTo(expected.Name);
            actualDerivedClass.Derived2Property.Must().BeEqualTo(expected.Derived2Property);
        }

        [Fact]
        public static void GetAllRecords_TObject___Should_return_all_records_of_the_specified_object_type___When_called()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var object1 = A.Dummy<NamedResourceLocator>();
            var object2 = A.Dummy<NamedResourceLocator>();

            stream.Put(object1);
            stream.Put(object2);
            stream.Put(A.Dummy<string>());
            stream.Put(A.Dummy<NullResourceLocator>());

            var expected = new[]
            {
                object2,
                object1,
            };

            // Act
            var actual = stream
                .GetAllRecords<NamedResourceLocator>(orderRecordsBy: OrderRecordsBy.InternalRecordIdDescending)
                .Select(_ => _.Payload)
                .ToList();

            // Assert
            actual.AsTest().Must().BeSequenceEqualTo(expected);
        }

        [Fact]
        public static void GetAllRecordsMetadata___Should_return_expected_StreamRecordMetadata_objects___When_called()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var objectToPut = A.Dummy<NamedResourceLocator>();
            var tagToPut = new NamedValue<string>("tag", "tag-value");
            var id1 = A.Dummy<string>();
            var id2 = A.Dummy<string>();

            stream.PutWithId(id1, objectToPut, tags: new[] { tagToPut });
            stream.PutWithId(A.Dummy<int>(), objectToPut, tags: new[] { tagToPut });
            stream.PutWithId(A.Dummy<string>(), A.Dummy<decimal>(), tags: new[] { tagToPut });
            stream.PutWithId(A.Dummy<string>(), objectToPut, tags: new[] { A.Dummy<NamedValue<string>>() });
            stream.PutWithId(id2, A.Dummy<NamedResourceLocator>(), tags: new[] { tagToPut, A.Dummy<NamedValue<string>>() });

            StreamRecordMetadata ToMetadataWithoutId(StreamRecordMetadata<string> metadata)
            {
                var stringSerializedId = stream.IdSerializer.SerializeToString(metadata.Id);

                return new StreamRecordMetadata(stringSerializedId, metadata.SerializerRepresentation, metadata.TypeRepresentationOfId, metadata.TypeRepresentationOfObject, metadata.Tags, metadata.TimestampUtc, metadata.ObjectTimestampUtc);
            }

            var expected = new[]
            {
                ToMetadataWithoutId(stream.GetAllRecordsById(id1).Single().Metadata),
                ToMetadataWithoutId(stream.GetAllRecordsById(id2).Single().Metadata),
            };

            // Act
            var actual = stream
                .GetAllRecordsMetadata(
                    identifierType: typeof(string).ToRepresentation(),
                    objectType: typeof(NamedResourceLocator).ToRepresentation(),
                    tagsToMatch: new[] { tagToPut })
                .ToArray();

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void GetAllRecordsMetadata_TId___Should_return_expected_StreamRecordMetadata_TId_objects___When_called()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var objectToPut = A.Dummy<NamedResourceLocator>();
            var tagToPut = new NamedValue<string>("tag", "tag-value");
            var id1 = A.Dummy<string>();
            var id2 = A.Dummy<string>();

            stream.PutWithId(id1, objectToPut, tags: new[] { tagToPut });
            stream.PutWithId(A.Dummy<int>(), objectToPut, tags: new[] { tagToPut });
            stream.PutWithId(A.Dummy<string>(), A.Dummy<decimal>(), tags: new[] { tagToPut });
            stream.PutWithId(A.Dummy<string>(), objectToPut, tags: new[] { A.Dummy<NamedValue<string>>() });
            stream.PutWithId(id2, A.Dummy<NamedResourceLocator>(), tags: new[] { tagToPut, A.Dummy<NamedValue<string>>() });

            var expected = new[]
            {
                stream.GetAllRecordsById(id1).Single().Metadata,
                stream.GetAllRecordsById(id2).Single().Metadata,
            };

            // Act
            var actual = stream
                .GetAllRecordsMetadata<string>(
                    objectType: typeof(NamedResourceLocator).ToRepresentation(),
                    tagsToMatch: new[] { tagToPut })
                .ToArray();

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void GetDistinctIds___Should_return_not_deprecated_ids___When_called_with_DeprecatedIdTypes()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var item1 = new MyObject("1", "my-obj-1");
            var item2 = new MyObject("2", "my-obj-2");
            var item3 = new MyObject2("1", "my-obj-2");
            var item4 = new MyObject2("2", "my-obj-2");
            var item5 = new MyObject("3", "my-obj-1");
            var item6 = new MyObject2("4", "my-obj-2");

            var depreciated1 = new IdDeprecatedEvent<MyObject>(DateTime.UtcNow);
            var depreciated2 = new IdDeprecatedEvent<MyObject2>(DateTime.UtcNow);

            stream.PutWithId(item1.Id, item1);
            stream.PutWithId(item2.Id, item2);
            stream.PutWithId(item3.Id, item3);
            stream.PutWithId(item4.Id, item4);
            stream.PutWithId(item5.Id, item5);
            stream.PutWithId(item6.Id, item6);
            stream.PutWithId(item1.Id, depreciated1);
            stream.PutWithId(item4.Id, depreciated2);
            stream.PutWithId(item6.Id, depreciated2);
            stream.PutWithId(item6.Id, item6);

            // Act
            var actual1 = stream.GetDistinctIds<string>(new[] { typeof(MyObject).ToRepresentation() }, deprecatedIdTypes: new[] { depreciated1.GetType().ToRepresentation() });

            var actual2 = stream.GetDistinctIds<string>(new[] { typeof(MyObject2).ToRepresentation() }, deprecatedIdTypes: new[] { depreciated2.GetType().ToRepresentation() });

            // Assert
            actual1.AsTest().Must().BeEqualTo((IReadOnlyCollection<string>)new[] { item2.Id, item5.Id });
            actual2.AsTest().Must().BeEqualTo((IReadOnlyCollection<string>)new[] { item3.Id, item6.Id });
        }

        [Fact]
        public static void GetDistinctIds___Should_return_latest_ids_that_survive_filtering___When_RecordsToFilterSelectionStrategy_LatestById_used_with_tag_based_RecordFilter()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var id1 = "id-1";
            var id2 = "id-2";
            var id3 = "id-3";
            var id4 = "id-4";

            var tags = new[] { A.Dummy<NamedValue<string>>() };

            stream.PutWithId(id1, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id2, A.Dummy<NamedResourceLocator>());
            stream.PutWithId(id3, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id4, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id1, A.Dummy<NamedResourceLocator>());
            stream.PutWithId(id4, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            stream.PutWithId(id2, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow), tags: tags);

            var expected = new[] { id3 };

            var operation = new GetDistinctIdsOp<string>(
                tagsToMatch: tags,
                deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() },
                recordsToFilterCriteria: new RecordsToFilterCriteria(RecordsToFilterSelectionStrategy.LatestById));

            // Act
            var actual = stream.GetStreamReadingWithIdProtocols<string>().Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void GetDistinctIds___Should_return_latest_ids_that_survive_filtering___When_RecordsToFilterSelectionStrategy_LatestByIdAndObjectType_used_with_tag_based_RecordFilter()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var id1 = "id-1";
            var id2 = "id-2";
            var id3 = "id-3";
            var id4 = "id-4";

            var tags = new[] { A.Dummy<NamedValue<string>>() };

            stream.PutWithId(id1, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id2, A.Dummy<NamedResourceLocator>());
            stream.PutWithId(id3, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id4, A.Dummy<NamedResourceLocator>(), tags: tags);
            stream.PutWithId(id1, A.Dummy<MyObject>());
            stream.PutWithId(id4, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            stream.PutWithId(id2, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow), tags: tags);

            var expected = new[] { id1, id3 };

            var operation = new GetDistinctIdsOp<string>(
                tagsToMatch: tags,
                deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() },
                recordsToFilterCriteria: new RecordsToFilterCriteria(RecordsToFilterSelectionStrategy.LatestByIdAndObjectType));

            // Act
            var actual = stream.GetStreamReadingWithIdProtocols<string>().Execute(operation);

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static void GetLatestObjectById_TId_TObject___Should_get_object_put_into_stream___When_called()
        {
            // Arrange
            var stream = BuildCreatedStream();
            var expected = 1000;
            var id = A.Dummy<string>();

            stream.PutWithId(A.Dummy<string>(), expected - 1);
            stream.PutWithId(id, expected);

            // Act
            var actual = stream.GetLatestObjectById<string, int>(id);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetLatestObjectById_TId_TObject___Should_return_null___When_object_has_been_deprecated()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var objectToPut = A.Dummy<MyObject>();

            stream.PutWithId(objectToPut.Id, objectToPut);
            stream.PutWithId(objectToPut.Id, new IdDeprecatedEvent<MyObject>(DateTime.UtcNow));

            // Act
            var actual = stream.GetLatestObjectById<string, MyObject>(
                objectToPut.Id,
                deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<MyObject>).ToRepresentation() });

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void GetLatestObjectById_TId_TObject___Should_return_latest_object___When_object_has_been_deprecated_and_then_put_again_with_same_id()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var objectToPut1 = A.Dummy<MyObject>();
            var objectToPut2 = new MyObject(objectToPut1.Id, A.Dummy<string>());

            stream.PutWithId(objectToPut1.Id, objectToPut1);
            stream.PutWithId(objectToPut1.Id, new IdDeprecatedEvent<MyObject>(DateTime.UtcNow));
            stream.PutWithId(objectToPut1.Id, objectToPut2);

            // Act
            var actual = stream.GetLatestObjectById<string, MyObject>(
                objectToPut1.Id,
                deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<MyObject>).ToRepresentation() });

            // Assert
            actual.Field.AsTest().Must().BeEqualTo(objectToPut2.Field);
        }

        [Fact]
        public static async Task GetLatestObjectsById_TId_TObject___Should_get_all_matching_objects___When_ids_specified()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var object1A = A.Dummy<NamedResourceLocator>();
            var object1B = A.Dummy<NamedResourceLocator>();
            var object2A = A.Dummy<NamedResourceLocator>();
            var object2B = A.Dummy<NamedResourceLocator>();
            var object3A = A.Dummy<NamedResourceLocator>();
            var object3B = A.Dummy<NamedResourceLocator>();
            var object4A = A.Dummy<NamedResourceLocator>();
            var object4B = A.Dummy<NamedResourceLocator>();

            var id1 = "id-1";
            var id2 = "id-2";
            var id3 = "id-3";
            var id4 = "id-4";

            await stream.PutWithIdAsync(id1, object1A);
            await stream.PutWithIdAsync(id2, object2A);
            await stream.PutWithIdAsync(id3, object3A);
            await stream.PutWithIdAsync(id4, object4A);
            await stream.PutWithIdAsync(id2, object2B);
            await stream.PutWithIdAsync(id3, object3B);
            await stream.PutWithIdAsync(id4, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            await stream.PutWithIdAsync(id1, object1B);
            await stream.PutWithIdAsync(id2, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            await stream.PutWithIdAsync(id4, object4B);
            await stream.PutWithIdAsync(id1, A.Dummy<MyObject>());

            var expected = new[] { object1B, object4B };

            // Act
            var actual = await stream.GetLatestObjectsByIdAsync<string, NamedResourceLocator>(
                new[] { id4, id2, id1 },
                deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() });

            // Assert
            actual.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        [Fact]
        public static async Task GetLatestObjectsById_TId_TObject___Should_get_all_matching_objects___When_ids_are_not_specified()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var object1A = A.Dummy<NamedResourceLocator>();
            var object1B = A.Dummy<NamedResourceLocator>();
            var object2A = A.Dummy<NamedResourceLocator>();
            var object2B = A.Dummy<NamedResourceLocator>();
            var object3A = A.Dummy<NamedResourceLocator>();
            var object3B = A.Dummy<NamedResourceLocator>();
            var object4A = A.Dummy<NamedResourceLocator>();
            var object4B = A.Dummy<NamedResourceLocator>();

            var id1 = "id-1";
            var id2 = "id-2";
            var id3 = "id-3";
            var id4 = "id-4";

            await stream.PutWithIdAsync(id1, object1A);
            await stream.PutWithIdAsync(id2, object2A);
            await stream.PutWithIdAsync(id3, object3A);
            await stream.PutWithIdAsync(id4, object4A);
            await stream.PutWithIdAsync(id2, object2B);
            await stream.PutWithIdAsync(id3, object3B);
            await stream.PutWithIdAsync(id4, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            await stream.PutWithIdAsync(id1, object1B);
            await stream.PutWithIdAsync(id2, new IdDeprecatedEvent<NamedResourceLocator>(DateTime.UtcNow));
            await stream.PutWithIdAsync(id4, object4B);
            await stream.PutWithIdAsync(id4, A.Dummy<MyObject>());

            var expected = new[] { object1B, object3B, object4B };

            // Act
            var actual1 = await stream.GetLatestObjectsByIdAsync<string, NamedResourceLocator>(
                null,
                deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() });

            var actual2 = await stream.GetLatestObjectsByIdAsync<string, NamedResourceLocator>(
                new string[0],
                deprecatedIdTypes: new[] { typeof(IdDeprecatedEvent<NamedResourceLocator>).ToRepresentation() });

            // Assert
            actual1.AsTest().Must().BeUnorderedEqualTo(expected);
            actual2.AsTest().Must().BeUnorderedEqualTo(expected);
        }

        private static MemoryStandardStream BuildCreatedStream()
        {
            var configurationTypeRepresentation = typeof(StreamTestSerializationConfiguration).ToRepresentation();

            var result = new MemoryStandardStream(
                "test-stream-name",
                new SerializerRepresentation(SerializationKind.Json, configurationTypeRepresentation),
                SerializationFormat.String,
                new JsonSerializerFactory());

            return result;
        }

        private class StreamTestSerializationConfiguration : JsonSerializationConfigurationBase
        {
            /// <inheritdoc />
            protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes =>
                new[]
                {
                    typeof(DatabaseJsonSerializationConfiguration).ToJsonSerializationConfigurationType(),
                    typeof(TypesToRegisterJsonSerializationConfiguration<MyObject, MyObject2>).ToJsonSerializationConfigurationType(),
                    typeof(TypesToRegisterJsonSerializationConfiguration<MyBaseClass, MyDerivedClass1, MyDerivedClass2>).ToJsonSerializationConfigurationType(),
                };
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

            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for serialization
            public string Id { get; private set; }

            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for serialization
            public string Field { get; private set; }

            /// <inheritdoc />
            public IReadOnlyCollection<NamedValue<string>> Tags => new List<NamedValue<string>>();
        }

        private class MyObject2 : IHaveId<string>
        {
            public MyObject2(
                string id,
                string field)
            {
                this.Id = id;
                this.Field = field;
            }

            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for serialization
            public string Id { get; private set; }

            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for serialization
            // ReSharper disable once MemberCanBePrivate.Local - mimicking model objects
            // ReSharper disable once UnusedAutoPropertyAccessor.Local - mimicking model objects
            public string Field { get; private set; }
        }

        private class MyBaseClass
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by dervied class constructors.")]
            protected MyBaseClass(
                string id,
                string name)
            {
                this.Id = id;
                this.Name = name;
            }

            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for serialization
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for testing")]
            public string Id { get; private set; }

            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for serialization
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for testing")]

            public string Name { get; private set; }
        }

        // ReSharper disable once ClassNeverInstantiated.Local - instantiated as a dummy
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = NaosSuppressBecause.CA1812_AvoidUninstantiatedInternalClasses_ClassExistsToUseItsTypeInUnitTests)]
        private class MyDerivedClass1 : MyBaseClass
        {
            public MyDerivedClass1(
                string id,
                string name,
                string derived1Property)
                : base(id, name)
            {
                this.Derived1Property = derived1Property;
            }

            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for serialization
            // ReSharper disable once MemberCanBePrivate.Local - mimicking model objects
            // ReSharper disable once UnusedAutoPropertyAccessor.Local - mimicking model objects
            public string Derived1Property { get; private set; }
        }

        // ReSharper disable once ClassNeverInstantiated.Local - instantiated as a dummy
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = NaosSuppressBecause.CA1812_AvoidUninstantiatedInternalClasses_ClassExistsToUseItsTypeInUnitTests)]
        private class MyDerivedClass2 : MyBaseClass
        {
            public MyDerivedClass2(
                string id,
                string name,
                decimal derived2Property)
                : base(id, name)
            {
                this.Derived2Property = derived2Property;
            }

            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for serialization
            public decimal Derived2Property { get; private set; }
        }
    }
}
