// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamDistributedMutexProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Set of protocols to use a stream to manage a distributed mutex.
    /// </summary>
    public class StandardStreamDistributedMutexProtocols :
        IStreamDistributedMutexProtocols
    {
        private static readonly TypeRepresentation MutexObjectTypeRepresentation = typeof(MutexObject).ToRepresentation();
        private readonly IStandardStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamDistributedMutexProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamDistributedMutexProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public ReleaseMutexOp Execute(
            WaitOneOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var stringSerializedIdentifier = this.stream.GetStringSerializedIdentifier(
                operation.Id);

            var tryHandleOp = new StandardTryHandleRecordOp(
                operation.Concern,
                new RecordFilter(
                    ids: new[] { stringSerializedIdentifier },
                    idTypes: new[] { stringSerializedIdentifier.IdentifierType },
                    objectTypes: new[] { MutexObjectTypeRepresentation }),
                details: operation.Details);

            var attempts = 0;

            while (true)
            {
                var tryHandleResult = this.stream.Execute(tryHandleOp);
                if (tryHandleResult.RecordToHandle != null)
                {
                    var result = new ReleaseMutexOp(
                        operation,
                        tryHandleResult.RecordToHandle.InternalRecordId);

                    return result;
                }

                if (operation.RetryStrategy is InfiniteWaitOneRetryStrategy)
                {
                    // no-op
                }
                else if (operation.RetryStrategy is NumberOfAttemptsWaitOneRetryStrategy numberOfAttemptsWaitOneRetryStrategy)
                {
                    attempts++;

                    if (attempts == numberOfAttemptsWaitOneRetryStrategy.Attempts)
                    {
                        return null;
                    }
                }
                else
                {
                    throw new NotSupportedException(Invariant($"This type of {nameof(WaitOneRetryStrategyBase)} is not supported: {operation.RetryStrategy.GetType().ToStringReadable()}."));
                }

                Thread.Sleep((int)operation.PollingWaitTime.TotalMilliseconds);
            }
        }

        /// <inheritdoc />
        public async Task<ReleaseMutexOp> ExecuteAsync(
            WaitOneOp operation)
        {
            var result = this.Execute(operation);

            await Task.FromResult(true);

            return result;
        }

        /// <inheritdoc />
        public void Execute(
            ReleaseMutexOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var updateHandlingStatusOp = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.WaitOneOp.Concern,
                HandlingStatus.AvailableAfterSelfCancellation,
                new[] { HandlingStatus.Running },
                operation.WaitOneOp.Details);

            this.stream.Execute(updateHandlingStatusOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            ReleaseMutexOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }
    }
}
