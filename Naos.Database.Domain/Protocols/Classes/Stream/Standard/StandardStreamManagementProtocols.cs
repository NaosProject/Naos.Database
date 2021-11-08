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
    /// Set of protocols to execute stream management operations.
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
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordDateOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            this.stream.Execute(standardOp);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordIdOp operation)
        {
            this.Execute(operation);

            await Task.FromResult(true);
        }
    }
}
