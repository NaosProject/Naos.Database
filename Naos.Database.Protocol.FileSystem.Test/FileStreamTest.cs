// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Protocol.SqlServer.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Naos.Database.Domain;
    using Naos.Database.Protocol.FileSystem;
    using Naos.Database.Protocol.Memory;
    using Naos.Protocol.Domain;
    using Naos.Protocol.Serialization.Json;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using Xunit;
    using Xunit.Abstractions;

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
        public void Create_Put_Get_Delete___Given_valid_data___Should_roundtrip_to_file_system()
        {
            var streamName = "FileStream_ReadWriteTest";

            var testingFilePath = Path.GetTempPath();
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(SerializationKind.Json);

            var defaultSerializationFormat = SerializationFormat.String;

            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.GetStreamManagementProtocols().Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Skip));
            var key = stream.Name + "Key";
            var firstValue = "Testing again.";
            var firstObject = new MyObject(key, firstValue);
            var secondValue = "Testing again latest.";
            var secondObject = new MyObject(key, secondValue);
            stream.GetStreamWritingProtocols<string, MyObject>().Execute(new PutOp<string, MyObject>(firstObject.Id, firstObject, firstObject.Tags));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            stream.GetStreamWritingProtocols<string, MyObject>().Execute(new PutOp<string, MyObject>(secondObject.Id, secondObject, secondObject.Tags));
            stopwatch.Stop();
            this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
            stopwatch.Reset();
            stopwatch.Start();
            var my1 = stream.GetStreamReadingProtocols<string, MyObject>().Execute(new GetLatestByIdAndTypeOp<string, MyObject>(key));
            this.testOutputHelper.WriteLine(FormattableString.Invariant($"Get: {stopwatch.Elapsed.TotalMilliseconds} ms"));
            this.testOutputHelper.WriteLine(FormattableString.Invariant($"Key={my1.Id}, Field={my1.Field}"));
            my1.Id.MustForTest().BeEqualTo(key);
            var my2 = stream.GetStreamReadingProtocols<string, MyObject>().Execute(new GetLatestByIdAndTypeOp<string, MyObject>(key));
            this.testOutputHelper.WriteLine(FormattableString.Invariant($"Get: {stopwatch.Elapsed.TotalMilliseconds} ms"));
            this.testOutputHelper.WriteLine(FormattableString.Invariant($"Key={my2.Id}, Field={my2.Field}"));
            my2.Id.MustForTest().BeEqualTo(key);

            stream.GetStreamManagementProtocols().Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
        }

        [Fact]
        public static void GetNextUniqueLongOp___Given_valid_data___Should_execute_and_persist_to_file_system()
        {
            var streamName = "FileStreamTest_GetNextUniqueLong";

            var testingFilePath = Path.GetTempPath();
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol(fileSystemLocator);

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(SerializationKind.Json);

            var defaultSerializationFormat = SerializationFormat.String;

            var stream = new FileReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.GetStreamManagementProtocols().Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Overwrite));

            var nextLong = stream.GetStreamWritingProtocols().Execute(new GetNextUniqueLongOp());
            nextLong.MustForTest().BeEqualTo(1L);
            var nextNextLong = stream.GetStreamWritingProtocols().Execute(new GetNextUniqueLongOp());
            nextNextLong.MustForTest().BeEqualTo(2L);

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