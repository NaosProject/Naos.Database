// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOneTestBase.cs">
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

    public abstract class RemoveOneTestBase
    {
        protected ITestDatabase Database { get; set; }
        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }
        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Fact]
        public void Removes_expected_result_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var removeTask = Database.ModelCommands.RemoveOneAsync(TestModels[1]);
            removeTask.RunUntilCompletion();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Where(m => m.Id != TestModels[1].Id).ToList());
        }

        [Fact]
        public void Removes_expected_result_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var removeTask = Database.StorageModelCommands.RemoveOneAsync(StorageModels[1].Model);
            removeTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Where(m => m.Model.Id != StorageModels[1].Model.Id).ToList());
        }

        [Fact]
        public void Remove_does_nothing_when_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var nonExistent = new TestModel("me no exist", Guid.NewGuid());
            var removeTask = Database.ModelCommands.RemoveOneAsync(nonExistent);
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

            var nonExistent = new TestModel("me no exist", Guid.NewGuid());
            var removeTask = Database.StorageModelCommands.RemoveOneAsync(nonExistent);
            removeTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels);
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelCommands.RemoveOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelCommands.RemoveOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
