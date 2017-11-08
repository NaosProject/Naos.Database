// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateDatabaseMessageHandler.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
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

    using Naos.Database.Domain;
    using Naos.Database.MessageBus.Scheduler;
    using Naos.Database.SqlServer;
    using Naos.MessageBus.Domain;

    using Spritely.Recipes;

    /// <summary>
    /// Naos.MessageBus handler for CreateDatabaseMessages.
    /// </summary>
    public class CreateDatabaseMessageHandler : MessageHandlerBase<CreateDatabaseMessage>, IShareDatabaseName
    {
        /// <inheritdoc cref="MessageHandlerBase{T}" />
        public override async Task HandleAsync(CreateDatabaseMessage message)
        {
            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            await Task.Run(() => this.Handle(message, settings));
        }

        /// <summary>
        /// Handles a RestoreDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Keeping, seems reasonable.")]
        public void Handle(
            CreateDatabaseMessage message,
            DatabaseMessageHandlerSettings settings)
        {
            new { message }.Must().NotBeNull().OrThrowFirstFailure();
            new { settings }.Must().NotBeNull().OrThrowFirstFailure();
            new { message.DatabaseKind }.Must().BeEqualTo(DatabaseKind.SqlServer).OrThrowFirstFailure();

            using (var activity = Log.Enter(() => new { Message = message, DatabaseName = message.DatabaseName }))
            {
                {
                    // use this to avoid issues with database not there or going offline
                    var localhostConnection = settings.DatabaseKindToLocalhostConnectionDefinitionMap[message.DatabaseKind];
                    var masterConnectionString =
                        ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(
                            localhostConnection.ToSqlServerConnectionString(),
                            SqlServerDatabaseManager.MasterDatabaseName);

                    var existingDatabases = SqlServerDatabaseManager.Retrieve(masterConnectionString);
                    if (existingDatabases.Any(_ => string.Equals(_.DatabaseName, message.DatabaseName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        throw new ArgumentException("Cannot create a database because it's already present, please delete first.");
                    }

                    var dataDirectory = settings.DatabaseKindToDataDirectoryMap[message.DatabaseKind];
                    var dataFilePath = Path.Combine(dataDirectory, message.DataFileName);
                    var logFilePath = Path.Combine(dataDirectory, message.DataFileName);

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
                                                        LogFileMaxSizeInKb = message.LogFileMaxSizeInKb,
                                                    };

                    SqlServerDatabaseManager.Create(masterConnectionString, databaseConfiguration);

                    this.DatabaseName = message.DatabaseName;

                    activity.Trace(() => "Completed successfully.");
                }
            }
        }

        /// <inheritdoc />
        public string DatabaseName { get; set; }
    }
}
