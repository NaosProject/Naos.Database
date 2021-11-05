// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationOp{TStreamRepresentation,TStream}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Operation to get a specific <see cref="IStream"/> by a specific <see cref="IStreamRepresentation"/>.
    /// </summary>
    /// <typeparam name="TStreamRepresentation">Type of <see cref="IStreamRepresentation"/> to use.</typeparam>
    /// <typeparam name="TStream">Type of <see cref="IStream"/> to get.</typeparam>
    public partial class GetStreamFromRepresentationOp<TStreamRepresentation, TStream> :
        ReturningOperationBase<TStream>
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
        /// Gets the stream representation.
        /// </summary>
        public IStreamRepresentation StreamRepresentation => this.TypedStreamRepresentation;

        /// <summary>
        /// Gets the typed stream representation.
        /// </summary>
        public TStreamRepresentation TypedStreamRepresentation { get; private set; }
    }
}
