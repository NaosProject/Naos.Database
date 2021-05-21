// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutOp{TObject}Test.cs" company="Naos Project">
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
    public static partial class PutOpTObjectTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static PutOpTObjectTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(ConstructorArgumentValidationTestScenario<PutOp<Version>>.ForceGeneratedTestsToPassAndWriteMyOwnScenario);

            EquatableTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                    {
                        var referenceObjectIsPruning =
                            ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy
                         == ExistingRecordEncounteredStrategy.PruneIfFoundById
                         || ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy
                         == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType;

                        var unequalObjectToPut = new PutOp<Version>(
                            A.Dummy<PutOp<Version>>()
                             .Whose(
                                  _ => !_.ObjectToPut.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios.ObjectToPut))
                             .ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios.TypeVersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator);

                        var unequalTags = new PutOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            A.Dummy<PutOp<Version>>()
                             .Whose(
                                  _ => !_.Tags.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios.Tags))
                             .Tags,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios
                               .TypeVersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator);

                        var unequalExistingRecordStrategy = new PutOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            referenceObjectIsPruning
                                ? A.Dummy<ExistingRecordEncounteredStrategy>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy && (_ == ExistingRecordEncounteredStrategy.PruneIfFoundById || _ == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType))
                                : ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById,
                            referenceObjectIsPruning
                                ? A.Dummy<int>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)
                                : (int?)null,
                            ReferenceObjectForEquatableTestScenarios
                               .TypeVersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator);

                        var unequalRecordRetentionCount = new PutOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            referenceObjectIsPruning
                                ? A.Dummy<ExistingRecordEncounteredStrategy>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy && (_ == ExistingRecordEncounteredStrategy.PruneIfFoundById || _ == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType))
                                : ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById,
                            referenceObjectIsPruning
                                ? A.Dummy<int>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)
                                : (int?)null,
                            ReferenceObjectForEquatableTestScenarios
                               .TypeVersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator);

                        var unequalTypeMatchStrategy = new PutOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            A.Dummy<PutOp<Version>>()
                             .Whose(
                                  _ => !_.TypeVersionMatchStrategy.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .TypeVersionMatchStrategy))
                             .TypeVersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator);

                        var unequalLocator = new PutOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios
                               .TypeVersionMatchStrategy,
                            A.Dummy<PutOp<Version>>()
                             .Whose(
                                  _ => !_.SpecifiedResourceLocator.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .SpecifiedResourceLocator))
                             .SpecifiedResourceLocator);

                        return new EquatableTestScenario<PutOp<Version>>
                               {
                                   Name = "Default Code Generated Scenario",
                                   ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                                   ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new PutOp<Version>[]
                                                                                         {
                                                                                             new PutOp<Version>(
                                                                                                 ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                                                                                 ReferenceObjectForEquatableTestScenarios.Tags,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .ExistingRecordEncounteredStrategy,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .RecordRetentionCount,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .TypeVersionMatchStrategy,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .SpecifiedResourceLocator),
                                                                                         },
                                   ObjectsThatAreNotEqualToReferenceObject = new PutOp<Version>[]
                                                                             {
                                                                                 unequalObjectToPut,
                                                                                 unequalTags,
                                                                                 unequalExistingRecordStrategy,
                                                                                 unequalRecordRetentionCount,
                                                                                 unequalTypeMatchStrategy,
                                                                                 unequalLocator,
                                                                             },
                                   ObjectsThatAreNotOfTheSameTypeAsReferenceObject = new object[]
                                                                                     {
                                                                                         A.Dummy<object>(),
                                                                                         A.Dummy<string>(),
                                                                                         A.Dummy<int>(),
                                                                                         A.Dummy<int?>(),
                                                                                         A.Dummy<Guid>(),
                                                                                         A.Dummy<BlockRecordHandlingOp>(),
                                                                                         A.Dummy<CancelBlockedRecordHandlingOp>(),
                                                                                         A.Dummy<CancelHandleRecordExecutionRequestOp>(),
                                                                                         A.Dummy<CancelRunningHandleRecordExecutionOp>(),
                                                                                         A.Dummy<CompleteRunningHandleRecordExecutionOp>(),
                                                                                         A.Dummy<RetryFailedHandleRecordExecutionOp>(),
                                                                                         A.Dummy<FailRunningHandleRecordExecutionOp>(),
                                                                                         A.Dummy<SelfCancelRunningHandleRecordExecutionOp>(),
                                                                                         A.Dummy<GetHandlingHistoryOfRecordOp>(),
                                                                                         A.Dummy<GetHandlingStatusOfRecordsByIdOp>(),
                                                                                         A.Dummy<GetHandlingStatusOfRecordsByIdOp<Version>>(),
                                                                                         A.Dummy<GetHandlingStatusOfRecordSetByTagOp>(),
                                                                                         A.Dummy<HandleRecordOp>(),
                                                                                         A.Dummy<HandleRecordOp<Version>>(),
                                                                                         A.Dummy<HandleRecordWithIdOp<Version, Version>>(),
                                                                                         A.Dummy<HandleRecordWithIdOp<Version>>(),
                                                                                         A.Dummy<TryHandleRecordOp>(),
                                                                                         A.Dummy<TryHandleRecordOp<Version>>(),
                                                                                         A.Dummy<TryHandleRecordWithIdOp<Version, Version>>(),
                                                                                         A.Dummy<TryHandleRecordWithIdOp<Version>>(),
                                                                                         A.Dummy<PruneBeforeInternalRecordDateOp>(),
                                                                                         A.Dummy<PruneBeforeInternalRecordIdOp>(),
                                                                                         A.Dummy<DoesAnyExistByIdOp>(),
                                                                                         A.Dummy<DoesAnyExistByIdOp<Version>>(),
                                                                                         A.Dummy<GetAllRecordsByIdOp>(),
                                                                                         A.Dummy<GetAllRecordsByIdOp<Version>>(),
                                                                                         A.Dummy<GetAllRecordsMetadataByIdOp>(),
                                                                                         A.Dummy<GetAllRecordsMetadataByIdOp<Version>>(),
                                                                                         A.Dummy<GetLatestObjectByIdOp<Version, Version>>(),
                                                                                         A.Dummy<GetLatestObjectOp<Version>>(),
                                                                                         A.Dummy<GetLatestRecordByIdOp<Version, Version>>(),
                                                                                         A.Dummy<GetLatestRecordByIdOp<Version>>(),
                                                                                         A.Dummy<GetLatestRecordMetadataByIdOp>(),
                                                                                         A.Dummy<GetLatestRecordByIdOp>(),
                                                                                         A.Dummy<GetLatestRecordMetadataByIdOp<Version>>(),
                                                                                         A.Dummy<GetDistinctStringSerializedIdsOp>(),
                                                                                         A.Dummy<GetLatestRecordOp>(),
                                                                                         A.Dummy<GetRecordByInternalRecordIdOp>(),
                                                                                         A.Dummy<GetLatestRecordOp<Version>>(),
                                                                                         A.Dummy<PutAndReturnInternalRecordIdOp<Version>>(),
                                                                                         A.Dummy<PutRecordOp>(),
                                                                                         A.Dummy<CreateStreamOp>(),
                                                                                         A.Dummy<DeleteStreamOp>(),
                                                                                         A.Dummy<GetNextUniqueLongOp>(),
                                                                                         A.Dummy<GetStreamFromRepresentationOp<
                                                                                             FileStreamRepresentation,
                                                                                             MemoryReadWriteStream>>(),
                                                                                         A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version
                                                                                         >>(),
                                                                                         A.Dummy<PutWithIdOp<Version, Version>>(),
                                                                                     },
                               };
                    });
        }
    }
}