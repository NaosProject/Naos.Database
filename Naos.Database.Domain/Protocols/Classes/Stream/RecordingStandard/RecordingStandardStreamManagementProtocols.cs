// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamManagementProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamManagementProtocols"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    public class RecordingStandardStreamManagementProtocols : IStreamManagementProtocols
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamManagementProtocols"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamManagementProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordDateOp operation)
        {
            var recording = new RecordedStreamOpExecution<PruneBeforeInternalRecordDateOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamManagementProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordDateOp operation)
        {
            var recording = new RecordedStreamOpExecution<PruneBeforeInternalRecordDateOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamManagementProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordIdOp operation)
        {
            var recording = new RecordedStreamOpExecution<PruneBeforeInternalRecordIdOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamManagementProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordIdOp operation)
        {
            var recording = new RecordedStreamOpExecution<PruneBeforeInternalRecordIdOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamManagementProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }
    }
}