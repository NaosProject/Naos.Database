// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableRecordHandlingForStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Block the handling of records on an <see cref="IRecordHandlingOnlyStream"/>
    /// until a <see cref="EnableRecordHandlingForStreamOp"/> is executed.
    /// </summary>
    public partial class DisableRecordHandlingForStreamOp : VoidOperationBase, IHaveDetails, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisableRecordHandlingForStreamOp"/> class.
        /// </summary>
        /// <param name="details">The details about the block.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting event(s).  DEFAULT is no tags.</param>
        public DisableRecordHandlingForStreamOp(
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}
