//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="MergeCompleteSetReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using NUnit.Framework;

//    [TestFixture]
//    public class MergeCompleteSetReadModelDatabaseTest : MergeCompleteSetTestBase
//    {
//        [SetUp]
//        public void Setup()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(MergeCompleteSetReadModelDatabaseTest), count: 5);
//            TestModels = TestModel.CreateMany(nameof(MergeCompleteSetReadModelDatabaseTest), count: 5);
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
//                () => Commands.MergeCompleteSetAsync<Guid, TestModel>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Commands.MergeCompleteSetAsync<Guid, TestModel, TestMetadata>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
