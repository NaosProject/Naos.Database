// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class UpdateManyInMemoryDatabaseTest : UpdateManyTestBase, IDisposable
    {
        public UpdateManyInMemoryDatabaseTest()
        {
            this.Database = new TestInMemoryDatabase();
            this.StorageModels = StorageModel.CreateMany(nameof(UpdateManyInMemoryDatabaseTest), count: 5);
            this.TestModels = TestModel.CreateMany(nameof(UpdateManyInMemoryDatabaseTest), count: 5);
        }

        public void Dispose()
        {
            this.Database.Drop();
        }
    }
}
