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
