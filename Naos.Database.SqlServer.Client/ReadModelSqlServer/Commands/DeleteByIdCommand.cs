// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteByIdCommand.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql
{
    /// <summary>
    ///     Command for deleting a single result by id from a particular model type (aka. table name).
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    public class DeleteByIdCommand<TId> : DeleteByCommand
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
