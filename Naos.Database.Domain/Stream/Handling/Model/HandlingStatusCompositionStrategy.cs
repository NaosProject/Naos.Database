// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingStatusCompositionStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Strategy on how to compose multiple strategies.
    /// </summary>
    public class HandlingStatusCompositionStrategy : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingStatusCompositionStrategy"/> class.
        /// </summary>
        /// <param name="ignoreCancel">if set to <c>true</c> [ignore cancel].</param>
        public HandlingStatusCompositionStrategy(
            bool ignoreCancel = false)
        {
            this.IgnoreCancel = ignoreCancel;
        }

        /// <summary>
        /// Gets a value indicating whether [ignore cancel].
        /// </summary>
        /// <value><c>true</c> if [ignore cancel]; otherwise, <c>false</c>.</value>
        public bool IgnoreCancel { get; private set; }
    }

    /// <summary>
    /// Extensions on <see cref="HandlingStatus"/> and <see cref="HandlingStatusCompositionStrategy"/>.
    /// </summary>
    public static class HandlingStatusExtensions
    {
        /// <summary>
        /// Reduces to composite handling status.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <param name="statuses">The statuses.</param>
        /// <returns>HandlingStatus.</returns>
        public static HandlingStatus ReduceToCompositeHandlingStatus(
            this HandlingStatusCompositionStrategy strategy,
            IReadOnlyCollection<HandlingStatus> statuses)
        {
            if (statuses.Any(_ => _ == HandlingStatus.Failed))
            {
                return HandlingStatus.Failed;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Running))
            {
                return HandlingStatus.Running;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Canceled))
            {
                return HandlingStatus.Canceled;
            }

            if (statuses.Any(_ => _ == HandlingStatus.SelfCanceled))
            {
                return HandlingStatus.SelfCanceled;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Unknown))
            {
                return HandlingStatus.Unknown;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Requested))
            {
                return HandlingStatus.Requested;
            }

            if (statuses.Any(_ => _ == HandlingStatus.None))
            {
                return HandlingStatus.None;
            }

            if (statuses.All(_ => _ == HandlingStatus.Completed))
            {
                return HandlingStatus.Completed;
            }

            throw new NotSupportedException(Invariant($"Could not reduce statuses to a composite status; {statuses.Select(_ => _.ToString()).ToDelimitedString(",")}"));
        }

        /// <summary>
        /// Reduces to composite handling status.
        /// </summary>
        /// <param name="statuses">The statuses.</param>
        /// <param name="strategy">The strategy.</param>
        /// <returns>HandlingStatus.</returns>
        public static HandlingStatus ReduceToCompositeHandlingStatus(
            this IReadOnlyCollection<HandlingStatus> statuses,
            HandlingStatusCompositionStrategy strategy)
        {
            var result = strategy.ReduceToCompositeHandlingStatus(statuses);
            return result;
        }
    }
}
