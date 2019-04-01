// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveAllTestBase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Naos.Database.Domain;
    using Naos.Recipes.RunWithRetry;
    using Xunit;

    public abstract class RemoveAllTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Fact]
        public void Removes_all_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var removeTask = Database.ModelCommands.RemoveAllAsync();
            removeTask.RunUntilCompletion();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Removes_all_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var removeTask = Database.StorageModelCommands.RemoveAllAsync();
            removeTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }
    }
}
