// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamHandlingProtocols{TObject}.cs" company="Naos Project">
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
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class RecordingStandardStreamHandlingProtocols<TObject> : IStreamRecordHandlingProtocols<TObject>
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamHandlingProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamHandlingProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public StreamRecord<TObject> Execute(
            TryHandleRecordOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<TryHandleRecordOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols<TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecord<TObject>> ExecuteAsync(
            TryHandleRecordOp<TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<TryHandleRecordOp<TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamRecordHandlingProtocols<TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }
    }
}