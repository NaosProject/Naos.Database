// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamDistributedMutexProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

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
