// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedJobWithinThreshold.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Configuration to declare a job that should run within a specify time range on a cadence.
    /// </summary>
    public partial class ExpectedJobWithinThreshold : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedJobWithinThreshold"/> class.
        /// </summary>
        /// <param name="name">The job name.</param>
        /// <param name="threshold">The time threshold to check against.</param>
        public ExpectedJobWithinThreshold(string name, TimeSpan threshold)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            threshold.MustForArg(nameof(threshold)).BeGreaterThanOrEqualTo(TimeSpan.Zero);

            this.Name = name;
            this.Threshold = threshold;
        }

        /// <summary>
        /// Gets the job name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the time threshold to check against.
        /// </summary>
        public TimeSpan Threshold { get; private set; }
    }
}
