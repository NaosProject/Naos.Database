// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetOneInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class GetOneInMemoryDatabaseTest : GetOneTestBase, IDisposable
    {
        public GetOneInMemoryDatabaseTest()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(GetOneInMemoryDatabaseTest), count: 3);
            TestModels = TestModel.CreateMany(nameof(GetOneInMemoryDatabaseTest), count: 3);
            TestStorageName = nameof(GetOneInMemoryDatabaseTest);
        }

        public void Dispose()
        {
            Database.Drop();
        }
    }
}
