// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelfCancelRunningExecutionException.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// This exception is thrown during execution that allows the protocol to communicate that it wishes to self-cancel the running handling operation.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Exception is for a specific usage, not serializing or marshaling.")]
    [SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Justification = "Exception is for a specific usage, not serializing or marshaling.")]
    public partial class SelfCancelRunningExecutionException : Exception, IHaveDetails, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelfCancelRunningExecutionException"/> class.
        /// </summary>
        /// <param name="details">Details for the event produced.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no tags.</param>
        public SelfCancelRunningExecutionException(
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null)
            : base(details)
        {
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.Details = details;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
