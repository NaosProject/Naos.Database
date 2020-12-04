// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagMatchStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Strategy on how to handle an existing record encountered during a write.
    /// </summary>
    public class TagMatchStrategy : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagMatchStrategy"/> class.
        /// </summary>
        /// <param name="scopeOfFindSet">The scope of find set.</param>
        /// <param name="scopeOfTarget">The scope of the target.</param>
        public TagMatchStrategy(
            TagMatchScope scopeOfFindSet,
            TagMatchScope scopeOfTarget)
        {
            this.ScopeOfFindSet = scopeOfFindSet;
            this.ScopeOfTarget = scopeOfTarget;
        }

        /// <summary>
        /// Gets the scope of find set.
        /// </summary>
        /// <value>The scope of find set.</value>
        public TagMatchScope ScopeOfFindSet { get; private set; }

        /// <summary>
        /// Gets the scope of target.
        /// </summary>
        /// <value>The scope of target.</value>
        public TagMatchScope ScopeOfTarget { get; private set; }
    }

    /// <summary>
    /// Scope of tag consideration.
    /// </summary>
    public enum TagMatchScope
    {
        /// <summary>
        /// All of the tags should be considered.
        /// </summary>
        All,

        /// <summary>
        /// Any of the tags should be considered.
        /// </summary>
        Any,
    }
}
