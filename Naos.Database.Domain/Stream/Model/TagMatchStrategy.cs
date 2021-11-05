// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagMatchStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
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
        public TagMatchScope ScopeOfFindSet { get; private set; }

        /// <summary>
        /// Gets the scope of target.
        /// </summary>
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
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public static bool FuzzyMatchAccordingToStrategy(
            this IReadOnlyCollection<NamedValue<string>> findSet,
            IReadOnlyCollection<NamedValue<string>> target,
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

            var findSetKeys = findSet.Select(_ => _.Name).Distinct().ToList();
            var targetKeys = target.Select(_ => _.Name).Distinct().ToList();
            if (strategy.ScopeOfFindSet == TagMatchScope.Any && strategy.ScopeOfTarget == TagMatchScope.Any)
            {
                var keyOverlap = findSetKeys.Intersect(targetKeys);
                foreach (var keyToCheckAny in keyOverlap)
                {
                    // if (findSet[keyToCheckAny].Equals(target[keyToCheckAny])) -- old code
                    if (target.Any(_ => _.Name == keyToCheckAny
                                     && findSet.Any(__ => __.Name == keyToCheckAny && __.Value == _.Value)))
                    {
                        // only need one target key to match.
                        return true;
                    }
                }

                return false;
            }
            else if (strategy.ScopeOfFindSet == TagMatchScope.Any && strategy.ScopeOfTarget == TagMatchScope.All)
            {
                if (targetKeys.Except(findSetKeys).Any())
                {
                    // cannot have any target keys not appearing in find set.
                    return false;
                }

                foreach (var targetKey in targetKeys)
                {
                    // if (!findSet[targetKey].Equals(target[targetKey])) -- old code
                    if (!target.Any(_ => _.Name == targetKey
                                      && findSet.Any(__ => __.Name == targetKey && __.Value == _.Value)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else if (strategy.ScopeOfFindSet == TagMatchScope.All && strategy.ScopeOfTarget == TagMatchScope.Any)
            {
                if (findSetKeys.Except(targetKeys).Any())
                {
                    // cannot have any in the find set not appearing in the target.
                    return false;
                }

                foreach (var findSetKey in findSetKeys)
                {
                    // if (!findSet[findSetKey].Equals(target[findSetKey])) -- old code
                    if (!target.Any(_ => _.Name == findSetKey
                                      && findSet.Any(__ => __.Name == findSetKey && __.Value == _.Value)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else if (strategy.ScopeOfFindSet == TagMatchScope.All && strategy.ScopeOfTarget == TagMatchScope.All)
            {
                var result = findSet.IsUnorderedEqualTo(target);
                return result;
            }

            throw new NotSupportedException(Invariant($"{nameof(TagMatchStrategy)} is not supported in the combination of {nameof(TagMatchStrategy.ScopeOfFindSet)}: {strategy.ScopeOfFindSet} and {nameof(TagMatchStrategy.ScopeOfTarget)}: {strategy.ScopeOfTarget}"));
        }
    }
}
