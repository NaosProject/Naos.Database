// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordResultTest.cs" company="Naos Project">
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
    public static partial class TryHandleRecordResultTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static TryHandleRecordResultTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TryHandleRecordResult>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'recordToHandle' is not null and 'isBlocked' is true scenario",
                        ConstructionFunc = () =>
                        {
                            var result = new TryHandleRecordResult(
                                A.Dummy<StreamRecord>(),
                                true);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "isBlocked is true", "Provided value (name: 'recordToHandle') is not null", },
                    });

            EquatableTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new EquatableTestScenario<TryHandleRecordResult>
                    {
                        Name = "Default Code Generated Scenario",
                        ReferenceObject = ReferenceObjectForEquatableTestScenarios,
                        ObjectsThatAreEqualToButNotTheSameAsReferenceObject = new TryHandleRecordResult[]
                        {
                            new TryHandleRecordResult(
                                ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                                ReferenceObjectForEquatableTestScenarios.IsBlocked),
                        },
                        ObjectsThatAreNotEqualToReferenceObject = new TryHandleRecordResult[]
                        {
                            // Too much effort to setup the cases to deal with the scenario that throws.
                            ////new TryHandleRecordResult(
                            ////    A.Dummy<TryHandleRecordResult>().Whose(_ => !_.RecordToHandle.IsEqualTo(ReferenceObjectForEquatableTestScenarios.RecordToHandle)).RecordToHandle,
                            ////    ReferenceObjectForEquatableTestScenarios.IsBlocked),
                            ////new TryHandleRecordResult(
                            ////    ReferenceObjectForEquatableTestScenarios.RecordToHandle,
                            ////    A.Dummy<TryHandleRecordResult>().Whose(_ => !_.IsBlocked.IsEqualTo(ReferenceObjectForEquatableTestScenarios.IsBlocked)).IsBlocked),
                        },
                        ObjectsThatAreNotOfTheSameTypeAsReferenceObject = new object[]
                        {
                            A.Dummy<object>(),
                            A.Dummy<string>(),
                            A.Dummy<int>(),
                            A.Dummy<int?>(),
                            A.Dummy<Guid>(),
                        },
                    });
        }
    }
}