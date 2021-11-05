// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardCreateStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Create a stream's persistence.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardCreateStreamOp : ReturningOperationBase<StandardCreateStreamResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardCreateStreamOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">A representation of the stream.</param>
        /// <param name="existingStreamStrategy">A value that specifies what do when the stream already exists.</param>
        /// <exception cref="ArgumentNullException">stream.</exception>
        public StandardCreateStreamOp(
            IStreamRepresentation streamRepresentation,
            ExistingStreamStrategy existingStreamStrategy)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            existingStreamStrategy.MustForArg(nameof(existingStreamStrategy)).NotBeEqualTo(ExistingStreamStrategy.Unknown);

            this.StreamRepresentation = streamRepresentation;
            this.ExistingStreamStrategy = existingStreamStrategy;
        }

        /// <summary>
        /// Gets a representation of the stream.
        /// </summary>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets a value that specifies what do when the stream already exists.
        /// </summary>
        public ExistingStreamStrategy ExistingStreamStrategy { get; private set; }
    }
}
