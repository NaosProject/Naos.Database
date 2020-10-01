// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryRecord{TId}Test.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory.Test
{
    using System;

    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Serialization;
    using Xunit;

    public static partial class MemoryRecordTest
    {
        static MemoryRecordTest()
        {
            ConstructorArgumentValidationTestScenarios.RemoveAllScenarios();
            ConstructorArgumentValidationTestScenarios
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<MemoryRecord<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'describedSerialization' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<MemoryRecord<Version>>();

                                                   var result = new MemoryRecord<Version>(
                                                       referenceObject.Id,
                                                       null,
                                                       referenceObject.DateTimeUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "describedSerialization"
                                                               },
                        });
        }
    }
}