// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestByIdAndTypeOp{TId,TObject}Test.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;

    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using Xunit;

    using static System.FormattableString;

    public static partial class GetLatestByIdAndTypeOpTest
    {
        static GetLatestByIdAndTypeOpTest()
        {
            ConstructorArgumentValidationTestScenarios.RemoveAllScenarios();
            ConstructorArgumentValidationTestScenarios
               .AddScenario(ConstructorArgumentValidationTestScenario<GetLatestByIdAndTypeOp<Version, Version>>.ConstructorCannotThrowScenario);
        }
    }
}