// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestStringSerializedObjectByIdOp{TId}Test.cs" company="Naos Project">
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
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class GetLatestStringSerializedObjectByIdOpTIdTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetLatestStringSerializedObjectByIdOpTIdTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestStringSerializedObjectByIdOp<Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'versionMatchStrategy' is VersionMatchStrategy.Unknown",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestStringSerializedObjectByIdOp<Version>>();

                            var result = new GetLatestStringSerializedObjectByIdOp<Version>(
                                                 referenceObject.Id,
                                                 referenceObject.ObjectType,
                                                 VersionMatchStrategy.Unknown,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 referenceObject.DeprecatedIdTypes);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "versionMatchStrategy", "Unknown", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestStringSerializedObjectByIdOp<Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordNotFoundStrategy' is RecordNotFoundStrategy.Unknown",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestStringSerializedObjectByIdOp<Version>>();

                            var result = new GetLatestStringSerializedObjectByIdOp<Version>(
                                                 referenceObject.Id,
                                                 referenceObject.ObjectType,
                                                 referenceObject.VersionMatchStrategy,
                                                 RecordNotFoundStrategy.Unknown,
                                                 referenceObject.DeprecatedIdTypes);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "recordNotFoundStrategy", "Unknown", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestStringSerializedObjectByIdOp<Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'deprecatedIdTypes' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestStringSerializedObjectByIdOp<Version>>();

                            var result = new GetLatestStringSerializedObjectByIdOp<Version>(
                                                 referenceObject.Id,
                                                 referenceObject.ObjectType,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 new TypeRepresentation[0].Concat(referenceObject.DeprecatedIdTypes).Concat(new TypeRepresentation[] { null }).Concat(referenceObject.DeprecatedIdTypes).ToList());

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "deprecatedIdTypes", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestStringSerializedObjectByIdOp<Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'typeSelectionStrategy' is TypeSelectionStrategy.Unknown",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestStringSerializedObjectByIdOp<Version>>();

                            var result = new GetLatestStringSerializedObjectByIdOp<Version>(
                                referenceObject.Id,
                                referenceObject.ObjectType,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.RecordNotFoundStrategy,
                                referenceObject.DeprecatedIdTypes,
                                TypeSelectionStrategy.Unknown);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "typeSelectionStrategy", "Unknown", },
                    });
        }
    }
}