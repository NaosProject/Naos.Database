// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationOp.cs" company="Naos Project">
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
    public partial class GetStreamFromRepresentationOp :
        ReturningOperationBase<IStreamRepresentation>,
        IReturningOperation<IStream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The typed stream representation.</param>
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
