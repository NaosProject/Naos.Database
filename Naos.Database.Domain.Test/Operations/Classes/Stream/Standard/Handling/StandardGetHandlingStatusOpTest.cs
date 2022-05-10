// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetHandlingStatusOpTest.cs" company="Naos Project">
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
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class StandardGetHandlingStatusOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardGetHandlingStatusOpTest()
        {
            /*
            concern.ThrowIfInvalidOrReservedConcern();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
             */
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                                                   var result = new StandardGetHandlingStatusOp(
                                                       null,
                                                       referenceObject.RecordFilter,
                                                       referenceObject.HandlingFilter,
                                                       specifiedResourceLocator: referenceObject.SpecifiedResourceLocator);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "concern",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'recordFilter' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                                                   var result = new StandardGetHandlingStatusOp(
                                                       referenceObject.Concern,
                                                       null,
                                                       referenceObject.HandlingFilter,
                                                       specifiedResourceLocator: referenceObject.SpecifiedResourceLocator);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "recordFilter",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'handlingFilter' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                                                   var result = new StandardGetHandlingStatusOp(
                                                       referenceObject.Concern,
                                                       referenceObject.RecordFilter,
                                                       null,
                                                       specifiedResourceLocator: referenceObject.SpecifiedResourceLocator);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "handlingFilter",
                                                               },
                        });
        }
    }
}