// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAllTestBase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Naos.Database.Domain;
    using Naos.Recipes.RunWithRetry;
    using Xunit;

    public abstract class ProjectAllTestBase
    {
        protected ITestDatabase Database { get; set; }
        protected IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }
        protected IReadOnlyCollection<TestModel> TestModels { get; set; }

        [Fact]
        public void Gets_empty_results_when_no_data_present()
        {
            var getTask = Database.ModelQueries.ProjectAllAsync(m => m.Name);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Gets_empty_results_when_no_data_present_with_custom_metadata()
        {
            var getTask =
                Database.StorageModelQueries.ProjectAllAsync(sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().BeEmpty();
        }

        [Fact]
        public void Gets_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectAllAsync(m => m.Name);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            results.Should().Contain(TestModels.Select(m => m.Name));
        }

        [Fact]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask =
                Database.StorageModelQueries.ProjectAllAsync(sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
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
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelQueries.ProjectAllAsync((Expression<Func<TestModel, ProjectAllTestBase>>)null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelQueries.ProjectAllAsync(
                                (Expression<Func<StorageModel<TestModel, TestMetadata>, ProjectAllTestBase>>)null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
