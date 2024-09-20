// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeSelectionStrategyExtensionsTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    public static class TypeSelectionStrategyExtensionsTest
    {
        [Fact]
        public static void Apply___Should_throw_ArgumentOutOfRangeException___When_parameter_strategy_is_Unknown()
        {
            // Arrange, Act
            var actual = Record.Exception(() => TypeSelectionStrategy.Unknown.Apply(A.Dummy<object>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentOutOfRangeException>();
            actual.Message.AsTest().Must().ContainString("Unknown");
        }

        [Fact]
        public static void Apply___Should_throw_InvalidOperationException___When_parameter_strategy_is_UseRuntimeType_and_parameter_item_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => TypeSelectionStrategy.UseRuntimeType.Apply<object>(null));

            // Assert
            actual.AsTest().Must().BeOfType<InvalidOperationException>();
            actual.Message.AsTest().Must().ContainString("Cannot apply TypeSelectionStrategy.UseRuntimeType when item is null.");
        }

        [Fact]
        public static void Apply___Should_return_runtime_type_of_item___When_parameter_strategy_is_UseRuntimeType()
        {
            // Arrange, Act
            var actual = TypeSelectionStrategy.UseRuntimeType.Apply<object>(A.Dummy<string>());

            // Assert
            actual.AsTest().Must().BeEqualTo(typeof(string));
        }

        [Fact]
        public static void Apply___Should_return_declared_type_of_item___When_parameter_strategy_is_UseDeclaredType()
        {
            // Arrange, Act
            var actual = TypeSelectionStrategy.UseDeclaredType.Apply<object>(A.Dummy<string>());

            // Assert
            actual.AsTest().Must().BeEqualTo(typeof(object));
        }
    }
}