// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordToCheckForExcessiveHandling.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Configuration to check that a record in a specified state was not handled over the provided threshold.
    /// </summary>
    public partial class RecordToCheckForExcessiveHandling : IHaveId<string>, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordToCheckForExcessiveHandling"/> class.
        /// </summary>
        /// <param name="id">A unique identifier to key against results.</param>
        /// <param name="concern">The concern to check handling for.</param>
        /// <param name="recordFilter">Filter to find the records to check for appropriate handling.</param>
        /// <param name="handlingFilter">Filter to define statuses to check for excessive handling.</param>
        /// <param name="threshold">Entry count threshold to consider excessive.</param>
        public RecordToCheckForExcessiveHandling(string id, string concern, RecordFilter recordFilter, HandlingFilter handlingFilter, int threshold)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            handlingFilter.MustForArg(nameof(handlingFilter)).NotBeNull();
            threshold.MustForArg(nameof(threshold)).BeGreaterThan(0);

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
        /// Gets the filter to find the records to check for appropriate handling.
        /// </summary>
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets the handling filter to define statuses to check for excessive handling.
        /// </summary>
        public HandlingFilter HandlingFilter { get; private set; }

        /// <summary>
        /// Gets entry count threshold to consider excessive.
        /// </summary>
        public int Threshold { get; private set; }
    }
}
