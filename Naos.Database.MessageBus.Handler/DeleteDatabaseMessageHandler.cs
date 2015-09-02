// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteDatabaseMessageHandler.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handler
{
    using System;
    using System.Linq;

    using Its.Configuration;
    using Its.Log.Instrumentation;

    using Naos.Database.MessageBus.Contract;
    using Naos.Database.Tools;
    using Naos.MessageBus.HandlingContract;

    /// <summary>
    /// Naos.MessageBus handler for RestoreMessages.
    /// </summary>
    public class DeleteDatabaseMessageHandler : IHandleMessages<DeleteDatabaseMessage>, IShareDatabaseName
    {
        /// <inheritdoc />
        public void Handle(DeleteDatabaseMessage message)
        {
            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            this.Handle(message, settings);
        }

        /// <summary>
        /// Handles a RestoreDatabaseMessage.
        /// </summary>
        /// <param name="message">Message to handle.</param>
        /// <param name="settings">Needed settings to handle messages.</param>
        public void Handle(
            DeleteDatabaseMessage message,
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
                        activity.Trace(() => "Deleting existing database before restore.");
                        DatabaseManager.Delete(masterConnectionString, message.DatabaseName);
                    }
                    else
                    {
                        activity.Trace(() => "No existing database found to delete.");
                    }

                    this.DatabaseName = message.DatabaseName;

                    activity.Trace(() => "Completed successfully.");
                }
            }
        }

        /// <inheritdoc />
        public string DatabaseName { get; set; }
    }
}
