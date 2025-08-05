// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlwaysThrowingStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A null object pattern implementation of an <see cref="IStandardStream"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public class AlwaysThrowingStandardStream : IStandardStream
    {
        private const string ExceptionMessage = "This stream always throws.";

        /// <inheritdoc />
        public string Name => throw new InvalidOperationException(ExceptionMessage);

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation => throw new InvalidOperationException(ExceptionMessage);

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols => throw new InvalidOperationException(ExceptionMessage);

        /// <inheritdoc />
        public ISerializerFactory SerializerFactory => throw new InvalidOperationException(ExceptionMessage);

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation => throw new InvalidOperationException(ExceptionMessage);

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat => throw new InvalidOperationException(ExceptionMessage);

        /// <inheritdoc />
        public IStringSerializeAndDeserialize IdSerializer => throw new InvalidOperationException(ExceptionMessage);

        /// <inheritdoc />
        public IStreamReadProtocols GetStreamReadingProtocols()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamWriteProtocols GetStreamWritingProtocols()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamDistributedMutexProtocols GetStreamDistributedMutexProtocols()
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public string Execute(
            StandardGetLatestStringSerializedObjectOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public PutRecordResult Execute(
            StandardPutRecordOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public long Execute(
            StandardGetNextUniqueLongOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public CreateStreamResult Execute(
            StandardCreateStreamOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(
            StandardDeleteStreamOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(
            StandardPruneStreamOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public TryHandleRecordResult Execute(
            StandardTryHandleRecordOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(
            StandardGetHandlingHistoryOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(
            StandardUpdateHandlingStatusForStreamOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }
    }
}
