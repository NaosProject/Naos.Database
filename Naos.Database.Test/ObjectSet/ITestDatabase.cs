// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITestDatabase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;
    using System.Collections.Generic;
    using Naos.Database.Domain;

    public interface ITestDatabase
    {
        IQueries<TestModel> ModelQueries { get; }

        IQueries<TestModel, TestMetadata> StorageModelQueries { get; }

        ICommands<Guid, TestModel> ModelCommands { get; }

        ICommands<Guid, TestModel, TestMetadata> StorageModelCommands { get; }

        void Drop();

        void AddTestModelsToDatabase(IReadOnlyCollection<TestModel> testModels);

        void AddStorageModelsToDatabase(IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> storageModels);
    }
}
