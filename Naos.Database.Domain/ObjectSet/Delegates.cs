// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delegates.cs" company="Naos Project">
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
    /// Queries for a single result using the specified filter. Throws if more than one result is returned.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="where">The filter clause limiting data to one result.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>One model result.</returns>
    public delegate Task<TModel> GetOneQueryAsync<TModel>(
        Expression<Func<TModel, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Queries for a single result using the specified filter. Throws if more than one result is returned.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="where">The filter clause limiting data to one result.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>One model result.</returns>
    public delegate Task<TModel> GetOneQueryAsync<TModel, TMetadata>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Queries for many results using the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="where">The filter clause limiting results.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching results.</returns>
    public delegate Task<IReadOnlyCollection<TModel>> GetManyQueryAsync<TModel>(
        Expression<Func<TModel, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Queries for many results using the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="where">The filter clause limiting results.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching results.</returns>
    public delegate Task<IReadOnlyCollection<TModel>> GetManyQueryAsync<TModel, TMetadata>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Queries for all results in the collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All results in the collection.</returns>
    public delegate Task<IReadOnlyCollection<TModel>> GetAllQueryAsync<TModel>(
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Queries for all results in the collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All results in the collection.</returns>
    public delegate Task<IReadOnlyCollection<TModel>> GetAllQueryAsync<TModel, TMetadata>(
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Projects a single result into a new type using the specified filter. Throws if more than one
    /// result is returned.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="where">The filter clause limiting data to one result.</param>
    /// <param name="project">The callback responsible for creating a projection from a model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>One model projection.</returns>
    public delegate Task<TProjection> ProjectOneQueryAsync<TModel, TProjection>(
        Expression<Func<TModel, bool>> where,
        Expression<Func<TModel, TProjection>> project,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Projects a single result into a new type using the specified filter. Throws if more than one
    /// result is returned.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="where">The filter clause limiting data to one result.</param>
    /// <param name="project">The callback responsible for creating a projection from a model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>One model projection.</returns>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Using types for a specific purpose here.")]
    public delegate Task<TProjection> ProjectOneQueryAsync<TModel, TMetadata, TProjection>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Projects many results into new types using the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="where">The filter clause limiting results.</param>
    /// <param name="project">The callback responsible for creating a projection from a model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching projections.</returns>
    public delegate Task<IReadOnlyCollection<TProjection>> ProjectManyQueryAsync<TModel, TProjection>(
        Expression<Func<TModel, bool>> where,
        Expression<Func<TModel, TProjection>> project,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Projects many results into new types using the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="where">The filter clause limiting results.</param>
    /// <param name="project">The callback responsible for creating a projection from a model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching projections.</returns>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Using types for a specific purpose here.")]
    public delegate Task<IReadOnlyCollection<TProjection>> ProjectManyQueryAsync<TModel, TMetadata, TProjection>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Projects all results in the collection into new types.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="project">The callback responsible for creating a projection from a model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All projected results from the collection.</returns>
    public delegate Task<IReadOnlyCollection<TProjection>> ProjectAllQueryAsync<TModel, TProjection>(
        Expression<Func<TModel, TProjection>> project,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Projects all results in the collection into new types.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="project">The callback responsible for creating a projection from a model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All projected results from the collection.</returns>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Using types for a specific purpose here.")]
    public delegate Task<IReadOnlyCollection<TProjection>> ProjectAllQueryAsync<TModel, TMetadata, TProjection>(
        Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes the specified model from the collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="model">The model to remove.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task RemoveOneCommandAsync<in TModel>(
        TModel model,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes models that match the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="where">The filter clause specifying which results should be removed.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task RemoveManyCommandAsync<TModel>(
        Expression<Func<TModel, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes models that match the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="where">The filter clause specifying which results should be removed.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task RemoveManyCommandAsync<TModel, TMetadata>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes all models in the specified collection. Collection name must be specified.
    /// </summary>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task RemoveAllCommandAsync(
        string collectionName,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes all models in the specified collection. Collection name is optional and will be
    /// generated from the model name if unspecified.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task RemoveAllCommandAsync<TModel>(
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Add one model to the collection. Throws if a model with the same Id already exists.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="model">The model to add.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task AddOneCommandAsync<in TModel>(
        TModel model,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Add one model to the collection. Throws if a model with the same Id already exists.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="model">The model to add.</param>
    /// <param name="metadata">The metadata to include.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task AddOneCommandAsync<in TModel, in TMetadata>(
        TModel model,
        TMetadata metadata = default(TMetadata),
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Adds a set of models to the collection. Throws if any of the models have the same Id as an
    /// existing model.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="models">The models to add.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task AddManyCommandAsync<in TModel>(
        IReadOnlyCollection<TModel> models,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Adds a set of models with associated metadata to the collection. Throws if any of the models
    /// have the same Id as an existing model.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="storageModels">The storage models to add.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task AddManyCommandAsync<TModel, TMetadata>(
        IReadOnlyCollection<StorageModel<TModel, TMetadata>> storageModels,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Updates an existing model in the collection. Throws if a model with the same Id cannot be found.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="model">The updated model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task UpdateOneCommandAsync<in TModel>(
        TModel model,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Updates an existing model with associated metadata in the collection. Throws if a model with
    /// the same Id cannot be found.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="model">The updated model.</param>
    /// <param name="metadata">The updated metadata.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task UpdateOneCommandAsync<in TModel, in TMetadata>(
        TModel model,
        TMetadata metadata = default(TMetadata),
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Updates a set of models in the collection. Throws if updates are specified for a model with
    /// an Id that cannot be found.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="models">The updated models.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task UpdateManyCommandAsync<TId, TModel>(
        IDictionary<TId, TModel> models,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Updates a set of existing models with associated metadata in the collection. Throws if
    /// updates are specified for a model with an Id that cannot be found.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="storageModels">The updated storage models.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Using types for a specific purpose here.")]
    public delegate Task UpdateManyCommandAsync<TId, TModel, TMetadata>(
        IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Adds or updates a model in the collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="model">The model to add or update.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task AddOrUpdateOneCommandAsync<in TModel>(
        TModel model,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Adds or updates a model with associated metadata in the collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="model">The model to add or update.</param>
    /// <param name="metadata">The associated metadata.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task AddOrUpdateOneCommandAsync<in TModel, in TMetadata>(
        TModel model,
        TMetadata metadata = default(TMetadata),
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Adds or updates a set of models in the collection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="models">The set of models to add or update.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task AddOrUpdateManyCommandAsync<TId, TModel>(
        IDictionary<TId, TModel> models,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Adds or updates a set of models with associated metadata in the collection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="storageModels">The set of storage models to add or update.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Using types for a specific purpose here.")]
    public delegate Task AddOrUpdateManyCommandAsync<TId, TModel, TMetadata>(
        IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Assumes a complete set of models is included and adds, updates, or removes as appropriate to
    /// bring collection into synchronization.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="models">The complete set of models to add, update, or remove.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task MergeCompleteSetCommandAsync<TId, TModel>(
        IDictionary<TId, TModel> models,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Assumes a complete set of models with associated metadata is included and adds, updates, or
    /// removes as appropriate to bring collection into synchronization.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    /// <param name="storageModels">The complete set of storage models to add, update, or remove.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Using types for a specific purpose here.")]
    public delegate Task MergeCompleteSetCommandAsync<TId, TModel, TMetadata>(
        IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Reads the identifier from the model provided.
    /// </summary>
    /// <param name="model">Model to read from.</param>
    /// <returns>Identifier on the provided model.</returns>
    public delegate object ReadIdFromModel(object model);
}
