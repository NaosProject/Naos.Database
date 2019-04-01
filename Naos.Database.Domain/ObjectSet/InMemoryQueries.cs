// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryQueries.cs" company="Naos Project">
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
    /// Provides an object model for executing queries for simple models using an in memory
    /// database. Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal sealed class InMemoryQueries<TModel> : IQueries<TModel>
    {
        private readonly InMemoryDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryQueries{TModel}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal InMemoryQueries(InMemoryDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc/>
        public async Task<TModel> GetOneAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.GetOneQueryAsync(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TModel>> GetManyAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.GetManyQueryAsync(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TModel>> GetAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.GetAllQueryAsync<TModel>(collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.ProjectOneQueryAsync(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.ProjectManyQueryAsync(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.ProjectAllQueryAsync(project, collectionName, cancellationToken);
        }
    }

    /// <summary>
    /// Provides an object model for executing queries for storage models using an in memory
    /// database. Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    internal sealed class InMemoryQueries<TModel, TMetadata> : IQueries<TModel, TMetadata>
    {
        private readonly InMemoryDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryQueries{TModel, TMetadata}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal InMemoryQueries(InMemoryDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc/>
        public async Task<TModel> GetOneAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.GetOneQueryAsync(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TModel>> GetManyAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.GetManyQueryAsync(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TModel>> GetAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.GetAllQueryAsync<TModel, TMetadata>(collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.ProjectOneQueryAsync(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.ProjectManyQueryAsync(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.database.ProjectAllQueryAsync(project, collectionName, cancellationToken);
        }
    }
}
