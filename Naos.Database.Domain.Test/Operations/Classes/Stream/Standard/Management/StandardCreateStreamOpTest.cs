// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardCreateStreamOpTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using Xunit;

    public static partial class StandardCreateStreamOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardCreateStreamOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardCreateStreamOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'existingStreamStrategy' is Unknown",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardCreateStreamOp>();

                            var result = new StandardCreateStreamOp(
                                referenceObject.StreamRepresentation,
                                ExistingStreamStrategy.Unknown);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "existingStreamStrategy", "Unknown" },
                    });
        }
    }
}