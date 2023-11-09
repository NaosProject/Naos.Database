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
    public partial class ChecksPerformedEvent : EventBase<string>, IHaveCheckStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksPerformedEvent"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp UTC.</param>
        /// <param name="status">Evaluated check status.</param>
        /// <param name="checkDrivesReport">The check drives report.</param>
        /// <param name="checkJobsReport">The check jobs report.</param>
        /// <param name="checkStreamsReport">The check streams report.</param>
        public ChecksPerformedEvent(
            string id,
            DateTime timestampUtc,
            CheckStatus status,
            CheckDrivesReport checkDrivesReport,
            CheckJobsReport checkJobsReport,
            CheckStreamsReport checkStreamsReport)
            : base(id, timestampUtc)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            status.MustForArg(nameof(status)).NotBeEqualTo(CheckStatus.Invalid);
            checkDrivesReport.MustForArg(nameof(checkDrivesReport)).NotBeNull();
            checkJobsReport.MustForArg(nameof(checkJobsReport)).NotBeNull();
            checkStreamsReport.MustForArg(nameof(checkStreamsReport)).NotBeNull();

            this.Status = status;
            this.CheckDrivesReport = checkDrivesReport;
            this.CheckJobsReport = checkJobsReport;
            this.CheckStreamsReport = checkStreamsReport;
        }

        /// <inheritdoc />
        public CheckStatus Status { get; private set; }

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
