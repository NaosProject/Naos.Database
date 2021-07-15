// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    using OBeautifulCode.Type;

    /// <summary>
    /// Delete a stream's persistence.
    /// </summary>
    public partial class DeleteStreamOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteStreamOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The stream.</param>
        /// <param name="existingStreamNotEncounteredStrategy">Existing stream encountered strategy.</param>
        /// <exception cref="ArgumentNullException">stream.</exception>
        public DeleteStreamOp(
            IStreamRepresentation streamRepresentation,
            ExistingStreamNotEncounteredStrategy existingStreamNotEncounteredStrategy)
        {
            this.StreamRepresentation = streamRepresentation ?? throw new ArgumentNullException(nameof(streamRepresentation));
            this.ExistingStreamNotEncounteredStrategy = existingStreamNotEncounteredStrategy;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <value>The stream.</value>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the existing stream not encountered strategy.
        /// </summary>
        /// <value>The existing stream not encountered strategy.</value>
        public ExistingStreamNotEncounteredStrategy ExistingStreamNotEncounteredStrategy { get; private set; }
    }
}
