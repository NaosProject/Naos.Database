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

    using Naos.Database.Domain;
    using Naos.Database.MessageBus.Scheduler;
    using Naos.Database.SqlServer;
    using Naos.MessageBus.Domain;

    using Spritely.Recipes;

    /// <summary>
    /// Naos.MessageBus handler for Share.
    /// </summary>
    public class CopyDatabaseObjectMessageHandler : MessageHandlerBase<CopyDatabaseObjectMessage>
    {
        /// <inheritdoc cref="MessageHandlerBase{T}" />
        public override async Task HandleAsync(CopyDatabaseObjectMessage message)
        {
            new { message }.Must().NotBeNull().OrThrowFirstFailure();
            new { message.DatabaseKind }.Must().BeEqualTo(DatabaseKind.SqlServer).OrThrowFirstFailure();

            var settings = Settings.Get<DatabaseMessageHandlerSettings>();
            new { settings }.Must().NotBeNull().OrThrowFirstFailure();

            var localhostConnectionString = settings.DatabaseKindToLocalhostConnectionStringMap[message.DatabaseKind];
            var sourceDatabaseConnectionString = ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(localhostConnectionString, message.SourceDatabaseName);
            var targetDatabaseConnectionString = ConnectionStringHelper.SpecifyInitialCatalogInConnectionString(localhostConnectionString, message.TargetDatabaseName);
            await DatabaseObjectCopier.CopyObjects(message.OrderedObjectNamesToCopy, sourceDatabaseConnectionString, targetDatabaseConnectionString);
        }
    }
}
