// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryExecuteSynchronouslyUsingStreamMutexAsyncResult{T}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// The results of asynchronously attempting to execute a specified delegate synchronously, using a stream-implemented mutex.
    /// </summary>
    /// <typeparam name="T">The return type of the func that was executed if the mutex was acquired.</typeparam>
    public class TryExecuteSynchronouslyUsingStreamMutexAsyncResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryExecuteSynchronouslyUsingStreamMutexAsyncResult{T}"/> class.
        /// </summary>
        /// <param name="mutexAcquired">A value indicating whether the mutex was acquired.  false means that the mutex object didn't exist or was already being handled and that the delegate was not executed.</param>
        /// <param name="delegateResult">The result of executing the delegate (if the mutex was acquired).</param>
        public TryExecuteSynchronouslyUsingStreamMutexAsyncResult(
            bool mutexAcquired,
            T delegateResult)
        {
            this.MutexAcquired = mutexAcquired;
            this.DelegateResult = delegateResult;
        }

        /// <summary>
        /// Gets a value indicating whether the mutex was acquired.
        /// </summary>
        /// <remarks>
        /// false means that the mutex object didn't exist or was already being handled and that the delegate was not executed.
        /// </remarks>
        public bool MutexAcquired { get; }

        /// <summary>
        /// Gets the result of executing the delegate (if the mutex was acquired).
        /// </summary>
        public T DelegateResult { get; }
    }
}
