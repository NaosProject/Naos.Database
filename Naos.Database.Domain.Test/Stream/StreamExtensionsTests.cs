﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamExtensionsTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test.MemoryStream
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Naos.Database.Domain;
    using Naos.Database.Serialization.Json;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using Xunit;
    using Xunit.Abstractions;
    using static System.FormattableString;

    /// <summary>
    /// Tests for <see cref="MemoryReadWriteStream"/>.
    /// </summary>
    public partial class StreamExtensionsTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public StreamExtensionsTests(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CheckMethods()
        {
            /*
GetLatestObjectByIdOp - TObject
GetLatestObjectByIdOpAsync - Task<TObject>
GetNextUniqueLong - long
GetNextUniqueLongAsync - Task<long>
Put - void
PutAsync - Task
PutWithId - void
PutWithIdAsync - Task
            var expectedOperationTypes = new[]
                                         {
                                             typeof(GetNextUniqueLongOp),
                                             typeof(GetLatestRecordByIdOp),
                                             typeof(GetLatestRecordByIdOp<>),
                                             typeof(GetLatestRecordByIdOp<,>),
                                             typeof(GetLatestObjectOp<>),
                                             typeof(GetLatestObjectByIdOp<,>),
                                             typeof(PutRecordOp),
                                             typeof(PutOp<>),
                                             typeof(PutWithIdOp<,>),
                                         };
*/

            var extType = typeof(StreamExtensions);
            var methods = extType.GetMethodsFiltered(
                MemberRelationships.DeclaredInType,
                MemberOwners.Static,
                MemberAccessModifiers.Public,
                MemberAttributes.NotCompilerGenerated,
                OrderMembersBy.MemberName);

            foreach (var methodInfo in methods)
            {
                this.testOutputHelper.WriteLine(Invariant($"{methodInfo.Name} - {methodInfo.ReturnType.ToStringReadable()}"));
            }
        }
    }
}
