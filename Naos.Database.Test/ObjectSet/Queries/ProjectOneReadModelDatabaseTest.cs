//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="ProjectOneReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using NUnit.Framework;

//    [TestFixture]
//    public class ProjectOneReadModelDatabaseTest : ProjectOneTestBase
//    {
//        [SetUp]
//        public void Setup()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(ProjectOneReadModelDatabaseTest), count: 3);
//            TestModels = TestModel.CreateMany(nameof(ProjectOneReadModelDatabaseTest), count: 3);
//            TestStorageName = nameof(ProjectOneReadModelDatabaseTest);
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
//                () => Queries.ProjectOneAsync<TestModel, ProjectOneTestBase>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Queries.ProjectOneAsync<TestModel, TestMetadata, ProjectOneTestBase>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
