// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardUpdateStreamHandlingOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    using static System.FormattableString;

    /// <summary>
    /// Operation to update the <see cref="HandlingStatus"/> of a given record for a given concern.
    /// </summary>
    public partial class StandardUpdateStreamHandlingOp : VoidOperationBase, IHaveDetails, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardUpdateStreamHandlingOp"/> class.
        /// </summary>
        /// <param name="newStatus">The new <see cref="HandlingStatus"/> the stream handling should have.</param>
        /// <param name="acceptableCurrentStatuses">The acceptable <see cref="HandlingStatus"/>'s that the stream handling should have.</param>
        /// <param name="details">The details about the update.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public StandardUpdateStreamHandlingOp(
            HandlingStatus newStatus,
            IReadOnlyCollection<HandlingStatus> acceptableCurrentStatuses,
            string details,
            IResourceLocator specifiedResourceLocator = null)
        {
            if (newStatus != HandlingStatus.Blocked && newStatus != HandlingStatus.Requested)
            {
                throw new ArgumentException(Invariant($"The {nameof(newStatus)} of '{newStatus}' is not supported; must be {HandlingStatus.Blocked} or {HandlingStatus.Requested}."));
            }

            acceptableCurrentStatuses.MustForArg(nameof(acceptableCurrentStatuses)).NotContainElement(newStatus, "Cannot update status to the same status.");

            if (acceptableCurrentStatuses.Any(_ => _ != HandlingStatus.Blocked && _ != HandlingStatus.Requested))
            {
                throw new ArgumentException(Invariant($"The {nameof(acceptableCurrentStatuses)} has unsupported statuses; must be {HandlingStatus.Blocked} or {HandlingStatus.Requested}."));
            }

            this.NewStatus = newStatus;
            this.AcceptableCurrentStatuses = acceptableCurrentStatuses;
            this.Details = details;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the new status to update to.
        /// </summary>
        /// <value>The new status.</value>
        public HandlingStatus NewStatus { get; private set; }

        /// <summary>
        /// Gets the acceptable current statuses.
        /// </summary>
        public IReadOnlyCollection<HandlingStatus> AcceptableCurrentStatuses { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
