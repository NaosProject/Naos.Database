// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleOnlyStreamExtensionsTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Serialization.Json;
    using Xunit;

    public static class HandleOnlyStreamExtensionsTest
    {
        [Fact]
        public static async Task ExecuteSynchronouslyUsingStreamMutex___Should_block_other_callers_from_acquiring_lock_until_action_is_run___When_multiple_callers_require_mutex()
        {
            // Arrange
            var stream = BuildCreatedStream();

            var mutexId = A.Dummy<string>();
            var mutexProtocol = stream.GetStreamDistributedMutexProtocols();

            var mre = new ManualResetEvent(false);

            var action1Running = false;

            var action1 = new Action(() =>
            {
                action1Running = true;
                mre.WaitOne();
                Thread.Sleep(2000);
            });

            var action2 = new Action(() => { });

            await stream.PutWithIdAsync(mutexId, new MutexObject(mutexId));

            var mutexInternalRecordId = stream.GetLatestRecordById(mutexId).InternalRecordId;

            var expectedDetails = new[]
            {
                Concerns.AvailableByDefaultHandlingEntryDetails,
                nameof(action1),
                nameof(action1),
                nameof(action2),
                nameof(action2),
            };

            var expectedStatuses = new[]
            {
                HandlingStatus.AvailableByDefault,
                HandlingStatus.Running,
                HandlingStatus.AvailableAfterSelfCancellation,
                HandlingStatus.Running,
                HandlingStatus.AvailableAfterSelfCancellation,
            };

            // Act
            var threadStart1 = new ThreadStart(() => action1.ExecuteSynchronouslyUsingStreamMutex(
                mutexProtocol,
                mutexId,
                nameof(action1),
                pollingWaitTime: TimeSpan.FromMilliseconds(5)));

            var thread1 = new Thread(threadStart1);

            thread1.Start();

            while (!action1Running)
            {
                Thread.Sleep(50);
            }

            var action2AboutToStart = false;

            var threadStart2 = new ThreadStart(() =>
            {
                action2AboutToStart = true;

                action2.ExecuteSynchronouslyUsingStreamMutex(
                    mutexProtocol,
                    mutexId,
                    nameof(action2),
                    pollingWaitTime: TimeSpan.FromMilliseconds(5));
            });

            var thread2 = new Thread(threadStart2);
            thread2.Start();

            while (!action2AboutToStart)
            {
                Thread.Sleep(50);
            }

            // We can't guarantee that action1 has begun to attempt to WaitOne,
            // so we are just going to give it a bunch of time to try.
            Thread.Sleep(5000);

            mre.Set();
            thread2.Join();
            thread1.Join();

            // Assert
            var handlingHistory = stream.GetStreamRecordHandlingProtocols().Execute(new GetHandlingHistoryOp(mutexInternalRecordId, Concerns.DefaultMutexConcern));

            var actualDetails = handlingHistory
                .OrderBy(_ => _.InternalHandlingEntryId)
                .Select(_ => _.Details)
                .ToList();

            var actualStatuses = handlingHistory
                .OrderBy(_ => _.InternalHandlingEntryId)
                .Select(_ => _.Status)
                .ToList();

            actualDetails.AsTest().Must().BeSequenceEqualTo(expectedDetails);
            actualStatuses.AsTest().Must().BeSequenceEqualTo(expectedStatuses);
        }

        private static MemoryStandardStream BuildCreatedStream()
        {
            var result = new MemoryStandardStream(
                "test-stream-name",
                new SerializerRepresentation(SerializationKind.Json),
                SerializationFormat.String,
                new JsonSerializerFactory());

            return result;
        }
    }
}
