// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckStreamsReport.cs" company="Naos Project">
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
    /// Report returned from <see cref="CheckStreamsOp" />.
    /// </summary>
    public partial class CheckStreamsReport : IHaveCheckStatus, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckStreamsReport"/> class.
        /// </summary>
        /// <param name="status">Evaluated check status.</param>
        /// <param name="streamNameToReportMap">A map of stream name sampled to it's individual report.</param>
        /// <param name="sampleTimeUtc">The time report was sampled in UTC.</param>
        public CheckStreamsReport(
            CheckStatus status,
            IReadOnlyDictionary<string, CheckSingleStreamReport> streamNameToReportMap,
            DateTime sampleTimeUtc)
        {
            status.MustForArg(nameof(status)).NotBeEqualTo(CheckStatus.Invalid);
            streamNameToReportMap.MustForArg(nameof(streamNameToReportMap)).NotBeNull().And().NotContainAnyKeyValuePairsWithNullValue();
            sampleTimeUtc.MustForArg(nameof(sampleTimeUtc)).BeUtcDateTime();

            this.Status = status;
            this.StreamNameToReportMap = streamNameToReportMap;
            this.SampleTimeUtc = sampleTimeUtc;
        }

        /// <inheritdoc />
        public CheckStatus Status { get; private set; }

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
