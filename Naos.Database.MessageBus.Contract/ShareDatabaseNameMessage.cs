// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShareDatabaseNameMessage.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
    using Naos.MessageBus.DataContract;

    /// <summary>
    /// Message to share the database name with future messages in the sequence.
    /// </summary>
    public class ShareDatabaseNameMessage : IMessage
    {
        /// <inheritdoc />
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the database to share.
        /// </summary>
        public string DatabaseNameToShare { get; set; }
    }
}
