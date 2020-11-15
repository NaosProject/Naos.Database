// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrunedEvent{TId}Test.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class PrunedEventTIdTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static PrunedEventTIdTest()
        {
            ConstructorArgumentValidationTestScenarios.RemoveAllScenarios();
            ConstructorArgumentValidationTestScenarios
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<PrunedEvent<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'details' is null scenario",
                            ConstructionFunc = () =>
                            {
                                var referenceObject = A.Dummy<PrunedEvent<Version>>();

                                var result = new PrunedEvent<Version>(
                                    referenceObject.Id,
                                    referenceObject.TimestampUtc,
                                    null,
                                    referenceObject.Tags);

                                return result;
                            },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "details",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<PrunedEvent<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'details' is white space scenario",
                            ConstructionFunc = () =>
                            {
                                var referenceObject = A.Dummy<PrunedEvent<Version>>();

                                var result = new PrunedEvent<Version>(
                                    referenceObject.Id,
                                    referenceObject.TimestampUtc,
                                    Invariant($"  {Environment.NewLine}  "),
                                    referenceObject.Tags);

                                return result;
                            },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "details",
                                                                   "white space",
                                                               },
                        });
        }
    }
}