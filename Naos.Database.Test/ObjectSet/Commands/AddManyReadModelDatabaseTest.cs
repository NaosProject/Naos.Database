//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="AddManyReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using System.Linq;
//    using FluentAssertions;
//    using Naos.Recipes.RunWithRetry;
//    using Xunit;

//    public class AddManyReadModelDatabaseTest : AddManyTestBase, IDisposable
//    {
//        public AddManyReadModelDatabaseTest()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(AddManyReadModelDatabaseTest), count: 3);
//            TestModels = TestModel.CreateMany(nameof(AddManyReadModelDatabaseTest), count: 3);
//        }

//        public void Dispose()
//        {
//            Database.Drop();
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments()
//        {

//            var ex = Record.Exception(() => .RunUntilCompletion());

//            ex.Should().NotBeNull();
//            ex.Should().BeOfType<AggregateException>();
//            var aggEx = ex as AggregateException;
//            aggEx.Should().NotBeNull();
//            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof());

//            Assert.That(
//                () => Commands.AddManyAsync<TestModel>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Commands.AddManyAsync<TestModel, TestMetadata>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
