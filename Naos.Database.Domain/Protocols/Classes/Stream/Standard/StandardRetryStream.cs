// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardRetryStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Wrapping stream to execute each operation against the backing stream with retry logic per the specified inputs.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class StandardRetryStream : StandardStreamBase
    {
        private readonly IStandardStream backingStream;
        private readonly int retryCount;
        private readonly TimeSpan backOffDelay;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardRetryStream"/> class.
        /// </summary>
        /// <param name="backingStream">The backing stream.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="backOffDelay">The back off delay.</param>
        public StandardRetryStream(
            IStandardStream backingStream,
            int retryCount,
            TimeSpan backOffDelay)
            : base(
                backingStream.Name,
                backingStream.SerializerFactory,
                backingStream.DefaultSerializerRepresentation,
                backingStream.DefaultSerializationFormat,
                backingStream.ResourceLocatorProtocols)
        {
            backingStream.MustForArg(nameof(backingStream)).NotBeNull();

            this.backingStream = backingStream;
            this.retryCount = retryCount;
            this.backOffDelay = backOffDelay;
        }

        /// <inheritdoc />
        public override long Execute(
            StandardGetNextUniqueLongOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override TryHandleRecordResult Execute(
            StandardTryHandleRecordOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override PutRecordResult Execute(
            StandardPutRecordOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            StandardUpdateHandlingStatusForStreamOp operation)
        {
            Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);
        }

        /// <inheritdoc />
        public override IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordHandlingEntry> Execute(
            StandardGetHandlingHistoryOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override string Execute(
            StandardGetLatestStringSerializedObjectOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override CreateStreamResult Execute(
            StandardCreateStreamOp operation)
        {
            var result = Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);

            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            StandardDeleteStreamOp operation)
        {
            Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);
        }

        /// <inheritdoc />
        public override void Execute(
            StandardPruneStreamOp operation)
        {
            Run.WithRetry(
                () => this.backingStream.Execute(operation),
                this.retryCount,
                this.backOffDelay);
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation => this.backingStream.StreamRepresentation;
    }
}
