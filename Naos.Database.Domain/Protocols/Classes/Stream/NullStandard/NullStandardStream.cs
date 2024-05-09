// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A null object pattern implementation of an <see cref="IStandardStream"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public class NullStandardStream : IStandardStream
    {
        private static readonly Random Random = new Random();

        /// <inheritdoc />
        public string Name => nameof(NullStandardStream);

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation => new NullStreamRepresentation();

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols => new NullResourceLocatorProtocols();

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = NaosSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
        public ISerializerFactory SerializerFactory => throw new NotImplementedException();

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation => null;

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat => SerializationFormat.Invalid;

        /// <inheritdoc />
        public IStreamReadProtocols GetStreamReadingProtocols()
        {
            return new NullStandardStreamReadWriteProtocols(this);
        }

        /// <inheritdoc />
        public IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>()
        {
            return new NullStandardStreamReadWriteProtocols<TObject>(this);
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>()
        {
            return new NullStandardStreamReadWriteWithIdProtocols<TId>(this);
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>()
        {
            return new NullStandardStreamReadWriteWithIdProtocols<TId, TObject>(this);
        }

        /// <inheritdoc />
        public IStreamWriteProtocols GetStreamWritingProtocols()
        {
            return new NullStandardStreamReadWriteProtocols(this);
        }

        /// <inheritdoc />
        public IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            return new NullStandardStreamReadWriteProtocols<TObject>(this);
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>()
        {
            return new NullStandardStreamReadWriteWithIdProtocols<TId>(this);
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>()
        {
            return new NullStandardStreamReadWriteWithIdProtocols<TId, TObject>(this);
        }

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamDistributedMutexProtocols GetStreamDistributedMutexProtocols()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IReadOnlyCollection<StringSerializedIdentifier> Execute(StandardGetDistinctStringSerializedIdsOp operation)
        {
            return new List<StringSerializedIdentifier>();
        }

        /// <inheritdoc />
        public IReadOnlyCollection<long> Execute(StandardGetInternalRecordIdsOp operation)
        {
            return new List<long>();
        }

        /// <inheritdoc />
        public StreamRecord Execute(StandardGetLatestRecordOp operation)
        {
            return null;
        }

        /// <inheritdoc />
        public string Execute(StandardGetLatestStringSerializedObjectOp operation)
        {
            return null;
        }

        /// <inheritdoc />
        public PutRecordResult Execute(StandardPutRecordOp operation)
        {
            return new PutRecordResult(null);
        }

        /// <inheritdoc />
        public long Execute(StandardGetNextUniqueLongOp operation)
        {
            var result = Random.Next();
            return result;
        }

        /// <inheritdoc />
        public CreateStreamResult Execute(StandardCreateStreamOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(StandardDeleteStreamOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(StandardPruneStreamOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TryHandleRecordResult Execute(StandardTryHandleRecordOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<long, HandlingStatus> Execute(StandardGetHandlingStatusOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(StandardGetHandlingHistoryOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(StandardUpdateHandlingStatusForStreamOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(StandardUpdateHandlingStatusForRecordOp operation)
        {
            throw new NotImplementedException();
        }
    }
}
