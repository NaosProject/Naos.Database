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
    using Naos.MessageBus.HandlingContract;

    /// <summary>
    /// Naos.MessageBus handler for BackupMessages.
    /// </summary>
    public class BackupMessageHandler : IHandleMessages<BackupDatabaseMessage>
    {
        /// <inheritdoc />
        public void Handle(BackupDatabaseMessage message)
        {
            Action<string> logAction = Console.WriteLine;

            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            var backupFileName = Path.Combine(settings.BackupDirectory, message.BackupName) + ".bak";
            var backupFileNameUri = new Uri(backupFileName);
            var backupDetails = new BackupDetails()
                                    {
                                        Name = message.BackupName,
                                        BackupTo = backupFileNameUri,
                                        ChecksumOption = ChecksumOption.Checksum,
                                        Cipher = Cipher.NoEncryption,
                                        CompressionOption = CompressionOption.NoCompression,
                                        Description = message.Description,
                                        Device = Device.Disk,
                                        ErrorHandling = ErrorHandling.StopOnError,
                                    };

            DatabaseManager.BackupFull(
                settings.LocalhostConnectionString,
                message.DatabaseName,
                backupDetails,
                settings.DefaultTimeout,
                logAction);
        }
    }
}
