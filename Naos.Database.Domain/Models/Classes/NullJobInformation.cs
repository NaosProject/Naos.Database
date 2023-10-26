// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullJobInformation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Null object implementation of <see cref="IJobInformation"/>.
    /// </summary>
    public partial class NullJobInformation : IJobInformation, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullJobInformation"/> class.
        /// </summary>
        /// <param name="jobName">Name of the database.</param>
        public NullJobInformation(
            string jobName)
        {
            jobName.MustForArg(nameof(jobName)).NotBeNullNorWhiteSpace();

            this.JobName = jobName;
        }

        /// <inheritdoc />
        public string JobName { get; private set; }
    }
}
