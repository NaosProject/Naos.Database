// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeRepresentationWithAndWithoutVersionTest.cs" company="Naos Project">
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
    using OBeautifulCode.Representation.System;
    using Xunit;

    using static System.FormattableString;

    public static partial class TypeRepresentationWithAndWithoutVersionTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static TypeRepresentationWithAndWithoutVersionTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<TypeRepresentationWithAndWithoutVersion>
                   {
                       Name = "constructor should throw ArgumentNullException when parameter 'withVersion' is null scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<TypeRepresentationWithAndWithoutVersion>();

                           var result = new TypeRepresentationWithAndWithoutVersion(
                               null,
                               referenceObject.WithoutVersion);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentNullException),
                       ExpectedExceptionMessageContains = new[] { "withVersion", },
                   })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<TypeRepresentationWithAndWithoutVersion>
                   {
                       Name = "constructor should throw ArgumentNullException when parameter 'withVersion' does not contain an AssemblyVersion scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<TypeRepresentationWithAndWithoutVersion>();

                           var result = new TypeRepresentationWithAndWithoutVersion(
                               typeof(TypeRepresentationWithAndWithoutVersionTest).ToRepresentation().RemoveAssemblyVersions(),
                               referenceObject.WithoutVersion);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentNullException),
                       ExpectedExceptionMessageContains = new[] { "withVersion.AssemblyVersion", },
                   });

            ConstructorPropertyAssignmentTestScenarios
                .AddScenario(() =>
                    new ConstructorPropertyAssignmentTestScenario<TypeRepresentationWithAndWithoutVersion>
                    {
                        Name = "WithoutVersion should return 'withVersion' with AssemblyVersion removed when 'withoutVersion' is null",
                        SystemUnderTestExpectedPropertyValueFunc = () =>
                        {
                            var referenceObject = A.Dummy<TypeRepresentationWithAndWithoutVersion>();

                            var result = new SystemUnderTestExpectedPropertyValue<TypeRepresentationWithAndWithoutVersion>
                            {
                                SystemUnderTest = new TypeRepresentationWithAndWithoutVersion(
                                    referenceObject.WithVersion,
                                    null),
                                ExpectedPropertyValue = referenceObject.WithVersion.RemoveAssemblyVersions(),
                            };

                            return result;
                        },
                        PropertyName = "WithoutVersion",
                    });
        }
    }
}