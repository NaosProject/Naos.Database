// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Naos.Database.Domain;
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
    /// TODO: Starting point for new project.
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
                    ProtocolJsonSerializationConfiguration>).ToRepresentation();

            SerializerRepresentation defaultSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configurationTypeRepresentation);

            var defaultSerializationFormat = SerializationFormat.String;

            var stream = new MemoryReadWriteStream(
                streamName,
                defaultSerializerRepresentation,
                defaultSerializationFormat,
                new JsonSerializerFactory());

            stream.Execute(new CreateStreamOp(stream.StreamRepresentation, ExistingStreamEncounteredStrategy.Skip));
            var key = stream.Name;
            var firstValue = "Testing again.";
            var firstObject = new MyObject(key, firstValue);
            var secondValue = "Testing again latest.";
            var secondObject = new MyObject(key, secondValue);
            for (int idx = 0;
                idx < 10;
                idx++)
            {
                stream.GetStreamWritingWithIdProtocols<string, MyObject>().Execute(new PutWithIdOp<string, MyObject>(firstObject.Id, firstObject, firstObject.Tags));
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                stream.GetStreamWritingWithIdProtocols<string, MyObject>().Execute(new PutWithIdOp<string, MyObject>(secondObject.Id, secondObject, secondObject.Tags));
                stopwatch.Stop();
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Put: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                stopwatch.Reset();
                stopwatch.Start();
                var my = stream.GetStreamReadingWithIdProtocols<string, MyObject>().Execute(new GetLatestObjectByIdOp<string, MyObject>(key));
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Get: {stopwatch.Elapsed.TotalMilliseconds} ms"));
                this.testOutputHelper.WriteLine(FormattableString.Invariant($"Key={my.Id}, Field={my.Field}"));
                my.Id.MustForTest().BeEqualTo(key);
            }

            stream.Execute(new DeleteStreamOp(stream.StreamRepresentation, ExistingStreamNotEncounteredStrategy.Throw));
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
