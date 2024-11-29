// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleOnlyStreamExtensions.Mutex.cs" company="Naos Project">
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
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="stream">The stream containing the <see cref="MutexObject"/>.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        public static void ExecuteSynchronouslyUsingStreamMutex(
            this Action action,
            IRecordHandlingOnlyStream stream,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var protocols = stream.GetStreamDistributedMutexProtocols();

            action.ExecuteSynchronouslyUsingStreamMutex(protocols, id, details, concern, pollingWaitTime);
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="func">The func to execute.</param>
        /// <param name="stream">The stream containing the <see cref="MutexObject"/>.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// A task.
        /// </returns>
        public static async Task ExecuteSynchronouslyUsingStreamMutexAsync(
            this Func<Task> func,
            IRecordHandlingOnlyStream stream,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var protocols = stream.GetStreamDistributedMutexProtocols();

            await func.ExecuteSynchronouslyUsingStreamMutexAsync(protocols, id, details, concern, pollingWaitTime);
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        public static void ExecuteSynchronouslyUsingStreamMutex(
            this Action action,
            IStreamDistributedMutexProtocols protocol,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            action.ExecuteSynchronouslyUsingStreamMutex(protocol, protocol, id, details, concern, pollingWaitTime);
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="func">The func to execute.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// A task.
        /// </returns>
        public static async Task ExecuteSynchronouslyUsingStreamMutexAsync(
            this Func<Task> func,
            IStreamDistributedMutexProtocols protocol,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            await func.ExecuteSynchronouslyUsingStreamMutexAsync(protocol, protocol, id, details, concern, pollingWaitTime);
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="waitOneProtocol">The protocol to acquire a mutex.</param>
        /// <param name="releaseMutexProtocol">The protocol to release a mutex.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        public static void ExecuteSynchronouslyUsingStreamMutex(
            this Action action,
            IWaitOne waitOneProtocol,
            IReleaseMutex releaseMutexProtocol,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            waitOneProtocol.MustForArg(nameof(waitOneProtocol)).NotBeNull();
            releaseMutexProtocol.MustForArg(nameof(releaseMutexProtocol)).NotBeNull();

            var operation = new WaitOneOp(id, details, concern, pollingWaitTime);

            var releaseMutexOp = waitOneProtocol.Execute(operation);

            try
            {
                action();
            }
            finally
            {
                releaseMutexProtocol.Execute(releaseMutexOp);
            }
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <param name="func">The func to execute.</param>
        /// <param name="waitOneProtocol">The protocol to acquire a mutex.</param>
        /// <param name="releaseMutexProtocol">The protocol to release a mutex.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// A task.
        /// </returns>
        public static async Task ExecuteSynchronouslyUsingStreamMutexAsync(
            this Func<Task> func,
            IWaitOne waitOneProtocol,
            IReleaseMutex releaseMutexProtocol,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            waitOneProtocol.MustForArg(nameof(waitOneProtocol)).NotBeNull();
            releaseMutexProtocol.MustForArg(nameof(releaseMutexProtocol)).NotBeNull();

            var operation = new WaitOneOp(id, details, concern, pollingWaitTime);

            var releaseMutexOp = await waitOneProtocol.ExecuteAsync(operation);

            try
            {
                await func();
            }
            finally
            {
                await releaseMutexProtocol.ExecuteAsync(releaseMutexOp);
            }
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <typeparam name="T">The return type of the func to execute.</typeparam>
        /// <param name="func">The func to execute.</param>
        /// <param name="stream">The stream containing the <see cref="MutexObject"/>.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// The result of executing the specified func.
        /// </returns>
        public static T ExecuteSynchronouslyUsingStreamMutex<T>(
            this Func<T> func,
            IRecordHandlingOnlyStream stream,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var protocols = stream.GetStreamDistributedMutexProtocols();

            var result = func.ExecuteSynchronouslyUsingStreamMutex(protocols, id, details, concern, pollingWaitTime);

            return result;
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <typeparam name="T">The return type of the func to execute.</typeparam>
        /// <param name="func">The func to execute.</param>
        /// <param name="stream">The stream containing the <see cref="MutexObject"/>.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// The result of executing the specified func.
        /// </returns>
        public static async Task<T> ExecuteSynchronouslyUsingStreamMutexAsync<T>(
            this Func<Task<T>> func,
            IRecordHandlingOnlyStream stream,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var protocols = stream.GetStreamDistributedMutexProtocols();

            var result = await func.ExecuteSynchronouslyUsingStreamMutexAsync(protocols, id, details, concern, pollingWaitTime);

            return result;
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <typeparam name="T">The return type of the func to execute.</typeparam>
        /// <param name="func">The action to execute.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// The result of executing the specified func.
        /// </returns>
        public static T ExecuteSynchronouslyUsingStreamMutex<T>(
            this Func<T> func,
            IStreamDistributedMutexProtocols protocol,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var result = func.ExecuteSynchronouslyUsingStreamMutex(protocol, protocol, id, details, concern, pollingWaitTime);

            return result;
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <typeparam name="T">The return type of the func to execute.</typeparam>
        /// <param name="func">The action to execute.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// The result of executing the specified func.
        /// </returns>
        public static async Task<T> ExecuteSynchronouslyUsingStreamMutexAsync<T>(
            this Func<Task<T>> func,
            IStreamDistributedMutexProtocols protocol,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var result = await func.ExecuteSynchronouslyUsingStreamMutexAsync(protocol, protocol, id, details, concern, pollingWaitTime);

            return result;
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <typeparam name="T">The return type of the func to execute.</typeparam>
        /// <param name="func">The action to execute.</param>
        /// <param name="waitOneProtocol">The protocol to acquire a mutex.</param>
        /// <param name="releaseMutexProtocol">The protocol to release a mutex.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// The result of executing the specified func.
        /// </returns>
        public static T ExecuteSynchronouslyUsingStreamMutex<T>(
            this Func<T> func,
            IWaitOne waitOneProtocol,
            IReleaseMutex releaseMutexProtocol,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            waitOneProtocol.MustForArg(nameof(waitOneProtocol)).NotBeNull();
            releaseMutexProtocol.MustForArg(nameof(releaseMutexProtocol)).NotBeNull();

            var operation = new WaitOneOp(id, details, concern, pollingWaitTime);

            var releaseMutexOp = waitOneProtocol.Execute(operation);

            T result;

            try
            {
                result = func();
            }
            finally
            {
                releaseMutexProtocol.Execute(releaseMutexOp);
            }

            return result;
        }

        /// <summary>
        /// Executes the specified delegate synchronously, using a stream-implemented mutex.
        /// </summary>
        /// <typeparam name="T">The return type of the func to execute.</typeparam>
        /// <param name="func">The action to execute.</param>
        /// <param name="waitOneProtocol">The protocol to acquire a mutex.</param>
        /// <param name="releaseMutexProtocol">The protocol to release a mutex.</param>
        /// <param name="id">The identifier of the <see cref="MutexObject"/>.</param>
        /// <param name="details">Details about the caller.</param>
        /// <param name="concern">OPTIONAL concern to use when handling the <see cref="MutexObject"/>.  DEFAULT is <see cref="Concerns.DefaultMutexConcern"/>.</param>
        /// <param name="pollingWaitTime">OPTIONAL time to wait between attempts to handle the <see cref="MutexObject"/>.  DEFAULT is 200 milliseconds.</param>
        /// <returns>
        /// The result of executing the specified func.
        /// </returns>
        public static async Task<T> ExecuteSynchronouslyUsingStreamMutexAsync<T>(
            this Func<Task<T>> func,
            IWaitOne waitOneProtocol,
            IReleaseMutex releaseMutexProtocol,
            string id,
            string details,
            string concern = Concerns.DefaultMutexConcern,
            TimeSpan pollingWaitTime = default)
        {
            waitOneProtocol.MustForArg(nameof(waitOneProtocol)).NotBeNull();
            releaseMutexProtocol.MustForArg(nameof(releaseMutexProtocol)).NotBeNull();

            var operation = new WaitOneOp(id, details, concern, pollingWaitTime);

            var releaseMutexOp = await waitOneProtocol.ExecuteAsync(operation);

            T result;

            try
            {
                result = await func();
            }
            finally
            {
                await releaseMutexProtocol.ExecuteAsync(releaseMutexOp);
            }

            return result;
        }
    }
}
