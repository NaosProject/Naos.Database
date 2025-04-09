// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetInternalRecordIdsOpTest.cs" company="Naos Project">
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
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class StandardGetInternalRecordIdsOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardGetInternalRecordIdsOpTest()
        {
        }

        [Fact]
        public static void Constructor___Should_coalesce_recordsToFilterCriteria_to_default___When_recordsToFilterCriteria_is_null()
        {
            // Arrange
            var systemUnderTest = new StandardGetInternalRecordIdsOp(new RecordFilter(), recordsToFilterCriteria: null);

            var expected = new RecordsToFilterCriteria();

            // Act, Assert
            systemUnderTest.RecordsToFilterCriteria.AsTest().Must().BeEqualTo(expected);
        }
    }
}