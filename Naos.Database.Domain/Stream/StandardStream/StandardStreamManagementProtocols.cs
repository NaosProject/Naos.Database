// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamManagementProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Set of protocols to perform management operations.
    /// Implements the <see cref="IStandardStreamManagementProtocols" />.
    /// </summary>
    public class StandardStreamManagementProtocols : IStreamManagementProtocols
    {
        private readonly IStandardStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamManagementProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamManagementProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordDateOp operation)
        {
            this.stream.Execute(operation.Standardize());
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordDateOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordIdOp operation)
        {
            this.stream.Execute(operation.Standardize());
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordIdOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }
    }
}
