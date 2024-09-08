// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardUpdateHandlingStatusForRecordOpTest.cs" company="Naos Project">
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
    public static partial class StandardUpdateHandlingStatusForRecordOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardUpdateHandlingStatusForRecordOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardUpdateHandlingStatusForRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is reserved",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                referenceObject.InternalRecordId,
                                Concerns.StreamHandlingDisabledConcern,
                                referenceObject.NewStatus,
                                referenceObject.AcceptableCurrentStatuses,
                                referenceObject.Details,
                                referenceObject.Tags,
                                referenceObject.InheritRecordTags,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "is reserved for internal use and may not be used", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardUpdateHandlingStatusForRecordOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'newStatus' is HandlingStatus.DisabledForStream",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                referenceObject.InternalRecordId,
                                referenceObject.Concern,
                                HandlingStatus.DisabledForStream,
                                referenceObject.AcceptableCurrentStatuses,
                                referenceObject.Details,
                                referenceObject.Tags,
                                referenceObject.InheritRecordTags,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "newStatus", "DisabledForStream", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardUpdateHandlingStatusForRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'acceptableCurrentStatuses' contains HandlingStatus.Unknown",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                referenceObject.InternalRecordId,
                                referenceObject.Concern,
                                referenceObject.NewStatus,
                                new HandlingStatus[] { A.Dummy<HandlingStatus>(), HandlingStatus.Unknown, A.Dummy<HandlingStatus>() },
                                referenceObject.Details,
                                referenceObject.Tags,
                                referenceObject.InheritRecordTags,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "acceptableCurrentStatuses", "contains an element that is equal to", "Unknown" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardUpdateHandlingStatusForRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                referenceObject.InternalRecordId,
                                referenceObject.Concern,
                                referenceObject.NewStatus,
                                referenceObject.AcceptableCurrentStatuses,
                                referenceObject.Details,
                                new[] { A.Dummy<NamedValue<string>>(), null, A.Dummy<NamedValue<string>>() },
                                referenceObject.InheritRecordTags,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tags", "contains at least one null element" },
                    });
        }
    }
}