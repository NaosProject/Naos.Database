// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckStreamsProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Diagnostics.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Protocol to execute <see cref="CheckStreamsOp" />.
    /// </summary>
    public class CheckStreamsProtocol : SyncSpecificReturningProtocolBase<CheckStreamsOp, CheckStreamsReport>
    {
        private readonly ISyncReturningProtocol<GetStreamFromRepresentationOp, IStream> getStreamByRepresentationProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckStreamsProtocol"/> class.
        /// </summary>
        /// <param name="getUtcNow">Function to get the sampling time; allows for a single time to be used in multiple protocols.</param>
        /// <param name="getStreamByRepresentationProtocol">The protocol for <see cref="GetStreamFromRepresentationOp" /> MUST return streams that implement <see cref="IStandardStream" />.</param>
        public CheckStreamsProtocol(
            Func<DateTime> getUtcNow,
            ISyncReturningProtocol<GetStreamFromRepresentationOp, IStream> getStreamByRepresentationProtocol)
        {
            getUtcNow.MustForArg(nameof(getUtcNow)).NotBeNull();
            getStreamByRepresentationProtocol.MustForArg(nameof(getStreamByRepresentationProtocol)).NotBeNull();

            this.GetUtcNow = getUtcNow;
            this.getStreamByRepresentationProtocol = getStreamByRepresentationProtocol;
        }

        /// <summary>
        /// Gets or sets the function to get the sampling time; allows for a single time to be used in multiple protocols.
        /// </summary>
        public Func<DateTime> GetUtcNow { get; set; }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override CheckStreamsReport Execute(
            CheckStreamsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var utcNow = this.GetUtcNow();
            var report = new Dictionary<string, CheckSingleStreamReport>();
            foreach (var streamRepInstruction in operation.StreamNameToCheckStreamInstructionsMap)
            {
                var streamRaw =
                    this.getStreamByRepresentationProtocol.Execute(
                        new GetStreamFromRepresentationOp(new StreamRepresentation(streamRepInstruction.Key)));
                var stream = (IStandardStream)streamRaw;
                var instruction = streamRepInstruction.Value;

                /*
                // Can be managed via RecordExpectedToBeHandled checking for a failed state so considered redundant.

                var recordToCheckForFailedHandlingIdToFailingRecordInternalIdsMap = new Dictionary<string, IReadOnlyCollection<long>>();
                foreach (var recordToCheckForFailedHandling in instruction.RecordsToCheckForFailedHandling)
                {
                    var handlingStatusMap = stream.Execute(
                        new StandardGetHandlingStatusOp(
                            recordToCheckForFailedHandling.Concern,
                            recordToCheckForFailedHandling.RecordFilter,
                            new HandlingFilter(
                                new[]
                                {
                                    HandlingStatus.Failed,
                                })));

                    if (handlingStatusMap.Any())
                    {
                        // found unexpected failures.
                        status = CheckStatus.Failure;
                        shouldAlert = true;
                    }

                    recordToCheckForFailedHandlingIdToFailingRecordInternalIdsMap.Add(recordToCheckForFailedHandling.Id, handlingStatusMap.Keys.ToList());
                }
                */

                var expectedRecordWithinThresholdIdToReportMap = new Dictionary<string, ExpectedRecordWithinThresholdReport>();
                foreach (var expectedEventWithinThreshold in instruction.ExpectedRecordsWithinThreshold)
                {
                    var status = CheckStatus.Success;
                    var skipDueToDisabledStream = false;
                    if (expectedEventWithinThreshold.SkipWhenStreamHandlingIsDisabled)
                    {
                        var disabledStatusCheck = stream.Execute(
                            new StandardGetHandlingStatusOp(
                                Naos.Database.Domain.Concerns.StreamHandlingDisabledConcern,
                                new RecordFilter(
                                    internalRecordIds: new[]
                                                       {
                                                           0L,
                                                       }),
                                new HandlingFilter()));
                        skipDueToDisabledStream = disabledStatusCheck.Single().Value != HandlingStatus.AvailableByDefault;
                    }

                    if (!skipDueToDisabledStream)
                    {
                        var latestRecord = stream.Execute(
                            new StandardGetLatestRecordOp(
                                expectedEventWithinThreshold.RecordFilter,
                                streamRecordItemsToInclude: StreamRecordItemsToInclude.MetadataOnly));

                        if (latestRecord == null
                         || utcNow       > latestRecord.Metadata.TimestampUtc.Add(expectedEventWithinThreshold.Threshold))
                        {
                            status = CheckStatus.Failure;
                        }

                        var expectedWithinThresholdReport = new ExpectedRecordWithinThresholdReport(
                            status,
                            expectedEventWithinThreshold,
                            latestRecord?.Metadata.TimestampUtc ?? default);

                        expectedRecordWithinThresholdIdToReportMap.Add(
                            expectedEventWithinThreshold.Id,
                            expectedWithinThresholdReport);
                    }
                    else
                    {
                        expectedRecordWithinThresholdIdToReportMap.Add(
                            expectedEventWithinThreshold.Id,
                            default);
                    }
                }

                var recordExpectedToBeHandledIdToReportMap = new Dictionary<string, RecordExpectedToBeHandledReport>();
                foreach (var eventExpectedToBeHandled in instruction.RecordsExpectedToBeHandled)
                {
                    var status = CheckStatus.Success;
                    var handlingStatusMap = stream
                       .Execute(
                            new StandardGetHandlingStatusOp(
                                eventExpectedToBeHandled.Concern,
                                eventExpectedToBeHandled.RecordFilter,
                                eventExpectedToBeHandled.HandlingFilter));

                    if (handlingStatusMap.Any())
                    {
                        if (eventExpectedToBeHandled.Threshold == TimeSpan.Zero)
                        {
                            // if there isn't a threshold to check then do not spend time on subsequent calls.
                            status = CheckStatus.Failure;
                        }
                        else
                        {
                            // found records in invalid state but need to check threshold.
                            foreach (var recordIdToHandlingStatus in handlingStatusMap)
                            {
                                var latestRecordMetadata = stream.Execute(
                                    new StandardGetLatestRecordOp(
                                        new RecordFilter(
                                            internalRecordIds: new[]
                                                               {
                                                                   recordIdToHandlingStatus.Key,
                                                               }),
                                        streamRecordItemsToInclude: StreamRecordItemsToInclude.MetadataOnly));
                                var timestampToInterrogate = latestRecordMetadata.Metadata.TimestampUtc;

                                if (utcNow > timestampToInterrogate.Add(eventExpectedToBeHandled.Threshold))
                                {
                                    status = CheckStatus.Failure;
                                    break;
                                }
                            }
                        }
                    }

                    var expectedToBeHandledReport = new RecordExpectedToBeHandledReport(status, eventExpectedToBeHandled, handlingStatusMap);
                    recordExpectedToBeHandledIdToReportMap.Add(eventExpectedToBeHandled.Id, expectedToBeHandledReport);
                }

                /*
                // Can be managed via RecordExpectedToBeHandled checking for a non-terminal state given a threshold so considered redundant.

                var recordToCheckForExcessiveHandlingHandlingIdToExcessivelyHandledInternalIdCountMap = new Dictionary<string, IReadOnlyDictionary<long, int>>();
                foreach (var recordToCheckForExcessiveHandlingHandling in instruction.RecordsToCheckForExcessiveHandling)
                {
                    var handlingStatusMap = stream.Execute(
                        new StandardGetHandlingStatusOp(
                            recordToCheckForExcessiveHandlingHandling.Concern,
                            recordToCheckForExcessiveHandlingHandling.RecordFilter,
                            recordToCheckForExcessiveHandlingHandling.HandlingFilter));

                    var recordIdToCountMap = new Dictionary<long, int>();
                    foreach (var recordIdStatus in handlingStatusMap)
                    {
                        var handlingHistory = stream.Execute(
                            new StandardGetHandlingHistoryOp(
                                recordIdStatus.Key,
                                recordToCheckForExcessiveHandlingHandling.Concern));

                        if (handlingHistory.Count > recordToCheckForExcessiveHandlingHandling.Threshold)
                        {
                            // found excessive handling.
                            status = CheckStatus.Failure;
                            shouldAlert = true;
                        }

                        recordIdToCountMap.Add(recordIdStatus.Key, handlingHistory.Count);
                    }

                    recordToCheckForExcessiveHandlingHandlingIdToExcessivelyHandledInternalIdCountMap.Add(recordToCheckForExcessiveHandlingHandling.Id, recordIdToCountMap);
                }
                */

                var singleReportStatus = expectedRecordWithinThresholdIdToReportMap
                                        .Values.Select(_ => _.Status)
                                        .Union(recordExpectedToBeHandledIdToReportMap.Values.Select(_ => _.Status))
                                        .ToList()
                                        .ReduceToSingleStatus();

                var checkStreamReport = new CheckSingleStreamReport(
                    singleReportStatus,
                    expectedRecordWithinThresholdIdToReportMap,
                    recordExpectedToBeHandledIdToReportMap);
                report.Add(streamRepInstruction.Key, checkStreamReport);
            }

            var reportStatus = report.Values.Select(_ => _.Status).ToList().ReduceToSingleStatus();
            var result = new CheckStreamsReport(reportStatus, report, utcNow);
            return result;
        }
    }
}
