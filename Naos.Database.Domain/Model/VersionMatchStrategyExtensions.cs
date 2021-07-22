// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionMatchStrategyExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Extensions on a <see cref="VersionMatchStrategy"/>.
    /// </summary>
    public static class VersionMatchStrategyExtensions
    {
        /// <summary>
        /// Throws <see cref="NotSupportedException"/> for values that are not supported by <see cref="IStream"/>'s.
        /// </summary>
        /// <param name="versionMatchStrategy">The version match strategy to verify.</param>
        public static void ThrowOnUnsupportedVersionMatchStrategyForType(
            this VersionMatchStrategy versionMatchStrategy)
        {
            if (versionMatchStrategy != VersionMatchStrategy.Any || versionMatchStrategy != VersionMatchStrategy.SpecifiedVersion)
            {
                throw new NotSupportedException(FormattableString.Invariant($"{nameof(VersionMatchStrategy)} {versionMatchStrategy} is not supported."));
            }
        }
    }
}
