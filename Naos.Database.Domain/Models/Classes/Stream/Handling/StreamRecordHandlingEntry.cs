﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordHandlingEntry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// A stream record handling entry.
    /// </summary>
    public partial class StreamRecordHandlingEntry : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordHandlingEntry"/> class.
        /// </summary>
        /// <param name="internalHandlingEntryId">The internal handling entry identifier.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="payload">The payload.</param>
        public StreamRecordHandlingEntry(
            long internalHandlingEntryId,
            StreamRecordHandlingEntryMetadata metadata,
            DescribedSerializationBase payload)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            payload.MustForArg(nameof(payload)).NotBeNull();

            this.InternalHandlingEntryId = internalHandlingEntryId;
            this.Metadata = metadata;
            this.Payload = payload;
        }

        /// <summary>
        /// Gets the internal handling entry identifier.
        /// </summary>
        public long InternalHandlingEntryId { get; private set; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public StreamRecordHandlingEntryMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        public DescribedSerializationBase Payload { get; private set; }
    }
}