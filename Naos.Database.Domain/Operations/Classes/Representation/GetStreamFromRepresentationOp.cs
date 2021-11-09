// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets an <see cref="IStream"/> by the specified <see cref="IStreamRepresentation"/>.
    /// </summary>
    public partial class GetStreamFromRepresentationOp :
        ReturningOperationBase<IStreamRepresentation>,
        IReturningOperation<IStream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The stream representation.</param>
        public GetStreamFromRepresentationOp(
            IStreamRepresentation streamRepresentation)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();

            this.StreamRepresentation = streamRepresentation;
        }

        /// <summary>
        /// Gets the stream representation.
        /// </summary>
        public IStreamRepresentation StreamRepresentation { get; private set; }
    }
}
