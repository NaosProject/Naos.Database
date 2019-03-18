// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetByIdQuery.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql
{
    /// <summary>
    ///     Query getting a single result by id from a particular model type (aka. table name).
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class GetByIdQuery<TId, TResult> : GetOneByQuery<TResult>
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public TId Id { get; set; }
    }
}
