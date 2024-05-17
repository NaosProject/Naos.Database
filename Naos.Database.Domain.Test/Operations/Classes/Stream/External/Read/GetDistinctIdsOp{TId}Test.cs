// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetDistinctIdsOp{TId}Test.cs" company="Naos Project">
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
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class GetDistinctIdsOpTIdTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetDistinctIdsOpTIdTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetDistinctIdsOp<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'versionMatchStrategy' is not supported scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetDistinctIdsOp<Version>>();

                                                   var result = new GetDistinctIdsOp<Version>(
                                                       referenceObject.ObjectTypes,
                                                       VersionMatchStrategy.MinVersion,
                                                       referenceObject.TagsToMatch,
                                                       referenceObject.TagMatchStrategy,
                                                       referenceObject.DeprecatedIdTypes);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(NotSupportedException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "VersionMatchStrategy",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetDistinctIdsOp<Version>>
                        {
                            Name =
                                "constructor should throw ArgumentException when parameter 'tagsToMatch' has null values in a non-empty enumerable scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetDistinctIdsOp<Version>>();

                                                   var result = new GetDistinctIdsOp<Version>(
                                                       referenceObject.ObjectTypes,
                                                       referenceObject.VersionMatchStrategy,
                                                       new[]
                                                       {
                                                           A.Dummy<NamedValue<string>>(),
                                                           null,
                                                       },
                                                       referenceObject.TagMatchStrategy,
                                                       referenceObject.DeprecatedIdTypes);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "tagsToMatch",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<GetDistinctIdsOp<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'tagMatchStrategy' is unknown scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<GetDistinctIdsOp<Version>>();

                                                   var result = new GetDistinctIdsOp<Version>(
                                                       referenceObject.ObjectTypes,
                                                       referenceObject.VersionMatchStrategy,
                                                       referenceObject.TagsToMatch,
                                                       TagMatchStrategy.Unknown,
                                                       referenceObject.DeprecatedIdTypes);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "tagMatchStrategy",
                                                               },
                        })
               .AddScenario(
                   () =>
                       new ConstructorArgumentValidationTestScenario<GetDistinctIdsOp<Version>>
                       {
                           Name = "constructor should throw ArgumentException when parameter 'deprecatedIdTypes' contains a null element.",
                           ConstructionFunc = () =>
                           {
                               var referenceObject = A.Dummy<GetDistinctIdsOp<Version>>();

                               var result = new GetDistinctIdsOp<Version>(
                                   referenceObject.ObjectTypes,
                                   referenceObject.VersionMatchStrategy,
                                   referenceObject.TagsToMatch,
                                   referenceObject.TagMatchStrategy,
                                   new[] { A.Dummy<TypeRepresentation>(), null });

                               return result;
                           },
                           ExpectedExceptionType = typeof(ArgumentException),
                           ExpectedExceptionMessageContains = new[] { "deprecatedIdTypes", "contains at least one null element" },
                       });
        }
    }
}