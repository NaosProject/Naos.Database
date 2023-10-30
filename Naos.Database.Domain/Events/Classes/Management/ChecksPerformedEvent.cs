// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChecksPerformedEvent.cs" company="Naos Project">
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
    /// Report returned from <see cref="PerformDefaultDiagnosticChecksOp" />.
    /// </summary>
    public partial class ChecksPerformedEvent : EventBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksPerformedEvent"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp UTC.</param>
        /// <param name="alerted">if set to <c>true</c> then triggered an alert.</param>
        /// <param name="checkDrivesReport">The check drives report.</param>
        /// <param name="checkJobsReport">The check jobs report.</param>
        /// <param name="checkStreamsReport">The check streams report.</param>
        public ChecksPerformedEvent(
            string id,
            DateTime timestampUtc,
            bool alerted,
            CheckDrivesReport checkDrivesReport,
            CheckJobsReport checkJobsReport,
            CheckStreamsReport checkStreamsReport)
            : base(id, timestampUtc)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            checkDrivesReport.MustForArg(nameof(checkDrivesReport)).NotBeNull();
            checkJobsReport.MustForArg(nameof(checkJobsReport)).NotBeNull();
            checkStreamsReport.MustForArg(nameof(checkStreamsReport)).NotBeNull();

            this.Alerted = alerted;
            this.CheckDrivesReport = checkDrivesReport;
            this.CheckJobsReport = checkJobsReport;
            this.CheckStreamsReport = checkStreamsReport;
        }

        /// <summary>
        /// Gets a value indicating whether an alert was triggered.
        /// </summary>
        public bool Alerted { get; private set; }

        /// <summary>
        /// Gets the check drives report.
        /// </summary>
        public CheckDrivesReport CheckDrivesReport { get; private set; }

        /// <summary>
        /// Gets the check jobs report.
        /// </summary>
        public CheckJobsReport CheckJobsReport { get; private set; }

        /// <summary>
        /// Gets the check streams report.
        /// </summary>
        public CheckStreamsReport CheckStreamsReport { get; private set; }
    }
}
