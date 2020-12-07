// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingStatusExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;

    /// <summary>
    /// Extensions on <see cref="HandlingStatus"/> and <see cref="HandlingStatusCompositionStrategy"/>.
    /// </summary>
    public static class HandlingStatusExtensions
    {
        /// <summary>
        /// Determines whether the specified status is in need of handling.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns><c>true</c> if the specified status is active; otherwise, <c>false</c>.</returns>
        public static bool IsHandlingNeeded(
            this HandlingStatus status)
        {
            switch (status)
            {
                case HandlingStatus.Requested:
                case HandlingStatus.RetryFailed:
                case HandlingStatus.CanceledRunning:
                case HandlingStatus.SelfCanceledRunning:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Reduces to composite handling status.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <param name="statuses">The statuses.</param>
        /// <returns>HandlingStatus.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public static HandlingStatus ReduceToCompositeHandlingStatus(
            this HandlingStatusCompositionStrategy strategy,
            IReadOnlyCollection<HandlingStatus> statuses)
        {
            strategy.MustForArg(nameof(strategy)).NotBeNull();
            statuses.MustForArg(nameof(statuses)).NotBeNull();

            if (!statuses.Any())
            {
                return HandlingStatus.None;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Blocked))
            {
                return HandlingStatus.Blocked;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Failed))
            {
                return HandlingStatus.Failed;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Running))
            {
                return HandlingStatus.Running;
            }

            if (!strategy.IgnoreCancel && statuses.Any(_ => _ == HandlingStatus.Canceled))
            {
                return HandlingStatus.Canceled;
            }

            if (statuses.Any(_ => _ == HandlingStatus.SelfCanceledRunning))
            {
                return HandlingStatus.SelfCanceledRunning;
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

            if (statuses.All(_ => strategy.IgnoreCancel ? (_ == HandlingStatus.Canceled || _ == HandlingStatus.Completed) : _ == HandlingStatus.Completed))
            {
                return HandlingStatus.Completed;
            }

            throw new NotSupportedException(FormattableString.Invariant($"Could not reduce statuses to a composite status; {statuses.Select(_ => _.ToString()).ToDelimitedString(",")}"));
        }

        /// <summary>
        /// Reduces to composite handling status.
        /// </summary>
        /// <param name="statuses">The statuses.</param>
        /// <param name="strategy">The strategy.</param>
        /// <returns>HandlingStatus.</returns>
        public static HandlingStatus ReduceToCompositeHandlingStatus(
            this IReadOnlyCollection<HandlingStatus> statuses,
            HandlingStatusCompositionStrategy strategy = null)
        {
            var result = (strategy ?? new HandlingStatusCompositionStrategy()).ReduceToCompositeHandlingStatus(statuses);
            return result;
        }
    }
}