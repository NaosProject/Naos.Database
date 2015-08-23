// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreMessageHandler.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handler
{
    using System;

    using Its.Configuration;

    using Naos.Database.MessageBus.Contract;
    using Naos.Database.Tools;
    using Naos.Database.Tools.Backup;
    using Naos.MessageBus.HandlingContract;

    /// <summary>
    /// Naos.MessageBus handler for RestoreMessages.
    /// </summary>
    public class RestoreMessageHandler : IHandleMessages<RestoreDatabaseMessage>
    {
        /// <inheritdoc />
        public void Handle(RestoreDatabaseMessage message)
        {
            Action<string> logAction = Console.WriteLine;
            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            this.Handle(message, settings, logAction);
        }

        /// <summary>
        /// Handles a RestoreDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        /// <param name="logAction">Action for logging notifications.</param>
        public void Handle(RestoreDatabaseMessage message, DatabaseMessageHandlerSettings settings, Action<string> logAction)
        {
            var dataFilePath = DatabaseManager.GetInstanceDefaultDataPath(settings.LocalhostConnectionString);
            var logFilePath = DatabaseManager.GetInstanceDefaultLogPath(settings.LocalhostConnectionString);

            var restoreFileUri = new Uri(message.FilePath);
            var restoreDetails = new RestoreDetails
            {
                ChecksumOption = ChecksumOption.Checksum,
                Device = Device.Disk,
                ErrorHandling = ErrorHandling.StopOnError,
                DataFilePath = dataFilePath,
                LogFilePath = logFilePath,
                RecoveryOption = RecoveryOption.NoRecovery,
                ReplaceOption =
                    ReplaceOption.ReplaceExistingDatabase,
                RestoreFrom = restoreFileUri,
                RestrictedUserOption =
                    RestrictedUserOption.Normal
            };

            DatabaseManager.RestoreFull(
                settings.LocalhostConnectionString,
                message.DatabaseName,
                restoreDetails,
                settings.DefaultTimeout,
                logAction);
        }
    }
}
