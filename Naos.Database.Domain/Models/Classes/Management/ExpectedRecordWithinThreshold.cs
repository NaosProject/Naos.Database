// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedRecordWithinThreshold.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Configuration to declare the filter to find a record that should exist within a specify time range on a cadence.
    /// </summary>
    public partial class ExpectedRecordWithinThreshold : IHaveId<string>, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedRecordWithinThreshold"/> class.
        /// </summary>
        /// <param name="id">A unique identifier to key against results.</param>
        /// <param name="recordFilter">Filter to find the records expected to be handled.</param>
        /// <param name="threshold">The time threshold to check against.</param>
        /// <param name="skipWhenStreamHandlingIsDisabled">OPTIONAL value indicating whether or not to skip this check when stream handling is disabled; DEFAULT is false.</param>
        public ExpectedRecordWithinThreshold(string id, RecordFilter recordFilter, TimeSpan threshold, bool skipWhenStreamHandlingIsDisabled = false)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            threshold.MustForArg(nameof(threshold)).BeGreaterThanOrEqualTo(TimeSpan.Zero);

            this.Id = id;
            this.RecordFilter = recordFilter;
            this.Threshold = threshold;
            this.SkipWhenStreamHandlingIsDisabled = skipWhenStreamHandlingIsDisabled;
        }

        /// <summary>
        /// Gets a unique identifier to key against results.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the filter to find the records expected to be handled.
        /// </summary>
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets the time threshold to check against.
        /// </summary>
        public TimeSpan Threshold { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not to skip this check when stream handling is disabled.
        /// </summary>
        public bool SkipWhenStreamHandlingIsDisabled { get; private set; }
    }
}
