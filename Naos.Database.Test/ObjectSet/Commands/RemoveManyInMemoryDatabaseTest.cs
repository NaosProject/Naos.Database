// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class RemoveManyInMemoryDatabaseTest : RemoveManyTestBase, IDisposable
    {
        public RemoveManyInMemoryDatabaseTest()
        {
            this.Database = new TestInMemoryDatabase();
            this.StorageModels = StorageModel.CreateMany(nameof(RemoveManyInMemoryDatabaseTest), count: 5);
            this.TestModels = TestModel.CreateMany(nameof(RemoveManyInMemoryDatabaseTest), count: 5);
            this.TestStorageName = nameof(RemoveManyInMemoryDatabaseTest);
        }

        public void Dispose()
        {
            Database.Drop();
        }
    }
}
