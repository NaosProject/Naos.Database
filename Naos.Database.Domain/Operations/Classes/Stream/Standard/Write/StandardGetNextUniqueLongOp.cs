// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetNextUniqueLongOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Get a unique 64-bit integer, in sequence, that is brokered via an internal stream construct.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetNextUniqueLongOp : ReturningOperationBase<long>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetNextUniqueLongOp"/> class.
        /// </summary>
        /// <param name="details">The optional details.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetNextUniqueLongOp(
            string details = null,
            IResourceLocator specifiedResourceLocator = null)
        {
            this.SpecifiedResourceLocator = specifiedResourceLocator;
            this.Details = details;
        }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        public string Details { get; private set; }
    }
}
