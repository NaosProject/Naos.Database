// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelfCancelRunningExecutionException.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Exception to be thrown during execution that allows the protocol to communicate the difference between a failure and a self cancellation.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Exception is for a specific usage, not serializing or marshaling.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Justification = "Exception is for a specific usage, not serializing or marshaling.")]
    [Serializable]
    public partial class SelfCancelRunningExecutionException : Exception, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelfCancelRunningExecutionException"/> class.
        /// </summary>
        /// <param name="details">Details for the event produced.</param>
        /// <param name="tags">The optional tags for the event produced.</param>
        public SelfCancelRunningExecutionException(
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.Details = details;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
