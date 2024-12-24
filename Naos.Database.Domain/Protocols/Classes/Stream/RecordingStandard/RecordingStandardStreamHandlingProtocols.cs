// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamHandlingProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamRecordHandlingProtocols"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    public class RecordingStandardStreamHandlingProtocols : IStreamRecordHandlingProtocols
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamHandlingProtocols"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamHandlingProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetHandlingHistoryOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordHandlingEntry>> ExecuteAsync(
            GetHandlingHistoryOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetHandlingHistoryOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetHandlingStatusOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<HandlingStatus> ExecuteAsync(
            GetHandlingStatusOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetHandlingStatusOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusByIdsOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetCompositeHandlingStatusByIdsOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<CompositeHandlingStatus> ExecuteAsync(
            GetCompositeHandlingStatusByIdsOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetCompositeHandlingStatusByIdsOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusByTagsOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetCompositeHandlingStatusByTagsOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<CompositeHandlingStatus> ExecuteAsync(
            GetCompositeHandlingStatusByTagsOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetCompositeHandlingStatusByTagsOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            DisableHandlingForStreamOp operation)
        {
            var recording = new RecordedStreamOpExecution<DisableHandlingForStreamOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DisableHandlingForStreamOp operation)
        {
            var recording = new RecordedStreamOpExecution<DisableHandlingForStreamOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            EnableHandlingForStreamOp operation)
        {
            var recording = new RecordedStreamOpExecution<EnableHandlingForStreamOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            EnableHandlingForStreamOp operation)
        {
            var recording = new RecordedStreamOpExecution<EnableHandlingForStreamOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            DisableHandlingForRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<DisableHandlingForRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DisableHandlingForRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<DisableHandlingForRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            CancelRunningHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<CancelRunningHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CancelRunningHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<CancelRunningHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            CompleteRunningHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<CompleteRunningHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CompleteRunningHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<CompleteRunningHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            FailRunningHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<FailRunningHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            FailRunningHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<FailRunningHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            ArchiveFailureToHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<ArchiveFailureToHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ArchiveFailureToHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<ArchiveFailureToHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            ResetFailedHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<ResetFailedHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ResetFailedHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<ResetFailedHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            ResetCompletedHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<ResetCompletedHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ResetCompletedHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<ResetCompletedHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            SelfCancelRunningHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<SelfCancelRunningHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            SelfCancelRunningHandleRecordOp operation)
        {
            var recording = new RecordedStreamOpExecution<SelfCancelRunningHandleRecordOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }
    }
}