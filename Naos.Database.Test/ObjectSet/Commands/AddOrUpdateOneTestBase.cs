// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrUpdateOneTestBase.cs">
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

    public abstract class AddOrUpdateOneTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Fact]
        public void Adds_when_model_does_not_exist_in_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var model = new TestModel("New name", Guid.NewGuid());

            var addOrUpdateTask = Database.ModelCommands.AddOrUpdateOneAsync(model);
            addOrUpdateTask.RunUntilCompletion();

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Id == model.Id);
            getTask.RunUntilCompletion();

            var result = getTask.Result;
            result.Should().NotBeNull();
            result.Name.Should().Be(model.Name);
        }

        [Fact]
        public void Adds_when_model_does_not_exist_in_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var model = new TestModel("New name", Guid.NewGuid());
            var metadata = new TestMetadata("New name");

            var addOrUpdateTask = Database.StorageModelCommands.AddOrUpdateOneAsync(model, metadata);
            addOrUpdateTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Id == model.Id);
            getTask.RunUntilCompletion();

            var result = getTask.Result;
            result.Should().NotBeNull();
            result.Name.Should().Be(model.Name);
        }

        [Fact]
        public void Updates_when_model_exists_in_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var updatedModel = TestModels[1];
            updatedModel.Name = "Updated name";

            var addOrUpdateTask = Database.ModelCommands.AddOrUpdateOneAsync(updatedModel);
            addOrUpdateTask.RunUntilCompletion();

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Id == updatedModel.Id);
            getTask.RunUntilCompletion();

            var result = getTask.Result;
            result.Should().NotBeNull();
            result.Name.Should().Be(updatedModel.Name);
        }

        [Fact]
        public void Updates_when_model_exists_in_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var updatedModel = StorageModels[1];
            updatedModel.Model.Name = "Updated name";
            updatedModel.Metadata.FirstName = "Updated first name";
            updatedModel.Metadata.LastName = "Updated last name";

            var addOrUpdateTask = Database.StorageModelCommands.AddOrUpdateOneAsync(updatedModel.Model, updatedModel.Metadata);
            addOrUpdateTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Id == updatedModel.Model.Id);
            getTask.RunUntilCompletion();

            var result = getTask.Result;
            result.Should().NotBeNull();
            result.Name.Should().Be(updatedModel.Model.Name);
        }

        [Fact]
        public void Does_not_overwrite_other_records()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var updatedModel = TestModels[1];
            updatedModel.Name = "Updated name";

            var addOrUpdateTask = Database.ModelCommands.AddOrUpdateOneAsync(updatedModel);
            addOrUpdateTask.RunUntilCompletion();

            var getTask = Database.ModelQueries.GetManyAsync(m => m.Id != updatedModel.Id);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Where(m => m.Id != updatedModel.Id).ToList());
        }

        [Fact]
        public void Does_not_overwrite_other_records_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var updatedModel = StorageModels[1];
            updatedModel.Model.Name = "Updated name";
            updatedModel.Metadata.FirstName = "Updated first name";
            updatedModel.Metadata.LastName = "Updated last name";

            var addOrUpdateTask = Database.StorageModelCommands.AddOrUpdateOneAsync(updatedModel.Model, updatedModel.Metadata);
            addOrUpdateTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetManyAsync(m => m.Model.Id != updatedModel.Model.Id);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Where(m => m.Model.Id != updatedModel.Model.Id).ToList());
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelCommands.AddOrUpdateOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelCommands.AddOrUpdateOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
