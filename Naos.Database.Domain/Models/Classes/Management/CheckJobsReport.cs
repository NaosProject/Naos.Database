// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckJobsReport.cs" company="Naos Project">
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
    /// Report returned from <see cref="CheckJobsOp" />.
    /// </summary>
    public partial class CheckJobsReport : IHaveCheckStatus, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckJobsReport"/> class.
        /// </summary>
        /// <param name="status">Evaluated check status.</param>
        /// <param name="jobNameToInformationMap">The job name to information map.</param>
        /// <param name="sampleTimeUtc">The sample time UTC.</param>
        public CheckJobsReport(
            CheckStatus status,
            IReadOnlyDictionary<string, IJobInformation> jobNameToInformationMap,
            DateTime sampleTimeUtc)
        {
            status.MustForArg(nameof(status)).NotBeEqualTo(CheckStatus.Invalid);
            jobNameToInformationMap.MustForArg(nameof(jobNameToInformationMap)).NotBeNull().And().NotContainAnyKeyValuePairsWithNullValue();
            sampleTimeUtc.MustForArg(nameof(sampleTimeUtc)).BeUtcDateTime();

            this.Status = status;
            this.JobNameToInformationMap = jobNameToInformationMap;
            this.SampleTimeUtc = sampleTimeUtc;
        }

        /// <inheritdoc />
        public CheckStatus Status { get; private set; }

        /// <summary>
        /// Gets the job name to information map.
        /// </summary>
        public IReadOnlyDictionary<string, IJobInformation> JobNameToInformationMap { get; private set; }

        /// <summary>
        /// Gets the time report was sampled in UTC.
        /// </summary>
        public DateTime SampleTimeUtc { get; private set; }
    }
}
