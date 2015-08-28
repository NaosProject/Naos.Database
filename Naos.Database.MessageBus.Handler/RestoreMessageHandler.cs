// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreMessageHandler.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handler
{
    using System;
    using System.IO;
    using System.Linq;

    using Its.Configuration;
    using Its.Log.Instrumentation;

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
            if (!File.Exists(message.FilePath))
            {
                throw new FileNotFoundException("Could not find file to restore", message.FilePath);
            }

            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            this.Handle(message, settings);
        }

        /// <summary>
        /// Handles a RestoreDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        public void Handle(
            RestoreDatabaseMessage message,
            DatabaseMessageHandlerSettings settings)
        {
            using (var activity = Log.Enter(() => new { Message = message, DatabaseName = message.DatabaseName, FilePath = message.FilePath }))
            {
                {
                    // use this to avoid issues with database not there or going offline
                    var masterConnectionString =
                        ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(
                            settings.LocalhostConnectionString,
                            "master");

                    var datePart =
                        DateTime.UtcNow.ToString("u")
                            .Replace("-", string.Empty)
                            .Replace(":", string.Empty)
                            .Replace(" ", string.Empty);

                    var fileNameAddIn = "_dat_UsingBackupRestoredOn_" + datePart;

                    var dataFilePath = Path.Combine(
                        settings.DataDirectory,
                        message.DatabaseName + fileNameAddIn + ".mdf");

                    var logFilePath = Path.Combine(
                        settings.DataDirectory,
                        message.DatabaseName + fileNameAddIn + ".ldf");

                    activity.Trace(
                        () => string.Format("Using data path: {0}, log path: {1}", dataFilePath, logFilePath));

                    var restoreFileUri = new Uri(message.FilePath);
                    var restoreDetails = new RestoreDetails
                                             {
                                                 ChecksumOption =
                                                     message.RunChecksum
                                                         ? ChecksumOption.Checksum
                                                         : ChecksumOption.NoChecksum,
                                                 Device = Device.Disk,
                                                 ErrorHandling = ErrorHandling.StopOnError,
                                                 DataFilePath = dataFilePath,
                                                 LogFilePath = logFilePath,
                                                 RecoveryOption = RecoveryOption.Recovery,
                                                 ReplaceOption = ReplaceOption.DoNotReplaceExistingDatabaseAndThrow,
                                                 RestoreFrom = restoreFileUri,
                                                 RestrictedUserOption = RestrictedUserOption.Normal
                                             };

                    activity.Trace(() => "Deleting existing database before restore.");
                    DatabaseManager.Delete(masterConnectionString, message.DatabaseName);

                    activity.Trace(() => "Starting restore.");
                    DatabaseManager.RestoreFull(
                        masterConnectionString,
                        message.DatabaseName,
                        restoreDetails,
                        settings.DefaultTimeout);

                    activity.Trace(() => "Completed successfully.");
                }
            }
        }
    }
}
