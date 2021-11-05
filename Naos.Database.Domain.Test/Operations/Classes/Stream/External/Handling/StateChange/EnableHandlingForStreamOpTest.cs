// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnableHandlingForStreamOpTest.cs" company="Naos Project">
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
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class EnableHandlingForStreamOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static EnableHandlingForStreamOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<EnableHandlingForStreamOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'details' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<EnableHandlingForStreamOp>();

                            var result = new EnableHandlingForStreamOp(
                                null,
                                referenceObject.Tags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "details", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<EnableHandlingForStreamOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'details' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<EnableHandlingForStreamOp>();

                            var result = new EnableHandlingForStreamOp(
                                Invariant($"  {Environment.NewLine}  "),
                                referenceObject.Tags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "details", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<EnableHandlingForStreamOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<EnableHandlingForStreamOp>();

                            var result = new EnableHandlingForStreamOp(
                                referenceObject.Details,
                                new[] { A.Dummy<NamedValue<string>>(), null, A.Dummy<NamedValue<string>>() });

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tags", "contains at least one null element", },
                    });
        }
    }
}