﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Run.cs" company="Naos">
//   Copyright 2017 Naos
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in Naos.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Recipes.RunWithRetry
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;

    using OBeautifulCode.Validation.Recipes;

    using Spritely.Redo;

    using static System.FormattableString;

    /// <summary>
    /// Provides methods to run code and retry if that code throws.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Recipes", "See package version number")]
    internal static class Run
    {
        /// <summary>
        /// Default retry count.
        /// </summary>
        public const int DefaultRetryCount = 5;

        /// <summary>
        /// Default back off delay.
        /// </summary>
        public static readonly TimeSpan DefaultLinearBackoffDelay = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Default <see cref="TimeSpan" /> to wait before re-checking <see cref="Task.Status" />.
        /// </summary>
        public static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMilliseconds(10);

        /// <summary>
        /// Runs a function and retries if any exception is thrown, using a linear backoff strategy.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="retryCount">Optional number of retries; DEFAULT is <see cref="DefaultRetryCount" />.</param>
        /// <param name="backOffDelay">Optional backoff delay; DEFAULT is <see cref="DefaultLinearBackoffDelay" />.</param>
        /// <returns>
        /// A task.
        /// </returns>
        public static void WithRetry(this Action operation, int retryCount = DefaultRetryCount, TimeSpan backOffDelay = default(TimeSpan))
        {
            new { operation }.Must().NotBeNull();

            var localBackOff = backOffDelay == default(TimeSpan) ? DefaultLinearBackoffDelay : backOffDelay;

            Using
                .LinearBackOff(localBackOff)
                .WithMaxRetries(retryCount)
                .Run(operation)
                .Now();
        }

        /// <summary>
        /// Runs a function and retries if any exception is thrown, using a linear backoff strategy.
        /// </summary>
        /// <typeparam name="T">The type of task returned by the operation.</typeparam>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="retryCount">Optional number of retries; DEFAULT is <see cref="DefaultRetryCount" />.</param>
        /// <param name="backOffDelay">Optional backoff delay; DEFAULT is <see cref="DefaultLinearBackoffDelay" />.</param>
        /// <returns>
        /// A task of type returned by the operation.
        /// </returns>
        public static T WithRetry<T>(this Func<T> operation, int retryCount = DefaultRetryCount, TimeSpan backOffDelay = default(TimeSpan))
        {
            new { operation }.Must().NotBeNull();

            var localBackOff = backOffDelay == default(TimeSpan) ? DefaultLinearBackoffDelay : backOffDelay;

            var result = Using
                             .LinearBackOff(localBackOff)
                             .WithMaxRetries(retryCount)
                             .Run(operation)
                             .Now();
            return result;
        }

        /// <summary>
        /// Runs a function and retries if any exception is thrown, using a linear backoff strategy.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="reporter">Action to call to report exceptions as they occur.</param>
        /// <param name="messageBuilder">
        /// Optional.  Transforms the exception message and uses that as the Message property of the 
        /// anonymous object that's sent to the <paramref name="reporter"/>.  If null, then the exception's
        /// Message is used.
        /// </param>
        /// <param name="retryCount">Optional number of retries; DEFAULT is <see cref="DefaultRetryCount" />.</param>
        /// <param name="backOffDelay">Optional backoff delay; DEFAULT is <see cref="DefaultLinearBackoffDelay" />.</param>
        /// <returns>
        /// A task.
        /// </returns>
        public static void WithRetry(this Action operation, Action<object> reporter, Func<Exception, string> messageBuilder = null, int retryCount = DefaultRetryCount, TimeSpan backOffDelay = default(TimeSpan))
        {
            new { operation }.Must().NotBeNull();
            new { reporter }.Must().NotBeNull();

            var localBackOff = backOffDelay == default(TimeSpan) ? DefaultLinearBackoffDelay : backOffDelay;

            Using
                .LinearBackOff(localBackOff)
                .WithReporter(_ => reporter(new { Message = messageBuilder == null ? _.Message : messageBuilder(_), Exception = _ }))
                .WithMaxRetries(retryCount)
                .Run(operation)
                .Now();
        }

        /// <summary>
        /// Runs a function and retries if any exception is thrown, using a linear backoff strategy.
        /// </summary>
        /// <typeparam name="T">The type of task returned by the operation.</typeparam>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="reporter">Action to call to report exceptions as they occur.</param>
        /// <param name="messageBuilder">
        /// Optional.  Transforms the exception message and uses that as the Message property of the 
        /// anonymous object that's sent to the <paramref name="reporter"/>.  If null, then the exception's
        /// Message is used.
        /// </param>
        /// <param name="retryCount">Optional number of retries; DEFAULT is <see cref="DefaultRetryCount" />.</param>
        /// <param name="backOffDelay">Optional backoff delay; DEFAULT is <see cref="DefaultLinearBackoffDelay" />.</param>
        /// <returns>
        /// A task of type returned by the operation.
        /// </returns>
        public static T WithRetry<T>(this Func<T> operation, Action<object> reporter, Func<Exception, string> messageBuilder = null, int retryCount = DefaultRetryCount, TimeSpan backOffDelay = default(TimeSpan))
        {
            new { operation }.Must().NotBeNull();
            new { reporter }.Must().NotBeNull();

            var localBackOff = backOffDelay == default(TimeSpan) ? DefaultLinearBackoffDelay : backOffDelay;

            var result = Using
                             .LinearBackOff(localBackOff)
                             .WithReporter(_ => reporter(new { Message = messageBuilder == null ? _.Message : messageBuilder(_), Exception = _ }))
                             .WithMaxRetries(retryCount)
                             .Run(operation)
                             .Now();
            return result;
        }

        /// <summary>
        /// Runs a function and retries if any exception is thrown, using a linear backoff strategy.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="retryCount">Optional number of retries; DEFAULT is <see cref="DefaultRetryCount" />.</param>
        /// <param name="backOffDelay">Optional backoff delay; DEFAULT is <see cref="DefaultLinearBackoffDelay" />.</param>
        /// <returns>
        /// A task.
        /// </returns>
        public static async Task WithRetryAsync(this Func<Task> operation, int retryCount = DefaultRetryCount, TimeSpan backOffDelay = default(TimeSpan))
        {
            new { operation }.Must().NotBeNull();

            var localBackOff = backOffDelay == default(TimeSpan) ? DefaultLinearBackoffDelay : backOffDelay;

            await Using
                .LinearBackOff(localBackOff)
                .WithMaxRetries(retryCount)
                .RunAsync(operation)
                .Now();
        }

        /// <summary>
        /// Runs a function and retries if any exception is thrown, using a linear backoff strategy.
        /// </summary>
        /// <typeparam name="T">The type of task returned by the operation.</typeparam>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="retryCount">Optional number of retries; DEFAULT is <see cref="DefaultRetryCount" />.</param>
        /// <param name="backOffDelay">Optional backoff delay; DEFAULT is <see cref="DefaultLinearBackoffDelay" />.</param>
        /// <returns>
        /// A task of type returned by the operation.
        /// </returns>
        public static async Task<T> WithRetryAsync<T>(this Func<Task<T>> operation, int retryCount = DefaultRetryCount, TimeSpan backOffDelay = default(TimeSpan))
        {
            new { operation }.Must().NotBeNull();

            var localBackOff = backOffDelay == default(TimeSpan) ? DefaultLinearBackoffDelay : backOffDelay;

            var result = await Using
                             .LinearBackOff(localBackOff)
                             .WithMaxRetries(retryCount)
                             .RunAsync(operation)
                             .Now();
            return result;
        }

        /// <summary>
        /// Runs a function and retries if any exception is thrown, using a linear backoff strategy.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="reporter">Action to call to report exceptions as they occur.</param>
        /// <param name="messageBuilder">
        /// Optional.  Transforms the exception message and uses that as the Message property of the 
        /// anonymous object that's sent to the <paramref name="reporter"/>.  If null, then the exception's
        /// Message is used.
        /// </param>
        /// <param name="retryCount">Optional number of retries; DEFAULT is <see cref="DefaultRetryCount" />.</param>
        /// <param name="backOffDelay">Optional backoff delay; DEFAULT is <see cref="DefaultLinearBackoffDelay" />.</param>
        /// <returns>
        /// A task.
        /// </returns>
        public static async Task WithRetryAsync(this Func<Task> operation, Action<object> reporter, Func<Exception, string> messageBuilder = null, int retryCount = DefaultRetryCount, TimeSpan backOffDelay = default(TimeSpan))
        {
            new { operation }.Must().NotBeNull();
            new { reporter }.Must().NotBeNull();

            var localBackOff = backOffDelay == default(TimeSpan) ? DefaultLinearBackoffDelay : backOffDelay;

            await Using
                .LinearBackOff(localBackOff)
                .WithReporter(_ => reporter(new { Message = messageBuilder == null ? _.Message : messageBuilder(_), Exception = _ }))
                .WithMaxRetries(retryCount)
                .RunAsync(operation)
                .Now();
        }

        /// <summary>
        /// Runs a function and retries if any exception is thrown, using a linear backoff strategy.
        /// </summary>
        /// <typeparam name="T">The type of task returned by the operation.</typeparam>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="reporter">Action to call to report exceptions as they occur.</param>
        /// <param name="messageBuilder">
        /// Optional.  Transforms the exception message and uses that as the Message property of the 
        /// anonymous object that's sent to the <paramref name="reporter"/>.  If null, then the exception's
        /// Message is used.
        /// </param>
        /// <param name="retryCount">Optional number of retries; DEFAULT is <see cref="DefaultRetryCount" />.</param>
        /// <param name="backOffDelay">Optional backoff delay; DEFAULT is <see cref="DefaultLinearBackoffDelay" />.</param>
        /// <returns>
        /// A task of type returned by the operation.
        /// </returns>
        public static async Task<T> WithRetryAsync<T>(this Func<Task<T>> operation, Action<object> reporter, Func<Exception, string> messageBuilder = null, int retryCount = DefaultRetryCount, TimeSpan backOffDelay = default(TimeSpan))
        {
            new { operation }.Must().NotBeNull();
            new { reporter }.Must().NotBeNull();

            var localBackOff = backOffDelay == default(TimeSpan) ? DefaultLinearBackoffDelay : backOffDelay;

            var result = await Using
                             .LinearBackOff(localBackOff)
                             .WithReporter(_ => reporter(new { Message = messageBuilder == null ? _.Message : messageBuilder(_), Exception = _ }))
                             .WithMaxRetries(retryCount)
                             .RunAsync(operation)
                             .Now();
            return result;
        }

        /// <summary>
        /// Blocks on a task execution until it's in a <see cref="TaskStatus.Canceled" />, <see cref="TaskStatus.Faulted" />, or <see cref="TaskStatus.RanToCompletion" /> status, will start the task if in <see cref="TaskStatus.Created" />.
        /// </summary>
        /// <param name="task">Task to wait on.</param>
        /// <param name="pollingInterval">Optional time to poll and check status of task; DEFAULT is <see cref="DefaultPollingInterval" />.</param>
        /// <param name="taskWaitingStrategy">Optional strategy on how to wait; DEFAULT is <see cref="TaskWaitingStrategy.Sleep" />.</param>
        public static void TaskUntilCompletion(Task task, TimeSpan pollingInterval = default(TimeSpan), TaskWaitingStrategy taskWaitingStrategy = TaskWaitingStrategy.Sleep)
        {
            new { task }.Must().NotBeNull();

            async Task<string> UnnecessaryReturnTask()
            {
                await task;
                return string.Empty;
            }

            TaskUntilCompletion(UnnecessaryReturnTask(), pollingInterval, taskWaitingStrategy);
        }

        /// <summary>
        /// Blocks on a task execution until it's in a <see cref="TaskStatus.Canceled" />, <see cref="TaskStatus.Faulted" />, or <see cref="TaskStatus.RanToCompletion" /> status, will start the task if in <see cref="TaskStatus.Created" />.
        /// </summary>
        /// <param name="task">Task to wait on.</param>
        /// <param name="pollingInterval">Optional time to poll and check status of task; DEFAULT is <see cref="DefaultPollingInterval" />.</param>
        /// <param name="taskWaitingStrategy">Optional strategy on how to wait; DEFAULT is <see cref="TaskWaitingStrategy.Sleep" />.</param>
        /// <returns>Return value of task provided.</returns>
        public static T TaskUntilCompletion<T>(Task<T> task, TimeSpan pollingInterval = default(TimeSpan), TaskWaitingStrategy taskWaitingStrategy = TaskWaitingStrategy.Sleep)
        {
            new { task }.Must().NotBeNull();

            var localPollingTime = pollingInterval == default(TimeSpan) ? TimeSpan.FromMilliseconds(10) : pollingInterval;

            if (task.Status == TaskStatus.Created)
            {
                task.Start();
            }

            // running this way because i want to interrogate afterwards to throw if faulted...
            while (!task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
            {
                switch (taskWaitingStrategy)
                {
                    case TaskWaitingStrategy.YieldAndSleep:
                        Thread.Yield();
                        Thread.Sleep(pollingInterval);
                        break;
                    case TaskWaitingStrategy.Sleep:
                        Thread.Sleep(pollingInterval);
                        break;
                    default:
                        throw new NotSupportedException(Invariant($"Unsupported {nameof(TaskWaitingStrategy)} - {taskWaitingStrategy}"));
                }

                Thread.Sleep(localPollingTime);
            }

            if (task.Status == TaskStatus.Faulted)
            {
                var exception = task.Exception ?? new AggregateException(Invariant($"No exception came back from task but status was Faulted."));
                if (exception.GetType() == typeof(AggregateException) && exception.InnerExceptions.Count == 1 && exception.InnerException != null)
                {
                    // if this is just wrapping a single exception then no need to keep the wrapper...
                    ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                }
                else
                {
                    ExceptionDispatchInfo.Capture(exception).Throw();
                }
            }

            return task.Result;
        }
    }

    /// <summary>
    /// Strategy on how to wait until a <see cref="Task" /> is complete.
    /// </summary>
    internal enum TaskWaitingStrategy
    {
        /// <summary>
        /// <see cref="Thread.Sleep(TimeSpan)" /> while waiting.
        /// </summary>
        Sleep,

        /// <summary>
        /// <see cref="Thread.Yield" /> followed by a <see cref="Thread.Sleep(TimeSpan)" /> while waiting.
        /// </summary>
        YieldAndSleep,
    }
}