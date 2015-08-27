// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupMessageHandler.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handler
{
    using System;
    using System.IO;

    using Its.Configuration;
    using Its.Log.Instrumentation;

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
            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            this.Handle(message, settings);
        }

        /// <summary>
        /// Handles a BackupDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        public void Handle(BackupDatabaseMessage message, DatabaseMessageHandlerSettings settings)
        {
            using (var activity = Log.Enter(() => new { Message = message, DatabaseName = message.DatabaseName }))
            {
                // must have a date that is strictly alphanumeric...
                var datePart =
                    DateTime.UtcNow.ToString("u")
                        .Replace("-", string.Empty)
                        .Replace(":", string.Empty)
                        .Replace(" ", string.Empty);
                var backupFilePath = Path.Combine(settings.BackupDirectory, message.BackupName) + "TakenOn" + datePart
                                     + ".bak";
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

                activity.Trace(
                    () => string.Format("Backing up database {0} to {1}", message.DatabaseName, backupFilePath));

                DatabaseManager.BackupFull(
                    settings.LocalhostConnectionString,
                    message.DatabaseName,
                    backupDetails,
                    settings.DefaultTimeout);

                this.FilePath = backupFilePath;

                activity.Trace(() => "Completed successfully.");
            }
        }

        /// <inheritdoc />
        public string FilePath { get; set; }
    }
}
