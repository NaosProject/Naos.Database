//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="RemoveOneReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using System.Linq;
//    using FluentAssertions;
//    using Xunit;

//    public class RemoveOneReadModelDatabaseTest : RemoveOneTestBase, IDisposable
//    {
//        public RemoveOneReadModelDatabaseTest()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(RemoveOneReadModelDatabaseTest), count: 5);
//            TestModels = TestModel.CreateMany(nameof(RemoveOneReadModelDatabaseTest), count: 5);
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
//                () => Commands.RemoveOneAsync<TestModel>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
