// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveAllInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Naos.Recipes.RunWithRetry;
    using Xunit;

    public sealed class RemoveAllInMemoryDatabaseTest : RemoveAllTestBase, IDisposable
    {
        public RemoveAllInMemoryDatabaseTest()
        {
            this.Database = new TestInMemoryDatabase();
            this.StorageModels = StorageModel.CreateMany(nameof(RemoveAllInMemoryDatabaseTest), count: 5);
            this.TestModels = TestModel.CreateMany(nameof(RemoveAllInMemoryDatabaseTest), count: 5);
        }

        public void Dispose()
        {
            this.Database.Drop();
        }

        [Fact]
        public void Throws_on_invalid_arguments()
        {
            var ex = Record.Exception(() => ((TestInMemoryDatabase)this.Database).Database.RemoveAllCommandAsync(" ").RunUntilCompletion());

            ex.Should().NotBeNull();
            ex.Should().BeOfType<AggregateException>();
            var aggEx = ex as AggregateException;
            aggEx.Should().NotBeNull();
            aggEx.InnerExceptions.Select(_ => _.GetType()).Should().Contain(typeof(ArgumentNullException));
        }
    }
}
