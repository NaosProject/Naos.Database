// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageModel.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System.Collections.Generic;
    using Naos.Database.Domain;

    internal static class StorageModel
    {
        public static IReadOnlyList<StorageModel<TestModel, TestMetadata>> CreateMany(string name, int count = 3)
        {
            var storageModels = new List<StorageModel<TestModel, TestMetadata>>(count);

            for (var i = 0; i < count; i++)
            {
                var numberedName = name + " " + i;

                var storageModel = new StorageModel<TestModel, TestMetadata>
                {
                    Model = new TestModel(numberedName),
                    Metadata = new TestMetadata(numberedName)
                };

                storageModels.Add(storageModel);
            }

            return storageModels;
        }
    }
}
