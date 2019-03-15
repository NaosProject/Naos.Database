// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorHandling.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Determines what to do when a checksum fails.
    /// </summary>
    public enum ErrorHandling
    {
        /// <summary>
        /// No error handling specified.
        /// </summary>
        None,

        /// <summary>
        /// Fail if a page checksum does not verify.
        /// </summary>
        StopOnError,

        /// <summary>
        /// Continue despite encountering errors such as invalid checksums or torn pages.
        /// </summary>
        ContinueAfterError,
    }
}
