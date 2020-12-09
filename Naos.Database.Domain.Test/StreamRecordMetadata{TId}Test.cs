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
    using System.Linq;

    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class StreamRecordMetadataTIdTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StreamRecordMetadataTIdTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'timestampUtc' is not UTC scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordMetadata<Version>>();

                                                   var result = new StreamRecordMetadata<Version>(
                                                       referenceObject.Id,
                                                       referenceObject.SerializerRepresentation,
                                                       referenceObject.TypeRepresentationOfId,
                                                       referenceObject.TypeRepresentationOfObject,
                                                       referenceObject.Tags,
                                                       new DateTime(
                                                           referenceObject.TimestampUtc.Year,
                                                           referenceObject.TimestampUtc.Month,
                                                           referenceObject.TimestampUtc.Day,
                                                           referenceObject.TimestampUtc.Hour,
                                                           referenceObject.TimestampUtc.Minute,
                                                           referenceObject.TimestampUtc.Second,
                                                           DateTimeKind.Unspecified),
                                                       referenceObject.ObjectTimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "timestampUtc",
                                                                   "UTC",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'objectTimestampUtc' is not UTC scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordMetadata<Version>>();

                                                   var result = new StreamRecordMetadata<Version>(
                                                       referenceObject.Id,
                                                       referenceObject.SerializerRepresentation,
                                                       referenceObject.TypeRepresentationOfId,
                                                       referenceObject.TypeRepresentationOfObject,
                                                       referenceObject.Tags,
                                                       referenceObject.TimestampUtc,
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
                                                                   "UTC",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'serializerRepresentation' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordMetadata<Version>>();

                                                   var result = new StreamRecordMetadata<Version>(
                                                       referenceObject.Id,
                                                       null,
                                                       referenceObject.TypeRepresentationOfId,
                                                       referenceObject.TypeRepresentationOfObject,
                                                       referenceObject.Tags,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.ObjectTimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "serializerRepresentation",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'typeRepresentationOfId' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordMetadata<Version>>();

                                                   var result = new StreamRecordMetadata<Version>(
                                                       referenceObject.Id,
                                                       referenceObject.SerializerRepresentation,
                                                       null,
                                                       referenceObject.TypeRepresentationOfObject,
                                                       referenceObject.Tags,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.ObjectTimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "typeRepresentationOfId",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StreamRecordMetadata<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'typeRepresentationOfObject' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StreamRecordMetadata<Version>>();

                                                   var result = new StreamRecordMetadata<Version>(
                                                       referenceObject.Id,
                                                       referenceObject.SerializerRepresentation,
                                                       referenceObject.TypeRepresentationOfId,
                                                       null,
                                                       referenceObject.Tags,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.ObjectTimestampUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "typeRepresentationOfObject",
                                                               },
                        });
        }
    }
}