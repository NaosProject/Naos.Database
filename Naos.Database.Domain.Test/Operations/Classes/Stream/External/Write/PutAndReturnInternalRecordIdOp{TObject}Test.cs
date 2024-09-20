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
    using OBeautifulCode.Type;
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
               .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'recordRetentionCount' is null and parameter 'existingRecordStrategy' is a pruning strategy scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<PutAndReturnInternalRecordIdOp<Version>>();

                            var result = new PutAndReturnInternalRecordIdOp<Version>(
                                                 referenceObject.ObjectToPut,
                                                 referenceObject.Tags,
                                                 A.Dummy<ExistingRecordStrategy>().ThatIs(_ => _.ToString().ToUpperInvariant().Contains("PRUNE")),
                                                 null,
                                                 referenceObject.VersionMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "Must have a retention count if pruning", },
                    })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                   {
                       Name = "constructor should throw ArgumentException when parameter 'recordRetentionCount' is not null and parameter 'existingRecordStrategy' is not a pruning strategy scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<PutAndReturnInternalRecordIdOp<Version>>();

                           var result = new PutAndReturnInternalRecordIdOp<Version>(
                               referenceObject.ObjectToPut,
                               referenceObject.Tags,
                               A.Dummy<ExistingRecordStrategy>().ThatIs(_ => !_.ToString().ToUpperInvariant().Contains("PRUNE")),
                               A.Dummy<ZeroOrPositiveInteger>(),
                               referenceObject.VersionMatchStrategy);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentException),
                       ExpectedExceptionMessageContains = new[] { "Cannot have a retention count if not pruning", },
                   })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                   {
                       Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordRetentionCount' negative",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<PutAndReturnInternalRecordIdOp<Version>>();

                           var result = new PutAndReturnInternalRecordIdOp<Version>(
                               referenceObject.ObjectToPut,
                               referenceObject.Tags,
                               A.Dummy<ExistingRecordStrategy>().ThatIs(_ => _.ToString().ToUpperInvariant().Contains("PRUNE")),
                               A.Dummy<NegativeInteger>(),
                               referenceObject.VersionMatchStrategy);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                       ExpectedExceptionMessageContains = new[] { "recordRetentionCount", },
                   })
               .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<PutAndReturnInternalRecordIdOp<Version>>();

                            var result = new PutAndReturnInternalRecordIdOp<Version>(
                                                 referenceObject.ObjectToPut,
                                                 new NamedValue<string>[0].Concat(referenceObject.Tags).Concat(new NamedValue<string>[] { null }).Concat(referenceObject.Tags).ToList(),
                                                 referenceObject.ExistingRecordStrategy,
                                                 referenceObject.RecordRetentionCount,
                                                 referenceObject.VersionMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tags", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'existingRecordStrategy' is ExistingRecordStrategy.Unknown",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<PutAndReturnInternalRecordIdOp<Version>>();

                            var result = new PutAndReturnInternalRecordIdOp<Version>(
                                                 referenceObject.ObjectToPut,
                                                 referenceObject.Tags,
                                                 ExistingRecordStrategy.Unknown,
                                                 referenceObject.RecordRetentionCount,
                                                 referenceObject.VersionMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "existingRecordStrategy", "Unknown", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'versionMatchStrategy' is VersionMatchStrategy.Unknown",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<PutAndReturnInternalRecordIdOp<Version>>();

                            var result = new PutAndReturnInternalRecordIdOp<Version>(
                                                 referenceObject.ObjectToPut,
                                                 referenceObject.Tags,
                                                 referenceObject.ExistingRecordStrategy,
                                                 referenceObject.RecordRetentionCount,
                                                 VersionMatchStrategy.Unknown);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "versionMatchStrategy", "Unknown", },
                    })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                   {
                       Name = "constructor should throw ArgumentOutOfRangeException when parameter 'typeSelectionStrategy' is TypeSelectionStrategy.Unknown",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<PutAndReturnInternalRecordIdOp<Version>>();

                           var result = new PutAndReturnInternalRecordIdOp<Version>(
                               referenceObject.ObjectToPut,
                               referenceObject.Tags,
                               referenceObject.ExistingRecordStrategy,
                               referenceObject.RecordRetentionCount,
                               referenceObject.VersionMatchStrategy,
                               TypeSelectionStrategy.Unknown);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                       ExpectedExceptionMessageContains = new[] { "typeSelectionStrategy", "Unknown", },
                   });

            EquatableTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new EquatableTestScenario<PutAndReturnInternalRecordIdOp<Version>>
                    {
                        Name = "Default Code Generated Scenario",
                        ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                        ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new PutAndReturnInternalRecordIdOp<Version>[]
                        {
                            new PutAndReturnInternalRecordIdOp<Version>(
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                        },
                        ObjectsThatAreNotEqualToReferenceObject = new PutAndReturnInternalRecordIdOp<Version>[]
                        {
                            new PutAndReturnInternalRecordIdOp<Version>(
                                    A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => !_.ObjectToPut.IsEqualTo(ReferenceObjectForEquatableTestScenarios.ObjectToPut)).ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            new PutAndReturnInternalRecordIdOp<Version>(
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => !_.Tags.IsEqualTo(ReferenceObjectForEquatableTestScenarios.Tags)).Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            new PutAndReturnInternalRecordIdOp<Version>(
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy.ToString().ToUpperInvariant().Contains("PRUNE")
                                        ? A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => _.ToString().ToUpperInvariant().Contains("PRUNE") && !_.ExistingRecordStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy)).ExistingRecordStrategy
                                        : A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => (!_.ToString().ToUpperInvariant().Contains("PRUNE")) && !_.ExistingRecordStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy)).ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            // Harder to test because if RecordRetentionCount is null, then ExistingRecordStrategy needs to be tweaked as well
                            ////new PutAndReturnInternalRecordIdOp<Version>(
                            ////        ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                            ////        ReferenceObjectForEquatableTestScenarios.Tags,
                            ////        ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                            ////        A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => !_.RecordRetentionCount.IsEqualTo(ReferenceObjectForEquatableTestScenarios.RecordRetentionCount)).RecordRetentionCount,
                            ////        ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy),
                            new PutAndReturnInternalRecordIdOp<Version>(
                                    ReferenceObjectForEquatableTestScenarios.ObjectToPut,
                                    ReferenceObjectForEquatableTestScenarios.Tags,
                                    ReferenceObjectForEquatableTestScenarios.ExistingRecordStrategy,
                                    ReferenceObjectForEquatableTestScenarios.RecordRetentionCount,
                                    A.Dummy<PutAndReturnInternalRecordIdOp<Version>>().Whose(_ => !_.VersionMatchStrategy.IsEqualTo(ReferenceObjectForEquatableTestScenarios.VersionMatchStrategy)).VersionMatchStrategy),
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
                            A.Dummy<PutOp<Version>>(),
                            A.Dummy<PutWithIdAndReturnInternalRecordIdOp<Version, Version>>(),
                            A.Dummy<PutWithIdOp<Version, Version>>(),
                            A.Dummy<ResetFailedHandleRecordOp>(),
                            A.Dummy<SelfCancelRunningHandleRecordOp>(),
                            A.Dummy<StandardGetInternalRecordIdsOp>(),
                            A.Dummy<StandardGetDistinctStringSerializedIdsOp>(),
                            A.Dummy<StandardGetLatestRecordOp>(),
                            A.Dummy<StandardGetNextUniqueLongOp>(),
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