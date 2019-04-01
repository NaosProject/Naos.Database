//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="ProjectManyReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using NUnit.Framework;

//    [TestFixture]
//    public class ProjectManyReadModelDatabaseTest : ProjectManyTestBase
//    {
//        [SetUp]
//        public void Setup()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(ProjectManyReadModelDatabaseTest), count: 5);
//            TestModels = TestModel.CreateMany(nameof(ProjectManyReadModelDatabaseTest), count: 5);
//            TestStorageName = nameof(ProjectManyReadModelDatabaseTest);
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
//                () => Queries.ProjectManyAsync<TestModel, ProjectManyTestBase>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Queries.ProjectManyAsync<TestModel, TestMetadata, ProjectManyTestBase>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
