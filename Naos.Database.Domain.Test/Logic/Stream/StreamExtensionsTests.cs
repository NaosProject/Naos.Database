// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamExtensionsTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test.MemoryStream
{
    using System.Linq;
    using Naos.Database.Domain;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type.Recipes;
    using Xunit;
    using Xunit.Abstractions;
    using static System.FormattableString;

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
            var extTypeRead = typeof(ReadOnlyStreamExtensions);

            var extTypeWrite = typeof(WriteOnlyStreamExtensions);

            var methodsRead = extTypeRead.GetMethodsFiltered(
                MemberRelationships.DeclaredInType,
                MemberOwners.Static,
                MemberAccessModifiers.Public,
                MemberAttributes.NotCompilerGenerated,
                OrderMembersBy.MemberName);

            var methodsWrite = extTypeWrite.GetMethodsFiltered(
                MemberRelationships.DeclaredInType,
                MemberOwners.Static,
                MemberAccessModifiers.Public,
                MemberAttributes.NotCompilerGenerated,
                OrderMembersBy.MemberName);

            var methods = methodsRead.Union(methodsWrite).ToList();

            foreach (var methodInfo in methods)
            {
                this.testOutputHelper.WriteLine(Invariant($"{methodInfo.Name} - {methodInfo.ReturnType.ToStringReadable()}"));
            }
        }
    }
}
