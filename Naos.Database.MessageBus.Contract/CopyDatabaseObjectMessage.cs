// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyDatabaseObjectMessage.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
    using System.Collections.Generic;

    using Naos.MessageBus.Domain;

    /// <summary>
    /// Message to copy objects from one database to another.
    /// </summary>
    public class CopyDatabaseObjectMessage : IMessage
    {
        /// <inheritdoc />
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the database copy objects from.
        /// </summary>
        public string SourceDatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the name of the database copy objects to.
        /// </summary>
        public string TargetDatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the object names to copy in order to copy.
        /// </summary>
        public IReadOnlyList<string> OrderedObjectNamesToCopy { get; set; }
    }
}
