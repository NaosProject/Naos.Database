// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordMetadata{TId}Test.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;
    using Xunit;

    [SuppressMessage(
        "Microsoft.Maintainability",
        "CA1505:AvoidUnmaintainableCode",
        Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class StreamRecordMetadataTest
    {
        [SuppressMessage(
            "Microsoft.Maintainability",
            "CA1505:AvoidUnmaintainableCode",
            Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StreamRecordMetadataTest()
        {
            ConstructorArgumentValidationTestScenarios.RemoveAllScenarios();
            ConstructorArgumentValidationTestScenarios
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata>
                        {
                            Name =
                                "constructor should throw ArgumentException when parameter 'timesteampUtc' is not a UTC datetime scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject =
                                                       A.Dummy<StreamRecordMetadata>();

                                                   var result = new StreamRecordMetadata(
                                                       referenceObject.Id,
                                                       referenceObject.Tags,
                                                       referenceObject.TypeRepresentationWithVersion,
                                                       referenceObject.TypeRepresentationWithoutVersion,
                                                       new DateTime(referenceObject.TimestampUtc.Year, referenceObject.TimestampUtc.Month, referenceObject.TimestampUtc.Day, referenceObject.TimestampUtc.Hour, referenceObject.TimestampUtc.Minute, referenceObject.TimestampUtc.Second, referenceObject.TimestampUtc.Millisecond, new GregorianCalendar(), DateTimeKind.Unspecified));

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "UTC",
                                                                   "Unspecified",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'id' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject =
                                                       A.Dummy<StreamRecordMetadata>();

                                                   var result = new StreamRecordMetadata(
                                                       null,
                                                       referenceObject.Tags,
                                                       referenceObject.TypeRepresentationWithVersion,
                                                       referenceObject.TypeRepresentationWithoutVersion,
                                                       referenceObject.TimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "id",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'tags' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject =
                                                       A.Dummy<StreamRecordMetadata>();

                                                   var result = new StreamRecordMetadata(
                                                       referenceObject.Id,
                                                       null,
                                                       referenceObject.TypeRepresentationWithVersion,
                                                       referenceObject.TypeRepresentationWithoutVersion,
                                                       referenceObject.TimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "tags",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'typeRepresentationWithVersion' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject =
                                                       A.Dummy<StreamRecordMetadata>();

                                                   var result = new StreamRecordMetadata(
                                                       referenceObject.Id,
                                                       referenceObject.Tags,
                                                       null,
                                                       referenceObject.TypeRepresentationWithoutVersion,
                                                       referenceObject.TimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "typeRepresentationWithVersion",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'typeRepresentationWithoutVersion' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject =
                                                       A.Dummy<StreamRecordMetadata>();

                                                   var result = new StreamRecordMetadata(
                                                       referenceObject.Id,
                                                       referenceObject.Tags,
                                                       referenceObject.TypeRepresentationWithVersion,
                                                       null,
                                                       referenceObject.TimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "typeRepresentationWithoutVersion",
                                                               },
                        });
        }
    }
}