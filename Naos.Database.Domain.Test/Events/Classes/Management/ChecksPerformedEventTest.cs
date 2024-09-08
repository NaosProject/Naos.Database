// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChecksPerformedEventTest.cs" company="Naos Project">
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
    using Naos.Diagnostics.Domain;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.DateTime.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class ChecksPerformedEventTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static ChecksPerformedEventTest()
        {
            ConstructorArgumentValidationTestScenarios

                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<ChecksPerformedEvent>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'id' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<ChecksPerformedEvent>();

                            var result = new ChecksPerformedEvent(
                                                 null,
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.Status,
                                                 referenceObject.CheckDrivesReport,
                                                 referenceObject.CheckJobsReport,
                                                 referenceObject.CheckStreamsReport);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "id", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<ChecksPerformedEvent>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'id' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<ChecksPerformedEvent>();

                            var result = new ChecksPerformedEvent(
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.Status,
                                                 referenceObject.CheckDrivesReport,
                                                 referenceObject.CheckJobsReport,
                                                 referenceObject.CheckStreamsReport);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "id", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<ChecksPerformedEvent>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'checkDrivesReport' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<ChecksPerformedEvent>();

                            var result = new ChecksPerformedEvent(
                                                 referenceObject.Id,
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.Status,
                                                 null,
                                                 referenceObject.CheckJobsReport,
                                                 referenceObject.CheckStreamsReport);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "checkDrivesReport", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<ChecksPerformedEvent>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'checkJobsReport' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<ChecksPerformedEvent>();

                            var result = new ChecksPerformedEvent(
                                                 referenceObject.Id,
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.Status,
                                                 referenceObject.CheckDrivesReport,
                                                 null,
                                                 referenceObject.CheckStreamsReport);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "checkJobsReport", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<ChecksPerformedEvent>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'checkStreamsReport' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<ChecksPerformedEvent>();

                            var result = new ChecksPerformedEvent(
                                                 referenceObject.Id,
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.Status,
                                                 referenceObject.CheckDrivesReport,
                                                 referenceObject.CheckJobsReport,
                                                 null);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "checkStreamsReport", },
                    })
                .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ChecksPerformedEvent>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'timestampUtc' is not UTC scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ChecksPerformedEvent>();

                                                   var result = new ChecksPerformedEvent(
                                                       referenceObject.Id,
                                                       referenceObject.TimestampUtc.ToUnspecified(),
                                                       referenceObject.Status,
                                                       referenceObject.CheckDrivesReport,
                                                       referenceObject.CheckJobsReport,
                                                       referenceObject.CheckStreamsReport);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "timestampUtc",
                                                                   "not DateTimeKind.Utc",
                                                               },
                        })
                .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ChecksPerformedEvent>
                        {
                            Name = "constructor should throw ArgumentOutOfRangeException when parameter 'checkStatus' is not 'Invalid' scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ChecksPerformedEvent>();

                                                   var result = new ChecksPerformedEvent(
                                                       referenceObject.Id,
                                                       referenceObject.TimestampUtc,
                                                       CheckStatus.Invalid,
                                                       referenceObject.CheckDrivesReport,
                                                       referenceObject.CheckJobsReport,
                                                       referenceObject.CheckStreamsReport);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "status",
                                                               },
                        });
        }
    }
}