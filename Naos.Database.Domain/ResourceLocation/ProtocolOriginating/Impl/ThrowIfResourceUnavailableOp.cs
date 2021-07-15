// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowIfResourceUnavailableOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Checks to see if a resource is available and throws an exception if it is not.
    /// </summary>
    public partial class ThrowIfResourceUnavailableOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowIfResourceUnavailableOp"/> class.
        /// </summary>
        /// <param name="resourceLocator">The stream.</param>
        public ThrowIfResourceUnavailableOp(
            ResourceLocatorBase resourceLocator)
        {
            this.ResourceLocator = resourceLocator ?? throw new ArgumentNullException(nameof(resourceLocator));
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <value>The stream.</value>
        public ResourceLocatorBase ResourceLocator { get; private set; }
    }
}
