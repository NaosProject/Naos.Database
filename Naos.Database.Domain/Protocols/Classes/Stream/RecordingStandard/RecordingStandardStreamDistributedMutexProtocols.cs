// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamDistributedMutexProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamDistributedMutexProtocols"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    public class RecordingStandardStreamDistributedMutexProtocols : IStreamDistributedMutexProtocols
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamDistributedMutexProtocols"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamDistributedMutexProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public ReleaseMutexOp Execute(
            WaitOneOp operation)
        {
            var recording = new RecordedStreamOpExecution<WaitOneOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamDistributedMutexProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<ReleaseMutexOp> ExecuteAsync(
            WaitOneOp operation)
        {
            var recording = new RecordedStreamOpExecution<WaitOneOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamDistributedMutexProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            ReleaseMutexOp operation)
        {
            var recording = new RecordedStreamOpExecution<ReleaseMutexOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            this.recordingStandardStream.BackingStream.GetStreamDistributedMutexProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ReleaseMutexOp operation)
        {
            var recording = new RecordedStreamOpExecution<ReleaseMutexOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            await this.recordingStandardStream.BackingStream.GetStreamDistributedMutexProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
        }
    }
}