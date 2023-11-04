// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordExpectedToBeHandled.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Configuration to check that a record was handled in the expected way.
    /// </summary>
    public partial class RecordExpectedToBeHandled : IHaveId<string>, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordExpectedToBeHandled"/> class.
        /// </summary>
        /// <param name="id">A unique identifier to key against results.</param>
        /// <param name="concern">The concern to check handling for.</param>
        /// <param name="recordFilter">Filter to find the records expected to be handled.</param>
        /// <param name="handlingFilter">Filter to define unacceptable statuses.</param>
        /// <param name="threshold">Time threshold for handling to be in acceptable state.</param>
        public RecordExpectedToBeHandled(string id, string concern, RecordFilter recordFilter, HandlingFilter handlingFilter, TimeSpan threshold)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            handlingFilter.MustForArg(nameof(handlingFilter)).NotBeNull();
            threshold.MustForArg(nameof(threshold)).BeGreaterThanOrEqualTo(TimeSpan.Zero);

            this.Id = id;
            this.Concern = concern;
            this.RecordFilter = recordFilter;
            this.HandlingFilter = handlingFilter;
            this.Threshold = threshold;
        }

        /// <summary>
        /// Gets a unique identifier to key against results.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the concern to check handling for.
        /// </summary>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the filter to find the records expected to be handled.
        /// </summary>
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets the handling filter to define unacceptable statuses.
        /// </summary>
        public HandlingFilter HandlingFilter { get; private set; }

        /// <summary>
        /// Gets the time threshold for handling to be in acceptable state.
        /// </summary>
        public TimeSpan Threshold { get; private set; }
    }
}
