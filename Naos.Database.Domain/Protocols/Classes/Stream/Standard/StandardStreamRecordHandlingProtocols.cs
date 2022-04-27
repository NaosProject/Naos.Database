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
    /// Set of protocols to execute record handling operations
    /// without a typed identifier and without a typed record payload.
    /// </summary>
    public class StandardStreamRecordHandlingProtocols :
        IStreamRecordHandlingProtocols
    {
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
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = this.stream.Execute(operation.Standardize());

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordHandlingEntry>> ExecuteAsync(
            GetHandlingHistoryOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusByIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var handlingStatusMap = this.stream.Execute(standardOp);

            var handlingStatuses = handlingStatusMap.Values.ToList();

            var result = handlingStatuses.ToCompositeHandlingStatus();

            return result;
        }

        /// <inheritdoc />
        public async Task<CompositeHandlingStatus> ExecuteAsync(
            GetCompositeHandlingStatusByIdsOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusByTagsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var handlingStatusMap = this.stream.Execute(standardOp);

            var handlingStatuses = handlingStatusMap.Values.ToList();

            var result = handlingStatuses.ToCompositeHandlingStatus();

            return result;
        }

        /// <inheritdoc />
        public async Task<CompositeHandlingStatus> ExecuteAsync(
            GetCompositeHandlingStatusByTagsOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var handlingStatusMap = this.stream.Execute(standardOp);

            var handlingStatuses = handlingStatusMap.Values.ToList();

            var result = handlingStatuses.Single();

            return result;
        }

        /// <inheritdoc />
        public async Task<HandlingStatus> ExecuteAsync(
            GetHandlingStatusOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public void Execute(
            DisableHandlingForStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DisableHandlingForStreamOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            EnableHandlingForStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            EnableHandlingForStreamOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            DisableHandlingForRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DisableHandlingForRecordOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            CancelRunningHandleRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CancelRunningHandleRecordOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            CompleteRunningHandleRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CompleteRunningHandleRecordOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            FailRunningHandleRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            FailRunningHandleRecordOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            ArchiveFailureToHandleRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ArchiveFailureToHandleRecordOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            SelfCancelRunningHandleRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            SelfCancelRunningHandleRecordOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            ResetFailedHandleRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ResetFailedHandleRecordOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            ResetCompletedHandleRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ResetCompletedHandleRecordOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }
    }
}
