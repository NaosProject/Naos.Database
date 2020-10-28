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
    /// Tests for <see cref="FileStream{TKey}"/>.
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
            var streamName = "StreamName32";

            var testingFilePath = Path.GetTempPath();
            var fileSystemLocator = new FileSystemDatabaseLocator(testingFilePath);
            var resourceLocatorProtocol = new SingleResourceLocatorProtocol<string>(fileSystemLocator);

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(SerializationKind.Json);

            var defaultSerializationFormat = SerializationFormat.String;

            var stream = new FileStream<string>(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory(),
                resourceLocatorProtocol);

            stream.Execute(new CreateStreamOp<string>(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Skip));
            var key = stream.Name + "Key";
            var firstValue = "Testing again.";
            var secondValue = "Testing again latest.";
            stream.BuildPutProtocol<MyObject>().Execute(new PutOp<MyObject>(new MyObject(key, firstValue)));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            stream.BuildPutProtocol<MyObject>().Execute(new PutOp<MyObject>(new MyObject(key, secondValue)));
            stopwatch.Stop();
            this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
            stopwatch.Reset();
            stopwatch.Start();
            var my = stream.BuildGetLatestByIdAndTypeProtocol<MyObject>().Execute(new GetLatestByIdAndTypeOp<string, MyObject>(key));
            this.testOutputHelper.WriteLine(FormattableString.Invariant($"Get: {stopwatch.Elapsed.TotalMilliseconds} ms"));
            this.testOutputHelper.WriteLine(FormattableString.Invariant($"Key={my.Id}, Field={my.Field}"));
            my.Id.MustForTest().BeEqualTo(key);

            stream.Execute(new DeleteStreamOp<string>(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
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