// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingStatusOfRecordSetByTagOpTest.cs" company="Naos Project">
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
    public static partial class GetHandlingStatusOfRecordSetByTagOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetHandlingStatusOfRecordSetByTagOpTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordSetByTagOp>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordSetByTagOp>();

                                                   var result = new GetHandlingStatusOfRecordSetByTagOp(
                                                       null,
                                                       referenceObject.TagsToMatch,
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TagMatchStrategy);

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
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordSetByTagOp>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordSetByTagOp>();

                                                   var result = new GetHandlingStatusOfRecordSetByTagOp(
                                                       Invariant($"  {Environment.NewLine}  "),
                                                       referenceObject.TagsToMatch,
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TagMatchStrategy);

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
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordSetByTagOp>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'tagsToMatch' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordSetByTagOp>();

                                                   var result = new GetHandlingStatusOfRecordSetByTagOp(
                                                       referenceObject.Concern,
                                                       null,
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TagMatchStrategy);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "tagsToMatch",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetHandlingStatusOfRecordSetByTagOp>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' is an empty dictionary scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetHandlingStatusOfRecordSetByTagOp>();

                                                   var result = new GetHandlingStatusOfRecordSetByTagOp(
                                                       referenceObject.Concern,
                                                       new Dictionary<string, string>(),
                                                       referenceObject.HandlingStatusCompositionStrategy,
                                                       referenceObject.TagMatchStrategy);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "tagsToMatch",
                                                                   "is an empty dictionary",
                                                               },
                        });
        }
    }
}