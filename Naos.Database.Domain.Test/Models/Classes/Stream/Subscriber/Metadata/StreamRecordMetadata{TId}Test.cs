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
    using OBeautifulCode.Type;
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
                .AddScenario(() =>
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
                        ExpectedExceptionMessageContains = new[] { "serializerRepresentation", },
                    })
                .AddScenario(() =>
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
                        ExpectedExceptionMessageContains = new[] { "typeRepresentationOfId", },
                    })
                .AddScenario(() =>
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
                        ExpectedExceptionMessageContains = new[] { "typeRepresentationOfObject", },
                    })
               .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StreamRecordMetadata<Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StreamRecordMetadata<Version>>();

                            var result = new StreamRecordMetadata<Version>(
                                                 referenceObject.Id,
                                                 referenceObject.SerializerRepresentation,
                                                 referenceObject.TypeRepresentationOfId,
                                                 referenceObject.TypeRepresentationOfObject,
                                                 new NamedValue<string>[0].Concat(referenceObject.Tags).Concat(new NamedValue<string>[] { null }).Concat(referenceObject.Tags).ToList(),
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.ObjectTimestampUtc);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tags", "contains at least one null element", },
                    })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<StreamRecordMetadata<Version>>
                   {
                       Name = "constructor should throw ArgumentException when parameter 'timestampUtc' is not in UTC scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<StreamRecordMetadata<Version>>();

                           var result = new StreamRecordMetadata<Version>(
                               referenceObject.Id,
                               referenceObject.SerializerRepresentation,
                               referenceObject.TypeRepresentationOfId,
                               referenceObject.TypeRepresentationOfObject,
                               referenceObject.Tags,
                               A.Dummy<DateTime>().ThatIs(_ => _.Kind != DateTimeKind.Utc),
                               referenceObject.ObjectTimestampUtc);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentException),
                       ExpectedExceptionMessageContains = new[] { "timestampUtc must be in UTC format", },
                   })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<StreamRecordMetadata<Version>>
                   {
                       Name = "constructor should throw ArgumentException when parameter 'timestampUtc' is not in UTC scenario",
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
                               A.Dummy<DateTime>().ThatIs(_ => _.Kind != DateTimeKind.Utc));

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentException),
                       ExpectedExceptionMessageContains = new[] { "objectTimestampUtc must be in UTC format", },
                   });
        }
    }
}