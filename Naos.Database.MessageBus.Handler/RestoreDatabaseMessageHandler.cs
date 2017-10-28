// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreDatabaseMessageHandler.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handler
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Its.Configuration;
    using Its.Log.Instrumentation;

    using Naos.Database.Domain;
    using Naos.Database.MessageBus.Scheduler;
    using Naos.Database.SqlServer;
    using Naos.FileJanitor.MessageBus.Scheduler;
    using Naos.MessageBus.Domain;

    using Spritely.Recipes;

    /// <summary>
    /// Naos.MessageBus handler for RestoreMessages.
    /// </summary>
    public class RestoreDatabaseMessageHandler : MessageHandlerBase<RestoreDatabaseMessage>, IShareFilePath, IShareDatabaseName
    {
        /// <inheritdoc cref="MessageHandlerBase{T}" />
        public override async Task HandleAsync(RestoreDatabaseMessage message)
        {
            if (!File.Exists(message.FilePath))
            {
                throw new FileNotFoundException("Could not find file to restore", message.FilePath);
            }

            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            await this.HandleAsync(message, settings);
        }

        /// <summary>
        /// Handles a RestoreDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        /// <returns>Task to support async await calling.</returns>
        public async Task HandleAsync(
            RestoreDatabaseMessage message,
            DatabaseMessageHandlerSettings settings)
        {
            new { message }.Must().NotBeNull().OrThrowFirstFailure();
            new { settings }.Must().NotBeNull().OrThrowFirstFailure();

            using (var activity = Log.Enter(() => new { Message = message, message.DatabaseName, message.FilePath }))
            {
                {
                    this.DatabaseName = message.DatabaseName;
                    this.FilePath = message.FilePath;

                    // use this to avoid issues with database not there or going offline
                    var masterConnectionString =
                        ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(
                            settings.LocalhostConnectionString,
                            "master");

                    var dataFilePath = Path.Combine(
                        settings.DataDirectory,
                        this.DatabaseName + "Dat.mdf");

                    var logFilePath = Path.Combine(
                        settings.DataDirectory,
                        this.DatabaseName + "Log.ldf");

                    activity.Trace(
                        () => $"Using data path: {dataFilePath}, log path: {logFilePath}");

                    var restoreFileUri = new Uri(this.FilePath);
                    var restoreDetails = new RestoreDetails
                                             {
                                                 ChecksumOption = message.ChecksumOption,
                                                 Device = Device.Disk,
                                                 ErrorHandling = message.ErrorHandling,
                                                 DataFilePath = dataFilePath,
                                                 LogFilePath = logFilePath,
                                                 RecoveryOption = message.RecoveryOption,
                                                 ReplaceOption = message.ReplaceOption,
                                                 RestoreFrom = restoreFileUri,
                                                 RestrictedUserOption = message.RestrictedUserOption,
                                             };

                    activity.Trace(() => "Starting restore.");
                    await DatabaseManager.RestoreFullAsync(
                        masterConnectionString,
                        this.DatabaseName,
                        restoreDetails,
                        settings.DefaultTimeout);

                    activity.Trace(() => "Completed successfully.");
                }
            }
        }

        /// <inheritdoc />
        public string FilePath { get; set; }

        /// <inheritdoc />
        public string DatabaseName { get; set; }
    }
}
