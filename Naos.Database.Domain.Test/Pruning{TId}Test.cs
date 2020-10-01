// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pruning{TId}Test.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;
    using Xunit;

    using static System.FormattableString;

    public static partial class PruningTest
    {
        static PruningTest()
        {
            ConstructorArgumentValidationTestScenarios.RemoveAllScenarios();
            ConstructorArgumentValidationTestScenarios
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<Pruning<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'pruner' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<Pruning<Version>>();

                                                   var result = new Pruning<Version>(
                                                       referenceObject.Id,
                                                       referenceObject.TimestampUtc,
                                                       null,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "pruner"
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<Pruning<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'pruner' is white space scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<Pruning<Version>>();

                                                   var result = new Pruning<Version>(
                                                       referenceObject.Id,
                                                       referenceObject.TimestampUtc,
                                                       Invariant($"  {Environment.NewLine}  "),
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "pruner",
                                                                   "white space"
                                                               },
                        });
        }
    }
}