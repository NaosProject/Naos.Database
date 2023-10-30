// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckStreamInstruction.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Instructions for checking a stream.
    /// </summary>
    public partial class CheckStreamInstruction : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckStreamInstruction"/> class.
        /// </summary>
        /// <param name="expectedRecordsWithinThreshold">The configuration to find expected records to be within a time threshold.</param>
        /// <param name="recordsExpectedToBeHandled">The configuration to find records expected to be handled in a specific way.</param>
        public CheckStreamInstruction(
            IReadOnlyCollection<ExpectedRecordWithinThreshold> expectedRecordsWithinThreshold,
            IReadOnlyCollection<RecordExpectedToBeHandled> recordsExpectedToBeHandled)
        {
            expectedRecordsWithinThreshold.MustForArg(nameof(expectedRecordsWithinThreshold)).NotBeNull().And().NotContainAnyNullElements();
            recordsExpectedToBeHandled.MustForArg(nameof(recordsExpectedToBeHandled)).NotBeNull().NotContainAnyNullElements();

            this.ExpectedRecordsWithinThreshold = expectedRecordsWithinThreshold;
            this.RecordsExpectedToBeHandled = recordsExpectedToBeHandled;
        }

        /// <summary>
        /// Gets the configuration to find expected records to be within a time threshold.
        /// </summary>
        public IReadOnlyCollection<ExpectedRecordWithinThreshold> ExpectedRecordsWithinThreshold { get; private set; }

        /// <summary>
        /// Gets the configuration to find records expected to be handled in a specific way.
        /// </summary>
        public IReadOnlyCollection<RecordExpectedToBeHandled> RecordsExpectedToBeHandled { get; private set; }
    }
}
