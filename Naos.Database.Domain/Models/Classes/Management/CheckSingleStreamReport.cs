// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckSingleStreamReport.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Diagnostics.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Report of checks on a stream.
    /// </summary>
    public partial class CheckSingleStreamReport : IHaveCheckStatus, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckSingleStreamReport"/> class.
        /// </summary>
        /// <param name="status">Evaluated check status.</param>
        /// <param name="expectedRecordWithinThresholdIdToReportMap">The expected record within threshold identifier to report map.</param>
        /// <param name="recordExpectedToBeHandledIdToReportMap">The record expected to be handled identifier to report map.</param>
        public CheckSingleStreamReport(
            CheckStatus status,
            IReadOnlyDictionary<string, ExpectedRecordWithinThresholdReport> expectedRecordWithinThresholdIdToReportMap,
            IReadOnlyDictionary<string, RecordExpectedToBeHandledReport> recordExpectedToBeHandledIdToReportMap)
        {
            status.MustForArg(nameof(status)).NotBeEqualTo(CheckStatus.Invalid);
            expectedRecordWithinThresholdIdToReportMap.MustForArg(nameof(expectedRecordWithinThresholdIdToReportMap)).NotBeNull();
            recordExpectedToBeHandledIdToReportMap.MustForArg(nameof(recordExpectedToBeHandledIdToReportMap)).NotBeNull().And().NotContainAnyKeyValuePairsWithNullValue();

            this.Status = status;
            this.ExpectedRecordWithinThresholdIdToReportMap = expectedRecordWithinThresholdIdToReportMap;
            this.RecordExpectedToBeHandledIdToReportMap = recordExpectedToBeHandledIdToReportMap;
        }

        /// <inheritdoc />
        public CheckStatus Status { get; private set; }

        /// <summary>
        /// Gets the expected record within threshold identifier to report map.
        /// </summary>
        public IReadOnlyDictionary<string, ExpectedRecordWithinThresholdReport> ExpectedRecordWithinThresholdIdToReportMap { get; private set; }

        /// <summary>
        /// Gets the record expected expected to be handled identifier to report map.
        /// </summary>
        public IReadOnlyDictionary<string, RecordExpectedToBeHandledReport> RecordExpectedToBeHandledIdToReportMap { get; private set; }
    }
}
