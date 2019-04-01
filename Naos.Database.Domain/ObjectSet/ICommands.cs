// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommands.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an object for executing commands with simple models. Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface ICommands<TId, TModel>
    {
        /// <summary>
        /// Executes the command to remove one model from the database.
        /// </summary>
        /// <param name="model">The model to remove.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task RemoveOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to remove a set of models matching the specified filter from the database.
        /// </summary>
        /// <param name="where">The where filter indicating which models to remove.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task RemoveManyAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to remove all models from the database.
        /// </summary>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task RemoveAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to add one model to the database. Throws if model is already present.
        /// </summary>
        /// <param name="model">The model to add.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task AddOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to add a set of models to the database. Throws if any models are
        /// already present.
        /// </summary>
        /// <param name="models">The models to add.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task AddManyAsync(
            IReadOnlyCollection<TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to update a model in the database. Throws if model with matching
        /// identifier cannot be found.
        /// </summary>
        /// <param name="model">The model to update.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task UpdateOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to update a set of models in the database. Throws if any models
        /// cannot be found by their identifiers.
        /// </summary>
        /// <param name="models">The models to update.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task UpdateManyAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to add or update a model in the database.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task AddOrUpdateOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to add or update a set of models in the database.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task AddOrUpdateManyAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command that assumes a complete set of models is provided and proceeds to
        /// add, update, or remove models in the database to bring it in sync.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task MergeCompleteSetAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }

    /// <summary>
    /// Represents an object for executing commands with storage models. Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Using types for a specific purpose here.")]
    public interface ICommands<TId, TModel, TMetadata>
    {
        /// <summary>
        /// Executes the command to remove one model from the database.
        /// </summary>
        /// <param name="model">The model to remove.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task RemoveOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to remove a set of storage models matching the specified filter
        /// from the database.
        /// </summary>
        /// <param name="where">The where filter indicating which storage models to remove.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task RemoveManyAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to remove all storage models from the database.
        /// </summary>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task RemoveAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to add one model to the database. Throws if model is already present.
        /// </summary>
        /// <param name="model">The model to add.</param>
        /// <param name="metadata">The metadata to include.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task AddOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to add a set of storage models to the database. Throws if any
        /// storage models are already present.
        /// </summary>
        /// <param name="storageModels">The storage models to add.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task AddManyAsync(
            IReadOnlyCollection<StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to update a model and its metadata in the database. Throws if
        /// storage model with matching identifier cannot be found.
        /// </summary>
        /// <param name="model">The model to update.</param>
        /// <param name="metadata">The metadata to include.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task UpdateOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to update a set of storage models in the database. Throws if any
        /// storage models cannot be found by their identifiers.
        /// </summary>
        /// <param name="storageModels">The storage models to update.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task UpdateManyAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to add or update a model and its metadata in the database.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task AddOrUpdateOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command to add or update a set of storage models in the database.
        /// </summary>
        /// <param name="storageModels">The storage models.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task AddOrUpdateManyAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes the command that assumes a complete set of storage models is provided and
        /// proceeds to add, update, or remove storage models in the database to bring it in sync.
        /// </summary>
        /// <param name="storageModels">The storage models.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task MergeCompleteSetAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
