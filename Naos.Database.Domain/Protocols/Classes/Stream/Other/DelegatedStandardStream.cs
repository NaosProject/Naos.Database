// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegatedStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A fully delegated version of <see cref="IStandardStream"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public class DelegatedStandardStream : IStandardStream
    {
        private readonly IStandardStream readStream;
        private readonly IStandardStream writeStream;
        private readonly IStandardStream handleStream;
        private readonly IStandardStream managementStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatedStandardStream"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="streamRepresentation">The stream representation.</param>
        /// <param name="readStream">The read stream to delegate read operations to.</param>
        /// <param name="writeStream">The write stream to delegate write operations to.</param>
        /// <param name="handleStream">The handle stream to delegate handle operations to.</param>
        /// <param name="managementStream">The management stream to delegate management operations to.</param>
        public DelegatedStandardStream(
            string name,
            IStreamRepresentation streamRepresentation,
            IStandardStream readStream,
            IStandardStream writeStream,
            IStandardStream handleStream,
            IStandardStream managementStream)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            readStream.MustForArg(nameof(readStream)).NotBeNull();
            writeStream.MustForArg(nameof(writeStream)).NotBeNull();
            handleStream.MustForArg(nameof(handleStream)).NotBeNull();
            managementStream.MustForArg(nameof(managementStream)).NotBeNull();

            var streams = new[]
            {
                readStream,
                writeStream,
                handleStream,
                managementStream,
            };

            var notNullNorThrowingStreams = streams.Where(_ =>
                    (_.GetType() != typeof(NullStandardStream)) &&
                    (_.GetType() != typeof(AlwaysThrowingStandardStream)))
                .ToList();

            if (notNullNorThrowingStreams.Any())
            {
                // There is at least one non-null, non-throwing stream.  Make sure all of them
                // have the same factory and id serializer using reference equality.
                this.SerializerFactory = notNullNorThrowingStreams.Select(_ => _.SerializerFactory).Single();
                this.IdSerializer = notNullNorThrowingStreams.Select(_ => _.IdSerializer).Single();
            }
            else
            {
                // All of the streams are null or throwing streams.
                var nullStandardStream = streams.FirstOrDefault(_ => _.GetType() == typeof(NullStandardStream));

                if (nullStandardStream == null)
                {
                    // All of the streams are throwing.  Use the first throwing stream's factory and id serializer
                    // (which will be the same across all throwing  streams and hence why we just choose the first).
                    this.SerializerFactory = streams.First().SerializerFactory;
                    this.IdSerializer = streams.First().IdSerializer;
                }
                else
                {
                    // At least one stream is null.  Use the first null stream's factory and id serializer
                    // (which will be the same across all null streams and hence why we just choose the first).
                    this.SerializerFactory = nullStandardStream.SerializerFactory;
                    this.IdSerializer = nullStandardStream.IdSerializer;
                }
            }

            this.Name = name;
            this.StreamRepresentation = streamRepresentation;
            this.readStream = readStream;
            this.writeStream = writeStream;
            this.handleStream = handleStream;
            this.managementStream = managementStream;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation { get; }

        /// <inheritdoc />
        public ISerializerFactory SerializerFactory { get; }

        /// <inheritdoc />
        public IStringSerializeAndDeserialize IdSerializer { get; }

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols => throw new NotSupportedException("You can have different resource locator configurations based on the access kind and there's no good way to choose among the locators.  We could possibly return all the locators across all the streams, but waiting for this scenario to come up so we can further discuss.");

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation => this.writeStream.DefaultSerializerRepresentation;

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat => this.writeStream.DefaultSerializationFormat;

        /// <inheritdoc />
        public IStreamReadProtocols GetStreamReadingProtocols()
        {
            return this.readStream.GetStreamReadingProtocols();
        }

        /// <inheritdoc />
        public IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>()
        {
            return this.readStream.GetStreamReadingProtocols<TObject>();
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>()
        {
            return this.readStream.GetStreamReadingWithIdProtocols<TId>();
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>()
        {
            return this.readStream.GetStreamReadingWithIdProtocols<TId, TObject>();
        }

        /// <inheritdoc />
        public IStreamWriteProtocols GetStreamWritingProtocols()
        {
            return this.writeStream.GetStreamWritingProtocols();
        }

        /// <inheritdoc />
        public IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            return this.writeStream.GetStreamWritingProtocols<TObject>();
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>()
        {
            return this.writeStream.GetStreamWritingWithIdProtocols<TId>();
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>()
        {
            return this.writeStream.GetStreamWritingWithIdProtocols<TId, TObject>();
        }

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols()
        {
            return this.managementStream.GetStreamManagementProtocols();
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols()
        {
            return this.handleStream.GetStreamRecordHandlingProtocols();
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>()
        {
            return this.handleStream.GetStreamRecordHandlingProtocols<TObject>();
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>()
        {
            return this.handleStream.GetStreamRecordWithIdHandlingProtocols<TId>();
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>()
        {
            return this.handleStream.GetStreamRecordWithIdHandlingProtocols<TId, TObject>();
        }

        /// <inheritdoc />
        public IStreamDistributedMutexProtocols GetStreamDistributedMutexProtocols()
        {
            return this.handleStream.GetStreamDistributedMutexProtocols();
        }

        /// <inheritdoc />
        public IReadOnlyCollection<StringSerializedIdentifier> Execute(StandardGetDistinctStringSerializedIdsOp operation)
        {
            return this.readStream.Execute(operation);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<long> Execute(StandardGetInternalRecordIdsOp operation)
        {
            return this.readStream.Execute(operation);
        }

        /// <inheritdoc />
        public StreamRecord Execute(StandardGetLatestRecordOp operation)
        {
            return this.readStream.Execute(operation);
        }

        /// <inheritdoc />
        public string Execute(StandardGetLatestStringSerializedObjectOp operation)
        {
            return this.readStream.Execute(operation);
        }

        /// <inheritdoc />
        public PutRecordResult Execute(StandardPutRecordOp operation)
        {
            return this.writeStream.Execute(operation);
        }

        /// <inheritdoc />
        public long Execute(StandardGetNextUniqueLongOp operation)
        {
            return this.writeStream.Execute(operation);
        }

        /// <inheritdoc />
        public CreateStreamResult Execute(StandardCreateStreamOp operation)
        {
            return this.managementStream.Execute(operation);
        }

        /// <inheritdoc />
        public void Execute(StandardDeleteStreamOp operation)
        {
            this.managementStream.Execute(operation);
        }

        /// <inheritdoc />
        public void Execute(StandardPruneStreamOp operation)
        {
            this.managementStream.Execute(operation);
        }

        /// <inheritdoc />
        public TryHandleRecordResult Execute(StandardTryHandleRecordOp operation)
        {
            return this.handleStream.Execute(operation);
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<long, HandlingStatus> Execute(StandardGetHandlingStatusOp operation)
        {
            return this.handleStream.Execute(operation);
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(StandardGetHandlingHistoryOp operation)
        {
            return this.handleStream.Execute(operation);
        }

        /// <inheritdoc />
        public void Execute(StandardUpdateHandlingStatusForStreamOp operation)
        {
            this.handleStream.Execute(operation);
        }

        /// <inheritdoc />
        public void Execute(StandardUpdateHandlingStatusForRecordOp operation)
        {
            this.handleStream.Execute(operation);
        }
    }
}
