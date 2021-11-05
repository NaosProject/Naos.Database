// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordHandlingAvailableEventTest.cs" company="Naos Project">
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
    public static partial class RecordHandlingAvailableEventTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static RecordHandlingAvailableEventTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<RecordHandlingAvailableEvent>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<RecordHandlingAvailableEvent>();

                            var result = new RecordHandlingAvailableEvent(
                                                 referenceObject.InternalRecordId,
                                                 null,
                                                 referenceObject.RecordToHandle,
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.Details);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "concern", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<RecordHandlingAvailableEvent>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<RecordHandlingAvailableEvent>();

                            var result = new RecordHandlingAvailableEvent(
                                                 referenceObject.InternalRecordId,
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.RecordToHandle,
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.Details);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<RecordHandlingAvailableEvent>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'recordToHandle' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<RecordHandlingAvailableEvent>();

                            var result = new RecordHandlingAvailableEvent(
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.Concern,
                                                 null,
                                                 referenceObject.TimestampUtc,
                                                 referenceObject.Details);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "recordToHandle", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<RecordHandlingAvailableEvent>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'internalRecordId' does not equal to 'recordToHandle.InternalRecordId' scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<RecordHandlingAvailableEvent>();

                            var result = new RecordHandlingAvailableEvent(
                                A.Dummy<long>().ThatIsNot(referenceObject.RecordToHandle.InternalRecordId),
                                referenceObject.Concern,
                                referenceObject.RecordToHandle,
                                referenceObject.TimestampUtc,
                                referenceObject.Details);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "internalRecordId and recordToHandle.InternalRecordId must match", },
                    });
        }
    }
}