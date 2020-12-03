// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordByIdOp{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using static System.FormattableString;

    /// <summary>
    /// Gets the latest record with provided identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the ID of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class GetLatestRecordByIdOp<TId, TObject> : ReturningOperationBase<StreamRecordWithId<TId, TObject>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordByIdOp{TId, TObject}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        public GetLatestRecordByIdOp(
            TId id,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            this.Id = id;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }
    }
}
