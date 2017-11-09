// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupDatabaseMessageHandler.cs" company="Naos">
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
    /// Naos.MessageBus handler for BackupMessages.
    /// </summary>
    public class BackupDatabaseMessageHandler : MessageHandlerBase<BackupDatabaseMessage>, IShareFilePath, IShareDatabaseName
    {
        /// <inheritdoc cref="MessageHandlerBase{T}" />
        public override async Task HandleAsync(BackupDatabaseMessage message)
        {
            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            await this.HandleAsync(message, settings);
        }

        /// <summary>
        /// Handles a BackupDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        /// <returns>Task to support async await calling.</returns>
        public async Task HandleAsync(BackupDatabaseMessage message, DatabaseMessageHandlerSettings settings)
        {
            new { message }.Must().NotBeNull().OrThrowFirstFailure();
            new { settings }.Must().NotBeNull().OrThrowFirstFailure();
            new { message.DatabaseKind }.Must().NotBeEqualTo(DatabaseKind.Invalid).OrThrowFirstFailure();

            using (var activity = Log.Enter(() => new { Message = message, DatabaseName = message.DatabaseName }))
            {
                // must have a date that is strictly alphanumeric...
                var datePart =
                    DateTime.UtcNow.ToString("u")
                        .Replace("-", string.Empty)
                        .Replace(":", string.Empty)
                        .Replace(" ", string.Empty);

                var backupDirectory = settings.DatabaseKindToBackupDirectoryMap[message.DatabaseKind];
                var backupFilePath = Path.Combine(backupDirectory, message.BackupName) + "TakenOn" + datePart + ".bak";

                this.FilePath = backupFilePath;
                this.DatabaseName = message.DatabaseName;

                var backupFilePathUri = new Uri(this.FilePath);
                var backupDetails = new BackupDetails()
                                        {
                                            Name = message.BackupName,
                                            BackupTo = backupFilePathUri,
                                            ChecksumOption = message.ChecksumOption,
                                            Cipher = message.Cipher,
                                            CompressionOption = message.CompressionOption,
                                            Description = message.BackupDescription,
                                            Device = Device.Disk,
                                            ErrorHandling = message.ErrorHandling,
                                        };

                activity.Trace(() => Invariant($"Backing up database {this.DatabaseName} to {backupFilePath} for kind {message.DatabaseKind}"));

                var localhostConnection = settings.DatabaseNameToLocalhostConnectionDefinitionMap[message.DatabaseName];
                switch (message.DatabaseKind)
                {
                    case DatabaseKind.SqlServer:
                        await SqlServerDatabaseManager.BackupFullAsync(
                            localhostConnection.ToSqlServerConnectionString(),
                            this.DatabaseName,
                            backupDetails,
                            message.Timeout == default(TimeSpan) ? settings.DefaultTimeout : message.Timeout);
                        break;
                    case DatabaseKind.Mongo:
                        await MongoDatabaseManager.BackupFullAsync(
                            localhostConnection,
                            this.DatabaseName,
                            backupDetails,
                            settings.WorkingDirectoryPath,
                            settings.MongoUtilityDirectory);
                        break;
                    default:
                        throw new NotSupportedException(Invariant($"Unsupported {nameof(DatabaseKind)} - {message.DatabaseKind}"));
                }

                activity.Trace(() => "Completed successfully.");
            }
        }

        /// <inheritdoc />
        public string FilePath { get; set; }

        /// <inheritdoc />
        public string DatabaseName { get; set; }
    }
}
