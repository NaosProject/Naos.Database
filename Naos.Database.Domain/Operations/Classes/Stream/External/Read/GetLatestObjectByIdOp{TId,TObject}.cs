// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectByIdOp{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the most recent object with a specified identifier.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier of the record.</typeparam>
    /// <typeparam name="TObject">Type of the object in the record.</typeparam>
    public partial class GetLatestObjectByIdOp<TId, TObject> : ReturningOperationBase<TObject>, IHaveId<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectByIdOp{TId, TObject}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        public GetLatestObjectByIdOp(
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Id = id;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the object type.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }
    }
}
