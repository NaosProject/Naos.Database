// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJobInformation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to represent job information for use in operations like <see cref="GetLatestJobInformationOp" />.
    /// </summary>
    public interface IJobInformation
    {
        /// <summary>
        /// Gets the name of the job.
        /// </summary>
        string JobName { get; }
    }
}
