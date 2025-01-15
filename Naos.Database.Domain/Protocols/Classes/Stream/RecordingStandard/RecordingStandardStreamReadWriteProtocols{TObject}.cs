// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamReadProtocols{TObject}"/> and <see cref="IStreamWriteProtocols{TObject}"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    internal class RecordingStandardStreamReadWriteProtocols<TObject> : IStreamReadProtocols<TObject>, IStreamWriteProtocols<TObject>
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamReadWriteProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamReadWriteProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestObjectOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingProtocols<TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestObjectOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingProtocols<TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public StreamRecord<TObject> Execute(
            GetLatestRecordOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestRecordOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingProtocols<TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecord<TObject>> ExecuteAsync(
            GetLatestRecordOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestRecordOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingProtocols<TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public long? Execute(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<PutAndReturnInternalRecordIdOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamWritingProtocols<TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<long?> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<PutAndReturnInternalRecordIdOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamWritingProtocols<TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<PutOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamWritingProtocols<TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<PutOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamWritingProtocols<TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public IReadOnlyList<TObject> Execute(
            GetAllObjectsOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllObjectsOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingProtocols<TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetAllObjectsOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllObjectsOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingProtocols<TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecord<TObject>> Execute(
            GetAllRecordsOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingProtocols<TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecord<TObject>>> ExecuteAsync(
            GetAllRecordsOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingProtocols<TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }
    }
}