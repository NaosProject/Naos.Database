﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdDeprecatedEvent{TId,TObject}Test.cs" company="Naos Project">
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
    using OBeautifulCode.DateTime.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class IdDeprecatedEventTIdTObjectTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static IdDeprecatedEventTIdTObjectTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<IdDeprecatedEvent<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'timestampUtc' is not a UTC DateTime (it's Local)",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<IdDeprecatedEvent<Version, Version>>();

                            var result = new IdDeprecatedEvent<Version, Version>(
                                                 referenceObject.Id,
                                                 DateTime.Now,
                                                 referenceObject.Details);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "timestampUtc", "Kind that is not DateTimeKind.Utc", "DateTimeKind.Local" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<IdDeprecatedEvent<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'timestampUtc' is not a UTC DateTime (it's Unspecified)",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<IdDeprecatedEvent<Version, Version>>();

                            var result = new IdDeprecatedEvent<Version, Version>(
                                                 referenceObject.Id,
                                                 DateTime.UtcNow.ToUnspecified(),
                                                 referenceObject.Details);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "timestampUtc", "Kind that is not DateTimeKind.Utc", "DateTimeKind.Unspecified" },
                    });
        }
    }
}