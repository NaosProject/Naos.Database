// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllTestBase.cs">
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

    public abstract class GetAllTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyCollection<TestModel> TestModels { get; set; }

        [Fact]
        public void Gets_empty_results_when_no_data_present()
        {
            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Gets_empty_results_when_no_data_present_with_custom_metadata()
        {
            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Gets_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels);
        }

        [Fact]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels);
        }
    }
}
