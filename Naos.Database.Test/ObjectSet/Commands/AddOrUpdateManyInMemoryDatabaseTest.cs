// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrUpdateManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class AddOrUpdateManyInMemoryDatabaseTest : AddOrUpdateManyTestBase, IDisposable
    {
        public AddOrUpdateManyInMemoryDatabaseTest()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(AddOrUpdateManyInMemoryDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(AddOrUpdateManyInMemoryDatabaseTest), count: 5);
        }

        public void Dispose()
        {
            Database.Drop();
        }
    }
}
