// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssertResults.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Naos.Database.Domain;

    internal static class AssertResults
    {
        public static void Match(IReadOnlyCollection<TestModel> results, IReadOnlyCollection<TestModel> testModels)
        {
            results.Count.Should().Be(testModels.Count);
            foreach (var testModel in testModels)
            {
                results.Should().Contain(m => m.Id == testModel.Id && m.Name == testModel.Name);
            }
        }

        public static void Match(
            IReadOnlyCollection<TestModel> results,
            IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> storageModels)
        {
            results.Count.Should().Be(storageModels.Count);
            foreach (var storageModel in storageModels)
            {
                results.Should().Contain(m => m.Id == storageModel.Model.Id && m.Name == storageModel.Model.Name);
            }
        }
    }
}
