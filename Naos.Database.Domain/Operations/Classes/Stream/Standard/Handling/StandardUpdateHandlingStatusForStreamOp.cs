// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardUpdateHandlingStatusForStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Update the <see cref="HandlingStatus"/> for all records in an <see cref="IRecordHandlingOnlyStream"/>.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardUpdateHandlingStatusForStreamOp : VoidOperationBase, IHaveDetails, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardUpdateHandlingStatusForStreamOp"/> class.
        /// </summary>
        /// <param name="newStatus">The new <see cref="HandlingStatus"/> to apply to all records in the stream.</param>
        /// <param name="details">Details to write to the resulting <see cref="IHandlingEvent"/>.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardUpdateHandlingStatusForStreamOp(
            HandlingStatus newStatus,
            string details,
            IResourceLocator specifiedResourceLocator = null)
        {
            newStatus.MustForArg(nameof(newStatus)).BeEqualToAnyOf(new[] { HandlingStatus.DisabledForStream, HandlingStatus.AvailableByDefault });

            this.NewStatus = newStatus;
            this.Details = details;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the new <see cref="HandlingStatus"/> to apply to all records in the stream.
        /// </summary>
        public HandlingStatus NewStatus { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
