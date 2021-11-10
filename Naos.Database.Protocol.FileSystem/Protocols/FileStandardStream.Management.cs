// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStandardStream.Management.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.IO;
    using System.Threading;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    public partial class FileStandardStream
    {
        /// <inheritdoc />
        public override CreateStreamResult Execute(
            StandardCreateStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation)
                                        ?? throw new ArgumentException(FormattableString.Invariant($"Invalid implementation of {nameof(IStreamRepresentation)}, expected '{nameof(FileStreamRepresentation)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            var alreadyExists = false;
            var wasCreated = true;
            lock (this.fileLock)
            {
                foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
                {
                    var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.Name);
                    var exists = Directory.Exists(directoryPath);
                    if (exists)
                    {
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse - this is not true as it's iterating the potential locators...
                        alreadyExists = alreadyExists || exists;
                        switch (operation.ExistingStreamStrategy)
                        {
                            case ExistingStreamStrategy.Overwrite:
                                DeleteDirectoryAndConfirm(directoryPath, true);
                                CreateDirectoryAndConfirm(directoryPath);
                                break;
                            case ExistingStreamStrategy.Skip:
                                wasCreated = false;
                                break;
                            case ExistingStreamStrategy.Throw:
                                throw new InvalidOperationException(
                                    FormattableString.Invariant(
                                        $"Path '{directoryPath}' already exists and {nameof(operation.ExistingStreamStrategy)} on the operation is {operation.ExistingStreamStrategy}."));
                            default:
                                throw new NotSupportedException(
                                    FormattableString.Invariant(
                                        $"{nameof(operation.ExistingStreamStrategy)} value '{operation.ExistingStreamStrategy}' is not supported."));
                        }
                    }
                    else
                    {
                        CreateDirectoryAndConfirm(directoryPath);
                    }
                }
            }

            var result = new CreateStreamResult(alreadyExists, wasCreated);
            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            StandardDeleteStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var fileStreamRepresentation = (operation.StreamRepresentation as FileStreamRepresentation)
                                        ?? throw new ArgumentException(Invariant($"Invalid implementation of {nameof(IStreamRepresentation)}, expected '{nameof(FileStreamRepresentation)}' but was '{operation.StreamRepresentation.GetType().ToStringReadable()}'."));

            lock (this.fileLock)
            {
                foreach (var fileSystemDatabaseLocator in fileStreamRepresentation.FileSystemDatabaseLocators)
                {
                    var directoryPath = Path.Combine(fileSystemDatabaseLocator.RootFolderPath, this.Name);
                    var exists = Directory.Exists(directoryPath);
                    if (!exists)
                    {
                        switch (operation.StreamNotFoundStrategy)
                        {
                            case StreamNotFoundStrategy.Throw:
                                throw new InvalidOperationException(
                                    Invariant(
                                        $"Expected stream {operation.StreamRepresentation} to exist, it does not and the operation {nameof(operation.StreamNotFoundStrategy)} is '{operation.StreamNotFoundStrategy}'."));
                            case StreamNotFoundStrategy.Skip:
                                break;
                        }
                    }
                    else
                    {
                        Directory.Delete(directoryPath, true);
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Execute(
            StandardPruneStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var locator = operation.GetSpecifiedLocatorConverted<FileSystemDatabaseLocator>() ?? this.TryGetSingleLocator();
            var rootPath = this.GetRootPathFromLocator(locator);

            lock (this.fileLock)
            {
                var filesToPotentiallyRemove
                    = Directory.GetFiles(
                        rootPath,
                        Invariant($"*___*"),
                        SearchOption.TopDirectoryOnly);
                foreach (var fileToConsiderRemoving in filesToPotentiallyRemove)
                {
                    var internalRecordId = GetInternalRecordIdFromRecordFilePath(fileToConsiderRemoving);
                    var internalRecordDate = this.GetRootDateFromFilePath(fileToConsiderRemoving);
                    if (operation.ShouldPrune(internalRecordId, internalRecordDate))
                    {
                        File.Delete(fileToConsiderRemoving);
                    }
                }

                var concerns = Directory.GetDirectories(rootPath);
                foreach (var concern in concerns)
                {
                    var concernDirectory = this.GetHandlingConcernDirectory(locator, concern);
                    var handlingFilesToConsiderRemoving
                        = Directory.GetFiles(
                            concernDirectory,
                            Invariant($"*___*"),
                            SearchOption.TopDirectoryOnly);

                    foreach (var fileToConsiderRemoving in handlingFilesToConsiderRemoving)
                    {
                        var internalRecordId = GetInternalRecordIdFromEntryFilePath(fileToConsiderRemoving);
                        var internalEntryDate = this.GetRootDateFromFilePath(fileToConsiderRemoving);
                        if (operation.ShouldPrune(internalRecordId, internalEntryDate))
                        {
                            File.Delete(fileToConsiderRemoving);
                        }
                    }
                }
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

        private DateTime GetRootDateFromFilePath(
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is null or whitespace.", nameof(filePath));
            }

            var internalRecordDateString = Path.GetFileName(filePath)
                ?.Split(
                    new[]
                    {
                        "___",
                    },
                    StringSplitOptions.RemoveEmptyEntries)[1];

            if (string.IsNullOrWhiteSpace(internalRecordDateString))
            {
                throw new InvalidOperationException(Invariant($"Failed to extract internal record id from file path: '{filePath}'."));
            }

            var prepped = internalRecordDateString.Replace("--", ":");
            var result = this.dateTimeStringSerializer.Deserialize<DateTime>(prepped);
            return result;
        }
    }
}