// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStream{TId}.CreateStream.cs" company="Naos Project">
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

    public partial class FileStream<TId>
    {
        /// <inheritdoc />
        public override void Execute(
            CreateStreamOp<TId> operation)
        {
            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation<TId>)
                                        ?? throw new ArgumentException(FormattableString.Invariant($"Invalid implementation of {nameof(IStreamRepresentation<TId>)}, expected '{nameof(FileStreamRepresentation<TId>)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
            {
                var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.Name);
                var exists = Directory.Exists(directoryPath);
                if (exists)
                {
                    switch (operation.ExistingStreamEncounteredStrategy)
                    {
                        case ExistingStreamEncounteredStrategy.Overwrite:
                            Directory.Delete(directoryPath, true);
                            Directory.CreateDirectory(directoryPath);
                            break;
                        case ExistingStreamEncounteredStrategy.Skip:
                            /* no-op */
                            break;
                        case ExistingStreamEncounteredStrategy.Throw:
                            throw new InvalidOperationException(FormattableString.Invariant($"Path '{directoryPath}' already exists and {nameof(operation.ExistingStreamEncounteredStrategy)} on the operation is {operation.ExistingStreamEncounteredStrategy}."));
                        default:
                            throw new NotSupportedException(FormattableString.Invariant($"{nameof(operation.ExistingStreamEncounteredStrategy)} value '{operation.ExistingStreamEncounteredStrategy}' is not supported."));
                    }
                }
                else
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }

        /// <inheritdoc />
        public override async Task ExecuteAsync(
            CreateStreamOp<TId> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }
    }
}
