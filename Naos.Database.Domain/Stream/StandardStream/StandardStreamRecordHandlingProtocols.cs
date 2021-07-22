// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamRecordHandlingProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Set of protocols to handle a record in a stream.
    /// </summary>
    /// <seealso cref="IStreamReadProtocols" />
    /// <seealso cref="IStreamWriteProtocols" />
    public class StandardStreamRecordHandlingProtocols :
        IStreamRecordHandlingProtocols
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
        private readonly IStandardReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamRecordHandlingProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamRecordHandlingProtocols(
            IStandardReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();
            this.stream = stream;
        }

        /// <inheritdoc />
        public TryHandleRecordResult Execute(
            TryHandleRecordOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<TryHandleRecordResult> ExecuteAsync(
            TryHandleRecordOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOfRecordOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordHandlingEntry>> ExecuteAsync(
            GetHandlingHistoryOfRecordOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOfRecordsByIdOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<HandlingStatus> ExecuteAsync(
            GetHandlingStatusOfRecordsByIdOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOfRecordSetByTagOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<HandlingStatus> ExecuteAsync(
            GetHandlingStatusOfRecordSetByTagOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            BlockRecordHandlingOp operation)
        {
            this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            BlockRecordHandlingOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            CancelBlockedRecordHandlingOp operation)
        {
            this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CancelBlockedRecordHandlingOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            CancelHandleRecordExecutionRequestOp operation)
        {
            this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CancelHandleRecordExecutionRequestOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            CancelRunningHandleRecordExecutionOp operation)
        {
            this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CancelRunningHandleRecordExecutionOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            CompleteRunningHandleRecordExecutionOp operation)
        {
            this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CompleteRunningHandleRecordExecutionOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            FailRunningHandleRecordExecutionOp operation)
        {
            this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            FailRunningHandleRecordExecutionOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            SelfCancelRunningHandleRecordExecutionOp operation)
        {
            this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            SelfCancelRunningHandleRecordExecutionOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            RetryFailedHandleRecordExecutionOp operation)
        {
            this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            RetryFailedHandleRecordExecutionOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }
    }
}
