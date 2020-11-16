// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
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
    public partial class FileStreamReadWriteProtocols
        : IStreamReadProtocols,
          IStreamWriteProtocols
    {
        private const string NextUniqueLongTrackingFileName = "_NextUniqueLongTracking.nfo";
        private readonly object nextUniqueLongLock = new object();
        private readonly FileReadWriteStream stream;
        private readonly ISerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamReadWriteProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamReadWriteProtocols(
            FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
        }

        /// <inheritdoc />
        public void Execute(
            CreateStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

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
                            DeleteDirectoryAndConfirm(directoryPath, true);
                            CreateDirectoryAndConfirm(directoryPath);
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
                    CreateDirectoryAndConfirm(directoryPath);
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
            operation.MustForArg(nameof(operation)).NotBeNull();
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = NaosSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var resourceLocator = this.stream.ResourceLocatorProtocols.Execute(new GetResourceLocatorForUniqueIdentifierOp());
            var fileLocator = resourceLocator as FileSystemDatabaseLocator
                           ?? throw new InvalidOperationException(
                                  Invariant(
                                      $"Resource locator was expected to be a {nameof(FileSystemDatabaseLocator)} but was a '{resourceLocator?.GetType()?.ToStringReadable() ?? "<null>"}'."));
            var rootPath = Path.Combine(fileLocator.RootFolderPath, this.stream.Name);
            var trackingFilePath = Path.Combine(rootPath, NextUniqueLongTrackingFileName);

            long nextLong;

            lock (this.nextUniqueLongLock)
            {
                // open the file in locking mode to restrict a single thread changing the list of unique longs index at a time.
                using (var fileStream = new FileStream(
                    trackingFilePath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None))
                {
                    var reader = new StreamReader(fileStream);
                    var currentSerializedListText = reader.ReadToEnd();
                    var currentList = !string.IsNullOrWhiteSpace(currentSerializedListText)
                        ? this.serializer.Deserialize<IList<UniqueLongIssuedEvent>>(currentSerializedListText)
                        : new List<UniqueLongIssuedEvent>();

                    nextLong = currentList.Any()
                        ? currentList.Max(_ => _.Id) + 1
                        : 1;

                    currentList.Add(new UniqueLongIssuedEvent(nextLong, DateTime.UtcNow, operation.Details, operation.Tags));
                    var updatedSerializedListText = this.serializer.SerializeToString(currentList);

                    fileStream.Position = 0;
                    var writer = new StreamWriter(fileStream);
                    writer.Write(updatedSerializedListText);

                    // necessary to flush buffer.
                    writer.Close();
                }
            }

            return nextLong;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            GetNextUniqueLongOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        private static void CreateDirectoryAndConfirm(
            string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
            var timeoutTimeSpan = TimeSpan.FromSeconds(1);
            var timeout = DateTime.UtcNow.Add(timeoutTimeSpan);
            var directoryExists = false;
            while (!directoryExists && DateTime.UtcNow < timeout)
            {
                directoryExists = Directory.Exists(directoryPath);
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }

            if (!directoryExists)
            {
                throw new InvalidOperationException(Invariant($"Directory '{directoryPath}' was created but not found on disk after checking for '{timeoutTimeSpan.TotalSeconds}' seconds."));
            }
        }

        private static void DeleteDirectoryAndConfirm(
            string directoryPath,
            bool recursive)
        {
            Directory.Delete(directoryPath, recursive);
            var timeoutTimeSpan = TimeSpan.FromSeconds(1);
            var timeout = DateTime.UtcNow.Add(timeoutTimeSpan);
            var directoryExists = true;
            while (directoryExists && DateTime.UtcNow < timeout)
            {
                directoryExists = Directory.Exists(directoryPath);
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }

            if (directoryExists)
            {
                throw new InvalidOperationException(Invariant($"Directory '{directoryPath}' was deleted but remains on disk after checking for '{timeoutTimeSpan.TotalSeconds}' seconds."));
            }
        }
    }
}
