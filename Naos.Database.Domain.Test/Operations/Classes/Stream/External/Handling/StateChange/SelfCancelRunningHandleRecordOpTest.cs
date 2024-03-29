﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelfCancelRunningHandleRecordOpTest.cs" company="Naos Project">
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
    public static partial class SelfCancelRunningHandleRecordOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static SelfCancelRunningHandleRecordOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SelfCancelRunningHandleRecordOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SelfCancelRunningHandleRecordOp>();

                            var result = new SelfCancelRunningHandleRecordOp(
                                                 referenceObject.InternalRecordId,
                                                 null,
                                                 referenceObject.Details,
                                                 referenceObject.Tags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "concern", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SelfCancelRunningHandleRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SelfCancelRunningHandleRecordOp>();

                            var result = new SelfCancelRunningHandleRecordOp(
                                                 referenceObject.InternalRecordId,
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.Details,
                                                 referenceObject.Tags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SelfCancelRunningHandleRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is reserved",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SelfCancelRunningHandleRecordOp>();

                            var result = new SelfCancelRunningHandleRecordOp(
                                referenceObject.InternalRecordId,
                                Concerns.StreamHandlingDisabledConcern,
                                referenceObject.Details,
                                referenceObject.Tags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "is reserved for internal use and may not be used", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SelfCancelRunningHandleRecordOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'details' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SelfCancelRunningHandleRecordOp>();

                            var result = new SelfCancelRunningHandleRecordOp(
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.Concern,
                                                 null,
                                                 referenceObject.Tags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "details", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SelfCancelRunningHandleRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'details' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SelfCancelRunningHandleRecordOp>();

                            var result = new SelfCancelRunningHandleRecordOp(
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.Concern,
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.Tags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "details", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SelfCancelRunningHandleRecordOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SelfCancelRunningHandleRecordOp>();

                            var result = new SelfCancelRunningHandleRecordOp(
                                referenceObject.InternalRecordId,
                                referenceObject.Concern,
                                referenceObject.Details,
                                new[] { A.Dummy<NamedValue<string>>(), null, A.Dummy<NamedValue<string>>() });

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tags", "contains at least one null element", },
                    });
        }
    }
}