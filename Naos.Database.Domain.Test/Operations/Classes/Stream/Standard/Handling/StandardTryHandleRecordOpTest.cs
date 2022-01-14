// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardTryHandleRecordOpTest.cs" company="Naos Project">
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
    public static partial class StandardTryHandleRecordOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardTryHandleRecordOpTest()
        {
            /*
            orderRecordsBy.MustForArg(nameof(orderRecordsBy)).NotBeEqualTo(OrderRecordsBy.Unknown);
            streamRecordItemsToInclude.MustForArg(nameof(streamRecordItemsToInclude)).NotBeEqualTo(StreamRecordItemsToInclude.Unknown);
             */
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StandardTryHandleRecordOp>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StandardTryHandleRecordOp>();

                                                   var result = new StandardTryHandleRecordOp(
                                                       null,
                                                       referenceObject.RecordFilter,
                                                       referenceObject.OrderRecordsBy,
                                                       referenceObject.Details,
                                                       referenceObject.MinimumInternalRecordId,
                                                       referenceObject.InheritRecordTags,
                                                       referenceObject.StreamRecordItemsToInclude,
                                                       referenceObject.SpecifiedResourceLocator);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "concern",
                                                               },
                        })
               .AddScenario(() =>
                                new ConstructorArgumentValidationTestScenario<StandardTryHandleRecordOp>
                                {
                                    Name = "constructor should throw ArgumentNullException when parameter 'recordFilter' is null scenario",
                                    ConstructionFunc = () =>
                                                       {
                                                           var referenceObject = A.Dummy<StandardTryHandleRecordOp>();

                                                           var result = new StandardTryHandleRecordOp(
                                                               referenceObject.Concern,
                                                               null,
                                                               referenceObject.OrderRecordsBy,
                                                               referenceObject.Details,
                                                               referenceObject.MinimumInternalRecordId,
                                                               referenceObject.InheritRecordTags,
                                                               referenceObject.StreamRecordItemsToInclude,
                                                               referenceObject.SpecifiedResourceLocator);

                                                           return result;
                                                       },
                                    ExpectedExceptionType = typeof(ArgumentNullException),
                                    ExpectedExceptionMessageContains = new[] { "recordFilter", },
                                })
               .AddScenario(() =>
                                new ConstructorArgumentValidationTestScenario<StandardTryHandleRecordOp>
                                {
                                    Name = "constructor should throw ArgumentOutOfRangeException when parameter 'orderRecordsBy' is Unknown scenario",
                                    ConstructionFunc = () =>
                                                       {
                                                           var referenceObject = A.Dummy<StandardTryHandleRecordOp>();

                                                           var result = new StandardTryHandleRecordOp(
                                                               referenceObject.Concern,
                                                               referenceObject.RecordFilter,
                                                               OrderRecordsBy.Unknown,
                                                               referenceObject.Details,
                                                               referenceObject.MinimumInternalRecordId,
                                                               referenceObject.InheritRecordTags,
                                                               referenceObject.StreamRecordItemsToInclude,
                                                               referenceObject.SpecifiedResourceLocator);

                                                           return result;
                                                       },
                                    ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                                    ExpectedExceptionMessageContains = new[] { "orderRecordsBy", },
                                })
               .AddScenario(() =>
                                new ConstructorArgumentValidationTestScenario<StandardTryHandleRecordOp>
                                {
                                    Name = "constructor should throw ArgumentOutOfRangeException when parameter 'streamRecordItemsToInclude' is Unknown scenario",
                                    ConstructionFunc = () =>
                                                       {
                                                           var referenceObject = A.Dummy<StandardTryHandleRecordOp>();

                                                           var result = new StandardTryHandleRecordOp(
                                                               referenceObject.Concern,
                                                               referenceObject.RecordFilter,
                                                               referenceObject.OrderRecordsBy,
                                                               referenceObject.Details,
                                                               referenceObject.MinimumInternalRecordId,
                                                               referenceObject.InheritRecordTags,
                                                               StreamRecordItemsToInclude.Unknown,
                                                               referenceObject.SpecifiedResourceLocator);

                                                           return result;
                                                       },
                                    ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                                    ExpectedExceptionMessageContains = new[] { "streamRecordItemsToInclude", },
                                });
        }
    }
}