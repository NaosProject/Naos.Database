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
            GetHandlingHistoryOp operation)
        {
            var standardizedOperation = new StandardGetHandlingHistoryOp(
                operation.InternalRecordId,
                operation.Concern);

            var result = this.stream.Execute(standardizedOperation);
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordHandlingEntry>> ExecuteAsync(
            GetHandlingHistoryOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusByIdsOp operation)
        {
            var standardizedOperation = new StandardGetHandlingStatusOp(
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
            GetCompositeHandlingStatusByIdsOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusByTagsOp operation)
        {
            var standardizedOperation = new StandardGetHandlingStatusOp(
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
            GetCompositeHandlingStatusByTagsOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOp operation)
        {
            var standardizedOperation = new StandardGetHandlingStatusOp(
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
            GetHandlingStatusOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            DisableHandlingForStreamOp operation)
        {
            var standardizedOperation = new StandardUpdateHandlingStatusForStreamOp(
                HandlingStatus.DisabledForStream,
                operation.Details);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DisableHandlingForStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            EnableHandlingForStreamOp operation)
        {
            var standardizedOperation = new StandardUpdateHandlingStatusForStreamOp(
                HandlingStatus.AvailableByDefault,
                operation.Details);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            EnableHandlingForStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            DisableHandlingForRecordOp operation)
        {
            var standardizedOperation = new StandardUpdateHandlingStatusForRecordOp(
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
                operation.Tags,
                operation.InheritRecordTags);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DisableHandlingForRecordOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            CancelRunningHandleRecordOp operation)
        {
            var standardizedOperation = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterExternalCancellation,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CancelRunningHandleRecordOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            CompleteRunningHandleRecordOp operation)
        {
            var standardizedOperation = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.Completed,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CompleteRunningHandleRecordOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            FailRunningHandleRecordOp operation)
        {
            var standardizedOperation = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.Failed,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            FailRunningHandleRecordOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            SelfCancelRunningHandleRecordOp operation)
        {
            var standardizedOperation = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterSelfCancellation,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            SelfCancelRunningHandleRecordOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }

        /// <inheritdoc />
        public void Execute(
            ResetFailedHandleRecordOp operation)
        {
            var standardizedOperation = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterFailure,
                new[]
                {
                    HandlingStatus.Failed,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags);

            this.stream.Execute(standardizedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ResetFailedHandleRecordOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await.
        }
    }
}
