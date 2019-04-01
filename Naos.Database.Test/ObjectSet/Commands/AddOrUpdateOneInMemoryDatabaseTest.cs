// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class AddOrUpdateOneInMemoryDatabaseTest : AddOrUpdateOneTestBase, IDisposable
    {
        public AddOrUpdateOneInMemoryDatabaseTest()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(AddOrUpdateOneInMemoryDatabaseTest), count: 3);
            TestModels = TestModel.CreateMany(nameof(AddOrUpdateOneInMemoryDatabaseTest), count: 3);
        }

        public void Dispose()
        {
            Database.Drop();
        }
    }
}
