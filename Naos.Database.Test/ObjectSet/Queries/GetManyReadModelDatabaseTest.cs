//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="GetManyReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using NUnit.Framework;

//    [TestFixture]
//    public class GetManyReadModelDatabaseTest : GetManyTestBase
//    {
//        [SetUp]
//        public void Setup()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(GetManyReadModelDatabaseTest), count: 5);
//            TestModels = TestModel.CreateMany(nameof(GetManyReadModelDatabaseTest), count: 5);
//            TestStorageName = nameof(GetManyReadModelDatabaseTest);
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
//                () => Queries.GetManyAsync<TestModel>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Queries.GetManyAsync<TestModel, TestMetadata>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
