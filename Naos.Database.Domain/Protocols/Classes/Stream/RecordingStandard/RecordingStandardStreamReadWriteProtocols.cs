// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamReadProtocols"/> and <see cref="IStreamWriteProtocols"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    public class RecordingStandardStreamReadWriteProtocols : IStreamReadProtocols, IStreamWriteProtocols
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamReadWriteProtocols"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamReadWriteProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetNextUniqueLongOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamWritingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            GetNextUniqueLongOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetNextUniqueLongOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamWritingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata> Execute(
            GetAllRecordsMetadataOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsMetadataOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamReadingProtocols().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordMetadata>> ExecuteAsync(
            GetAllRecordsMetadataOp operation)
        {
            var recording = new RecordedStreamOpExecution<GetAllRecordsMetadataOp>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamReadingProtocols().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }
    }
}