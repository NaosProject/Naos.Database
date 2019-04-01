// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class GetAllInMemoryDatabaseTest : GetAllTestBase, IDisposable
    {
        public GetAllInMemoryDatabaseTest()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(GetAllInMemoryDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(GetAllInMemoryDatabaseTest), count: 5);
        }

        public void Dispose()
        {
            Database.Drop();
        }
    }
}
