// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordItemsToInclude.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Determines which aspects of a <see cref="StreamRecord"/> to include when querying for one or more records.
    /// </summary>
    public enum StreamRecordItemsToInclude
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Include only the <see cref="StreamRecord.Metadata"/>;
        /// set <see cref="StreamRecord.Payload"/> to <see cref="NullDescribedSerialization"/>.
        /// </summary>
        MetadataOnly,

        /// <summary>
        /// Include both the <see cref="StreamRecord.Metadata"/> and the <see cref="StreamRecord.Payload"/>.
        /// </summary>
        MetadataAndPayload,
    }
}
