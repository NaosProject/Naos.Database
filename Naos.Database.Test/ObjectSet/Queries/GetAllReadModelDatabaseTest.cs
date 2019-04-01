//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="GetAllReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;

//    public class GetAllReadModelDatabaseTest : GetAllTestBase, IDisposable
//    {
//        public GetAllReadModelDatabaseTest()
//        {
//            Database = new TestMongoDatabase();
//            StorageModels = StorageModel.CreateMany(nameof(GetAllReadModelDatabaseTest), count: 5);
//            TestModels = TestModel.CreateMany(nameof(GetAllReadModelDatabaseTest), count: 5);
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
//                () => Queries.GetAllAsync<TestModel>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Queries.GetAllAsync<TestModel, TestMetadata>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
