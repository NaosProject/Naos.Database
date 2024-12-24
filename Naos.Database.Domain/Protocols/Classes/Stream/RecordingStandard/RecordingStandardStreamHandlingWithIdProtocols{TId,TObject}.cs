// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordingStandardStreamHandlingWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Implementation of <see cref="IStreamRecordWithIdHandlingProtocols{TId, TObject}"/> that
    /// records all operations executed against a backing stream.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class RecordingStandardStreamHandlingWithIdProtocols<TId, TObject> : IStreamRecordWithIdHandlingProtocols<TId, TObject>
    {
        private readonly RecordingStandardStream recordingStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingStandardStreamHandlingWithIdProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="recordingStandardStream">The null standard stream.</param>
        public RecordingStandardStreamHandlingWithIdProtocols(
            RecordingStandardStream recordingStandardStream)
        {
            this.recordingStandardStream = recordingStandardStream;

            recordingStandardStream.MustForArg(nameof(recordingStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<TryHandleRecordWithIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = this.recordingStandardStream.BackingStream.GetStreamRecordWithIdHandlingProtocols<TId, TObject>().Execute(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            var recording = new RecordedStreamOpExecution<TryHandleRecordWithIdOp<TId, TObject>>(operation);
            this.recordingStandardStream.RecordStreamOpExecution(recording);

            var result = await this.recordingStandardStream.BackingStream.GetStreamRecordWithIdHandlingProtocols<TId, TObject>().ExecuteAsync(operation);

            recording.RecordTimestampPostExecution();
            return result;
        }
    }
}