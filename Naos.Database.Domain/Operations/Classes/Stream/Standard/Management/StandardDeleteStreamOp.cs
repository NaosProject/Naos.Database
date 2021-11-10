// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardDeleteStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Delete a stream's persistence.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardDeleteStreamOp : VoidOperationBase, IHaveStreamRepresentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardDeleteStreamOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">A representation of the stream.</param>
        /// <param name="streamNotFoundStrategy">A value that determines what to do when the stream is not found.</param>
        /// <exception cref="ArgumentNullException">stream.</exception>
        public StandardDeleteStreamOp(
            IStreamRepresentation streamRepresentation,
            StreamNotFoundStrategy streamNotFoundStrategy)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            streamNotFoundStrategy.MustForArg(nameof(streamNotFoundStrategy)).NotBeEqualTo(StreamNotFoundStrategy.Unknown);

            this.StreamRepresentation = streamRepresentation;
            this.StreamNotFoundStrategy = streamNotFoundStrategy;
        }

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the existing stream not encountered strategy.
        /// </summary>
        public StreamNotFoundStrategy StreamNotFoundStrategy { get; private set; }
    }
}
