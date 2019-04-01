//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="ProjectAllReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using NUnit.Framework;

//    [TestFixture]
//    public class ProjectAllReadModelDatabaseTest : ProjectAllTestBase
//    {
//        [SetUp]
//        public void Setup()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(ProjectAllReadModelDatabaseTest), count: 5);
//            TestModels = TestModel.CreateMany(nameof(ProjectAllReadModelDatabaseTest), count: 5);
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
//                () => Queries.ProjectAllAsync<TestModel, ProjectAllTestBase>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Queries.ProjectAllAsync<TestModel, TestMetadata, ProjectAllTestBase>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
