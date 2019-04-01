// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class AddManyInMemoryDatabaseTest : AddManyTestBase, IDisposable
    {
        public AddManyInMemoryDatabaseTest()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(AddManyInMemoryDatabaseTest), count: 3);
            TestModels = TestModel.CreateMany(nameof(AddManyInMemoryDatabaseTest), count: 3);
        }
        
        public void Dispose()
        {
            Database.Drop();
        }
    }
}
