// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestedHandleRecordExecutionEventTest.cs" company="Naos Project">
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
    public static partial class RequestedHandleRecordExecutionEventTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static RequestedHandleRecordExecutionEventTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(() =>
                                new ConstructorArgumentValidationTestScenario<RequestedHandleRecordExecutionEvent>
                                {
                                    Name = "constructor should throw ArgumentNullException when parameter 'recordToHandle' is null scenario",
                                    ConstructionFunc = () =>
                                                       {
                                                           var referenceObject = A.Dummy<RequestedHandleRecordExecutionEvent>();

                                                           var result = new RequestedHandleRecordExecutionEvent(
                                                               referenceObject.Id,
                                                               referenceObject.TimestampUtc,
                                                               null,
                                                               referenceObject.Details);

                                                           return result;
                                                       },
                                    ExpectedExceptionType = typeof(ArgumentNullException),
                                    ExpectedExceptionMessageContains = new[] { "recordToHandle", },
                                })
               .AddScenario(() =>
                                new ConstructorArgumentValidationTestScenario<RequestedHandleRecordExecutionEvent>
                                {
                                    Name = "constructor should throw ArgumentException when parameter 'recordToHandle' has a different id than 'id' scenario",
                                    ConstructionFunc = () =>
                                                       {
                                                           var referenceObject = A.Dummy<RequestedHandleRecordExecutionEvent>();

                                                           var result = new RequestedHandleRecordExecutionEvent(
                                                               A.Dummy<long>().ThatIsNot(referenceObject.Id),
                                                               referenceObject.TimestampUtc,
                                                               referenceObject.RecordToHandle,
                                                               referenceObject.Details);

                                                           return result;
                                                       },
                                    ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                                    ExpectedExceptionMessageContains = new[] { "id", },
                                });

            EquatableTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                    {
                        // need the ids to match...
                        var dummyRecord = A.Dummy<StreamRecord>();

                        return new EquatableTestScenario<RequestedHandleRecordExecutionEvent>
                               {
                                   Name = "Default Code Generated Scenario",
                                   ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                                   ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new RequestedHandleRecordExecutionEvent[]
                                                                                         {
                                                                                             new RequestedHandleRecordExecutionEvent(
                                                                                                 ReferenceObjectForEquatableTestScenarios.Id,
                                                                                                 ReferenceObjectForEquatableTestScenarios.TimestampUtc,
                                                                                                 ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                                                                                                 ReferenceObjectForEquatableTestScenarios.Details),
                                                                                         },
                                   ObjectsThatAreNotEqualToReferenceObject = new RequestedHandleRecordExecutionEvent[]
                                                                             {
                                                                                 new RequestedHandleRecordExecutionEvent(
                                                                                     ReferenceObjectForEquatableTestScenarios.Id,
                                                                                     A.Dummy<RequestedHandleRecordExecutionEvent>()
                                                                                      .Whose(
                                                                                           _ => !_.TimestampUtc.IsEqualTo(
                                                                                               ReferenceObjectForEquatableTestScenarios
                                                                                                  .TimestampUtc))
                                                                                      .TimestampUtc,
                                                                                     ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                                                                                     ReferenceObjectForEquatableTestScenarios.Details),
                                                                                 new RequestedHandleRecordExecutionEvent(
                                                                                     dummyRecord.InternalRecordId,
                                                                                     ReferenceObjectForEquatableTestScenarios.TimestampUtc,
                                                                                     dummyRecord,
                                                                                     ReferenceObjectForEquatableTestScenarios.Details),
                                                                                 new RequestedHandleRecordExecutionEvent(
                                                                                     ReferenceObjectForEquatableTestScenarios.Id,
                                                                                     ReferenceObjectForEquatableTestScenarios.TimestampUtc,
                                                                                     ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                                                                                     A.Dummy<RequestedHandleRecordExecutionEvent>()
                                                                                      .Whose(
                                                                                           _ => !_.Details.IsEqualTo(
                                                                                               ReferenceObjectForEquatableTestScenarios
                                                                                                  .Details))
                                                                                      .Details),
                                                                             },
                                   ObjectsThatAreNotOfTheSameTypeAsReferenceObject = new object[]
                                                                                     {
                                                                                         A.Dummy<object>(),
                                                                                         A.Dummy<string>(),
                                                                                         A.Dummy<int>(),
                                                                                         A.Dummy<int?>(),
                                                                                         A.Dummy<Guid>(),
                                                                                         A.Dummy<CanceledRequestedHandleRecordExecutionEvent>(),
                                                                                         A.Dummy<CanceledRunningHandleRecordExecutionEvent>(),
                                                                                         A.Dummy<CompletedHandleRecordExecutionEvent>(),
                                                                                         A.Dummy<FailedHandleRecordExecutionEvent>(),
                                                                                         A.Dummy<RetryFailedHandleRecordExecutionEvent>(),
                                                                                         A.Dummy<RunningHandleRecordExecutionEvent>(),
                                                                                         A.Dummy<SelfCanceledRunningHandleRecordExecutionEvent>(),
                                                                                         A.Dummy<BlockedRecordHandlingEvent>(),
                                                                                         A.Dummy<CanceledBlockedRecordHandlingEvent>(),
                                                                                         A.Dummy<CanceledPruneRequestedEvent>(),
                                                                                         A.Dummy<PruneOperationExecutedEvent>(),
                                                                                         A.Dummy<PruneOperationRequestedEvent>(),
                                                                                         A.Dummy<UniqueLongIssuedEvent>(),
                                                                                     },
                               };
                    });
        }
    }
}