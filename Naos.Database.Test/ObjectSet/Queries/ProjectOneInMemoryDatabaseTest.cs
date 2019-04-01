// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectOneInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class ProjectOneInMemoryDatabaseTest : ProjectOneTestBase, IDisposable
    {
        public ProjectOneInMemoryDatabaseTest()
        {
            this.Database = new TestInMemoryDatabase();
            this.StorageModels = StorageModel.CreateMany(nameof(ProjectOneInMemoryDatabaseTest), count: 3);
            this.TestModels = TestModel.CreateMany(nameof(ProjectOneInMemoryDatabaseTest), count: 3);
            this.TestStorageName = nameof(ProjectOneInMemoryDatabaseTest);
        }

        public void Dispose()
        {
            this.Database.Drop();
        }
    }
}
