// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutRecordOpTest.cs" company="Naos Project">
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
    public static partial class PutRecordOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static PutRecordOpTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StandardPutRecordOp>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'metadata' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StandardPutRecordOp>();

                                                   var result = new StandardPutRecordOp(
                                                       null,
                                                       referenceObject.Payload,
                                                       referenceObject.SpecifiedResourceLocator);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "metadata",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StandardPutRecordOp>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'payload' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StandardPutRecordOp>();

                                                   var result = new StandardPutRecordOp(
                                                       referenceObject.Metadata,
                                                       null,
                                                       referenceObject.SpecifiedResourceLocator);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "payload",
                                                               },
                        });

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

                        var unequalMetadata = new StandardPutRecordOp(
                            A.Dummy<StandardPutRecordOp>()
                             .Whose(
                                  _ => !_.Metadata.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios.Metadata))
                             .Metadata,
                            ReferenceObjectForEquatableTestScenarios.Payload,
                            ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                            ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy);

                        var unequalPayload = new StandardPutRecordOp(
                            ReferenceObjectForEquatableTestScenarios.Metadata,
                            A.Dummy<StandardPutRecordOp>()
                             .Whose(
                                  _ => !_.Payload.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .Payload))
                             .Payload,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                            ReferenceObjectForEquatableTestScenarios
                               .VersionMatchStrategy);

                        var unequalLocator = new StandardPutRecordOp(
                            ReferenceObjectForEquatableTestScenarios.Metadata,
                            ReferenceObjectForEquatableTestScenarios.Payload,
                            A.Dummy<StandardPutRecordOp>()
                             .Whose(
                                  _ => !_.SpecifiedResourceLocator.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .SpecifiedResourceLocator))
                             .SpecifiedResourceLocator,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                            ReferenceObjectForEquatableTestScenarios
                               .VersionMatchStrategy);

                        var unequalExistingStrategy = new StandardPutRecordOp(
                            ReferenceObjectForEquatableTestScenarios.Metadata,
                            ReferenceObjectForEquatableTestScenarios.Payload,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator,
                            referenceObjectIsPruning
                                ? A.Dummy<ExistingRecordEncounteredStrategy>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy && (_ == ExistingRecordEncounteredStrategy.PruneIfFoundById || _ == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType))
                                : ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById,
                            referenceObjectIsPruning
                                ? A.Dummy<int>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)
                                : (int?)null,
                            ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                            ReferenceObjectForEquatableTestScenarios
                               .VersionMatchStrategy);

                        var unequalRetentionCount = new StandardPutRecordOp(
                            ReferenceObjectForEquatableTestScenarios.Metadata,
                            ReferenceObjectForEquatableTestScenarios.Payload,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator,
                            referenceObjectIsPruning
                                ? A.Dummy<ExistingRecordEncounteredStrategy>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.ExistingRecordEncounteredStrategy && (_ == ExistingRecordEncounteredStrategy.PruneIfFoundById || _ == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType))
                                : ExistingRecordEncounteredStrategy.DoNotWriteIfFoundById,
                            referenceObjectIsPruning
                                ? A.Dummy<int>().Whose(_ => _ != ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)
                                : (int?)null,
                            ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                            ReferenceObjectForEquatableTestScenarios
                               .VersionMatchStrategy);

                        var unequalInternalRecordId = new StandardPutRecordOp(
                            ReferenceObjectForEquatableTestScenarios.Metadata,
                            ReferenceObjectForEquatableTestScenarios.Payload,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            A.Dummy<StandardPutRecordOp>()
                             .Whose(
                                  _ => !_.InternalRecordId.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .InternalRecordId))
                             .InternalRecordId,
                            ReferenceObjectForEquatableTestScenarios
                               .VersionMatchStrategy);

                        var unequalTypeMatchStrategy = new StandardPutRecordOp(
                            ReferenceObjectForEquatableTestScenarios.Metadata,
                            ReferenceObjectForEquatableTestScenarios.Payload,
                            ReferenceObjectForEquatableTestScenarios
                               .SpecifiedResourceLocator,
                            ReferenceObjectForEquatableTestScenarios
                               .ExistingRecordEncounteredStrategy,
                            ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                            ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                            A.Dummy<StandardPutRecordOp>()
                             .Whose(
                                  _ => !_.VersionMatchStrategy.IsEqualTo(
                                      ReferenceObjectForEquatableTestScenarios
                                         .VersionMatchStrategy))
                             .VersionMatchStrategy);

                        return new EquatableTestScenario<StandardPutRecordOp>
                               {
                                   Name = "Default Code Generated Scenario",
                                   ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                                   ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new StandardPutRecordOp[]
                                                                                         {
                                                                                             new StandardPutRecordOp(
                                                                                                 ReferenceObjectForEquatableTestScenarios.Metadata,
                                                                                                 ReferenceObjectForEquatableTestScenarios.Payload,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .SpecifiedResourceLocator,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .ExistingRecordEncounteredStrategy,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .RecordRetentionCount,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .InternalRecordId,
                                                                                                 ReferenceObjectForEquatableTestScenarios
                                                                                                    .VersionMatchStrategy),
                                                                                         },
                                   ObjectsThatAreNotEqualToReferenceObject = new StandardPutRecordOp[]
                                                                             {
                                                                                 unequalMetadata,
                                                                                 unequalPayload,
                                                                                 unequalLocator,
                                                                                 unequalExistingStrategy,
                                                                                 unequalRetentionCount,
                                                                                 unequalInternalRecordId,
                                                                                 unequalTypeMatchStrategy,
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
                                                                                         A.Dummy<PutAndReturnInternalRecordIdOp<Version>>(),
                                                                                         A.Dummy<PutOp<Version>>(),
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