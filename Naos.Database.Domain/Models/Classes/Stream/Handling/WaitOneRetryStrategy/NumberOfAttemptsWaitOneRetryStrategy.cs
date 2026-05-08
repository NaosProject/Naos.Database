// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberOfAttemptsWaitOneRetryStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A mutex acquisition retry strategy that uses a fixed maximum number of retry attempts.
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class NumberOfAttemptsWaitOneRetryStrategy : WaitOneRetryStrategyBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOfAttemptsWaitOneRetryStrategy"/> class.
        /// </summary>
        /// <param name="retryAttempts">The number of retry attempts. Does not include the first/original attempt.</param>
        public NumberOfAttemptsWaitOneRetryStrategy(
            int retryAttempts)
        {
            retryAttempts.MustForArg(nameof(retryAttempts)).NotBeLessThan(0);

            this.RetryAttempts = retryAttempts;
        }

        /// <summary>
        /// Gets the number of retry attempts. Does not include the first/original attempt.
        /// </summary>
        public int RetryAttempts { get; private set; }
    }
}
