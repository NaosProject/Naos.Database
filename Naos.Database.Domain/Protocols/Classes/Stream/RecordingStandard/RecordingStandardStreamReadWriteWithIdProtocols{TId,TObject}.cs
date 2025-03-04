// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamReadWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamReadWithIdProtocols{TId, TObject}"/> and <see cref="IStreamWriteWithIdProtocols{TId, TObject}"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    internal class RecordingStandardStreamReadWriteWithIdProtocols<TId, TObject> : IStreamReadWithIdProtocols<TId, TObject>, IStreamWriteWithIdProtocols<TId, TObject>
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamReadWriteWithIdProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamReadWriteWithIdProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestObjectByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestObjectByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<TObject> Execute(
            GetLatestObjectsByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestObjectsByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetLatestObjectsByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestObjectsByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestRecordByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestRecordByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public long? Execute(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamWritingWithIdProtocols<TId, TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<long?> ExecuteAsync(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamWritingWithIdProtocols<TId, TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            PutWithIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<PutWithIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamWritingWithIdProtocols<TId, TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutWithIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<PutWithIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamWritingWithIdProtocols<TId, TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public bool Execute(
            DoesAnyExistByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<DoesAnyExistByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<DoesAnyExistByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<TObject> Execute(
            GetAllObjectsByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllObjectsByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetAllObjectsByIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllObjectsByIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId, TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }
    }
}