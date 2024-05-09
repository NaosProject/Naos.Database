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
        /// <param name="name">The name.</param>
        /// <param name="streamRepresentation">The stream representation.</param>
        /// <param name="resourceLocatorProtocols">The resource locator protocols.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="readStream">The read stream to delegate read operations to.</param>
        /// <param name="writeStream">The write stream to delegate write operations to.</param>
        /// <param name="handleStream">The handle stream to delegate handle operations to.</param>
        /// <param name="managementStream">The management stream to delegate management operations to.</param>
        public DelegatedStandardStream(
            string name,
            IStreamRepresentation streamRepresentation,
            IResourceLocatorProtocols resourceLocatorProtocols,
            ISerializerFactory serializerFactory,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            IStandardStream readStream,
            IStandardStream writeStream,
            IStandardStream handleStream,
            IStandardStream managementStream)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            resourceLocatorProtocols.MustForArg(nameof(resourceLocatorProtocols)).NotBeNull();
            serializerFactory.MustForArg(nameof(serializerFactory)).NotBeNull();
            defaultSerializerRepresentation.MustForArg(nameof(defaultSerializerRepresentation)).NotBeNull();
            defaultSerializationFormat.MustForArg(nameof(defaultSerializationFormat)).NotBeEqualTo(SerializationFormat.Invalid);
            readStream.MustForArg(nameof(readStream)).NotBeNull();
            writeStream.MustForArg(nameof(writeStream)).NotBeNull();
            handleStream.MustForArg(nameof(handleStream)).NotBeNull();
            managementStream.MustForArg(nameof(managementStream)).NotBeNull();

            this.readStream = readStream;
            this.writeStream = writeStream;
            this.handleStream = handleStream;
            this.managementStream = managementStream;

            this.Name = name;
            this.StreamRepresentation = streamRepresentation;
            this.ResourceLocatorProtocols = resourceLocatorProtocols;
            this.SerializerFactory = serializerFactory;
            this.DefaultSerializerRepresentation = defaultSerializerRepresentation;
            this.DefaultSerializationFormat = defaultSerializationFormat;
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols { get; private set; }

        /// <inheritdoc />
        public ISerializerFactory SerializerFactory { get; private set; }

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat { get; private set; }

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
