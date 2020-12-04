// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagMatchStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Strategy on how to handle an existing record encountered during a write.
    /// </summary>
    public partial class TagMatchStrategy : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagMatchStrategy"/> class.
        /// Default scenario will match all of the querying tags against the target tags
        /// but will not require target tags to ONLY have those tags.
        /// </summary>
        /// <param name="scopeOfFindSet">The scope of find set.</param>
        /// <param name="scopeOfTarget">The scope of the target.</param>
        public TagMatchStrategy(
            TagMatchScope scopeOfFindSet = TagMatchScope.All,
            TagMatchScope scopeOfTarget = TagMatchScope.Any)
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

    /// <summary>
    /// Extensions used for <see cref="TagMatchStrategy"/>.
    /// </summary>
    public static class TagMatchExtensions
    {
        /// <summary>
        /// Fuzzy matches according to strategy.
        /// </summary>
        /// <param name="findSet">The first.</param>
        /// <param name="target">The second.</param>
        /// <param name="strategy">The strategy.</param>
        /// <returns><c>true</c> if matching according to strategy, <c>false</c> otherwise.</returns>
        public static bool FuzzyMatchAccordingToStrategy(
            this IReadOnlyDictionary<string, string> findSet,
            IReadOnlyDictionary<string, string> target,
            TagMatchStrategy strategy)
        {
            if (strategy == null)
            {
                strategy = new TagMatchStrategy();
            }

            if (!findSet.Any())
            {
                return false;
            }

            if (!target.Any())
            {
                return false;
            }

            if (strategy.ScopeOfFindSet == TagMatchScope.Any && strategy.ScopeOfTarget == TagMatchScope.Any)
            {
                var keyOverlap = findSet.Keys.Intersect(target.Keys);
                foreach (var keyToCheckAny in keyOverlap)
                {
                    if (findSet[keyToCheckAny].Equals(target[keyToCheckAny]))
                    {
                        // only need one target key to match.
                        return true;
                    }
                }

                return false;
            }
            else if (strategy.ScopeOfFindSet == TagMatchScope.Any && strategy.ScopeOfTarget == TagMatchScope.All)
            {
                if (target.Keys.Except(findSet.Keys).Any())
                {
                    // cannot have any target keys not appearing in find set.
                    return false;
                }

                foreach (var targetKey in target.Keys)
                {
                    if (!findSet[targetKey].Equals(target[targetKey]))
                    {
                        return false;
                    }
                }

                return true;
            }
            else if (strategy.ScopeOfFindSet == TagMatchScope.All && strategy.ScopeOfTarget == TagMatchScope.Any)
            {
                if (findSet.Keys.Except(target.Keys).Any())
                {
                    // cannot have any in the find set not appearing in the target.
                    return false;
                }

                foreach (var findSetKey in findSet.Keys)
                {
                    if (!findSet[findSetKey].Equals(target[findSetKey]))
                    {
                        return false;
                    }
                }

                return true;
            }
            else if (strategy.ScopeOfFindSet == TagMatchScope.All && strategy.ScopeOfTarget == TagMatchScope.All)
            {
                var result = findSet.IsReadOnlyDictionaryEqualTo(target);
                return result;
            }

            throw new NotSupportedException(Invariant($"{nameof(TagMatchStrategy)} is not supported in the combination of {nameof(TagMatchStrategy.ScopeOfFindSet)}: {strategy.ScopeOfFindSet} and {nameof(TagMatchStrategy.ScopeOfTarget)}: {strategy.ScopeOfTarget}"));
        }
    }
}
