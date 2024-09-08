// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionMatchStrategyExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Extensions on a <see cref="VersionMatchStrategy"/>.
    /// </summary>
    public static class VersionMatchStrategyExtensions
    {
        /// <summary>
        /// Throws <see cref="NotSupportedException"/> for <see cref="VersionMatchStrategy"/> values that are not supported by an <see cref="IStream"/>.
        /// </summary>
        /// <param name="versionMatchStrategy">The version match strategy to verify.</param>
        public static void ThrowOnUnsupportedVersionMatchStrategyForType(
            this VersionMatchStrategy versionMatchStrategy)
        {
            versionMatchStrategy.MustForArg(nameof(versionMatchStrategy)).NotBeEqualTo(VersionMatchStrategy.Unknown);

            if ((versionMatchStrategy != VersionMatchStrategy.Any) && (versionMatchStrategy != VersionMatchStrategy.SpecifiedVersion))
            {
                throw new NotSupportedException(FormattableString.Invariant($"{nameof(VersionMatchStrategy)} {versionMatchStrategy} is not supported."));
            }
        }
    }
}
