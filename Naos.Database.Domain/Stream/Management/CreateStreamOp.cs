// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    using OBeautifulCode.Type;

    /// <summary>
    /// Create a stream's persistence.
    /// </summary>
    public partial class CreateStreamOp : ReturningOperationBase<CreateStreamResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStreamOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The stream.</param>
        /// <param name="existingStreamStrategy">Existing stream encountered strategy.</param>
        /// <exception cref="ArgumentNullException">stream.</exception>
        public CreateStreamOp(
            IStreamRepresentation streamRepresentation,
            ExistingStreamStrategy existingStreamStrategy)
        {
            this.StreamRepresentation = streamRepresentation ?? throw new ArgumentNullException(nameof(streamRepresentation));
            this.ExistingStreamStrategy = existingStreamStrategy;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <value>The stream.</value>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the existing stream encountered strategy.
        /// </summary>
        /// <value>The existing stream encountered strategy.</value>
        public ExistingStreamStrategy ExistingStreamStrategy { get; private set; }
    }
}
