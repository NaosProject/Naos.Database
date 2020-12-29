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
    public partial class PutRecordOp : ReturningOperationBase<long>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutRecordOp"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        public PutRecordOp(
            StreamRecordMetadata metadata,
            DescribedSerialization payload,
            IResourceLocator specifiedResourceLocator = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            payload.MustForArg(nameof(payload)).NotBeNull();

            this.Metadata = metadata;
            this.Payload = payload;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
            this.ExistingRecordEncounteredStrategy = existingRecordEncounteredStrategy;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
        }

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

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }

        /// <summary>
        /// Gets the existing record encountered strategy.
        /// </summary>
        /// <value>The existing record encountered strategy.</value>
        public ExistingRecordEncounteredStrategy ExistingRecordEncounteredStrategy { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }
    }
}
