// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStream{TId}.DeleteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    public partial class FileStream<TId>
    {
        /// <inheritdoc />
        public override void Execute(
            DeleteStreamOp<TId> operation)
        {
            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation<TId>)
                                        ?? throw new ArgumentException(Invariant($"Invalid implementation of {nameof(IStreamRepresentation<TId>)}, expected '{nameof(FileStreamRepresentation<TId>)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
            {
                var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.Name);
                var exists = Directory.Exists(directoryPath);
                if (!exists)
                {
                    switch (operation.ExistingStreamNotEncounteredStrategy)
                    {
                        case ExistingStreamNotEncounteredStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {operation.StreamRepresentation} to exist, it does not and the operation {nameof(operation.ExistingStreamNotEncounteredStrategy)} is '{operation.ExistingStreamNotEncounteredStrategy}'."));
                        case ExistingStreamNotEncounteredStrategy.Skip:
                            break;
                    }
                }
                else
                {
                    Directory.Delete(directoryPath, true);
                }
            }
        }

        /// <inheritdoc />
        public override async Task ExecuteAsync(
            DeleteStreamOp<TId> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }
    }
}
