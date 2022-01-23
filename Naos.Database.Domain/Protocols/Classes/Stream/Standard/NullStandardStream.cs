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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamWriteProtocols GetStreamWritingProtocols()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>()
        {
            throw new NotImplementedException();
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
        public IReadOnlyCollection<StringSerializedIdentifier> Execute(StandardGetDistinctStringSerializedIdsOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Execute(StandardDoesAnyExistByIdOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecord Execute(StandardGetRecordByInternalRecordIdOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IReadOnlyCollection<long> Execute(StandardGetRecordIdsOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecord Execute(StandardGetLatestRecordByTagsOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecord Execute(StandardGetLatestRecordOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecordMetadata Execute(StandardGetLatestRecordMetadataByIdOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecord Execute(StandardGetLatestRecordByIdOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string Execute(StandardGetLatestStringSerializedObjectByIdOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public PutRecordResult Execute(StandardPutRecordOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public long Execute(StandardGetNextUniqueLongOp operation)
        {
            throw new NotImplementedException();
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
