// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingStatusExtensionsTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using Xunit;

    public static class HandlingStatusExtensionsTests
    {
        [Fact]
        public static void ToCompositeHandlingStatus____Works____With_only_ArchivedAfter()
        {
            // Arrange

            var tests = new[]
                        {
                            new
                            {
                                Expected = CompositeHandlingStatus.AllArchivedAfterFailure,
                                Inputs = new[]
                                         {
                                             HandlingStatus.ArchivedAfterFailure,
                                         },
                            },
                            new
                            {
                                Expected = CompositeHandlingStatus.SomeAvailable
                                         | CompositeHandlingStatus.SomeArchivedAfterFailure
                                         | CompositeHandlingStatus.NoneCompleted
                                         | CompositeHandlingStatus.NoneRunning
                                         | CompositeHandlingStatus.NoneDisabled
                                         | CompositeHandlingStatus.NoneFailed,
                                Inputs = new[]
                                         {
                                             HandlingStatus.AvailableAfterFailure,
                                             HandlingStatus.ArchivedAfterFailure,
                                         },
                            },
                        };

            // Act & Assert
            foreach (var test in tests)
            {
                var actual = test.Inputs.ToCompositeHandlingStatus();
                actual.MustForTest().BeEqualTo(test.Expected);
            }
        }
    }
}