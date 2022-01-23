// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetInternalRecordIdsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the internal record identifiers for all records matching the specified <see cref="RecordFilter"/>.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetInternalRecordIdsOp : ReturningOperationBase<IReadOnlyCollection<long>>, ISpecifyRecordFilter, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetInternalRecordIdsOp"/> class.
        /// </summary>
        /// <param name="recordFilter">The <see cref="RecordFilter"/> to use.</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StandardGetInternalRecordIdsOp(
            RecordFilter recordFilter,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            IResourceLocator specifiedResourceLocator = null)
        {
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            recordNotFoundStrategy.MustForArg(nameof(recordNotFoundStrategy)).NotBeEqualTo(RecordNotFoundStrategy.Unknown);

            this.RecordFilter = recordFilter;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
