// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IStreamReadProtocols{TId,TObject}"/>
    /// and <see cref="IStreamWriteProtocols{TId,TObject}"/>.
    /// </summary>
    public partial class FileStreamProtocols
        : IStreamReadProtocols,
          IStreamWriteProtocols
    {
        private readonly FileReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamProtocols(
            FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public void Execute(
            CreateStreamOp operation)
        {
            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation)
                                        ?? throw new ArgumentException(FormattableString.Invariant($"Invalid implementation of {nameof(IStreamRepresentation)}, expected '{nameof(FileStreamRepresentation)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
            {
                var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.stream.Name);
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
        public async Task ExecuteAsync(
            CreateStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }

        /// <inheritdoc />
        public void Execute(
            DeleteStreamOp operation)
        {
            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation)
                                        ?? throw new ArgumentException(Invariant($"Invalid implementation of {nameof(IStreamRepresentation)}, expected '{nameof(FileStreamRepresentation)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
            {
                var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.stream.Name);
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
        public async Task ExecuteAsync(
            DeleteStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            GetNextUniqueLongOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
