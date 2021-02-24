// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordHandlingEntry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Metadata of a stream entry.
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
        /// <value>The internal handling entry identifier.</value>
        public long InternalHandlingEntryId { get; private set; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public StreamRecordHandlingEntryMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        public DescribedSerializationBase Payload { get; private set; }
    }
}
