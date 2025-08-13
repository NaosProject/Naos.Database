// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryStreamRecordPayloadTest.cs" company="Naos Project">
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
    using OBeautifulCode.Serialization;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class BinaryStreamRecordPayloadTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static BinaryStreamRecordPayloadTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(ConstructorArgumentValidationTestScenario<BinaryStreamRecordPayload>.ConstructorCannotThrowScenario);
        }

        [Fact]
        public static void GetSerializationFormat___Should_return_SerializationFormat_Binary___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<BinaryStreamRecordPayload>();

            // Act
            var actual = systemUnderTest.GetSerializationFormat();

            // Assert
            actual.AsTest().Must().BeEqualTo(SerializationFormat.Binary);
        }

        [Fact]
        public static void GetSerializedPayloadAsEncodedString___Should_return_null___When_serialized_payload_is_null()
        {
            // Arrange
            var systemUnderTest = new BinaryStreamRecordPayload(null);

            // Act
            var actual = systemUnderTest.GetSerializedPayloadAsEncodedString();

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void GetSerializedPayloadAsEncodedString___Should_return_serialized_payload_converted_to_base_64_string___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<BinaryStreamRecordPayload>();

            var expected = Convert.ToBase64String(systemUnderTest.SerializedPayload);

            // Act
            var actual = systemUnderTest.GetSerializedPayloadAsEncodedString();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetSerializedPayloadAsEncodedBytes___Should_return_serialized_payload___When_called()
        {
            // Arrange
            var systemUnderTest = A.Dummy<BinaryStreamRecordPayload>();

            // Act
            var actual = systemUnderTest.GetSerializedPayloadAsEncodedBytes();

            // Assert
            actual.AsTest().Must().BeEqualTo(systemUnderTest.SerializedPayload);
        }
    }
}