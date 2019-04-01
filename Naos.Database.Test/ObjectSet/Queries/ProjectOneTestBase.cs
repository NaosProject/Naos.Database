// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectOneTestBase.cs">
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

    public abstract class ProjectOneTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        protected string TestStorageName { get; set; }

        [Fact]
        public void Gets_expected_result_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectOneAsync(m => m.Id == TestModels[1].Id, m => m.Name);
            var result = getTask.RunUntilCompletion();

            result.Should().Be(TestModels[1].Name);
        }

        [Fact]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectOneAsync(
                sm => sm.Model.Id == StorageModels[1].Model.Id,
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            var result = getTask.RunUntilCompletion();
            result.Name.Should().Be(StorageModels[1].Model.Name);
            result.FirstName.Should().Be(StorageModels[1].Metadata.FirstName);
            result.LastName.Should().Be(StorageModels[1].Metadata.LastName);
        }

        [Fact]
        public void Gets_empty_result_when_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectOneAsync(m => m.Id == Guid.NewGuid(), m => m.Name);
            var result = getTask.RunUntilCompletion();

            result.Should().BeNull();
        }

        [Fact]
        public void Gets_empty_result_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectOneAsync(
                sm => sm.Model.Id == Guid.NewGuid(),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            var result = getTask.RunUntilCompletion();

            result.Should().BeNull();
        }

        [Fact]
        public void Throws_when_querying_for_multiple_records()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectOneAsync(m => m.Name.StartsWith(TestStorageName), m => m.Name);

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

            var getTask = Database.StorageModelQueries.ProjectOneAsync(
                sm => sm.Model.Name.StartsWith(TestStorageName),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });


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
            var ex1 = Record.Exception(() => Database.ModelQueries.ProjectOneAsync(null, m => m.Name).RunUntilCompletion());

            ex1.Should().NotBeNull();
            ex1.Should().BeOfType<AggregateException>();
            var aggEx1 = ex1 as AggregateException;
            aggEx1.Should().NotBeNull();
            aggEx1.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));


            var ex2 = Record.Exception(() => Database.ModelQueries.ProjectOneAsync<ProjectManyTestBase>(m => true, null).RunUntilCompletion());

            ex2.Should().NotBeNull();
            ex2.Should().BeOfType<AggregateException>();
            var aggEx2 = ex2 as AggregateException;
            aggEx2.Should().NotBeNull();
            aggEx2.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex1 = Record.Exception(() => Database.StorageModelQueries.ProjectOneAsync(null, m => m.Model.Name).RunUntilCompletion());

            ex1.Should().NotBeNull();
            ex1.Should().BeOfType<AggregateException>();
            var aggEx1 = ex1 as AggregateException;
            aggEx1.Should().NotBeNull();
            aggEx1.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));

            var ex2 = Record.Exception(() => Database.StorageModelQueries.ProjectOneAsync<ProjectManyTestBase>(m => true, null).RunUntilCompletion());

            ex2.Should().NotBeNull();
            ex2.Should().BeOfType<AggregateException>();
            var aggEx2 = ex2 as AggregateException;
            aggEx2.Should().NotBeNull();
            aggEx2.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
