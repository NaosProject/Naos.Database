// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeCompleteSetInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    public sealed class AddOneInMemoryDatabaseTest : AddOneTestBase, IDisposable
    {
        public AddOneInMemoryDatabaseTest()
        {
            Database = new TestInMemoryDatabase();
            TestModel = new TestModel(nameof(AddOneInMemoryDatabaseTest));
            TestMetadata = new TestMetadata(nameof(AddOneInMemoryDatabaseTest));
        }

        public void Dispose()
        {
            Database.Drop();
        }
    }
}
