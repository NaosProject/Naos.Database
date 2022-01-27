// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardPutRecordOpTest.cs" company="Naos Project">
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
    public static partial class StandardPutRecordOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardPutRecordOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardPutRecordOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'metadata' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardPutRecordOp>();

                            var result = new StandardPutRecordOp(
                                                 null,
                                                 referenceObject.Payload,
                                                 referenceObject.ExistingRecordStrategy,
                                                 referenceObject.RecordRetentionCount,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "metadata", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardPutRecordOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'payload' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardPutRecordOp>();

                            var result = new StandardPutRecordOp(
                                                 referenceObject.Metadata,
                                                 null,
                                                 referenceObject.ExistingRecordStrategy,
                                                 referenceObject.RecordRetentionCount,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "payload", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardPutRecordOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'existingRecordStrategy' is ExistingRecordStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardPutRecordOp>();

                            var result = new StandardPutRecordOp(
                                referenceObject.Metadata,
                                referenceObject.Payload,
                                ExistingRecordStrategy.Unknown,
                                referenceObject.RecordRetentionCount,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.InternalRecordId,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "existingRecordStrategy", "Unknown" },
                    })
                 .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardPutRecordOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'recordRetentionCount' is null and parameter 'existingRecordStrategy' is a pruning strategy scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardPutRecordOp>();

                            var result = new StandardPutRecordOp(
                                referenceObject.Metadata,
                                referenceObject.Payload,
                                A.Dummy<ExistingRecordStrategy>().ThatIs(_ => _.ToString().ToUpperInvariant().Contains("PRUNE")),
                                null,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.InternalRecordId,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "Must have a retention count if pruning", },
                    })
                   .AddScenario(() =>
                       new ConstructorArgumentValidationTestScenario<StandardPutRecordOp>
                       {
                           Name = "constructor should throw ArgumentException when parameter 'recordRetentionCount' is not null and parameter 'existingRecordStrategy' is not a pruning strategy scenario",
                           ConstructionFunc = () =>
                           {
                               var referenceObject = A.Dummy<StandardPutRecordOp>();

                               var result = new StandardPutRecordOp(
                                   referenceObject.Metadata,
                                   referenceObject.Payload,
                                   A.Dummy<ExistingRecordStrategy>().ThatIs(_ => !_.ToString().ToUpperInvariant().Contains("PRUNE")),
                                   A.Dummy<ZeroOrPositiveInteger>(),
                                   referenceObject.VersionMatchStrategy,
                                   referenceObject.InternalRecordId,
                                   referenceObject.SpecifiedResourceLocator);

                               return result;
                           },
                           ExpectedExceptionType = typeof(ArgumentException),
                           ExpectedExceptionMessageContains = new[] { "Cannot have a retention count if not pruning", },
                       })
                   .AddScenario(() =>
                       new ConstructorArgumentValidationTestScenario<StandardPutRecordOp>
                       {
                           Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordRetentionCount' negative",
                           ConstructionFunc = () =>
                           {
                               var referenceObject = A.Dummy<StandardPutRecordOp>();

                               var result = new StandardPutRecordOp(
                                   referenceObject.Metadata,
                                   referenceObject.Payload,
                                   A.Dummy<ExistingRecordStrategy>().ThatIs(_ => _.ToString().ToUpperInvariant().Contains("PRUNE")),
                                   A.Dummy<NegativeInteger>(),
                                   referenceObject.VersionMatchStrategy,
                                   referenceObject.InternalRecordId,
                                   referenceObject.SpecifiedResourceLocator);

                               return result;
                           },
                           ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                           ExpectedExceptionMessageContains = new[] { "recordRetentionCount", },
                       });

            EquatableTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new EquatableTestScenario<StandardPutRecordOp>
                    {
                        Name = "Default Code Generated Scenario",
                        ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                        ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new StandardPutRecordOp[]
                        {
                            new StandardPutRecordOp(
                                    ReferenceObjectForEquatableTestScenarios.Metadata,
                                    ReferenceObjectForEquatableTestScenarios.Payload,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator),
                        },
                        ObjectsThatAreNotEqualToReferenceObject = new StandardPutRecordOp[]
                        {
                            new StandardPutRecordOp(
                                    A.Dummy<StandardPutRecordOp>().Whose(_ => !_.Metadata.IsEqualTo(ReferenceObjectForEquatableTestScenarios.Metadata)).Metadata,
                                    ReferenceObjectForEquatableTestScenarios.Payload,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator),
                            new StandardPutRecordOp(
                                    ReferenceObjectForEquatableTestScenarios.Metadata,
                                    A.Dummy<StandardPutRecordOp>().Whose(_ => !_.Payload.IsEqualTo(ReferenceObjectForEquatableTestScenarios.Payload)).Payload,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator),
                            new StandardPutRecordOp(
                                    ReferenceObjectForEquatableTestScenarios.Metadata,
                                    ReferenceObjectForEquatableTestScenarios.Payload,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy.ToString().ToUpperInvariant().Contains("PRUNE")
                                        ? A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => _.ToString().ToUpperInvariant().Contains("PRUNE") && !_.ExistingRecordStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy)).ExistingRecordStrategy
                                        : A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => (!_.ToString().ToUpperInvariant().Contains("PRUNE")) && !_.ExistingRecordStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy)).ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator),
                            // Harder to test because if RecordRetentionCount is null, then ExistingRecordStrategy needs to be tweaked as well
                            ////new StandardPutRecordOp(
                            ////        ReferenceObjectForEquatableTestScenarios.Metadata,
                            ////        ReferenceObjectForEquatableTestScenarios.Payload,
                            ////        ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                            ////        A.Dummy<StandardPutRecordOp>().Whose(_ => !_.RecordRetentionCount.IsEqualTo(ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)).RecordRetentionCount,
                            ////        ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                            ////        ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                            ////        ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator),
                            new StandardPutRecordOp(
                                    ReferenceObjectForEquatableTestScenarios.Metadata,
                                    ReferenceObjectForEquatableTestScenarios.Payload,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    A.Dummy<StandardPutRecordOp>().Whose(_ => !_.VersionMatchStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy)).VersionMatchStrategy,
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator),
                            new StandardPutRecordOp(
                                    ReferenceObjectForEquatableTestScenarios.Metadata,
                                    ReferenceObjectForEquatableTestScenarios.Payload,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                                    A.Dummy<StandardPutRecordOp>().Whose(_ => !_.InternalRecordId.IsEqualTo(ReferenceObjectForEquatableTestScenarios.InternalRecordId)).InternalRecordId,
                                    ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator),
                            new StandardPutRecordOp(
                                    ReferenceObjectForEquatableTestScenarios.Metadata,
                                    ReferenceObjectForEquatableTestScenarios.Payload,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy,
                                    ReferenceObjectForEquatableTestScenarios.InternalRecordId,
                                    A.Dummy<StandardPutRecordOp>().Whose(_ => !_.SpecifiedResourceLocator.IsEqualTo(ReferenceObjectForEquatableTestScenarios.SpecifiedResourceLocator)).SpecifiedResourceLocator),
                        },
                        ObjectsThatAreNotOfTheSameTypeAsReferenceObject = new object[]
                        {
                            A.Dummy<object>(),
                            A.Dummy<string>(),
                            A.Dummy<int>(),
                            A.Dummy<int?>(),
                            A.Dummy<Guid>(),
                            A.Dummy<DisableHandlingForStreamOp>(),
                            A.Dummy<EnableHandlingForStreamOp>(),
                            A.Dummy<DisableHandlingForRecordOp>(),
                            A.Dummy<CancelRunningHandleRecordOp>(),
                            A.Dummy<CompleteRunningHandleRecordOp>(),
                            A.Dummy<StandardCreateStreamOp>(),
                            A.Dummy<StandardDeleteStreamOp>(),
                            A.Dummy<DoesAnyExistByIdOp<Version>>(),
                            A.Dummy<FailRunningHandleRecordOp>(),
                            A.Dummy<GetAllRecordsByIdOp<Version>>(),
                            A.Dummy<GetAllRecordsMetadataByIdOp<Version>>(),
                            A.Dummy<GetAllResourceLocatorsOp>(),
                            A.Dummy<GetHandlingHistoryOp>(),
                            A.Dummy<GetHandlingStatusOp>(),
                            A.Dummy<GetCompositeHandlingStatusByIdsOp>(),
                            A.Dummy<GetCompositeHandlingStatusByIdsOp<Version>>(),
                            A.Dummy<GetCompositeHandlingStatusByTagsOp>(),
                            A.Dummy<GetLatestObjectByIdOp<Version, Version>>(),
                            A.Dummy<GetLatestObjectByTagsOp<Version>>(),
                            A.Dummy<GetLatestObjectOp<Version>>(),
                            A.Dummy<GetLatestRecordByIdOp<Version, Version>>(),
                            A.Dummy<GetLatestRecordByIdOp<Version>>(),
                            A.Dummy<GetLatestRecordMetadataByIdOp<Version>>(),
                            A.Dummy<GetLatestRecordOp<Version>>(),
                            A.Dummy<GetNextUniqueLongOp>(),
                            A.Dummy<GetResourceLocatorByIdOp<Version>>(),
                            A.Dummy<GetResourceLocatorForUniqueIdentifierOp>(),
                            A.Dummy<GetStreamFromRepresentationOp>(),
                            A.Dummy<GetStreamFromRepresentationOp<FileStreamRepresentation, MemoryStandardStream>>(),
                            A.Dummy<HandleRecordOp>(),
                            A.Dummy<HandleRecordOp<Version>>(),
                            A.Dummy<HandleRecordWithIdOp<Version, Version>>(),
                            A.Dummy<HandleRecordWithIdOp<Version>>(),
                            A.Dummy<PruneBeforeInternalRecordDateOp>(),
                            A.Dummy<PruneBeforeInternalRecordIdOp>(),
                            A.Dummy<PutAndReturnInternalRecordIdOp<Version>>(),
                            A.Dummy<PutOp<Version>>(),
                            A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>(),
                            A.Dummy<PutWithIdOp<Version, Version>>(),
                            A.Dummy<ResetFailedHandleRecordOp>(),
                            A.Dummy<SelfCancelRunningHandleRecordOp>(),
                            A.Dummy<StandardGetInternalRecordIdsOp>(),
                            A.Dummy<StandardGetDistinctStringSerializedIdsOp>(),
                            A.Dummy<StandardGetLatestRecordOp>(),
                            A.Dummy<StandardGetNextUniqueLongOp>(),
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