// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssertResult.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using Naos.Database.Domain;
    using Xunit;

    internal static class AssertResult
    {
        public static void Matches(TestModel result, TestModel testModel)
        {
            Assert.NotNull(result);
            Assert.Equal(testModel.Id, result.Id);
            Assert.Equal(testModel.Name, result.Name);
        }

        public static void Matches(TestModel result, StorageModel<TestModel, TestMetadata> storageModel)
        {
            Assert.NotNull(result);
            Assert.Equal(storageModel.Model.Id, result.Id);
            Assert.Equal(storageModel.Model.Name, result.Name);
        }
    }
}
