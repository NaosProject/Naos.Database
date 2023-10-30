// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckJobsReport.cs" company="Naos Project">
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
    /// Report returned from <see cref="CheckJobsOp" />.
    /// </summary>
    public partial class CheckJobsReport : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckJobsReport"/> class.
        /// </summary>
        /// <param name="shouldAlert">A value indicating whether the the results of the checks should be alerted.</param>
        /// <param name="jobNameToInformationMap">The job name to information map.</param>
        /// <param name="sampleTimeUtc">The sample time UTC.</param>
        public CheckJobsReport(
            bool shouldAlert,
            IReadOnlyDictionary<string, IJobInformation> jobNameToInformationMap,
            DateTime sampleTimeUtc)
        {
            jobNameToInformationMap.MustForArg(nameof(jobNameToInformationMap)).NotBeNull().And().NotContainAnyKeyValuePairsWithNullValue();
            sampleTimeUtc.MustForArg(nameof(sampleTimeUtc)).BeUtcDateTime();

            this.ShouldAlert = shouldAlert;
            this.JobNameToInformationMap = jobNameToInformationMap;
            this.SampleTimeUtc = sampleTimeUtc;
        }

        /// <summary>
        /// Gets a value indicating whether the the results of the checks should be alerted.
        /// </summary>
        public bool ShouldAlert { get; private set; }

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
