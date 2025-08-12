// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordExtensionsTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using Xunit;

    public static class StreamRecordExtensionsTest
    {
        [Fact]
        public static void GetDescribedSerialization___Should_throw_ArgumentNullException___When_parameter_streamRecord_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StreamRecordExtensions.GetDescribedSerialization(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("streamRecord");
        }

        [Fact]
        public static void GetDescribedSerialization___Should_return_StringDescribedSerialization___When_streamRecord_Payload_is_StringStreamRecordPayload()
        {
            // Arrange
            var streamRecordPayload = A.Dummy<StringStreamRecordPayload>();

            var streamRecord = new StreamRecord(
                A.Dummy<long>(),
                A.Dummy<StreamRecordMetadata>(),
                streamRecordPayload);

            var expected = (DescribedSerializationBase)new StringDescribedSerialization(
                streamRecord.Metadata.TypeRepresentationOfObject.WithVersion,
                streamRecord.Metadata.SerializerRepresentation,
                streamRecordPayload.SerializedPayload);

            // Act
            var actual = streamRecord.GetDescribedSerialization();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetDescribedSerialization___Should_return_BinaryDescribedSerialization___When_streamRecord_Payload_is_BinaryStreamRecordPayload()
        {
            // Arrange
            var streamRecordPayload = A.Dummy<BinaryStreamRecordPayload>();

            var streamRecord = new StreamRecord(
                A.Dummy<long>(),
                A.Dummy<StreamRecordMetadata>(),
                streamRecordPayload);

            var expected = (DescribedSerializationBase)new BinaryDescribedSerialization(
                streamRecord.Metadata.TypeRepresentationOfObject.WithVersion,
                streamRecord.Metadata.SerializerRepresentation,
                streamRecordPayload.SerializedPayload);

            // Act
            var actual = streamRecord.GetDescribedSerialization();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetDescribedSerialization___Should_return_NullDescribedSerialization___When_streamRecord_Payload_is_NullStreamRecordPayload()
        {
            // Arrange
            var streamRecordPayload = A.Dummy<NullStreamRecordPayload>();

            var streamRecord = new StreamRecord(
                A.Dummy<long>(),
                A.Dummy<StreamRecordMetadata>(),
                streamRecordPayload);

            var expected = (DescribedSerializationBase)new NullDescribedSerialization(
                streamRecord.Metadata.TypeRepresentationOfObject.WithVersion,
                streamRecord.Metadata.SerializerRepresentation);

            // Act
            var actual = streamRecord.GetDescribedSerialization();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetDescribedSerialization_TId___Should_throw_ArgumentNullException___When_parameter_streamRecord_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StreamRecordExtensions.GetDescribedSerialization<string>(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("streamRecord");
        }

        [Fact]
        public static void GetDescribedSerialization_TId___Should_return_StringDescribedSerialization___When_streamRecord_Payload_is_StringStreamRecordPayload()
        {
            // Arrange
            var streamRecordPayload = A.Dummy<StringStreamRecordPayload>();

            var streamRecord = new StreamRecordWithId<long>(
                A.Dummy<long>(),
                A.Dummy<StreamRecordMetadata>().ToStreamRecordMetadata(A.Dummy<long>()),
                streamRecordPayload);

            var expected = (DescribedSerializationBase)new StringDescribedSerialization(
                streamRecord.Metadata.TypeRepresentationOfObject.WithVersion,
                streamRecord.Metadata.SerializerRepresentation,
                streamRecordPayload.SerializedPayload);

            // Act
            var actual = streamRecord.GetDescribedSerialization();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetDescribedSerialization_TId___Should_return_BinaryDescribedSerialization___When_streamRecord_Payload_is_BinaryStreamRecordPayload()
        {
            // Arrange
            var streamRecordPayload = A.Dummy<BinaryStreamRecordPayload>();

            var streamRecord = new StreamRecordWithId<long>(
                A.Dummy<long>(),
                A.Dummy<StreamRecordMetadata>().ToStreamRecordMetadata(A.Dummy<long>()),
                streamRecordPayload);

            var expected = (DescribedSerializationBase)new BinaryDescribedSerialization(
                streamRecord.Metadata.TypeRepresentationOfObject.WithVersion,
                streamRecord.Metadata.SerializerRepresentation,
                streamRecordPayload.SerializedPayload);

            // Act
            var actual = streamRecord.GetDescribedSerialization();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetDescribedSerialization_TId___Should_return_NullDescribedSerialization___When_streamRecord_Payload_is_NullStreamRecordPayload()
        {
            // Arrange
            var streamRecordPayload = A.Dummy<NullStreamRecordPayload>();

            var streamRecord = new StreamRecordWithId<long>(
                A.Dummy<long>(),
                A.Dummy<StreamRecordMetadata>().ToStreamRecordMetadata(A.Dummy<long>()),
                streamRecordPayload);

            var expected = (DescribedSerializationBase)new NullDescribedSerialization(
                streamRecord.Metadata.TypeRepresentationOfObject.WithVersion,
                streamRecord.Metadata.SerializerRepresentation);

            // Act
            var actual = streamRecord.GetDescribedSerialization();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToStreamRecordPayload___Should_throw_ArgumentNullException___When_parameter_describedSerialization_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StreamRecordExtensions.ToStreamRecordPayload(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("describedSerialization");
        }

        [Fact]
        public static void ToStreamRecordPayload___Should_return_StringStreamRecordPayload___When_parameter_describedSerialization_is_a_StringDescribedSerialization()
        {
            // Arrange
            var expected = A.Dummy<StringStreamRecordPayload>();

            var describedSerialization = A.Dummy<StringDescribedSerialization>()
                .DeepCloneWithSerializedPayload(expected.SerializedPayload);

            // Act
            var actual = describedSerialization.ToStreamRecordPayload();

            // Assert
            actual.AsTest().Must().BeOfType<StringStreamRecordPayload>();
            ((StringStreamRecordPayload)actual).AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToStreamRecordPayload___Should_return_BinaryStreamRecordPayload___When_parameter_describedSerialization_is_a_BinaryDescribedSerialization()
        {
            // Arrange
            var expected = A.Dummy<BinaryStreamRecordPayload>();

            var describedSerialization = A.Dummy<BinaryDescribedSerialization>()
                .DeepCloneWithSerializedPayload(expected.SerializedPayload);

            // Act
            var actual = describedSerialization.ToStreamRecordPayload();

            // Assert
            actual.AsTest().Must().BeOfType<BinaryStreamRecordPayload>();
            ((BinaryStreamRecordPayload)actual).AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToStreamRecordPayload___Should_return_NullStreamRecordPayload___When_parameter_describedSerialization_is_a_NullDescribedSerialization()
        {
            // Arrange
            var expected = A.Dummy<NullStreamRecordPayload>();

            var describedSerialization = A.Dummy<NullDescribedSerialization>();

            // Act
            var actual = describedSerialization.ToStreamRecordPayload();

            // Assert
            actual.AsTest().Must().BeOfType<NullStreamRecordPayload>();
            ((NullStreamRecordPayload)actual).AsTest().Must().BeEqualTo(expected);
        }
    }
}
