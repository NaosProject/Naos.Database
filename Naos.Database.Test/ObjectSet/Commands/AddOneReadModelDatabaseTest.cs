//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="AddOneReadModelDatabaseTest.cs">
////     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
////     the project root for full license information.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Naos.Database.Test
//{
//    using System;
//    using NUnit.Framework;
//    using Xunit;

//    [TestFixture]
//    public class AddOneReadModelDatabaseTest : AddOneTestBase
//    {
//        [SetUp]
//        public void Setup()
//        {
//            Database = new TestMongoDatabase();
//            TestModel = new TestModel(nameof(AddOneReadModelDatabaseTest));
//            TestMetadata = new TestMetadata(nameof(AddOneReadModelDatabaseTest));
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
//                () => Commands.AddOneAsync<TestModel>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }

//        [Fact]
//        public void Create_throws_on_invalid_arguments_with_custom_metadata()
//        {
//            Assert.That(
//                () => Commands.AddOneAsync<TestModel, TestMetadata>(null),
//                Throws.TypeOf<ArgumentNullException>());
//        }
//    }
//}
