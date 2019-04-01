// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class GetManyInMemoryDatabaseTest : GetManyTestBase, IDisposable
    {
        public GetManyInMemoryDatabaseTest()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(GetManyInMemoryDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(GetManyInMemoryDatabaseTest), count: 5);
            TestStorageName = nameof(GetManyInMemoryDatabaseTest);
        }

        public void Dispose()
        {
            Database.Drop();
        }
    }
}
