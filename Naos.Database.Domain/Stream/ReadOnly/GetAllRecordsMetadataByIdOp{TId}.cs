// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllRecordsMetadataByIdOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Gets all records metadata with provided identifier.
    /// </summary>
    /// <typeparam name="TId">Type of identifier.</typeparam>
    public partial class GetAllRecordsMetadataByIdOp<TId> : ReturningOperationBase<IReadOnlyList<StreamRecordMetadata<TId>>>, IIdentifiableBy<TId>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsMetadataByIdOp{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The optional strategy on how to deal with no matching record; DEFAULT is the default of the requested type or null.</param>
        /// <param name="orderRecordsStrategy">The optional strategy of how to order records; DEFAULT is ascending by internal record identifier (order of insertion).</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public GetAllRecordsMetadataByIdOp(
            TId id,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault,
            OrderRecordsStrategy orderRecordsStrategy = OrderRecordsStrategy.ByInternalRecordIdAscending,
            IResourceLocator specifiedResourceLocator = null)
        {
            this.Id = id;
            this.ObjectType = objectType;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
            this.ExistingRecordNotEncounteredStrategy = existingRecordNotEncounteredStrategy;
            this.OrderRecordsStrategy = orderRecordsStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
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
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the existing record not encountered strategy.
        /// </summary>
        /// <value>The existing record not encountered strategy.</value>
        public ExistingRecordNotEncounteredStrategy ExistingRecordNotEncounteredStrategy { get; private set; }

        /// <summary>
        /// Gets the order records strategy.
        /// </summary>
        /// <value>The order records strategy.</value>
        public OrderRecordsStrategy OrderRecordsStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
