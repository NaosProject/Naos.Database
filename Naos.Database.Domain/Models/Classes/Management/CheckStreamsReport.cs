// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckStreamsReport.cs" company="Naos Project">
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
    /// Report returned from <see cref="CheckStreamsOp" />.
    /// </summary>
    public partial class CheckStreamsReport : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckStreamsReport"/> class.
        /// </summary>
        /// <param name="shouldAlert">A value indicating whether the the results of the checks should be alerted.</param>
        /// <param name="streamNameToReportMap">A map of stream name sampled to it's individual report.</param>
        /// <param name="sampleTimeUtc">The time report was sampled in UTC.</param>
        public CheckStreamsReport(
            bool shouldAlert,
            IReadOnlyDictionary<string, CheckSingleStreamReport> streamNameToReportMap,
            DateTime sampleTimeUtc)
        {
            streamNameToReportMap.MustForArg(nameof(streamNameToReportMap)).NotBeNullNorEmptyDictionaryNorContainAnyNullValues();
            sampleTimeUtc.MustForArg(nameof(sampleTimeUtc)).BeUtcDateTime();

            this.ShouldAlert = shouldAlert;
            this.StreamNameToReportMap = streamNameToReportMap;
            this.SampleTimeUtc = sampleTimeUtc;
        }

        /// <summary>
        /// Gets a value indicating whether the the results of the checks should be alerted.
        /// </summary>
        public bool ShouldAlert { get; private set; }

        /// <summary>
        /// Gets a map of stream name sampled to it's individual report.
        /// </summary>
        public IReadOnlyDictionary<string, CheckSingleStreamReport> StreamNameToReportMap { get; private set; }

        /// <summary>
        /// Gets the time report was sampled in UTC.
        /// </summary>
        public DateTime SampleTimeUtc { get; private set; }
    }
}
