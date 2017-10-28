// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyDatabaseObjectMessageHandler.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handler
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Its.Configuration;
    using Its.Log.Instrumentation;

    using Naos.Database.MessageBus.Scheduler;
    using Naos.Database.SqlServer;
    using Naos.MessageBus.Domain;

    /// <summary>
    /// Naos.MessageBus handler for Share.
    /// </summary>
    public class CopyDatabaseObjectMessageHandler : MessageHandlerBase<CopyDatabaseObjectMessage>
    {
        /// <inheritdoc cref="MessageHandlerBase{T}" />
        public override async Task HandleAsync(CopyDatabaseObjectMessage message)
        {
            var databaseSettings = Settings.Get<DatabaseMessageHandlerSettings>();
            var sourceDatabaseConnectionString = ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(databaseSettings.LocalhostConnectionString, message.SourceDatabaseName);
            var targetDatabaseConnectionString = ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(databaseSettings.LocalhostConnectionString, message.TargetDatabaseName);
            await DatabaseObjectCopier.CopyObjects(message.OrderedObjectNamesToCopy, sourceDatabaseConnectionString, targetDatabaseConnectionString);
        }
    }
}
