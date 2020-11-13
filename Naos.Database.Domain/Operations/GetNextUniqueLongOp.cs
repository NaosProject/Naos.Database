// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetNextUniqueLongOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Get a unique 64-bit integer, in sequence, that is brokered via an internal stream construct.
    /// </summary>
    public partial class GetNextUniqueLongOp : ReturningOperationBase<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNextUniqueLongOp"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GetNextUniqueLongOp(
            string context = null)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public string Context { get; private set; }
    }
}
