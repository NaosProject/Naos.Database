// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAllInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class ProjectAllInMemoryDatabaseTest : ProjectAllTestBase, IDisposable
    {
        public ProjectAllInMemoryDatabaseTest()
        {
            this.Database = new TestInMemoryDatabase();
            this.StorageModels = StorageModel.CreateMany(nameof(ProjectAllInMemoryDatabaseTest), count: 5);
            this.TestModels = TestModel.CreateMany(nameof(ProjectAllInMemoryDatabaseTest), count: 5);
        }

        public void Dispose()
        {
            Database.Drop();
        }
    }
}
