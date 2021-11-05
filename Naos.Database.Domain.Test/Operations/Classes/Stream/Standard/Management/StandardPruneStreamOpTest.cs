// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardPruneStreamOpTest.cs" company="Naos Project">
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
    public static partial class StandardPruneStreamOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardPruneStreamOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardPruneStreamOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'details' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardPruneStreamOp>();

                            var result = new StandardPruneStreamOp(
                                null,
                                null,
                                referenceObject.Details,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "Either 'internalRecordId' or 'internalRecordDate' must be specified", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardPruneStreamOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'details' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardPruneStreamOp>();

                            var result = new StandardPruneStreamOp(
                                referenceObject.InternalRecordId,
                                A.Dummy<DateTime>().Whose(_ => _.Kind != DateTimeKind.Utc),
                                referenceObject.Details,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "internalRecordDate", "Timestamp must be UTC", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardPruneStreamOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'details' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardPruneStreamOp>();

                            var result = new StandardPruneStreamOp(
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.InternalRecordDate,
                                                 null,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "details", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardPruneStreamOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'details' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardPruneStreamOp>();

                            var result = new StandardPruneStreamOp(
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.InternalRecordDate,
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "details", "white space", },
                    });
        }
    }
}