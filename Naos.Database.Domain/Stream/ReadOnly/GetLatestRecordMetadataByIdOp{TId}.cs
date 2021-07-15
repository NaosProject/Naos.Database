// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordMetadataByIdOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Gets the latest record metadata with provided identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the ID of the object.</typeparam>
    public partial class GetLatestRecordMetadataByIdOp<TId> : ReturningOperationBase<StreamRecordMetadata<TId>>, IHaveId<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordMetadataByIdOp{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="versionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The optional strategy on how to deal with no matching record; DEFAULT is the default of the requested type or null.</param>
        public GetLatestRecordMetadataByIdOp(
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault)
        {
            this.Id = id;
            this.ObjectType = objectType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.ExistingRecordNotEncounteredStrategy = existingRecordNotEncounteredStrategy;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        public TypeRepresentation ObjectType { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the existing record not encountered strategy.
        /// </summary>
        /// <value>The existing record not encountered strategy.</value>
        public ExistingRecordNotEncounteredStrategy ExistingRecordNotEncounteredStrategy { get; private set; }
    }
}
