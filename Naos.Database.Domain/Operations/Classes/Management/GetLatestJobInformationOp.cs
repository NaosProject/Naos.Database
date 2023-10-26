// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestJobInformationOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Create a database using the specified configuration.
    /// </summary>
    public partial class GetLatestJobInformationOp : ReturningOperationBase<IJobInformation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestJobInformationOp"/> class.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        public GetLatestJobInformationOp(
            string jobName)
        {
            jobName.MustForArg(nameof(jobName)).NotBeNullNorWhiteSpace();

            this.JobName = jobName;
        }

        /// <summary>
        /// Gets the name of the job.
        /// </summary>
        public string JobName { get; private set; }
    }
}
