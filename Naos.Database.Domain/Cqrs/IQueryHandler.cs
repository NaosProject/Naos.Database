// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryHandler.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    ///     Represents a query handler (typically a database read operation).
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IQueryHandler<in TQuery, out TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        ///     Handles the specified query.
        /// </summary>
        /// <param name="query">The query to handle.</param>
        /// <returns>The query results.</returns>
        TResult Handle(TQuery query);
    }
}
