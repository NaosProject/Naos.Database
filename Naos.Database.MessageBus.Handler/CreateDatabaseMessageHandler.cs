// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateDatabaseMessageHandler.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handler
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Its.Configuration;
    using Its.Log.Instrumentation;

    using Naos.Database.Contract;
    using Naos.Database.MessageBus.Contract;
    using Naos.Database.Tools;
    using Naos.MessageBus.HandlingContract;

    /// <summary>
    /// Naos.MessageBus handler for CreateDatabaseMessages.
    /// </summary>
    public class CreateDatabaseMessageHandler : IHandleMessages<CreateDatabaseMessage>, IShareDatabaseName
    {
        /// <inheritdoc />
        public async Task HandleAsync(CreateDatabaseMessage message)
        {
            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            await Task.Run(() => this.Handle(message, settings));
        }

        /// <summary>
        /// Handles a RestoreDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        public void Handle(
            CreateDatabaseMessage message,
            DatabaseMessageHandlerSettings settings)
        {
            using (var activity = Log.Enter(() => new { Message = message, DatabaseName = message.DatabaseName }))
            {
                {
                    // use this to avoid issues with database not there or going offline
                    var masterConnectionString =
                        ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(
                            settings.LocalhostConnectionString,
                            "master");

                    var existingDatabases = DatabaseManager.Retrieve(masterConnectionString);
                    if (existingDatabases.Any(_ => string.Equals(_.DatabaseName, message.DatabaseName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        throw new ArgumentException("Cannot create a database because it's already present, please delete first.");
                    }

                    var dataFilePath = Path.Combine(settings.DataDirectory, message.DataFileName);
                    var logFilePath = Path.Combine(settings.DataDirectory, message.DataFileName);

                    var databaseConfiguration = new DatabaseConfiguration
                                                    {
                                                        DatabaseName = message.DatabaseName,
                                                        DatabaseType = message.DatabaseType,
                                                        DataFileLogicalName = message.DataFileLogicalName,
                                                        LogFileLogicalName = message.LogFileLogicalName,
                                                        DataFilePath = dataFilePath,
                                                        LogFilePath = logFilePath,
                                                        DataFileCurrentSizeInKb = message.DataFileCurrentSizeInKb,
                                                        DataFileGrowthSizeInKb = message.DataFileGrowthSizeInKb,
                                                        DataFileMaxSizeInKb = message.DataFileMaxSizeInKb,
                                                        LogFileCurrentSizeInKb = message.LogFileCurrentSizeInKb,
                                                        LogFileGrowthSizeInKb = message.LogFileGrowthSizeInKb,
                                                        LogFileMaxSizeInKb = message.LogFileMaxSizeInKb
                                                    };

                    DatabaseManager.Create(masterConnectionString, databaseConfiguration);

                    this.DatabaseName = message.DatabaseName;

                    activity.Trace(() => "Completed successfully.");
                }
            }
        }

        /// <inheritdoc />
        public string DatabaseName { get; set; }
    }
}
