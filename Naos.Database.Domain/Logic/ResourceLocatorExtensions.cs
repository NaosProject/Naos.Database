// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceLocatorExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Extensions on <see cref="IResourceLocator"/> and <see cref="ISpecifyResourceLocator"/>.
    /// </summary>
    public static class ResourceLocatorExtensions
    {
        /// <summary>
        /// Confirm the locator can be converted to the specified type (throw if not) and then return the converted locator.
        /// </summary>
        /// <typeparam name="TLocator">The type of the target locator.</typeparam>
        /// <param name="locator">The locator to convert.</param>
        /// <returns>Converted TLocator if possible.</returns>
        public static TLocator ConfirmAndConvert<TLocator>(
            this IResourceLocator locator)
            where TLocator : IResourceLocator
        {
            locator.MustForArg(nameof(locator)).NotBeNull();

            if (!(locator is TLocator result))
            {
                throw new NotSupportedException(Invariant($"Cannot use locator of type '{locator.GetType().ToStringReadable()}'; expected {nameof(TLocator)}."));
            }

            return result;
        }

        /// <summary>
        /// Gets the specified locator from a <see cref="ISpecifyResourceLocator"/> and converts to target type of <see cref="IResourceLocator"/> if possible, throws if not.
        /// </summary>
        /// <typeparam name="TLocator">The type of the target locator.</typeparam>
        /// <param name="specifyResourceLocator">The <see cref="ISpecifyResourceLocator"/>.</param>
        /// <returns>TLocator.</returns>
        public static TLocator GetSpecifiedLocatorConverted<TLocator>(
            this ISpecifyResourceLocator specifyResourceLocator)
            where TLocator : class, IResourceLocator
        {
            specifyResourceLocator.MustForArg(nameof(specifyResourceLocator)).NotBeNull();

            var result = specifyResourceLocator.SpecifiedResourceLocator?.ConfirmAndConvert<TLocator>();

            return result;
        }
    }
}
