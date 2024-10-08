﻿// --------------------------------------------------------------------------------------------------------------------
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
    using OBeautifulCode.Equality.Recipes;
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

            EquatableTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new EquatableTestScenario<RecordHandlingAvailableEvent>
                    {
                        Name = "Default Code Generated Scenario",
                        ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                        ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new RecordHandlingAvailableEvent[]
                        {
                            new RecordHandlingAvailableEvent(
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.Concern,
                                    ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                                    ReferenceObjectForEquatableTestScenarios.TimestampUtc,
                                    ReferenceObjectForEquatableTestScenarios.Details),
                        },
                        ObjectsThatAreNotEqualToReferenceObject = new RecordHandlingAvailableEvent[]
                        {
                            new RecordHandlingAvailableEvent(
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.Concern,
                                    ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                                    A.Dummy<RecordHandlingAvailableEvent>().Whose(_ => !_.TimestampUtc.IsEqualTo(ReferenceObjectForEquatableTestScenarios.TimestampUtc)).TimestampUtc,
                                    ReferenceObjectForEquatableTestScenarios.Details),
                            new RecordHandlingAvailableEvent(
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.Concern,
                                    ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                                    ReferenceObjectForEquatableTestScenarios.TimestampUtc,
                                    A.Dummy<RecordHandlingAvailableEvent>().Whose(_ => !_.Details.IsEqualTo(ReferenceObjectForEquatableTestScenarios.Details)).Details),
                            new RecordHandlingAvailableEvent(
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    A.Dummy<RecordHandlingAvailableEvent>().Whose(_ => !_.Concern.IsEqualTo(ReferenceObjectForEquatableTestScenarios.Concern)).Concern,
                                    ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                                    ReferenceObjectForEquatableTestScenarios.TimestampUtc,
                                    ReferenceObjectForEquatableTestScenarios.Details),
                            new RecordHandlingAvailableEvent(
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.Concern,
                                    A.Dummy<RecordHandlingAvailableEvent>().Whose(_ => !_.RecordToHandle.IsEqualTo(ReferenceObjectForEquatableTestScenarios.RecordToHandle)).RecordToHandle.DeepCloneWithInternalRecordId(ReferenceObjectForEquatableTestScenarios.InternalRecordId),
                                    ReferenceObjectForEquatableTestScenarios.TimestampUtc,
                                    ReferenceObjectForEquatableTestScenarios.Details),
                        },
                        ObjectsThatAreNotOfTheSameTypeAsReferenceObject = new object[]
                        {
                            A.Dummy<object>(),
                            A.Dummy<string>(),
                            A.Dummy<int>(),
                            A.Dummy<int?>(),
                            A.Dummy<Guid>(),
                            A.Dummy<HandlingForRecordDisabledEvent>(),
                            A.Dummy<HandlingForStreamDisabledEvent>(),
                            A.Dummy<HandlingForStreamEnabledEvent>(),
                            A.Dummy<IdDeprecatedEvent>(),
                            A.Dummy<PruneOperationExecutedEvent>(),
                            A.Dummy<PruneOperationRequestedEvent>(),
                            A.Dummy<PruneRequestCanceledEvent>(),
                            A.Dummy<RecordHandlingCanceledEvent>(),
                            A.Dummy<RecordHandlingCompletedEvent>(),
                            A.Dummy<RecordHandlingFailedEvent>(),
                            A.Dummy<RecordHandlingFailureResetEvent>(),
                            A.Dummy<RecordHandlingRunningEvent>(),
                            A.Dummy<RecordHandlingSelfCanceledEvent>(),
                            A.Dummy<UniqueLongIssuedEvent>(),
                        },
                    });
        }
    }
}