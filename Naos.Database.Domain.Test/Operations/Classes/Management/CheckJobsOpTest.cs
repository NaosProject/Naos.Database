// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckJobsOpTest.cs" company="Naos Project">
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
    public static partial class CheckJobsOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static CheckJobsOpTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
                            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<CheckJobsOp>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'jobs' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var result = new CheckJobsOp(
                                             null);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "jobs", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<CheckJobsOp>
                {
                    Name = "constructor should throw ArgumentException when parameter 'jobs' contains a null element scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<CheckJobsOp>();

                        var result = new CheckJobsOp(
                                             new ExpectedJobWithinThreshold[0].Concat(referenceObject.Jobs).Concat(new ExpectedJobWithinThreshold[] { null }).Concat(referenceObject.Jobs).ToList());

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentException),
                    ExpectedExceptionMessageContains = new[] { "jobs", "contains at least one null element", },
                });
        }
    }
}