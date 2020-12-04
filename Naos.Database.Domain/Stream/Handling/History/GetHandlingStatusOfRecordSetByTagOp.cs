// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingStatusOfRecordSetByTagOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Gets the composite status of the set of records by specified tag matching on all locators.
    /// </summary>
    public partial class GetHandlingStatusOfRecordSetByTagOp : ReturningOperationBase<HandlingStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingStatusOfRecordSetByTagOp"/> class.
        /// </summary>
        /// <param name="tagsToMatch">The internal record ids to treat as a composite status.</param>
        public GetHandlingStatusOfRecordSetByTagOp(
            IReadOnlyDictionary<string, string> tagsToMatch)
        {
            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotBeNull().And().NotBeEmptyDictionary();

            this.TagsToMatch = tagsToMatch;
        }

        /// <summary>
        /// Gets the tags to match.
        /// </summary>
        /// <value>The tags to match.</value>
        public IReadOnlyDictionary<string, string> TagsToMatch { get; private set; }
    }
}
