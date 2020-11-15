// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IStreamReadProtocols{TId,TObject}"/>
    /// and <see cref="IStreamWriteProtocols{TId,TObject}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class FileStreamProtocols<TId, TObject>
        : IStreamReadProtocols<TId, TObject>,
          IStreamWriteProtocols<TId, TObject>
    {
        private readonly FileReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamProtocols(
            FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            var task = this.ExecuteAsync(operation);
            var result = Run.TaskUntilCompletion(task);
            return result;
        }

        /// <inheritdoc />
        public Task<TObject> ExecuteAsync(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public long Execute(
            PutAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var task = this.ExecuteAsync(operation);
            var result = Run.TaskUntilCompletion(task);
            return result;
        }

        /// <inheritdoc />
        public Task<long> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TId, TObject> operation)
        {
            var task = this.ExecuteAsync(operation);
            Run.TaskUntilCompletion(task);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(
            PutOp<TId, TObject> operation)
        {
            throw new NotImplementedException();
        }
    }
}
