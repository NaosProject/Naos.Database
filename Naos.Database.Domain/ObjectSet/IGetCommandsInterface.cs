// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetCommandsInterface.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Gets interfaces that allow executing commands against specific models in a database.
    /// </summary>
    public interface IGetCommandsInterface
    {
        /// <summary>
        /// Gets an interface that only allows executing commands against a specific model for this database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        ICommands<TId, TModel> GetCommandsInterface<TId, TModel>();

        /// <summary>
        /// Gets an interface that only allows executing commands against a specific model for this database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        ICommands<TId, TModel, TMetadata> GetCommandsInterface<TId, TModel, TMetadata>();
    }
}