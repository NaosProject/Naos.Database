// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueries.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an object for executing queries for simple models. Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IQueries<TModel>
    {
        /// <summary>
        /// Executes query to get the specified model into the requested form. Throws if filter does
        /// not produce a single result.
        /// </summary>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future result.</returns>
        Task<TModel> GetOneAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query to get the specified subset of models into the requested form.
        /// </summary>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of filtered results.</returns>
        Task<IReadOnlyCollection<TModel>> GetManyAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query to get all models into the requested form.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of results.</returns>
        Task<IReadOnlyCollection<TModel>> GetAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query and projects the specified model into the requested form. Throws if
        /// filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected result.</returns>
        Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query and projects the specified subset of models into the requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of filtered results.</returns>
        Task<IReadOnlyCollection<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query and projects all models into the requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of results.</returns>
        Task<IReadOnlyCollection<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }

    /// <summary>
    /// Represents an object for executing queries for storage models. Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    public interface IQueries<TModel, TMetadata>
    {
        /// <summary>
        /// Executes query to get the specified model with metadata into the requested form. Throws
        /// if filter does not produce a single result.
        /// </summary>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future result.</returns>
        Task<TModel> GetOneAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query to get the specified subset of models with metadata into the requested form.
        /// </summary>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of filtered results.</returns>
        Task<IReadOnlyCollection<TModel>> GetManyAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query to get all models with metadata into the requested form.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of results.</returns>
        Task<IReadOnlyCollection<TModel>> GetAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query and projects the specified model with metadata into the requested form.
        /// Throws if filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected result.</returns>
        Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query and projects the specified subset of models with metadata into the
        /// requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of filtered results.</returns>
        Task<IReadOnlyCollection<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query and projects all models with metadata into the requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of results.</returns>
        Task<IReadOnlyCollection<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
