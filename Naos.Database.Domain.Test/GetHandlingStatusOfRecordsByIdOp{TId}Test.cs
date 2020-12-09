// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingStatusOfRecordsByIdOp{TId}Test.cs" company="Naos Project">
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
    public static partial class GetHandlingStatusOfRecordsByIdOpTIdTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetHandlingStatusOfRecordsByIdOpTIdTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordsByIdOp<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordsByIdOp<Version>>();

                                                   var result = new GetHandlingStatusOfRecordsByIdOp<Version>(
                                                       null,
                                                       referenceObject.IdsToMatch,
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TypeVersionMatchStrategy);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "concern",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordsByIdOp<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordsByIdOp<Version>>();

                                                   var result = new GetHandlingStatusOfRecordsByIdOp<Version>(
                                                       Invariant($"  {Environment.NewLine}  "),
                                                       referenceObject.IdsToMatch,
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TypeVersionMatchStrategy);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "concern",
                                                                   "white space",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordsByIdOp<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'idsToMatch' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordsByIdOp<Version>>();

                                                   var result = new GetHandlingStatusOfRecordsByIdOp<Version>(
                                                       referenceObject.Concern,
                                                       null,
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TypeVersionMatchStrategy);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "idsToMatch",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordsByIdOp<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'idsToMatch' is an empty enumerable scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordsByIdOp<Version>>();

                                                   var result = new GetHandlingStatusOfRecordsByIdOp<Version>(
                                                       referenceObject.Concern,
                                                       new List<Version>(),
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TypeVersionMatchStrategy);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "idsToMatch",
                                                                   "is an empty enumerable",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordsByIdOp<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'idsToMatch' contains a null element scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordsByIdOp<Version>>();

                                                   var result = new GetHandlingStatusOfRecordsByIdOp<Version>(
                                                       referenceObject.Concern,
                                                       new Version[0].Concat(referenceObject.IdsToMatch)
                                                                     .Concat(
                                                                          new Version[]
                                                                          {
                                                                              null,
                                                                          })
                                                                     .Concat(referenceObject.IdsToMatch)
                                                                     .ToList(),
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TypeVersionMatchStrategy);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "idsToMatch",
                                                                   "contains at least one null element",
                                                               },
                        });
        }
    }
}