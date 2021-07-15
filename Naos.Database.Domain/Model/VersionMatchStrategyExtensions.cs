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

    public static class VersionMatchStrategyExtensions
    {
        public static void ThrowOnUnsupportedVersionMatchStrategyForType(
            this VersionMatchStrategy versionMatchStrategy)
        {
            if (versionMatchStrategy != VersionMatchStrategy.Any || versionMatchStrategy != VersionMatchStrategy.SpecifiedVersion)
            {
                throw new ArgumentException(FormattableString.Invariant($"{nameof(VersionMatchStrategy)} {versionMatchStrategy} is not supported."));
            }
        }
    }
}
