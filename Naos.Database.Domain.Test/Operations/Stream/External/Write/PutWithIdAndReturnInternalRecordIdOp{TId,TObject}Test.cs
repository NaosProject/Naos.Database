// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutWithIdAndReturnInternalRecordIdOp{TId,TObject}Test.cs" company="Naos Project">
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
    public static partial class PutWithIdAndReturnInternalRecordIdOpTIdTObjectTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static PutWithIdAndReturnInternalRecordIdOpTIdTObjectTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'recordRetentionCount' is null and parameter 'existingRecordStrategy' is a pruning strategy scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>();

                            var result = new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                                                 referenceObject.Id,
                                                 referenceObject.ObjectToPut,
                                                 referenceObject.Tags,
                                                 A.Dummy<ExistingRecordStrategy>().ThatIs(_ => _.ToString().ToLowerInvariant().Contains("prune")),
                                                 null,
                                                 referenceObject.VersionMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "Must have a retention count if pruning", },
                    })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>
                   {
                       Name = "constructor should throw ArgumentException when parameter 'recordRetentionCount' is not null and parameter 'existingRecordStrategy' is not a pruning strategy scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>();

                           var result = new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                               referenceObject.Id,
                               referenceObject.ObjectToPut,
                               referenceObject.Tags,
                               A.Dummy<ExistingRecordStrategy>().ThatIs(_ => !_.ToString().ToLowerInvariant().Contains("prune")),
                               A.Dummy<ZeroOrPositiveInteger>(),
                               referenceObject.VersionMatchStrategy);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentException),
                       ExpectedExceptionMessageContains = new[] { "Cannot have a retention count if not pruning", },
                   })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>
                   {
                       Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordRetentionCount' negative",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>();

                           var result = new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                               referenceObject.Id,
                               referenceObject.ObjectToPut,
                               referenceObject.Tags,
                               A.Dummy<ExistingRecordStrategy>().ThatIs(_ => _.ToString().ToLowerInvariant().Contains("prune")),
                               A.Dummy<NegativeInteger>(),
                               referenceObject.VersionMatchStrategy);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                       ExpectedExceptionMessageContains = new[] { "recordRetentionCount", },
                   });

            EquatableTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new EquatableTestScenario<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>
                    {
                        Name = "Default Code Generated Scenario",
                        ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                        ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new PutWithIdAndReturnInternalRecordIdOp<Version, Version>[]
                        {
                            new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                                    ReferenceObjectForEquatableTestScenarios.Id,
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                        },
                        ObjectsThatAreNotEqualToReferenceObject = new PutWithIdAndReturnInternalRecordIdOp<Version, Version>[]
                        {
                            new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                                    A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>().Whose(_ => !_.Id.IsEqualTo(ReferenceObjectForEquatableTestScenarios.Id)).Id,
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                                    ReferenceObjectForEquatableTestScenarios.Id,
                                    A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>().Whose(_ => !_.ObjectToPut.IsEqualTo(ReferenceObjectForEquatableTestScenarios.ObjectToPut)).ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                                    ReferenceObjectForEquatableTestScenarios.Id,
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>().Whose(_ => !_.Tags.IsEqualTo(ReferenceObjectForEquatableTestScenarios.Tags)).Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                                    ReferenceObjectForEquatableTestScenarios.Id,
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy.ToString().ToLowerInvariant().Contains("prune")
                                        ? A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => _.ToString().ToLowerInvariant().Contains("prune") && !_.ExistingRecordStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy)).ExistingRecordStrategy
                                        : A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => (!_.ToString().ToLowerInvariant().Contains("prune")) && !_.ExistingRecordStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy)).ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            // Harder to test because if RecordRetentionCount is null, then ExistingRecordStrategy needs to be tweaked as well
                            ////new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                            ////        ReferenceObjectForEquatableTestScenarios.Id,
                            ////        ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ////        ReferenceObjectForEquatableTestScenarios.Tags,
                            ////        ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                            ////        A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>().Whose(_ => !_.RecordRetentionCount.IsEqualTo(ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)).RecordRetentionCount,
                            ////        ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            new PutWithIdAndReturnInternalRecordIdOp<Version, Version>(
                                    ReferenceObjectForEquatableTestScenarios.Id,
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>().Whose(_ => !_.VersionMatchStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy)).VersionMatchStrategy),
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
                            A.Dummy<CreateStreamOp>(),
                            A.Dummy<DeleteStreamOp>(),
                            A.Dummy<DoesAnyExistByIdOp<Version>>(),
                            A.Dummy<FailRunningHandleRecordExecutionOp>(),
                            A.Dummy<GetAllRecordsByIdOp<Version>>(),
                            A.Dummy<GetAllRecordsMetadataByIdOp<Version>>(),
                            A.Dummy<GetAllResourceLocatorsOp>(),
                            A.Dummy<GetHandlingHistoryOfRecordOp>(),
                            A.Dummy<GetHandlingStatusOfRecordByInternalRecordIdOp>(),
                            A.Dummy<GetHandlingStatusOfRecordsByIdOp>(),
                            A.Dummy<GetHandlingStatusOfRecordsByIdOp<Version>>(),
                            A.Dummy<GetHandlingStatusOfRecordSetByTagOp>(),
                            A.Dummy<GetLatestObjectByIdOp<Version, Version>>(),
                            A.Dummy<GetLatestObjectByTagOp<Version>>(),
                            A.Dummy<GetLatestObjectOp<Version>>(),
                            A.Dummy<GetLatestRecordByIdOp<Version, Version>>(),
                            A.Dummy<GetLatestRecordByIdOp<Version>>(),
                            A.Dummy<GetLatestRecordMetadataByIdOp<Version>>(),
                            A.Dummy<GetLatestRecordOp<Version>>(),
                            A.Dummy<GetNextUniqueLongOp>(),
                            A.Dummy<GetResourceLocatorByIdOp<Version>>(),
                            A.Dummy<GetResourceLocatorForUniqueIdentifierOp>(),
                            A.Dummy<GetStreamFromRepresentationOp>(),
                            A.Dummy<GetStreamFromRepresentationOp<FileStreamRepresentation, MemoryReadWriteStream>>(),
                            A.Dummy<HandleRecordOp>(),
                            A.Dummy<HandleRecordOp<Version>>(),
                            A.Dummy<HandleRecordWithIdOp<Version, Version>>(),
                            A.Dummy<HandleRecordWithIdOp<Version>>(),
                            A.Dummy<PruneBeforeInternalRecordDateOp>(),
                            A.Dummy<PruneBeforeInternalRecordIdOp>(),
                            A.Dummy<PutAndReturnInternalRecordIdOp<Version>>(),
                            A.Dummy<PutOp<Version>>(),
                            A.Dummy<PutWithIdOp<Version, Version>>(),
                            A.Dummy<RetryFailedHandleRecordExecutionOp>(),
                            A.Dummy<SelfCancelRunningHandleRecordExecutionOp>(),
                            A.Dummy<StandardDoesAnyExistByIdOp>(),
                            A.Dummy<StandardGetAllRecordsByIdOp>(),
                            A.Dummy<StandardGetAllRecordsMetadataByIdOp>(),
                            A.Dummy<StandardGetDistinctStringSerializedIdsOp>(),
                            A.Dummy<StandardGetLatestRecordByIdOp>(),
                            A.Dummy<StandardGetLatestRecordByTagOp>(),
                            A.Dummy<StandardGetLatestRecordMetadataByIdOp>(),
                            A.Dummy<StandardGetLatestRecordOp>(),
                            A.Dummy<StandardGetNextUniqueLongOp>(),
                            A.Dummy<StandardGetRecordByInternalRecordIdOp>(),
                            A.Dummy<StandardPutRecordOp>(),
                            A.Dummy<ThrowIfResourceUnavailableOp>(),
                            A.Dummy<StandardTryHandleRecordOp>(),
                            A.Dummy<TryHandleRecordOp<Version>>(),
                            A.Dummy<TryHandleRecordWithIdOp<Version, Version>>(),
                            A.Dummy<TryHandleRecordWithIdOp<Version>>(),
                        },
                    });
        }
    }
}