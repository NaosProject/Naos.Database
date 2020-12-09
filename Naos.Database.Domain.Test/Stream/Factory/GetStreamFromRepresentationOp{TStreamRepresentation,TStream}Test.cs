// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationOp{TStreamRepresentation,TStream}Test.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;

    [SuppressMessage(
        "Microsoft.Maintainability",
        "CA1505:AvoidUnmaintainableCode",
        Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class GetStreamFromRepresentationOpTStreamRepresentationTStreamTest
    {
        [SuppressMessage(
            "Microsoft.Maintainability",
            "CA1505:AvoidUnmaintainableCode",
            Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetStreamFromRepresentationOpTStreamRepresentationTStreamTest()
        {
            ConstructorArgumentValidationTestScenarios.RemoveAllScenarios()
                                                      .AddScenario(
                                                           ConstructorArgumentValidationTestScenario<
                                                                   GetStreamFromRepresentationOp<FileStreamRepresentation, MemoryReadWriteStream>>
                                                              .ForceGeneratedTestsToPassAndWriteMyOwnScenario);
        }
    }
}