// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamReadWithIdProtocols{TId}"/> and <see cref="IStreamWriteWithIdProtocols{TId}"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    internal class RecordingStandardStreamReadWriteWithIdProtocols<TId> : IStreamReadWithIdProtocols<TId>, IStreamWriteWithIdProtocols<TId>
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamReadWriteWithIdProtocols{TId}"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamReadWriteWithIdProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            GetLatestRecordByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestRecordByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId>> ExecuteAsync(
            GetLatestRecordByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestRecordByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordWithId<TId>> Execute(
            GetAllRecordsByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordWithId<TId>>> ExecuteAsync(
            GetAllRecordsByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public StreamRecordMetadata<TId> Execute(
            GetLatestRecordMetadataByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestRecordMetadataByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordMetadata<TId>> ExecuteAsync(
            GetLatestRecordMetadataByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestRecordMetadataByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata<TId>> Execute(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsMetadataByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordMetadata<TId>>> ExecuteAsync(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsMetadataByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public bool Execute(
            DoesAnyExistByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<DoesAnyExistByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<DoesAnyExistByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public string Execute(
            GetLatestStringSerializedObjectByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestStringSerializedObjectByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<string> ExecuteAsync(
            GetLatestStringSerializedObjectByIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetLatestStringSerializedObjectByIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<TId> Execute(
            GetDistinctIdsOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetDistinctIdsOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<TId>> ExecuteAsync(
            GetDistinctIdsOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetDistinctIdsOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata<TId>> Execute(
            GetAllRecordsMetadataOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsMetadataOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordMetadata<TId>>> ExecuteAsync(
            GetAllRecordsMetadataOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsMetadataOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingWithIdProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }
    }
}