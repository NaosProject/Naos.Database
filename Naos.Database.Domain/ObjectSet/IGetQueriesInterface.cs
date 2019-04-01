// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetQueriesInterface.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Gets interfaces that allow querying specific models in a database.
    /// </summary>
    public interface IGetQueriesInterface
    {
        /// <summary>
        /// Gets an interface that only allows querying a specific model for this database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        IQueries<TModel> GetQueriesInterface<TModel>();

        /// <summary>
        /// Gets an interface that only allows querying a specific model for this database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        IQueries<TModel, TMetadata> GetQueriesInterface<TModel, TMetadata>();
    }
}
