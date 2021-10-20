// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamManagementProtocolFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Interface to get the protocols for the stream management operations.
    /// </summary>
    public interface IStreamManagementProtocolFactory
    {
        /// <summary>
        /// Builds the <see cref="IStreamManagementProtocols"/> protocol.
        /// </summary>
        /// <returns>Protocols for managing a <see cref="IStream"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Prefer method.")]
        IStreamManagementProtocols GetStreamManagementProtocols();
    }
}
