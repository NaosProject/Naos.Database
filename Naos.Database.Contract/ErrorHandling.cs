// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorHandling.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
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
