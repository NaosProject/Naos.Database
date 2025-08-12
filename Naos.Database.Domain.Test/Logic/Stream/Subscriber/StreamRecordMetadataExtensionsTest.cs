// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadataExtensionsTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FakeItEasy;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    public static class StreamRecordMetadataExtensionsTest
    {
        [Fact]
        public static void ToStreamRecordMetadata_TId___Should_throw_ArgumentNullException___When_parameter_metadata_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StreamRecordMetadataExtensions.ToStreamRecordMetadata(null, A.Dummy<string>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("metadata");
        }

        [Fact]
        public static void ToStreamRecordMetadata_TId___Should_return_metadata_converted_to_StreamRecordMetadata_TId___When_called()
        {
            // Arrange
            var metadata = A.Dummy<StreamRecordMetadata>();

            var id = A.Dummy<string>();

            var expected = new StreamRecordMetadata<string>(
                id,
                metadata.SerializerRepresentation,
                metadata.TypeRepresentationOfId,
                metadata.TypeRepresentationOfObject,
                metadata.Tags,
                metadata.TimestampUtc,
                metadata.ObjectTimestampUtc);

            // Act
            var actual = metadata.ToStreamRecordMetadata(id);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToStreamRecordMetadata_IStringDeserialize___Should_throw_ArgumentNullException___When_parameter_metadata_is_null()
        {
            // Arrange
            var identifierDeserializer = new ObcLambdaBackedStringSerializer(
                _ => A.Dummy<string>(),
                (_, t) => A.Dummy<string>());

            // Act
            var actual = Record.Exception(() => StreamRecordMetadataExtensions.ToStreamRecordMetadata<string>(null, identifierDeserializer));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("metadata");
        }

        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Deserializer", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStreamRecordMetadata_IStringDeserialize___Should_throw_ArgumentNullException___When_parameter_identifierDeserializer_is_null()
        {
            // Arrange
            var metadata = A.Dummy<StreamRecordMetadata>();

            // Act
            var actual = Record.Exception(() => metadata.ToStreamRecordMetadata<string>((IStringDeserialize)null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("identifierDeserializer");
        }

        [Fact]
        public static void ToStreamRecordMetadata_IStringDeserialize___Should_return_metadata_converted_to_StreamRecordMetadata_TId___When_called()
        {
            // Arrange
            var metadata = A.Dummy<StreamRecordMetadata>();

            var id = A.Dummy<string>();

            var identifierDeserializer = new ObcLambdaBackedStringSerializer(
                _ => A.Dummy<string>(),
                (_, t) =>
                {
                    if (_ != metadata.StringSerializedId)
                    {
                        throw new InvalidOperationException("should not get here");
                    }

                    return id;
                });

            var expected = new StreamRecordMetadata<string>(
                id,
                metadata.SerializerRepresentation,
                metadata.TypeRepresentationOfId,
                metadata.TypeRepresentationOfObject,
                metadata.Tags,
                metadata.TimestampUtc,
                metadata.ObjectTimestampUtc);

            // Act
            var actual = metadata.ToStreamRecordMetadata<string>(identifierDeserializer);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToStreamRecordMetadata_IStream___Should_throw_ArgumentNullException___When_parameter_metadata_is_null()
        {
            // Arrange
            var stream = new MemoryStandardStream(
                "test-stream-name",
                new SerializerRepresentation(SerializationKind.Json),
                SerializationFormat.String,
                new JsonSerializerFactory());

            // Act
            var actual = Record.Exception(() => StreamRecordMetadataExtensions.ToStreamRecordMetadata<string>(null, stream));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("metadata");
        }

        [Fact]
        public static void ToStreamRecordMetadata_IStandardStream___Should_throw_ArgumentNullException___When_parameter_stream_is_null()
        {
            // Arrange
            var metadata = A.Dummy<StreamRecordMetadata>();

            // Act
            var actual = Record.Exception(() => metadata.ToStreamRecordMetadata<string>((IStandardStream)null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("stream");
        }

        [Fact]
        public static void ToStreamRecordMetadata_IStream___Should_return_metadata_converted_to_StreamRecordMetadata_TId___When_called()
        {
            // Arrange
            var id = A.Dummy<long>();
            var metadata = A.Dummy<StreamRecordMetadata>().DeepCloneWithStringSerializedId(id.ToStringInvariantPreferred());

            var serializer = new ObcAlwaysThrowingSerializer();

            var stream = new MemoryStandardStream(
                "test-stream-name",
                new SerializerRepresentation(SerializationKind.Json),
                SerializationFormat.String,
                new SpecifiedSerializerFactory(serializer));

            var expected = new StreamRecordMetadata<long>(
                id,
                metadata.SerializerRepresentation,
                metadata.TypeRepresentationOfId,
                metadata.TypeRepresentationOfObject,
                metadata.Tags,
                metadata.TimestampUtc,
                metadata.ObjectTimestampUtc);

            // Act
            var actual = metadata.ToStreamRecordMetadata<long>(stream);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        private class SpecifiedSerializerFactory : SerializerFactoryBase
        {
            private readonly ISerializer serializerToUse;

            public SpecifiedSerializerFactory(
                ISerializer serializerToUse)
            {
                this.serializerToUse = serializerToUse;
            }

            public override ISerializer BuildSerializer(
                SerializerRepresentation serializerRepresentation,
                VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
            {
                return this.serializerToUse;
            }
        }
    }
}
