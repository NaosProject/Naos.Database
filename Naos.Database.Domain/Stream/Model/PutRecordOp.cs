// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Event to record the execution of <see cref="GetNextUniqueLongOp"/>.
    /// </summary>
    public partial class PutRecordOp : ReturningOperationBase<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutRecordOp"/> class.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="payload">The payload.</param>
        public PutRecordOp(
            IResourceLocator locator,
            StreamRecordMetadata metadata,
            DescribedSerialization payload)
        {
            locator.MustForArg(nameof(locator)).NotBeNull();
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            payload.MustForArg(nameof(payload)).NotBeNull();

            this.Locator = locator;
            this.Metadata = metadata;
            this.Payload = payload;
        }

        /// <summary>
        /// Gets the locator.
        /// </summary>
        /// <value>The locator.</value>
        public IResourceLocator Locator { get; private set; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public StreamRecordMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        public DescribedSerialization Payload { get; private set; }
    }
}
