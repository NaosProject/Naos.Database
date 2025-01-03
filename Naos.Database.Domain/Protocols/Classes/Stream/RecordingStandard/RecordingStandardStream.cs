// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// A stream that records all operations executed against a backing stream.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public class RecordingStandardStream : IStandardStream
    {
        private readonly IStandardStream backingStream;
        private readonly List<RecordedStreamOpExecutionBase> recordedStreamOpExecutions = new List<RecordedStreamOpExecutionBase>();
        private readonly object recordedStreamOpExecutionsLockObject = new object();
        private int recordedStreamOpExecutionPosition = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStream"/> class.
        /// </summary>
        /// <param name="backingStream">The backing stream.</param>
        public RecordingStandardStream(
            IStandardStream backingStream)
        {
            backingStream.MustForArg(nameof(backingStream)).NotBeNull();

            this.backingStream = backingStream;
        }

        /// <inheritdoc />
        public string Name => this.backingStream.Name;

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation => this.backingStream.StreamRepresentation;

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols => this.backingStream.ResourceLocatorProtocols;

        /// <inheritdoc />
        public ISerializerFactory SerializerFactory => this.backingStream.SerializerFactory;

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation => this.backingStream.DefaultSerializerRepresentation;

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat => this.backingStream.DefaultSerializationFormat;

        /// <summary>
        /// Gets the backing stream.
        /// </summary>
        public IStandardStream BackingStream => this.backingStream;

        /// <summary>
        /// Gets the recorded stream operation executions.
        /// </summary>
        public IReadOnlyList<RecordedStreamOpExecutionBase> RecordedStreamOpExecutions
        {
            get
            {
                lock (this.recordedStreamOpExecutionsLockObject)
                {
                    return this.recordedStreamOpExecutions.ToList();
                }
            }
        }

        /// <summary>
        /// Gets the executed operations of the specified type in the order they were executed.
        /// </summary>
        /// <typeparam name="TStreamOp">The type of stream operation.</typeparam>
        /// <returns>
        /// The executed operations of the specified type in the order they were executed.
        /// </returns>
        public IReadOnlyList<TStreamOp> GetExecutedStreamOps<TStreamOp>()
            where TStreamOp : IOperation
        {
            var result = this.RecordedStreamOpExecutions
                .OfType<RecordedStreamOpExecution<TStreamOp>>()
                .Select(_ => _.Operation)
                .ToList();

            return result;
        }

        /// <summary>
        /// Gets the recorded stream operation executions of the specified type in the order they were recorded.
        /// </summary>
        /// <typeparam name="TStreamOp">The type of stream operation.</typeparam>
        /// <returns>
        /// The recorded stream operation executions of the specified type in the order they were recorded.
        /// </returns>
        public IReadOnlyList<RecordedStreamOpExecution<TStreamOp>> GetRecordedStreamOpExecutions<TStreamOp>()
            where TStreamOp : IOperation
        {
            var result = this.RecordedStreamOpExecutions
                .OfType<RecordedStreamOpExecution<TStreamOp>>()
                .ToList();

            return result;
        }

        /// <inheritdoc />
        public IStreamReadProtocols GetStreamReadingProtocols()
        {
            var result = new RecordingStandardStreamReadWriteProtocols(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>()
        {
            var result = new RecordingStandardStreamReadWriteProtocols<TObject>(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>()
        {
            return new RecordingStandardStreamReadWriteWithIdProtocols<TId>(this);
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>()
        {
            return new RecordingStandardStreamReadWriteWithIdProtocols<TId, TObject>(this);
        }

        /// <inheritdoc />
        public IStreamWriteProtocols GetStreamWritingProtocols()
        {
            var result =  new RecordingStandardStreamReadWriteProtocols(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            var result = new RecordingStandardStreamReadWriteProtocols<TObject>(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>()
        {
            var result = new RecordingStandardStreamReadWriteWithIdProtocols<TId>(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>()
        {
            var result = new RecordingStandardStreamReadWriteWithIdProtocols<TId, TObject>(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols()
        {
            var result = new RecordingStandardStreamManagementProtocols(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols()
        {
            var result = new RecordingStandardStreamHandlingProtocols(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>()
        {
            var result = new RecordingStandardStreamHandlingProtocols<TObject>(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>()
        {
            var result = new RecordingStandardStreamHandlingWithIdProtocols<TId>(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>()
        {
            var result = new RecordingStandardStreamHandlingWithIdProtocols<TId, TObject>(this);

            return result;
        }

        /// <inheritdoc />
        public IStreamDistributedMutexProtocols GetStreamDistributedMutexProtocols()
        {
            var result = new RecordingStandardStreamDistributedMutexProtocols(this);

            return result;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardGetDistinctStringSerializedIdsOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardGetInternalRecordIdsOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardGetLatestRecordOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public string Execute(
            StandardGetLatestStringSerializedObjectOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardGetLatestStringSerializedObjectOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public PutRecordResult Execute(
            StandardPutRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardPutRecordOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public long Execute(
            StandardGetNextUniqueLongOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardGetNextUniqueLongOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public CreateStreamResult Execute(
            StandardCreateStreamOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardCreateStreamOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            StandardDeleteStreamOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardDeleteStreamOp>(operation);
            this.RecordStreamOpExecution(recording);

            this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            StandardPruneStreamOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardPruneStreamOp>(operation);
            this.RecordStreamOpExecution(recording);

            this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public TryHandleRecordResult Execute(
            StandardTryHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardTryHandleRecordOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardGetHandlingStatusOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(
            StandardGetHandlingHistoryOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardGetHandlingHistoryOp>(operation);
            this.RecordStreamOpExecution(recording);

            var result = this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            StandardUpdateHandlingStatusForStreamOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardUpdateHandlingStatusForStreamOp>(operation);
            this.RecordStreamOpExecution(recording);

            this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<StandardUpdateHandlingStatusForRecordOp>(operation);
            this.RecordStreamOpExecution(recording);

            this.BackingStream.Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <summary>
        /// Records the execution of a stream operation.
        /// </summary>
        /// <param name="recordedStreamOpExecution">The recorded stream operation execution.</param>
        internal void RecordStreamOpExecution(
            RecordedStreamOpExecutionBase recordedStreamOpExecution)
        {
            recordedStreamOpExecution.MustForArg(nameof(recordedStreamOpExecution)).NotBeNull();

            lock (this.recordedStreamOpExecutionsLockObject)
            {
                this.recordedStreamOpExecutionPosition++;

                this.recordedStreamOpExecutions.Add(recordedStreamOpExecution);
                recordedStreamOpExecution.RecordPosition(this.recordedStreamOpExecutionPosition);
            }
        }
    }
}
