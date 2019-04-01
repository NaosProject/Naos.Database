// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestInMemoryDatabase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Recipes.RunWithRetry;

    public sealed class TestInMemoryDatabase : ITestDatabase
    {
        public InMemoryDatabase Database { get; private set; } = new InMemoryDatabase();

        public IQueries<TestModel> ModelQueries { get; }

        public IQueries<TestModel, TestMetadata> StorageModelQueries { get; }

        public ICommands<Guid, TestModel> ModelCommands { get; }

        public ICommands<Guid, TestModel, TestMetadata> StorageModelCommands { get; }

        public TestInMemoryDatabase()
        {
            ModelQueries = Database.GetQueriesInterface<TestModel>();
            StorageModelQueries = Database.GetQueriesInterface<TestModel, TestMetadata>();
            ModelCommands = Database.GetCommandsInterface<Guid, TestModel>();
            StorageModelCommands = Database.GetCommandsInterface<Guid, TestModel, TestMetadata>();
        }

        public void Drop()
        {
            Database = new InMemoryDatabase();
        }

        public void AddTestModelsToDatabase(IReadOnlyCollection<TestModel> testModels)
        {
            var addTask = Task.Run(() => Database.AddManyCommandAsync(testModels, "TestModel", default(CancellationToken)));
            addTask.RunUntilCompletion();
        }

        public void AddStorageModelsToDatabase(IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> storageModels)
        {
            var addTask = Task.Run(() => Database.AddManyCommandAsync(storageModels, "TestModel", default(CancellationToken)));
            addTask.RunUntilCompletion();
        }
    }
}
