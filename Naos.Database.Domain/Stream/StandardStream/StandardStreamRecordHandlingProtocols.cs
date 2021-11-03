// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamRecordHandlingProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly IStandardStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamRecordHandlingProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamRecordHandlingProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();
            this.stream = stream;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOfRecordOp operation)
        {
            var standardizedOperation = new StandardGetHandlingHistoryOfRecordOp(
                operation.InternalRecordId,
                operation.Concern);

            var result = this.stream.Execute(standardizedOperation);
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
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusOfRecordsByIdOp operation)
        {
            var standardizedOperation = new StandardGetRecordHandlingStatusOp(
                operation.Concern,
                null,
                operation.IdsToMatch,
                operation.VersionMatchStrategy,
                null,
                null);

            var handlingStatuses = this.stream.Execute(standardizedOperation);

            var result = handlingStatuses.ToCompositeHandlingStatus();

            return result;
        }

        /// <inheritdoc />
        public async Task<CompositeHandlingStatus> ExecuteAsync(
            GetCompositeHandlingStatusOfRecordsByIdOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusOfRecordsByTagOp operation)
        {
            var standardizedOperation = new StandardGetRecordHandlingStatusOp(
                operation.Concern,
                null,
                null,
                null,
                operation.TagsToMatch,
                operation.TagMatchStrategy,
                null);

            var handlingStatuses = this.stream.Execute(standardizedOperation);

            var result = handlingStatuses.ToCompositeHandlingStatus();

            return result;
        }

        /// <inheritdoc />
        public async Task<CompositeHandlingStatus> ExecuteAsync(
            GetCompositeHandlingStatusOfRecordsByTagOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOfRecordByInternalRecordIdOp operation)
        {
            var standardizedOperation = new StandardGetRecordHandlingStatusOp(
                operation.Concern,
                operation.InternalRecordId,
                null,
                null,
                null,
                null);

            var handlingStatuses = this.stream.Execute(standardizedOperation);

            var result = handlingStatuses.Single();

            return result;
        }

        /// <inheritdoc />
        public async Task<HandlingStatus> ExecuteAsync(
            GetHandlingStatusOfRecordByInternalRecordIdOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            DisableRecordHandlingForStreamOp operation)
        {
            var standardizedOperation = new StandardUpdateStreamHandlingOp(
                HandlingStatus.DisabledForStream,
                new[]
                {
                    HandlingStatus.AvailableByDefault,
                },
                operation.Details);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DisableRecordHandlingForStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            EnableRecordHandlingForStreamOp operation)
        {
            var standardizedOperation = new StandardUpdateStreamHandlingOp(
                HandlingStatus.AvailableByDefault,
                new[]
                {
                    HandlingStatus.DisabledForStream,
                },
                operation.Details);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            EnableRecordHandlingForStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            DisableRecordHandlingForRecordOp operation)
        {
            var standardizedOperation = new StandardUpdateRecordHandlingOp(
                operation.InternalRecordId,
                null,
                HandlingStatus.DisabledForRecord,
                new[]
                {
                    HandlingStatus.AvailableByDefault,
                    HandlingStatus.Running,
                    HandlingStatus.Failed,
                },
                operation.Details,
                operation.Tags);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DisableRecordHandlingForRecordOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            CancelRunningHandleRecordExecutionOp operation)
        {
            var standardizedOperation = new StandardUpdateRecordHandlingOp(
                operation.Id,
                operation.Concern,
                HandlingStatus.AvailableAfterExternalCancellation,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags);

            this.stream.Execute(standardizedOperation);
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
            var standardizedOperation = new StandardUpdateRecordHandlingOp(
                operation.Id,
                operation.Concern,
                HandlingStatus.Completed,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags);

            this.stream.Execute(standardizedOperation);
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
            var standardizedOperation = new StandardUpdateRecordHandlingOp(
                operation.Id,
                operation.Concern,
                HandlingStatus.Failed,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags);

            this.stream.Execute(standardizedOperation);
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
            var standardizedOperation = new StandardUpdateRecordHandlingOp(
                operation.Id,
                operation.Concern,
                HandlingStatus.AvailableAfterSelfCancellation,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags);

            this.stream.Execute(standardizedOperation);
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
            var standardizedOperation = new StandardUpdateRecordHandlingOp(
                operation.Id,
                operation.Concern,
                HandlingStatus.AvailableAfterFailure,
                new[]
                {
                    HandlingStatus.Failed,
                },
                operation.Details,
                operation.Tags);

            this.stream.Execute(standardizedOperation);
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
