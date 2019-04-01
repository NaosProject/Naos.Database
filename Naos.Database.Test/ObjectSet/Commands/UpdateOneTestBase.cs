// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateOneTestBase.cs">
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

    public abstract class UpdateOneTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Fact]
        public void Updates_when_model_exists_in_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var updatedModel = TestModels[1];
            updatedModel.Name = "Updated name";

            var updateTask = Database.ModelCommands.UpdateOneAsync(updatedModel);
            updateTask.RunUntilCompletion();

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

            var updateTask = Database.StorageModelCommands.UpdateOneAsync(updatedModel.Model, updatedModel.Metadata);
            updateTask.RunUntilCompletion();

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

            var updateTask = Database.ModelCommands.UpdateOneAsync(updatedModel);
            updateTask.RunUntilCompletion();

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

            var updateTask = Database.StorageModelCommands.UpdateOneAsync(updatedModel.Model, updatedModel.Metadata);
            updateTask.RunUntilCompletion();

            var getTask = Database.StorageModelQueries.GetManyAsync(m => m.Model.Id != updatedModel.Model.Id);
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Where(m => m.Model.Id != updatedModel.Model.Id).ToList());
        }

        [Fact]
        public void Throws_when_model_does_not_exist_in_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var updatedModel = new TestModel("Updated name", Guid.NewGuid());

            var ex = Record.Exception(() => Database.ModelCommands.UpdateOneAsync(updatedModel).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(DatabaseException));
        }

        [Fact]
        public void Throws_when_model_does_not_exist_in_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var updatedModel = new TestModel("Updated name", Guid.NewGuid());
            var updatedMetadata = new TestMetadata("Updated name");

            var ex = Record.Exception(() => Database.StorageModelCommands.UpdateOneAsync(updatedModel, updatedMetadata).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(DatabaseException));
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelCommands.UpdateOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelCommands.UpdateOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
