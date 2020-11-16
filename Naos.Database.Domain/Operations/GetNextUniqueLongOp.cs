// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetNextUniqueLongOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;

    /// <summary>
    /// Get a unique 64-bit integer, in sequence, that is brokered via an internal stream construct.
    /// </summary>
    public partial class GetNextUniqueLongOp : ReturningOperationBase<long>, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNextUniqueLongOp"/> class.
        /// </summary>
        /// <param name="details">The optional details.</param>
        /// <param name="tags">The optional tags.</param>
        public GetNextUniqueLongOp(
            string details = null,
            IReadOnlyDictionary<string, string> tags = null)
        {
            this.Details = details;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
