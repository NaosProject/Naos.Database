// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Write.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    public static partial class RecordStandardizeExtensions
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
