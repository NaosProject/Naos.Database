//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="GetOneReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;

//    public class GetOneReadModelDatabaseTest : GetOneTestBase, IDisposable
//    {
//        public GetOneReadModelDatabaseTest()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(GetOneReadModelDatabaseTest), count: 3);
//            TestModels = TestModel.CreateMany(nameof(GetOneReadModelDatabaseTest), count: 3);
//            TestStorageName = nameof(GetOneReadModelDatabaseTest);
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
//                () => Queries.GetOneAsync<TestModel>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Queries.GetOneAsync<TestModel, TestMetadata>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
