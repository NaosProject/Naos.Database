// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamHandlingWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamRecordHandlingProtocols{TObject}"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    public class RecordingStandardStreamHandlingWithIdProtocols<TId> : IStreamRecordWithIdHandlingProtocols<TId>
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamHandlingWithIdProtocols{TId}"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamHandlingWithIdProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            TryHandleRecordWithIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<TryHandleRecordWithIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamRecordWithIdHandlingProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId>> ExecuteAsync(
            TryHandleRecordWithIdOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<TryHandleRecordWithIdOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamRecordWithIdHandlingProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusByIdsOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetCompositeHandlingStatusByIdsOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamRecordWithIdHandlingProtocols<TId>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<CompositeHandlingStatus> ExecuteAsync(
            GetCompositeHandlingStatusByIdsOp<TId> operation)
        {
            var recording = new RecordedStreamOpExecution<GetCompositeHandlingStatusByIdsOp<TId>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamRecordWithIdHandlingProtocols<TId>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }
    }
}