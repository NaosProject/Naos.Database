// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingStatusOfRecordSetByTagOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the composite status of the set of records by specified tag matching on all locators.
    /// </summary>
    public partial class GetHandlingStatusOfRecordSetByTagOp : ReturningOperationBase<HandlingStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingStatusOfRecordSetByTagOp"/> class.
        /// </summary>
        /// <param name="concern">The handling concern.</param>
        /// <param name="tagsToMatch">The internal record ids to treat as a composite status.</param>
        /// <param name="handlingStatusCompositionStrategy">The optional strategy for composing statuses.</param>
        /// <param name="tagMatchStrategy">The optional strategy for comparing tags; DEFAULT is <see cref="TagMatchStrategy"/>.</param>
        public GetHandlingStatusOfRecordSetByTagOp(
            string concern,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            HandlingStatusCompositionStrategy handlingStatusCompositionStrategy = null,
            TagMatchStrategy tagMatchStrategy = null)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();

            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotBeNullNorEmptyEnumerable();

            this.Concern = concern;
            this.TagsToMatch = tagsToMatch;
            this.HandlingStatusCompositionStrategy = handlingStatusCompositionStrategy;
            this.TagMatchStrategy = tagMatchStrategy;
        }

        /// <summary>
        /// Gets the handling concern.
        /// </summary>
        /// <value>The handling concern.</value>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the tags to match.
        /// </summary>
        /// <value>The tags to match.</value>
        public IReadOnlyCollection<NamedValue<string>> TagsToMatch { get; private set; }

        /// <summary>
        /// Gets the handling status composition strategy.
        /// </summary>
        /// <value>The handling status composition strategy.</value>
        public HandlingStatusCompositionStrategy HandlingStatusCompositionStrategy { get; private set; }

        /// <summary>
        /// Gets the tag match strategy.
        /// </summary>
        /// <value>The tag match strategy.</value>
        public TagMatchStrategy TagMatchStrategy { get; private set; }
    }
}
