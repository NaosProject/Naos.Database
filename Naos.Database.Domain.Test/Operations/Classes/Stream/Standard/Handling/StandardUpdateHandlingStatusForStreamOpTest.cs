// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardUpdateHandlingStatusForStreamOpTest.cs" company="Naos Project">
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
    public static partial class StandardUpdateHandlingStatusForStreamOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardUpdateHandlingStatusForStreamOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardUpdateHandlingStatusForStreamOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'newStatus' is neither HandlingStatus.DisabledForStream nor HandlingStatus.AvailableByDefault",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForStreamOp>();

                            var result = new StandardUpdateHandlingStatusForStreamOp(
                                                 A.Dummy<HandlingStatus>().ThatIsNotIn(new[] { HandlingStatus.DisabledForStream, HandlingStatus.AvailableByDefault }),
                                                 referenceObject.Details,
                                                 referenceObject.Tags,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "newStatus", "DisabledForStream", "AvailableByDefault" },
                    });
        }
    }
}