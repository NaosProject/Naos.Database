// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordStandardizeExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// For converting externally used read operations to the analogous standard read operations.
    /// </summary>
    public static class WriteRecordStandardizeExtensions
    {
        /// <summary>
        /// Converts to common base format <see cref="StandardGetNextUniqueLongOp"/>.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetNextUniqueLongOp Standardize(
            this GetNextUniqueLongOp operation)
        {
            var result = new StandardGetNextUniqueLongOp();

            return result;
        }
    }
}
