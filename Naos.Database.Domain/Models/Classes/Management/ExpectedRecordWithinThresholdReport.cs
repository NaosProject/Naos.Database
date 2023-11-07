// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedRecordWithinThresholdReport.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.Diagnostics.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Report returned from <see cref="Domain.ExpectedRecordWithinThreshold" /> evaluation.
    /// </summary>
    public partial class ExpectedRecordWithinThresholdReport : IHaveCheckStatus, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedRecordWithinThresholdReport"/> class.
        /// </summary>
        /// <param name="status">Evaluated check status.</param>
        /// <param name="expectedRecordWithinThreshold">Instructions used to evaluate.</param>
        /// <param name="latestMatchingRecordTimestampUtc">The sample time UTC.</param>
        public ExpectedRecordWithinThresholdReport(
            CheckStatus status,
            ExpectedRecordWithinThreshold expectedRecordWithinThreshold,
            DateTime latestMatchingRecordTimestampUtc)
        {
            status.MustForArg(nameof(status)).NotBeEqualTo(CheckStatus.Invalid);
            expectedRecordWithinThreshold.MustForArg(nameof(expectedRecordWithinThreshold)).NotBeNull();
            latestMatchingRecordTimestampUtc.MustForArg(nameof(latestMatchingRecordTimestampUtc)).BeUtcDateTime();

            this.Status = status;
            this.ExpectedRecordWithinThreshold = expectedRecordWithinThreshold;
            this.LatestMatchingRecordTimestampUtc = latestMatchingRecordTimestampUtc;
        }

        /// <inheritdoc />
        public CheckStatus Status { get; private set; }

        /// <summary>
        /// Gets the instructions used to evaluate.
        /// </summary>
        public ExpectedRecordWithinThreshold ExpectedRecordWithinThreshold { get; private set; }

        /// <summary>
        /// Gets the latest matching record timestamp UTC.
        /// </summary>
        public DateTime LatestMatchingRecordTimestampUtc { get; private set; }
    }
}
