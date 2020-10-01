// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pruning{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Event indicating a prune actively occurring on the stream.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    public partial class Pruning<TId> : EventBase<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pruning{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="pruner">The active harnessed code that is pruning.</param>
        /// <param name="tags">The optional tags.</param>
        public Pruning(
            TId id,
            DateTime timestampUtc,
            string pruner,
            IReadOnlyDictionary<string, string> tags = null)
            : base(id, timestampUtc, tags)
        {
            pruner.MustForArg(nameof(pruner)).NotBeNullNorWhiteSpace();
            this.Pruner = pruner;
        }

        /// <summary>
        /// Gets the pruner.
        /// </summary>
        /// <value>The pruner.</value>
        public string Pruner { get; private set; }
    }
}
