// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationOp{TStreamRepresentation,TStream}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets a <typeparamref name="TStream"/> by the specified <typeparamref name="TStreamRepresentation"/>.
    /// </summary>
    /// <typeparam name="TStreamRepresentation">Type of <see cref="IStreamRepresentation"/> to use.</typeparam>
    /// <typeparam name="TStream">Type of <see cref="IStream"/> to get.</typeparam>
    public partial class GetStreamFromRepresentationOp<TStreamRepresentation, TStream> :
        ReturningOperationBase<TStream>,
        IHaveStreamRepresentation
        where TStreamRepresentation : IStreamRepresentation
        where TStream : IStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationOp{TStreamRepresentation, TStream}"/> class.
        /// </summary>
        /// <param name="typedStreamRepresentation">The typed stream representation.</param>
        public GetStreamFromRepresentationOp(
            TStreamRepresentation typedStreamRepresentation)
        {
            typedStreamRepresentation.MustForArg(nameof(typedStreamRepresentation)).NotBeNull();

            this.TypedStreamRepresentation = typedStreamRepresentation;
        }

        /// <summary>
        /// Gets the typed stream representation.
        /// </summary>
        public TStreamRepresentation TypedStreamRepresentation { get; private set; }

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation => this.TypedStreamRepresentation;
    }
}
