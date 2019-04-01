// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetOneTestBase.cs">
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

    public abstract class GetOneTestBase
    {
        protected ITestDatabase Database { get; set; }
        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }
        protected IReadOnlyList<TestModel> TestModels { get; set; }
        protected string TestStorageName { get; set; }

        [Fact]
        public void Gets_expected_result_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Id == TestModels[1].Id);
            getTask.RunUntilCompletion();

            AssertResult.Matches(getTask.Result, TestModels[1]);
        }

        [Fact]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Id == StorageModels[1].Model.Id);
            getTask.RunUntilCompletion();

            AssertResult.Matches(getTask.Result, StorageModels[1]);
        }

        [Fact]
        public void Gets_empty_result_when_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Id == Guid.NewGuid());
            getTask.RunUntilCompletion();

            getTask.Result.Should().BeNull();
        }

        [Fact]
        public void Gets_empty_result_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Id == Guid.NewGuid());
            getTask.RunUntilCompletion();

            getTask.Result.Should().BeNull();
        }

        [Fact]
        public void Throws_when_querying_for_multiple_records()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Name.StartsWith(TestStorageName));

            var ex = Record.Exception(() => getTask.RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(InvalidOperationException));
        }

        [Fact]
        public void Throws_when_querying_for_multiple_records_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Name.StartsWith(TestStorageName));

            var ex = Record.Exception(() => getTask.RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(InvalidOperationException));
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelQueries.GetOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelQueries.GetOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
