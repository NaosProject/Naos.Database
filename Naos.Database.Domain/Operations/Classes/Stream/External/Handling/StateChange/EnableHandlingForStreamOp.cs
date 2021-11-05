// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnableHandlingForStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Enables handling for all records on an <see cref="IRecordHandlingOnlyStream"/>,
    /// which was previously disabled when <see cref="DisableHandlingForStreamOp"/> was executed.
    /// </summary>
    public partial class EnableHandlingForStreamOp : VoidOperationBase, IHaveDetails, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnableHandlingForStreamOp"/> class.
        /// </summary>
        /// <param name="details">Details to write to the resulting <see cref="IHandlingEvent"/>.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no tags.</param>
        public EnableHandlingForStreamOp(
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            this.Details = details;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}