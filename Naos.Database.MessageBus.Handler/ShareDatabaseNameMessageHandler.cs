// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShareDatabaseNameMessageHandler.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Handler
{
    using Its.Log.Instrumentation;

    using Naos.Database.MessageBus.Contract;
    using Naos.MessageBus.HandlingContract;

    /// <summary>
    /// Naos.MessageBus handler for Share.
    /// </summary>
    public class ShareDatabaseNameMessageHandler : IHandleMessages<ShareDatabaseNameMessage>, IShareDatabaseName
    {
        /// <inheritdoc />
        public void Handle(ShareDatabaseNameMessage message)
        {
            using (var log = Log.Enter(() => new { Message = message, DatabaseNameToShare = message.DatabaseNameToShare }))
            {
                log.Trace(() => "Sharing database name: " + message.DatabaseNameToShare);
                this.DatabaseName = message.DatabaseNameToShare;
            }
        }

        /// <inheritdoc />
        public string DatabaseName { get; set; }
    }
}
