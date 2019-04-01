// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyTestBase.cs">
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

    public abstract class AddManyTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Fact]
        public void Adds_records_to_database()
        {
            var addTask = Task.Run(() => Database.ModelCommands.AddManyAsync(TestModels));
            addTask.RunUntilCompletion();

            var getTask = Task.Run(() => Database.ModelQueries.GetAllAsync());
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels);
        }

        [Fact]
        public void Adds_record_to_database_with_custom_metadata()
        {
            var addTask = Task.Run(() => Database.StorageModelCommands.AddManyAsync(StorageModels));
            addTask.RunUntilCompletion();

            var getTask = Task.Run(() => Database.StorageModelQueries.GetAllAsync());
            getTask.RunUntilCompletion();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels);
        }

        [Fact]
        public void Throws_when_record_with_same_id_already_present_in_database()
        {
            var addTask = Task.Run(() => Database.ModelCommands.AddManyAsync(TestModels));
            addTask.RunUntilCompletion();

            var ex = Record.Exception(() => Database.ModelCommands.AddManyAsync(TestModels).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(DatabaseException));
        }

        [Fact]
        public void Throws_when_record_with_same_id_already_present_in_database_with_custom_metadata()
        {
            var addTask = Task.Run(() => Database.StorageModelCommands.AddManyAsync(StorageModels));
            addTask.RunUntilCompletion();

            var ex = Record.Exception(() => Database.StorageModelCommands.AddManyAsync(StorageModels).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(DatabaseException));
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelCommands.AddManyAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelCommands.AddManyAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
