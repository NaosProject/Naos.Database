// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteStreamOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;

    /// <summary>
    /// Delete a stream's persistence.
    /// </summary>
    /// <typeparam name="TId">Type of ID being used.</typeparam>
    public partial class DeleteStreamOp<TId> : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteStreamOp{TId}"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The stream.</param>
        /// <param name="existingStreamNotEncounteredStrategy">Existing stream encountered strategy.</param>
        /// <exception cref="ArgumentNullException">stream.</exception>
        public DeleteStreamOp(
            IStreamRepresentation<TId> streamRepresentation,
            ExistingStreamNotEncounteredStrategy existingStreamNotEncounteredStrategy)
        {
            this.StreamRepresentation = streamRepresentation ?? throw new ArgumentNullException(nameof(streamRepresentation));
            this.ExistingStreamNotEncounteredStrategy = existingStreamNotEncounteredStrategy;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <value>The stream.</value>
        public IStreamRepresentation<TId> StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the existing stream not encountered strategy.
        /// </summary>
        /// <value>The existing stream not encountered strategy.</value>
        public ExistingStreamNotEncounteredStrategy ExistingStreamNotEncounteredStrategy { get; private set; }
    }
}
