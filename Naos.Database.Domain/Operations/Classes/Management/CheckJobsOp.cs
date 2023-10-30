// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckJobsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to check Jobs.
    /// </summary>
    public partial class CheckJobsOp : ReturningOperationBase<CheckJobsReport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckJobsOp"/> class.
        /// </summary>
        /// <param name="jobs">The jobs to check.</param>
        public CheckJobsOp(
            IReadOnlyCollection<ExpectedJobWithinThreshold> jobs)
        {
            jobs.MustForArg(nameof(jobs)).NotBeNull().And().NotContainAnyNullElements();

            this.Jobs = jobs;
        }

        /// <summary>
        /// Gets the names of the jobs to check.
        /// </summary>
        public IReadOnlyCollection<ExpectedJobWithinThreshold> Jobs { get; private set; }
    }
}
