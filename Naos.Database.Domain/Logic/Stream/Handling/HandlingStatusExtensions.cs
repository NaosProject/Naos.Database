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
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extension methods on <see cref="HandlingStatus"/>.
    /// </summary>
    public static class HandlingStatusExtensions
    {
        private static readonly HashSet<HandlingStatus> SupportedHandlingStatuses = new HashSet<HandlingStatus>(
            new[]
            {
                HandlingStatus.AvailableAfterExternalCancellation,
                HandlingStatus.AvailableAfterFailure,
                HandlingStatus.AvailableAfterSelfCancellation,
                HandlingStatus.AvailableByDefault,
                HandlingStatus.Completed,
                HandlingStatus.DisabledForRecord,
                HandlingStatus.DisabledForStream,
                HandlingStatus.Failed,
                HandlingStatus.Running,
            });

        /// <summary>
        /// Determines whether the specified status indicates that the record is available for handling.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>
        /// <c>true</c> if the status indicates that the record is available for handling; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAvailable(
            this HandlingStatus status)
        {
            switch (status)
            {
                case HandlingStatus.AvailableByDefault:
                case HandlingStatus.AvailableAfterFailure:
                case HandlingStatus.AvailableAfterExternalCancellation:
                case HandlingStatus.AvailableAfterSelfCancellation:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified status indicates that the record has its handling disabled.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>
        /// <c>true</c> if the status indicates that the record has its handling disabled; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDisabled(
            this HandlingStatus status)
        {
            switch (status)
            {
                case HandlingStatus.DisabledForRecord:
                case HandlingStatus.DisabledForStream:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Synthesizes the <see cref="HandlingStatus"/> of one or more records.
        /// </summary>
        /// <param name="statuses">The statuses.</param>
        /// <returns>
        /// The composite handling status.
        /// </returns>
        public static CompositeHandlingStatus ToCompositeHandlingStatus(
            this IReadOnlyCollection<HandlingStatus> statuses)
        {
            // This is protection against other HandlingStatus values being added without this method being updated accordingly.
            statuses.MustForArg(nameof(statuses)).NotBeNull().And().Each().BeEqualToAnyOf(SupportedHandlingStatuses);

            var result = CompositeHandlingStatus.Unknown;

            if (statuses.Any(_ => _.IsAvailable()))
            {
                result |= CompositeHandlingStatus.SomeAvailable;
            }
            else
            {
                result |= CompositeHandlingStatus.NoneAvailable;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Running))
            {
                result |= CompositeHandlingStatus.SomeRunning;
            }
            else
            {
                result |= CompositeHandlingStatus.NoneRunning;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Failed))
            {
                result |= CompositeHandlingStatus.SomeFailed;
            }
            else
            {
                result |= CompositeHandlingStatus.NoneFailed;
            }

            if (statuses.Any(_ => _ == HandlingStatus.Completed))
            {
                result |= CompositeHandlingStatus.SomeCompleted;
            }
            else
            {
                result |= CompositeHandlingStatus.NoneCompleted;
            }

            if (statuses.Any(_ => _.IsDisabled()))
            {
                result |= CompositeHandlingStatus.SomeDisabled;
            }
            else
            {
                result |= CompositeHandlingStatus.NoneDisabled;
            }

            return result;
        }

        /// <summary>
        /// Gets all handling statuses from the supplied map.
        /// </summary>
        /// <param name="statusMap">The status map.</param>
        /// <returns>Collection of <see cref="HandlingStatus"/>.</returns>
        public static IReadOnlyCollection<HandlingStatus> GetAllHandlingStatuses(
            this IReadOnlyDictionary<IResourceLocator, IReadOnlyDictionary<long, HandlingStatus>> statusMap)
        {
            var result = statusMap.Values.SelectMany(_ => _.Values).ToList();
            return result;
        }

        /// <summary>
        /// Gets all handling statuses from the supplied map.
        /// </summary>
        /// <param name="statusMap">The status map.</param>
        /// <returns>Collection of <see cref="HandlingStatus"/>.</returns>
        public static IReadOnlyDictionary<IRecordLocator, HandlingStatus> ConvertToRecordLocatorToStatusMap(
            this IReadOnlyDictionary<IResourceLocator, IReadOnlyDictionary<long, HandlingStatus>> statusMap)
        {
            var list = statusMap
                      .SelectMany(
                           _ => _.Value
                                 .Select(__ => new Tuple<IRecordLocator, HandlingStatus>(new RecordLocator(_.Key, __.Key), __.Value)))
                      .ToList();
            var result = list.ToDictionary(k => k.Item1, v => v.Item2);
            return result;
        }
    }
}