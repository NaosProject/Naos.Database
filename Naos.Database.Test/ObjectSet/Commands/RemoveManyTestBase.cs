// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveManyTestBase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Naos.Database.Domain;
    using Naos.Recipes.RunWithRetry;
    using Xunit;

    public abstract class RemoveManyTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        protected string TestStorageName { get; set; }

        [Fact]
        public void Removes_all_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var removeTask = Database.ModelCommands.RemoveManyAsync(m => m.Name.StartsWith(TestStorageName));
            removeTask.RunUntilCompletion();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Removes_all_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var removeTask = Database.StorageModelCommands.RemoveManyAsync(m => m.Model.Name.StartsWith(TestStorageName));
            removeTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Removes_expected_subset_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var ids = TestModels.Skip(2).Take(3).Select(m => m.Id);
            var removeTask = Database.ModelCommands.RemoveManyAsync(m => ids.Contains(m.Id));
            removeTask.RunUntilCompletion();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Take(2).ToList());
        }

        [Fact]
        public void Removes_expected_subset_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var ids = StorageModels.Skip(2).Take(3).Select(m => m.Model.Id);
            var removeTask = Database.StorageModelCommands.RemoveManyAsync(m => ids.Contains(m.Model.Id));
            removeTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Take(2).ToList());
        }

        [Fact]
        public void Remove_does_nothing_when_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var removeTask = Database.ModelCommands.RemoveManyAsync(m => m.Id == Guid.NewGuid());
            removeTask.RunUntilCompletion();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels);
        }

        [Fact]
        public void Remove_does_nothing_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var removeTask = Database.StorageModelCommands.RemoveManyAsync(m => m.Model.Id == Guid.NewGuid());
            removeTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels);
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelCommands.RemoveManyAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelCommands.RemoveManyAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
