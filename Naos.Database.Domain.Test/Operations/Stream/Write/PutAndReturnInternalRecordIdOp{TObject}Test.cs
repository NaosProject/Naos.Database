// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutAndReturnInternalRecordIdOp{TObject}Test.cs" company="Naos Project">
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
    public static partial class PutAndReturnInternalRecordIdOpTObjectTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static PutAndReturnInternalRecordIdOpTObjectTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(ConstructorArgumentValidationTestScenario<PutAndReturnInternalRecordIdOp<Version>>.ForceGeneratedTestsToPassAndWriteMyOwnScenario);
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

                        var unequalObjectToPut = new PutAndReturnInternalRecordIdOp<Version>(
                            A.Dummy<PutAndReturnInternalRecordIdOp<Version>>()
                             .Whose(
                                  _ => !_.ObjectToPut.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .ObjectToPut))
                             .ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator);

                        var unequalTags = new PutAndReturnInternalRecordIdOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            A.Dummy<PutAndReturnInternalRecordIdOp<Version>>()
                             .Whose(
                                  _ => !_.Tags.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios.Tags))
                             .Tags,
                            ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator);

                        var unequalExistingRecordStrategy = new PutAndReturnInternalRecordIdOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            referenceObjectIsPruning
                                ? A.Dummy<ExistingRecordEncounteredStrategy>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy && (_ == ExistingRecordEncounteredStrategy.PruneIfFoundById || _ == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType))
                                : ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById,
                            referenceObjectIsPruning
                                ? A.Dummy<int>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)
                                : (int?)null,
                            ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator);

                        var unequalRetentionCount = new PutAndReturnInternalRecordIdOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            referenceObjectIsPruning
                                ? A.Dummy<ExistingRecordEncounteredStrategy>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy && (_ == ExistingRecordEncounteredStrategy.PruneIfFoundById || _ == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType))
                                : ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById,
                            referenceObjectIsPruning
                                ? A.Dummy<int>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)
                                : (int?)null,
                            ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator);

                        var unequalTypeMatchStrategy = new PutAndReturnInternalRecordIdOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            A.Dummy<PutAndReturnInternalRecordIdOp<Version>>()
                             .Whose(
                                  _ => !_.VersionMatchStrategy.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .VersionMatchStrategy))
                             .VersionMatchStrategy,
                            ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator);

                        var unequalLocator = new PutAndReturnInternalRecordIdOp<Version>(
                            ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ReferenceObjectForEquatableTestScenarios.Tags,
                            ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                            A.Dummy<PutAndReturnInternalRecordIdOp<Version>>()
                             .Whose(
                                  _ => !_.SpecifiedResourceLocator.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .SpecifiedResourceLocator))
                             .SpecifiedResourceLocator);

                        return new EquatableTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                               {
                                   Name = "Default Code Generated Scenario",
                                   ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                                   ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new PutAndReturnInternalRecordIdOp<Version>[]
                                                                                         {
                                                                                             new PutAndReturnInternalRecordIdOp<Version>(
                                                                                                 ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                                                                                 ReferenceObjectForEquatableTestScenarios.Tags,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .ExistingRecordEncounteredStrategy,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .RecordRetentionCount,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .VersionMatchStrategy,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .SpecifiedResourceLocator),
                                                                                         },
                                   ObjectsThatAreNotEqualToReferenceObject = new PutAndReturnInternalRecordIdOp<Version>[]
                                                                             {
                                                                                 unequalObjectToPut,
                                                                                 unequalTags,
                                                                                 unequalExistingRecordStrategy,
                                                                                 unequalRetentionCount,
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
                                                                                         A.Dummy<StandardDoesAnyExistByIdOp>(),
                                                                                         A.Dummy<DoesAnyExistByIdOp<Version>>(),
                                                                                         A.Dummy<StandardGetAllRecordsByIdOp>(),
                                                                                         A.Dummy<GetAllRecordsByIdOp<Version>>(),
                                                                                         A.Dummy<StandardGetAllRecordsMetadataByIdOp>(),
                                                                                         A.Dummy<GetAllRecordsMetadataByIdOp<Version>>(),
                                                                                         A.Dummy<GetLatestObjectByIdOp<Version, Version>>(),
                                                                                         A.Dummy<GetLatestObjectOp<Version>>(),
                                                                                         A.Dummy<GetLatestRecordByIdOp<Version, Version>>(),
                                                                                         A.Dummy<GetLatestRecordByIdOp<Version>>(),
                                                                                         A.Dummy<StandardGetLatestRecordMetadataByIdOp>(),
                                                                                         A.Dummy<StandardGetLatestRecordByIdOp>(),
                                                                                         A.Dummy<GetLatestRecordMetadataByIdOp<Version>>(),
                                                                                         A.Dummy<StandardGetDistinctStringSerializedIdsOp>(),
                                                                                         A.Dummy<StandardGetLatestRecordOp>(),
                                                                                         A.Dummy<StandardGetRecordByInternalRecordIdOp>(),
                                                                                         A.Dummy<GetLatestRecordOp<Version>>(),
                                                                                         A.Dummy<PutOp<Version>>(),
                                                                                         A.Dummy<StandardPutRecordOp>(),
                                                                                         A.Dummy<CreateStreamOp>(),
                                                                                         A.Dummy<DeleteStreamOp>(),
                                                                                         A.Dummy<GetNextUniqueLongOp>(),
                                                                                         A.Dummy<GetStreamFromRepresentationOp<
                                                                                             FileStreamRepresentation, MemoryReadWriteStream>>(),
                                                                                         A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version
                                                                                         >>(),
                                                                                         A.Dummy<PutWithIdOp<Version, Version>>(),
                                                                                     },
                               };
                    });
        }
    }
}