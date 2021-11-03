// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetNextUniqueLongOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Get a unique 64-bit integer, in sequence, that is brokered via an internal stream construct.
    /// </summary>
    public partial class GetNextUniqueLongOp : ReturningOperationBase<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNextUniqueLongOp"/> class.
        /// </summary>
        /// <param name="details">The optional details.</param>
        public GetNextUniqueLongOp(
            string details = null)
        {
            this.Details = details;
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        public string Details { get; private set; }
    }
}
