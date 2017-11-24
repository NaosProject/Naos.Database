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
    using Naos.Database.Mongo;
    using Naos.Database.SqlServer;
    using Naos.FileJanitor.MessageBus.Scheduler;
    using Naos.MessageBus.Domain;

    using Spritely.Recipes;

    using static System.FormattableString;

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
            new { message.DatabaseKind }.Must().NotBeEqualTo(DatabaseKind.Invalid).OrThrowFirstFailure();

            using (var activity = Log.Enter(() => new { Message = message, message.DatabaseName, message.FilePath }))
            {
                {
                    this.DatabaseName = message.DatabaseName;
                    this.FilePath = message.FilePath;

                    var dataDirectory = settings.DatabaseKindToDataDirectoryMap[message.DatabaseKind];
                    var dataFilePath = Path.Combine(dataDirectory, this.DatabaseName + "Dat.mdf");

                    var logFilePath = Path.Combine(dataDirectory, this.DatabaseName + "Log.ldf");

                    activity.Trace(() => $"Using data path: {dataFilePath}, log path: {logFilePath}");

                    var restoreFilePath = new Uri(this.FilePath);
                    var restoreDetails = new RestoreDetails
                                             {
                                                 ChecksumOption = message.ChecksumOption,
                                                 Device = Device.Disk,
                                                 ErrorHandling = message.ErrorHandling,
                                                 DataFilePath = dataFilePath,
                                                 LogFilePath = logFilePath,
                                                 RecoveryOption = message.RecoveryOption,
                                                 ReplaceOption = message.ReplaceOption,
                                                 RestoreFrom = restoreFilePath,
                                                 RestrictedUserOption = message.RestrictedUserOption,
                                             };

                    activity.Trace(() => Invariant($"Restoring database {this.DatabaseName} from {restoreFilePath} for kind {message.DatabaseKind}"));

                    var localhostConnection = settings.DatabaseNameToLocalhostConnectionDefinitionMap[message.DatabaseName.ToUpperInvariant()];
                    switch (message.DatabaseKind)
                    {
                        case DatabaseKind.SqlServer:
                            // use this to avoid issues with database not there or going offline
                            var masterConnectionString = ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(
                                localhostConnection.ToSqlServerConnectionString(),
                                SqlServerDatabaseManager.MasterDatabaseName);
                            await SqlServerDatabaseManager.RestoreFullAsync(
                                masterConnectionString,
                                this.DatabaseName,
                                restoreDetails,
                                message.Timeout == default(TimeSpan) ? settings.DefaultTimeout : message.Timeout);
                            break;
                        case DatabaseKind.Mongo:
                            await MongoDatabaseManager.RestoreFullAsync(
                                localhostConnection,
                                this.DatabaseName,
                                restoreDetails,
                                settings.WorkingDirectoryPath,
                                settings.MongoUtilityDirectory);
                            break;
                        default:
                            throw new NotSupportedException(Invariant($"Unsupported {nameof(DatabaseKind)} - {message.DatabaseKind}"));
                    }

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
