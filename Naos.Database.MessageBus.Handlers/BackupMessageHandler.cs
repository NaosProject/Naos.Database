// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupMessageHandler.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handlers
{
    using System;

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
            Action<string> logAction = s => { };

            var settings = Settings.Get<MessageHandlerSettings>();
            var backupDetails = new BackupDetails()
                                    {
                                        Name = message.BackupName,
                                        BackupTo = new Uri(settings.BackupDirectory),
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
