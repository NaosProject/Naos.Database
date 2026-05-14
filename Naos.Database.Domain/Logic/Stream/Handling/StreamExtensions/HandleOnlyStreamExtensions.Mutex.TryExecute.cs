// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleOnlyStreamExtensions.Mutex.TryExecute.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    public static partial class HandleOnlyStreamExtensions
    {
        /// <summary>
        /// Attempts to execute the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="stream">The stream containing the <see cref="MutexObject"/>.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="retryStrategy">Strategy that determines whether to retry to acquire the mutex when it cannot be acquired.  The protocol will wait <paramref name="pollingWaitTime"/> before retrying.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// true if the mutex was acquired, otherwise false.  false means that the mutex object doesn't exist or is already being handled and that the delegate was not executed.
        /// </returns>
        public static bool TryExecuteSynchronouslyUsingStreamMutex(
            this Action action,
            IRecordHandlingOnlyStream stream,
            string id,
            string details,
            WaitOneRetryStrategyBase retryStrategy,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var protocols = stream.GetStreamDistributedMutexProtocols();

            var result = action.TryExecuteSynchronouslyUsingStreamMutex(
                protocols,
                id,
                details,
                retryStrategy,
                concern,
                pollingWaitTime);

            return result;
        }

        /// <summary>
        /// Attempts to execute the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="func">The func to execute.</param>
        /// <param name="stream">The stream containing the <see cref="MutexObject"/>.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="retryStrategy">Strategy that determines whether to retry to acquire the mutex when it cannot be acquired.  The protocol will wait <paramref name="pollingWaitTime"/> before retrying.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// true if the mutex was acquired, otherwise false.  false means that the mutex object doesn't exist or is already being handled and that the delegate was not executed.
        /// </returns>
        public static async Task<bool> TryExecuteSynchronouslyUsingStreamMutexAsync(
            this Func<Task> func,
            IRecordHandlingOnlyStream stream,
            string id,
            string details,
            WaitOneRetryStrategyBase retryStrategy,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var protocols = stream.GetStreamDistributedMutexProtocols();

            var result = await func.TryExecuteSynchronouslyUsingStreamMutexAsync(
                protocols,
                id,
                details,
                retryStrategy,
                concern,
                pollingWaitTime);

            return result;
        }

        /// <summary>
        /// Attempts to execute the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="retryStrategy">Strategy that determines whether to retry to acquire the mutex when it cannot be acquired.  The protocol will wait <paramref name="pollingWaitTime"/> before retrying.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// true if the mutex was acquired, otherwise false.  false means that the mutex object doesn't exist or is already being handled and that the delegate was not executed.
        /// </returns>
        public static bool TryExecuteSynchronouslyUsingStreamMutex(
            this Action action,
            IStreamDistributedMutexProtocols protocol,
            string id,
            string details,
            WaitOneRetryStrategyBase retryStrategy,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var result = action.TryExecuteSynchronouslyUsingStreamMutex(
                protocol,
                protocol,
                id,
                details,
                retryStrategy,
                concern,
                pollingWaitTime);

            return result;
        }

        /// <summary>
        /// Attempts to execute the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="func">The func to execute.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="retryStrategy">Strategy that determines whether to retry to acquire the mutex when it cannot be acquired.  The protocol will wait <paramref name="pollingWaitTime"/> before retrying.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// true if the mutex was acquired, otherwise false.  false means that the mutex object doesn't exist or is already being handled and that the delegate was not executed.
        /// </returns>
        public static async Task<bool> TryExecuteSynchronouslyUsingStreamMutexAsync(
            this Func<Task> func,
            IStreamDistributedMutexProtocols protocol,
            string id,
            string details,
            WaitOneRetryStrategyBase retryStrategy,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var result = await func.TryExecuteSynchronouslyUsingStreamMutexAsync(
                protocol,
                protocol,
                id,
                details,
                retryStrategy,
                concern,
                pollingWaitTime);

            return result;
        }

        /// <summary>
        /// Attempts to execute the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="waitOneProtocol">The protocol to acquire a mutex.</param>
        /// <param name="releaseMutexProtocol">The protocol to release a mutex.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="retryStrategy">Strategy that determines whether to retry to acquire the mutex when it cannot be acquired.  The protocol will wait <paramref name="pollingWaitTime"/> before retrying.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// true if the mutex was acquired, otherwise false.  false means that the mutex object doesn't exist or is already being handled and that the delegate was not executed.
        /// </returns>
        public static bool TryExecuteSynchronouslyUsingStreamMutex(
            this Action action,
            IWaitOne waitOneProtocol,
            IReleaseMutex releaseMutexProtocol,
            string id,
            string details,
            WaitOneRetryStrategyBase retryStrategy,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            action.MustForArg(nameof(action)).NotBeNull();
            waitOneProtocol.MustForArg(nameof(waitOneProtocol)).NotBeNull();
            releaseMutexProtocol.MustForArg(nameof(releaseMutexProtocol)).NotBeNull();
            retryStrategy.MustForArg(nameof(retryStrategy)).NotBeNull();

            var operation = new WaitOneOp(id, details, concern, pollingWaitTime, retryStrategy);

            var releaseMutexOp = waitOneProtocol.Execute(operation);

            var result = releaseMutexOp != null;

            if (result)
            {
                try
                {
                    action();
                }
                finally
                {
                    releaseMutexProtocol.Execute(releaseMutexOp);
                }
            }

            return result;
        }

        /// <summary>
        /// Attempts to execute the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="func">The func to execute.</param>
        /// <param name="waitOneProtocol">The protocol to acquire a mutex.</param>
        /// <param name="releaseMutexProtocol">The protocol to release a mutex.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="retryStrategy">Strategy that determines whether to retry to acquire the mutex when it cannot be acquired.  The protocol will wait <paramref name="pollingWaitTime"/> before retrying.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// true if the mutex was acquired, otherwise false.  false means that the mutex object doesn't exist or is already being handled and that the delegate was not executed.
        /// </returns>
        public static async Task<bool> TryExecuteSynchronouslyUsingStreamMutexAsync(
            this Func<Task> func,
            IWaitOne waitOneProtocol,
            IReleaseMutex releaseMutexProtocol,
            string id,
            string details,
            WaitOneRetryStrategyBase retryStrategy,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            func.MustForArg(nameof(func)).NotBeNull();
            waitOneProtocol.MustForArg(nameof(waitOneProtocol)).NotBeNull();
            releaseMutexProtocol.MustForArg(nameof(releaseMutexProtocol)).NotBeNull();
            retryStrategy.MustForArg(nameof(retryStrategy)).NotBeNull();

            var operation = new WaitOneOp(id, details, concern, pollingWaitTime, retryStrategy);

            var releaseMutexOp = await waitOneProtocol.ExecuteAsync(operation);

            var result = releaseMutexOp != null;

            if (result)
            {
                try
                {
                    await func();
                }
                finally
                {
                    await releaseMutexProtocol.ExecuteAsync(releaseMutexOp);
                }
            }

            return result;
        }
    }
}
