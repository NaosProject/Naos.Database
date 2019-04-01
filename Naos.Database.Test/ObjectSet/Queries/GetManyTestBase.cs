// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyTestBase.cs">
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

    public abstract class GetManyTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        protected string TestStorageName { get; set; }

        [Fact]
        public void Gets_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetManyAsync(m => m.Name.StartsWith(TestStorageName));
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels);
        }

        [Fact]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetManyAsync(m => m.Model.Name.StartsWith(TestStorageName));
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels);
        }

        [Fact]
        public void Gets_expected_subset_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var ids = TestModels.Skip(1).Take(3).Select(m => m.Id);
            var getTask = Database.ModelQueries.GetManyAsync(m => ids.Contains(m.Id));
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Skip(1).Take(3).ToList());
        }

        [Fact]
        public void Gets_expected_subset_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var ids = StorageModels.Skip(1).Take(3).Select(m => m.Model.Id);
            var getTask = Database.StorageModelQueries.GetManyAsync(m => ids.Contains(m.Model.Id));
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Skip(1).Take(3).ToList());
        }

        [Fact]
        public void Gets_empty_results_with_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetManyAsync(m => m.Id == Guid.NewGuid());
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Gets_empty_results_with_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetManyAsync(m => m.Model.Id == Guid.NewGuid());
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelQueries.GetManyAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelQueries.GetManyAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
