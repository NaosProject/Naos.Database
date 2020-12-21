// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectByIdOp{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to get the most recent object of a certain ID and given type.
    /// </summary>
    /// <typeparam name="TId">The type of the ID of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class GetLatestObjectByIdOp<TId, TObject> : ReturningOperationBase<TObject>, IIdentifiableBy<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectByIdOp{TId, TObject}"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The optional strategy on how to deal with no matching record; DEFAULT is the default of the requested type or null.</param>
        public GetLatestObjectByIdOp(
            TId id,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault)
        {
            this.Id = id;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
            this.ExistingRecordNotEncounteredStrategy = existingRecordNotEncounteredStrategy;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the existing record not encountered strategy.
        /// </summary>
        /// <value>The existing record not encountered strategy.</value>
        public ExistingRecordNotEncounteredStrategy ExistingRecordNotEncounteredStrategy { get; private set; }
    }
}
