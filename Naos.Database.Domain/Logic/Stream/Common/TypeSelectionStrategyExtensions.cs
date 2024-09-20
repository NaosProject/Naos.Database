// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeSelectionStrategyExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Extension methods on <see cref="TypeSelectionStrategy"/>.
    /// </summary>
    public static class TypeSelectionStrategyExtensions
    {
        /// <summary>
        /// Applies the specified <see cref="TypeSelectionStrategy"/> to select a type.
        /// </summary>
        /// <typeparam name="TDeclared">The declared type.</typeparam>
        /// <param name="strategy">The strategy to use.</param>
        /// <param name="item">The item that is the subject of this type selection.</param>
        /// <returns>
        /// The selected type.
        /// </returns>
        public static Type Apply<TDeclared>(
            this TypeSelectionStrategy strategy,
            TDeclared item)
        {
            strategy.MustForArg(nameof(strategy)).NotBeEqualTo(TypeSelectionStrategy.Unknown);

            Type result;

            // If the declared type is NullIdentifier, then there's no such thing as a runtime type.
            // The item, at runtime, is always expected to be null and thus GetType() will throw.
            // NullIdentifier is a placeholder type to be used when you put an object without an id.
            // In this scenario, you always want to use the declared type for NullIdentifier,
            // but you may also be using TypeSelectionStrategy.UseRuntimeType to target the type to use for
            // the object your put putting.
            if (typeof(TDeclared) == typeof(NullIdentifier))
            {
                return typeof(NullIdentifier);
            }

            if (strategy == TypeSelectionStrategy.UseDeclaredType)
            {
                result = typeof(TDeclared);
            }
            else if (strategy == TypeSelectionStrategy.UseRuntimeType)
            {
                if (item == null)
                {
                    throw new InvalidOperationException(Invariant($"Cannot apply {nameof(TypeSelectionStrategy)}.{nameof(TypeSelectionStrategy.UseRuntimeType)} when {nameof(item)} is null."));
                }
                else
                {
                    result = item.GetType();
                }
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(TypeSelectionStrategy)} is not supported: {strategy}."));
            }

            return result;
        }
    }
}
