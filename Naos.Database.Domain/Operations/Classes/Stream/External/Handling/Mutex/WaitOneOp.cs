// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaitOneOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// An operation to acquire a mutex by exclusively handling a <see cref="MutexObject"/> in a stream,
    /// scoped by identifier and handling concern.
    /// </summary>
    /// <remarks>
    /// The protocol should return null if the mutex was not acquired (the <see cref="MutexObject"/> doesn't exist or is being handled).
    /// </remarks>
    public partial class WaitOneOp : ReturningOperationBase<ReleaseMutexOp>, IHaveStringId, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitOneOp"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <param name="retryStrategy">OPTIONAL strategy that determines whether to retry to acquire the mutex when it cannot be acquired.  The protocol will wait <paramref name="pollingWaitTime"/> before retrying. DEFAULT is infinite retry.</param>
        public WaitOneOp(
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default,
            WaitOneRetryStrategyBase retryStrategy = null)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            pollingWaitTime.MustForArg(nameof(pollingWaitTime)).BeGreaterThanOrEqualTo(TimeSpan.Zero);

            var localPollingWaitTime = pollingWaitTime == default
                ? TimeSpan.FromMilliseconds(200)
                : pollingWaitTime;

            var localRetryStrategy = retryStrategy == null
                ? new InfiniteWaitOneRetryStrategy()
                : retryStrategy;

            this.Id = id;
            this.Details = details;
            this.Concern = concern;
            this.PollingWaitTime = localPollingWaitTime;
            this.RetryStrategy = localRetryStrategy;
        }

        /// <inheritdoc />
        public string Id { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <summary>
        /// Gets the concern to use when handling the <see cref="MutexObject"/>.
        /// </summary>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the time to wait between attempts to handle the <see cref="MutexObject"/>.
        /// </summary>
        public TimeSpan PollingWaitTime { get; private set; }

        /// <summary>
        /// Gets a strategy that determines whether to retry to acquire the mutex when it cannot be acquired.
        /// </summary>
        /// <remarks>
        /// The protocol will wait <see cref="PollingWaitTime"/> before retrying.
        /// </remarks>
        public WaitOneRetryStrategyBase RetryStrategy { get; private set; }
    }
}
