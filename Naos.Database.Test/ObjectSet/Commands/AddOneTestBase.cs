// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOneTestBase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Naos.Database.Domain;
    using Naos.Recipes.RunWithRetry;
    using Xunit;

    public abstract class AddOneTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected TestMetadata TestMetadata { get; set; }

        protected TestModel TestModel { get; set; }

        [Fact]
        public void Adds_record_to_database()
        {
            var addTask = Task.Run(() => Database.ModelCommands.AddOneAsync(TestModel));
            addTask.RunUntilCompletion();

            var getTask = Task.Run(() => Database.ModelQueries.GetOneAsync(m => m.Id == TestModel.Id));
            getTask.RunUntilCompletion();

            var result = getTask.Result;
            result.Name.Should().Be(TestModel.Name);
        }

        [Fact]
        public void Adds_record_to_database_with_custom_metadata()
        {
            var addTask = Task.Run(() => Database.StorageModelCommands.AddOneAsync(TestModel, TestMetadata));
            addTask.RunUntilCompletion();

            var getTask = Task.Run(() => Database.StorageModelQueries.GetOneAsync(sm => sm.Model.Id == TestModel.Id));
            getTask.RunUntilCompletion();

            var result = getTask.Result;
            result.Name.Should().Be(TestModel.Name);
        }

        [Fact]
        public void Throws_when_record_with_same_id_already_present_in_database()
        {
            var addTask = Task.Run(() => Database.ModelCommands.AddOneAsync(TestModel));
            addTask.RunUntilCompletion();

            var ex = Record.Exception(() => Database.ModelCommands.AddOneAsync(TestModel).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(DatabaseException));
        }

        [Fact]
        public void Throws_when_record_with_same_id_already_present_in_database_with_custom_metadata()
        {
            var addTask = Task.Run(() => Database.StorageModelCommands.AddOneAsync(TestModel, TestMetadata));
            addTask.RunUntilCompletion();

            var ex = Record.Exception(() => Database.StorageModelCommands.AddOneAsync(TestModel, TestMetadata).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(DatabaseException));
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => Database.ModelCommands.AddOneAsync(null).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }

        [Fact]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var ex = Record.Exception(() => Database.StorageModelCommands.AddOneAsync(null, TestMetadata).RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
