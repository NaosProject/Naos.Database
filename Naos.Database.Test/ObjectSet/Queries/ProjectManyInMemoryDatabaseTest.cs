// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class ProjectManyInMemoryDatabaseTest : ProjectManyTestBase, IDisposable
    {
        public ProjectManyInMemoryDatabaseTest()
        {
            this.Database = new TestInMemoryDatabase();
            this.StorageModels = StorageModel.CreateMany(nameof(ProjectManyInMemoryDatabaseTest), count: 5);
            this.TestModels = TestModel.CreateMany(nameof(ProjectManyInMemoryDatabaseTest), count: 5);
            this.TestStorageName = nameof(ProjectManyInMemoryDatabaseTest);
        }

        public void Dispose()
        {
            this.Database.Drop();
        }
    }
}
