// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetNextUniqueLongOpTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using Xunit;

    using static System.FormattableString;

    public static partial class GetNextUniqueLongOpTest
    {
        [SuppressMessage(
            "Microsoft.Maintainability",
            "CA1505:AvoidUnmaintainableCode",
            Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetNextUniqueLongOpTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(ConstructorArgumentValidationTestScenario<GetNextUniqueLongOp>.ForceGeneratedTestsToPassAndWriteMyOwnScenario);
        }
    }
}