// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateStreamResultTest.cs" company="Naos Project">
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
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class CreateStreamResultTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static CreateStreamResultTest()
        {
            ConstructorArgumentValidationTestScenarios.RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<CreateStreamResult>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'alreadyExisted' and 'wasCreated' are false scenario",
                        ConstructionFunc = () =>
                        {
                            var result = new CreateStreamResult(
                                false,
                                false);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "the expectation is that the stream was created or there was an existing one", },
                    });

            EquatableTestScenarios
                .RemoveAllScenarios()
                .AddScenario(
                    () =>
                    {
                        var a = new CreateStreamResult(false, true);
                        var b = new CreateStreamResult(true, false);
                        var c = new CreateStreamResult(true, true);
                        var testOptions = new[]
                        {
                            a,
                            b,
                            c,
                        };

                        return new EquatableTestScenario<CreateStreamResult>
                        {
                            Name = "Default Code Generated Scenario",
                            ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                            ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new CreateStreamResult[]
                            {
                                new CreateStreamResult(
                                    ReferenceObjectForEquatableTestScenarios.AlreadyExisted,
                                    ReferenceObjectForEquatableTestScenarios.WasCreated),
                            },
                            ObjectsThatAreNotEqualToReferenceObject = testOptions.Where(_ => _ != ReferenceObjectForEquatableTestScenarios).ToList(),
                            ObjectsThatAreNotOfTheSameTypeAsReferenceObject = new object[]
                            {
                                A.Dummy<object>(),
                                A.Dummy<string>(),
                                A.Dummy<int>(),
                                A.Dummy<int?>(),
                                A.Dummy<Guid>(),
                            },
                        };
                    });
        }
    }
}