// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordHandlingEntryTest.cs" company="Naos Project">
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
    public static partial class StreamRecordHandlingEntryTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StreamRecordHandlingEntryTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordHandlingEntry>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordHandlingEntry>();

                                                   var result = new StreamRecordHandlingEntry(
                                                       referenceObject.InternalHandlingEntryId,
                                                       referenceObject.InternalRecordId,
                                                       null,
                                                       referenceObject.Status,
                                                       referenceObject.Tags,
                                                       referenceObject.Details,
                                                       referenceObject.TimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "concern",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordHandlingEntry>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordHandlingEntry>();

                                                   var result = new StreamRecordHandlingEntry(
                                                       referenceObject.InternalHandlingEntryId,
                                                       referenceObject.InternalRecordId,
                                                       Invariant($"  {Environment.NewLine}  "),
                                                       referenceObject.Status,
                                                       referenceObject.Tags,
                                                       referenceObject.Details,
                                                       referenceObject.TimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "concern",
                                                                   "white space",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordHandlingEntry>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordHandlingEntry>();

                                                   var result = new StreamRecordHandlingEntry(
                                                       referenceObject.InternalHandlingEntryId,
                                                       referenceObject.InternalRecordId,
                                                       referenceObject.Concern,
                                                       referenceObject.Status,
                                                       new NamedValue<string>[0].Concat(referenceObject.Tags)
                                                                                .Concat(
                                                                                     new NamedValue<string>[]
                                                                                     {
                                                                                         null,
                                                                                     })
                                                                                .Concat(referenceObject.Tags)
                                                                                .ToList(),
                                                       referenceObject.Details,
                                                       referenceObject.TimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "tags",
                                                                   "contains at least one null element",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordHandlingEntry>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'timestampUtc' is not UTC",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordHandlingEntry>();

                                                   var result = new StreamRecordHandlingEntry(
                                                       referenceObject.InternalHandlingEntryId,
                                                       referenceObject.InternalRecordId,
                                                       referenceObject.Concern,
                                                       referenceObject.Status,
                                                       referenceObject.Tags,
                                                       referenceObject.Details,
                                                       new DateTime(
                                                           referenceObject.TimestampUtc.Year,
                                                           referenceObject.TimestampUtc.Month,
                                                           referenceObject.TimestampUtc.Day,
                                                           referenceObject.TimestampUtc.Hour,
                                                           referenceObject.TimestampUtc.Minute,
                                                           referenceObject.TimestampUtc.Second,
                                                           DateTimeKind.Unspecified));

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "timestampUtc",
                                                                   "must be in UTC format",
                                                               },
                        });
        }
    }
}