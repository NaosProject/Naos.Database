// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckSingleStreamReport.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Report of checks on a stream.
    /// </summary>
    public partial class CheckSingleStreamReport : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckSingleStreamReport"/> class.
        /// </summary>
        /// <param name="expectedRecordWithinThresholdIdToMostRecentTimestampMap">The expected record within threshold identifier to most recent timestamp map.</param>
        /// <param name="eventExpectedToBeHandledIdToHandlingStatusResultMap">The event expected to be handled identifier to handling status result map.</param>
        public CheckSingleStreamReport(
            IReadOnlyDictionary<string, DateTime> expectedRecordWithinThresholdIdToMostRecentTimestampMap,
            IReadOnlyDictionary<string, IReadOnlyDictionary<long, HandlingStatus>> eventExpectedToBeHandledIdToHandlingStatusResultMap)
        {
            expectedRecordWithinThresholdIdToMostRecentTimestampMap.MustForArg(nameof(expectedRecordWithinThresholdIdToMostRecentTimestampMap)).NotBeNull();
            eventExpectedToBeHandledIdToHandlingStatusResultMap.MustForArg(nameof(eventExpectedToBeHandledIdToHandlingStatusResultMap)).NotBeNull().And().NotContainAnyKeyValuePairsWithNullValue();

            this.ExpectedRecordWithinThresholdIdToMostRecentTimestampMap = expectedRecordWithinThresholdIdToMostRecentTimestampMap;
            this.EventExpectedToBeHandledIdToHandlingStatusResultMap = eventExpectedToBeHandledIdToHandlingStatusResultMap;
        }

        /// <summary>
        /// Gets the expected record within threshold identifier to most recent timestamp map.
        /// </summary>
        public IReadOnlyDictionary<string, DateTime> ExpectedRecordWithinThresholdIdToMostRecentTimestampMap { get; private set; }

        /// <summary>
        /// Gets the event expected to be handled identifier to handling status result map.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyDictionary<long, HandlingStatus>> EventExpectedToBeHandledIdToHandlingStatusResultMap { get; private set; }
    }
}
