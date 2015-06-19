// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupMessageHandler.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handlers
{
    using System;
    using System.IO;

    using Its.Configuration;

    using Naos.Database.MessageBus.Contract;
    using Naos.Database.Tools;
    using Naos.Database.Tools.Backup;
    using Naos.FileJanitor.MessageBus.Contract;
    using Naos.MessageBus.HandlingContract;

    /// <summary>
    /// Naos.MessageBus handler for BackupMessages.
    /// </summary>
    public class BackupMessageHandler : IHandleMessages<BackupDatabaseMessage>, IShareFilePath
    {
        /// <inheritdoc />
        public void Handle(BackupDatabaseMessage message)
        {
            Action<string> logAction = Console.WriteLine;
            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            this.Handle(message, settings, logAction);
        }

        /// <summary>
        /// Handles a BackupDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        /// <param name="logAction">Action for logging notifications.</param>
        public void Handle(BackupDatabaseMessage message, DatabaseMessageHandlerSettings settings, Action<string> logAction)
        {
            // must have a date that is strictly alphanumeric...
            var datePart =
                DateTime.UtcNow.ToString("u")
                    .Replace("-", string.Empty)
                    .Replace(":", string.Empty)
                    .Replace(" ", string.Empty);
            var backupFilePath = Path.Combine(settings.BackupDirectory, message.BackupName) + "TakenOn" + datePart + ".bak";
            var backupFilePathUri = new Uri(backupFilePath);
            var backupDetails = new BackupDetails()
                                    {
                                        Name = message.BackupName,
                                        BackupTo = backupFilePathUri,
                                        ChecksumOption = ChecksumOption.Checksum,
                                        Cipher = Cipher.NoEncryption,
                                        CompressionOption = CompressionOption.NoCompression,
                                        Description = message.BackupDescription,
                                        Device = Device.Disk,
                                        ErrorHandling = ErrorHandling.StopOnError,
                                    };

            DatabaseManager.BackupFull(
                settings.LocalhostConnectionString,
                message.DatabaseName,
                backupDetails,
                settings.DefaultTimeout,
                logAction);

            this.FilePath = backupFilePath;
        }

        /// <inheritdoc />
        public string FilePath { get; set; }
    }
}
