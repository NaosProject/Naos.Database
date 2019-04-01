// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectManyTestBase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Naos.Database.Domain;
    using Naos.Recipes.RunWithRetry;
    using Xunit;

    public abstract class ProjectManyTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        protected string TestStorageName { get; set; }

        [Fact]
        public void Gets_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectManyAsync(m => m.Name.StartsWith(TestStorageName), m => m.Name);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().Contain(TestModels.Select(m => m.Name));
        }

        [Fact]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectManyAsync(
                m => m.Model.Name.StartsWith(TestStorageName),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            foreach (var storageModel in StorageModels)
            {
                var result = results.Single(m => m.Name == storageModel.Model.Name);
                result.FirstName.Should().Be(storageModel.Metadata.FirstName);
                result.LastName.Should().Be(storageModel.Metadata.LastName);
            }
        }

        [Fact]
        public void Gets_expected_subset_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var ids = TestModels.Skip(1).Take(3).Select(m => m.Id);
            var getTask = Database.ModelQueries.ProjectManyAsync(m => ids.Contains(m.Id), m => m.Name);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().Contain(TestModels.Skip(1).Take(3).Select(m => m.Name));
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling",
            Justification = "Despite the class coupling this code isn't overly complex.")]
        [Fact]
        public void Gets_expected_subset_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var ids = StorageModels.Skip(1).Take(3).Select(m => m.Model.Id);
            var getTask = Database.StorageModelQueries.ProjectManyAsync(
                m => ids.Contains(m.Model.Id),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            foreach (var storageModel in StorageModels.Skip(1).Take(3))
            {
                var result = results.Single(m => m.Name == storageModel.Model.Name);
                result.FirstName.Should().Be(storageModel.Metadata.FirstName);
                result.LastName.Should().Be(storageModel.Metadata.LastName);
            }
        }

        [Fact]
        public void Gets_empty_results_with_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectManyAsync(m => m.Id == Guid.NewGuid(), m => m);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Gets_empty_results_with_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectManyAsync(m => m.Model.Id == Guid.NewGuid(), m => m);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex1 = Record.Exception(() => Database.ModelQueries.ProjectManyAsync(null, m => m.Name).RunUntilCompletion());

            ex1.Should().NotBeNull();
            ex1.Should().BeOfType<AggregateException>();
            var aggEx1 = ex1 as AggregateException;
            aggEx1.Should().NotBeNull();
            aggEx1.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));


            var ex2 = Record.Exception(() => Database.ModelQueries.ProjectManyAsync<ProjectManyTestBase>(m => true, null).RunUntilCompletion());

            ex2.Should().NotBeNull();
            ex2.Should().BeOfType<AggregateException>();
            var aggEx2 = ex2 as AggregateException;
            aggEx2.Should().NotBeNull();
            aggEx2.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex1 = Record.Exception(() => Database.StorageModelQueries.ProjectManyAsync(null, m => m.Model.Name).RunUntilCompletion());

            ex1.Should().NotBeNull();
            ex1.Should().BeOfType<AggregateException>();
            var aggEx1 = ex1 as AggregateException;
            aggEx1.Should().NotBeNull();
            aggEx1.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));

            var ex2 = Record.Exception(() => Database.StorageModelQueries.ProjectManyAsync<ProjectManyTestBase>(m => true, null).RunUntilCompletion());

            ex2.Should().NotBeNull();
            ex2.Should().BeOfType<AggregateException>();
            var aggEx2 = ex2 as AggregateException;
            aggEx2.Should().NotBeNull();
            aggEx2.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(InvalidOperationException));
        }
    }
}
