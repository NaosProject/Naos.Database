// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationOp{TStream}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets a <typeparamref name="TStream"/> by the specified <see cref="IStreamRepresentation"/>.
    /// </summary>
    /// <typeparam name="TStream">Type of <see cref="IStream"/> to get.</typeparam>
    public partial class GetStreamFromRepresentationOp<TStream> :
        ReturningOperationBase<TStream>,
        IHaveStreamRepresentation
        where TStream : IStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationOp{TStream}"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The stream representation.</param>
        public GetStreamFromRepresentationOp(
            IStreamRepresentation streamRepresentation)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();

            this.StreamRepresentation = streamRepresentation;
        }

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation { get; private set; }
    }
}
