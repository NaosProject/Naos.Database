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
        /// <param name="attempts">The number of attempts to acquire the mutex before giving up.  Must be &gt;= 1.</param>
        public NumberOfAttemptsWaitOneRetryStrategy(
            int attempts)
        {
            attempts.MustForArg(nameof(attempts)).BeGreaterThanOrEqualTo(1);

            this.Attempts = attempts;
        }

        /// <summary>
        /// Gets the number of attempts to acquire the mutex before giving up.
        /// </summary>
        public int Attempts { get; private set; }
    }
}
