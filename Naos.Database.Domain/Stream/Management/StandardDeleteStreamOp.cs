// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardDeleteStreamOp.cs" company="Naos Project">
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
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardReadWriteStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardDeleteStreamOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardDeleteStreamOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The stream.</param>
        /// <param name="streamNotFoundStrategy">Existing stream encountered strategy.</param>
        /// <exception cref="ArgumentNullException">stream.</exception>
        public StandardDeleteStreamOp(
            IStreamRepresentation streamRepresentation,
            StreamNotFoundStrategy streamNotFoundStrategy)
        {
            this.StreamRepresentation = streamRepresentation ?? throw new ArgumentNullException(nameof(streamRepresentation));
            this.StreamNotFoundStrategy = streamNotFoundStrategy;
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
        public StreamNotFoundStrategy StreamNotFoundStrategy { get; private set; }
    }
}
