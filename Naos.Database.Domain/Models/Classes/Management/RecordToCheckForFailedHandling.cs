// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordToCheckForFailedHandling.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Configuration to check that a record is not stuck in a failed handling state.
    /// </summary>
    public partial class RecordToCheckForFailedHandling : IHaveId<string>, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordToCheckForFailedHandling"/> class.
        /// </summary>
        /// <param name="id">A unique identifier to key against results.</param>
        /// <param name="concern">The concern to check handling for.</param>
        /// <param name="recordFilter">Filter to find records expected to not be in a failed state.</param>
        public RecordToCheckForFailedHandling(string id, string concern, RecordFilter recordFilter)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();

            this.Id = id;
            this.Concern = concern;
            this.RecordFilter = recordFilter;
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
        /// Gets the filter to find records expected to not be in a failed state.
        /// </summary>
        public RecordFilter RecordFilter { get; private set; }
    }
}
