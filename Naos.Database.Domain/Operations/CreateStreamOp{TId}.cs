// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateStreamOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;

    /// <summary>
    /// Create a stream's persistence.
    /// </summary>
    /// <typeparam name="TId">Type of ID being used.</typeparam>
    public partial class CreateStreamOp<TId> : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStreamOp{TId}"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The stream.</param>
        /// <param name="existingStreamEncounteredStrategy">Existing stream encountered strategy.</param>
        /// <exception cref="ArgumentNullException">stream.</exception>
        public CreateStreamOp(
            StreamRepresentation<TId> streamRepresentation,
            ExistingStreamEncounteredStrategy existingStreamEncounteredStrategy)
        {
            this.StreamRepresentation = streamRepresentation ?? throw new ArgumentNullException(nameof(streamRepresentation));
            this.ExistingStreamEncounteredStrategy = existingStreamEncounteredStrategy;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <value>The stream.</value>
        public StreamRepresentation<TId> StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the existing stream encountered strategy.
        /// </summary>
        /// <value>The existing stream encountered strategy.</value>
        public ExistingStreamEncounteredStrategy ExistingStreamEncounteredStrategy { get; private set; }
    }
}
