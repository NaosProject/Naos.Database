// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateOneInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class UpdateOneInMemoryDatabaseTest : UpdateOneTestBase, IDisposable
    {
        public UpdateOneInMemoryDatabaseTest()
        {
            this.Database = new TestInMemoryDatabase();
            this.StorageModels = StorageModel.CreateMany(nameof(UpdateOneInMemoryDatabaseTest), count: 3);
            this.TestModels = TestModel.CreateMany(nameof(UpdateOneInMemoryDatabaseTest), count: 3);
        }

        public void Dispose()
        {
            this.Database.Drop();
        }
    }
}
