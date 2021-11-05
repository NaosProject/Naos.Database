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
                        Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                                 referenceObject.InternalRecordId,
                                                 null,
                                                 referenceObject.NewStatus,
                                                 referenceObject.AcceptableCurrentStatuses,
                                                 referenceObject.Details,
                                                 referenceObject.Tags,
                                                 referenceObject.InheritRecordTags,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "concern", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardUpdateHandlingStatusForRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                                 referenceObject.InternalRecordId,
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.NewStatus,
                                                 referenceObject.AcceptableCurrentStatuses,
                                                 referenceObject.Details,
                                                 referenceObject.Tags,
                                                 referenceObject.InheritRecordTags,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardUpdateHandlingStatusForRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is reserved",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                referenceObject.InternalRecordId,
                                Concerns.RecordHandlingConcern,
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
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'newStatus' is HandlingStatus.Unknown",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                referenceObject.InternalRecordId,
                                referenceObject.Concern,
                                HandlingStatus.Unknown,
                                referenceObject.AcceptableCurrentStatuses,
                                referenceObject.Details,
                                referenceObject.Tags,
                                referenceObject.InheritRecordTags,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "newStatus", "Unknown", },
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
                        Name = "constructor should throw ArgumentNullException when parameter 'acceptableCurrentStatuses' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.Concern,
                                                 referenceObject.NewStatus,
                                                 null,
                                                 referenceObject.Details,
                                                 referenceObject.Tags,
                                                 referenceObject.InheritRecordTags,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "acceptableCurrentStatuses", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardUpdateHandlingStatusForRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'acceptableCurrentStatuses' is an empty enumerable scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardUpdateHandlingStatusForRecordOp>();

                            var result = new StandardUpdateHandlingStatusForRecordOp(
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.Concern,
                                                 referenceObject.NewStatus,
                                                 new List<HandlingStatus>(),
                                                 referenceObject.Details,
                                                 referenceObject.Tags,
                                                 referenceObject.InheritRecordTags,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "acceptableCurrentStatuses", "is an empty enumerable", },
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
                        ExpectedExceptionMessageContains = new[] { "acceptableCurrentStatuses", "is an empty enumerable", },
                    });
        }
    }
}