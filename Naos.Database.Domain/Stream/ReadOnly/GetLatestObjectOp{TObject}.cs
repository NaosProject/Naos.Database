// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using static System.FormattableString;

    /// <summary>
    /// Abstract base of an operation.
    /// </summary>
    /// <typeparam name="TObject">Type of data being written.</typeparam>
    public partial class GetLatestObjectOp<TObject> : ReturningOperationBase<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectOp{TObject}"/> class.
        /// </summary>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        public GetLatestObjectOp(
        TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
        }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }
    }
}
