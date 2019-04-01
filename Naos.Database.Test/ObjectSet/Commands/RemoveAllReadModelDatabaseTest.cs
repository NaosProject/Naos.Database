//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="RemoveAllReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using System.Threading.Tasks;
//    using NUnit.Framework;

//    [TestFixture]
//    public class RemoveAllReadModelDatabaseTest : RemoveAllTestBase
//    {
//        [SetUp]
//        public void Setup()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(RemoveAllReadModelDatabaseTest), count: 5);
//            TestModels = TestModel.CreateMany(nameof(RemoveAllReadModelDatabaseTest), count: 5);
//        }

//        [TearDown]
//        public void CleanUp()
//        {
//            Database.Drop();
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments()
//        {
//            Assert.That(
//                () => Commands.RemoveAllAsync(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_model_overload()
//        {
//            Assert.That(
//                () => Commands.RemoveAllAsync<TestModel>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Throws_on_invalid_arguments()
//        {
//            Assert.That(
//                () => Task.Run(() => ((TestMongoDatabase)Database).RemoveAll(" ")).RunUntilCompletion(),
//                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
//        }
//    }
//}
