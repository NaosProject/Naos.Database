// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoDatabaseManager.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Mongo
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Naos.Database.Domain;
    using Naos.FileJanitor.Domain;

    using OBeautifulCode.Validation.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Documenter for database objects.
    /// </summary>
    public static class MongoDatabaseManager
    {
        /// <summary>
        /// Perform a database backup.
        /// </summary>
        /// <param name="connectionDefinition">Connection definition to the intended database server.</param>
        /// <param name="databaseName">Name of the database to backup.</param>
        /// <param name="backupDetails">The details of how to perform the backup.</param>
        /// <param name="workingDirectory">Working path to take temporary output.</param>
        /// <param name="mongoUtilityDirectory">Path where mongo utilies are located on disk.</param>
        /// <param name="announcer">Optional announcer to log messages about what is happening.</param>
        /// <returns>Archived directory created.</returns>
        public static async Task<ArchivedDirectory> BackupFullAsync(
            ConnectionDefinition connectionDefinition,
            string databaseName,
            BackupDetails backupDetails,
            string workingDirectory,
            string mongoUtilityDirectory,
            Action<Func<object>> announcer = null)
        {
            new { connectionDefinition }.Must().NotBeNull();
            new { databaseName }.Must().NotBeNullNorWhiteSpace();
            new { backupDetails }.Must().NotBeNull();

            backupDetails.ThrowIfInvalid();

            void NullAnnounce(Func<object> announcement)
            {
                /* no-op */
            }

            var localAnnouncer = announcer ?? NullAnnounce;

            localAnnouncer(() => new { Database = databaseName, BackupDetails = backupDetails });
            var backupToPath = BuildIndividualWorkingPath(workingDirectory, "Backup");
            var backupFilePath = backupDetails.BackupTo.LocalPath;

            var directoryArchiveKind = DirectoryArchiveKind.DotNetZipFile;
            var archiveCompressionKind = ArchiveCompressionKind.Fastest;
            var archiver = ArchiverFactory.Instance.BuildArchiver(directoryArchiveKind, archiveCompressionKind);

            var exePath = Path.Combine(mongoUtilityDirectory, "mongodump.exe");
            localAnnouncer(() => Invariant($"Backing up database '{databaseName}' to '{backupToPath}' using '{exePath}'"));
            var output = RunProcess(exePath, Invariant($"--host {connectionDefinition.Server} --db {databaseName} --authenticationDatabase {connectionDefinition.AuthenticationSource ?? databaseName} --username {connectionDefinition.UserName} --password {connectionDefinition.Password} --out {backupToPath}"));
            localAnnouncer(() => output);

            localAnnouncer(() => Invariant($"Creating backup file '{backupFilePath}' from '{backupToPath}'"));
            var archivedDirectory = await archiver.ArchiveDirectoryAsync(backupToPath, backupFilePath, false, Encoding.UTF8);
            new { archivedDirectory }.Must().NotBeNull();

            localAnnouncer(() => Invariant($"Cleaning up by removing temp directory '{backupToPath}'"));
            Directory.Delete(backupToPath, true);

            localAnnouncer(() => "Completed successfully.");

            return archivedDirectory;
        }

        /// <summary>
        /// Restores an entire database from a full database backup.
        /// </summary>
        /// <param name="connectionDefinition">Connection definition to the intended database server.</param>
        /// <param name="databaseName">Name of the database to restore.</param>
        /// <param name="restoreDetails">The details of how to perform the restore.</param>
        /// <param name="workingDirectory">Working path to take temporary output.</param>
        /// <param name="mongoUtilityDirectory">Path where mongo utilies are located on disk.</param>
        /// <param name="announcer">Optional announcer to log messages about what is happening.</param>
        /// <returns>Task to support async await calling.</returns>
        public static async Task RestoreFullAsync(
            ConnectionDefinition connectionDefinition,
            string databaseName,
            RestoreDetails restoreDetails,
            string workingDirectory,
            string mongoUtilityDirectory,
            Action<Func<object>> announcer = null)
        {
            new { connectionDefinition }.Must().NotBeNull();
            new { databaseName }.Must().NotBeNullNorWhiteSpace();
            new { restoreDetails }.Must().NotBeNull();

            restoreDetails.ThrowIfInvalid();

            void NullAnnounce(Func<object> announcement)
            {
                /* no-op */
            }

            var localAnnouncer = announcer ?? NullAnnounce;

            localAnnouncer(() => new { Database = databaseName, RestoreDetails = restoreDetails });
            var backupFilePath = restoreDetails.RestoreFrom.LocalPath;
            var inflatedBackupFilePath = BuildIndividualWorkingPath(workingDirectory, "Restore");
            var restoreFromPath = Path.Combine(inflatedBackupFilePath, databaseName); // will put in a sub folder by database name that restore does not understand so just exclude

            var directoryArchiveKind = DirectoryArchiveKind.DotNetZipFile;
            var archiveCompressionKind = ArchiveCompressionKind.Fastest;
            var archiver = ArchiverFactory.Instance.BuildArchiver(directoryArchiveKind, archiveCompressionKind);
            var archivedDirectory = new ArchivedDirectory(directoryArchiveKind, archiveCompressionKind, backupFilePath, false, Encoding.UTF8);

            localAnnouncer(() => Invariant($"Inflating backup file '{backupFilePath}' to '{inflatedBackupFilePath}'"));
            await archiver.RestoreDirectoryAsync(archivedDirectory, inflatedBackupFilePath);

            var exePath = Path.Combine(mongoUtilityDirectory, "mongorestore.exe");
            localAnnouncer(() => Invariant($"Restoring database '{databaseName}' from '{inflatedBackupFilePath}' using '{exePath}'"));

            var dropSwitchAddIn = restoreDetails.ReplaceOption == ReplaceOption.ReplaceExistingDatabase ? "--drop " : string.Empty;
            var output = RunProcess(exePath, Invariant($"{dropSwitchAddIn}--host {connectionDefinition.Server} --db {databaseName} --authenticationDatabase {connectionDefinition.AuthenticationSource ?? databaseName} --username {connectionDefinition.UserName} --password {connectionDefinition.Password} {restoreFromPath}"));
            localAnnouncer(() => output);

            localAnnouncer(() => Invariant($"Cleaning up by removing temp directory '{inflatedBackupFilePath}'"));
            Directory.Delete(inflatedBackupFilePath, true);

            localAnnouncer(() => "Completed successfully.");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Is being disposed.")]
        private static string RunProcess(string exePath, string arguments)
        {
            var process = new Process
                              {
                                  StartInfo = new ProcessStartInfo
                                                  {
                                                      FileName = exePath,
                                                      Arguments = arguments,
                                                      UseShellExecute = false,
                                                      RedirectStandardOutput = true,
                                                      RedirectStandardError = true,
                                                      CreateNoWindow = true,
                                                      ErrorDialog = false,
                                                  },
                              };

            using (process)
            {
                if (!process.Start())
                {
                    throw new InvalidOperationException(Invariant($"{exePath} could not be started."));
                }

                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException(Invariant($"{exePath} reported an error: {error}"));
                }

                return output;
            }
        }

        private static string BuildIndividualWorkingPath(string workingDirectory, string prefix)
        {
            var datePart = DateTime.UtcNow.ToString("u", CultureInfo.CurrentCulture).Replace("-", string.Empty).Replace(":", string.Empty).Replace(" ", string.Empty);

            var directoryName = Invariant($"{prefix}{datePart}");

            var individualWorkingPath = Path.Combine(workingDirectory, directoryName);

            return individualWorkingPath;
        }
    }
}