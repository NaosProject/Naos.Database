// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorHandling.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Backup
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
        ContinueAfterError
    }
}
