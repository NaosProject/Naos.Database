// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
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
    /// File system implementation of <see cref="IStreamReadProtocols{TObject}"/>
    /// and <see cref="IStreamWriteProtocols{TObject}"/>.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class FileStreamProtocols<TObject>
        : IStreamReadProtocols<TObject>,
          IStreamWriteProtocols<TObject>
    {
        private readonly FileReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamProtocols(
            FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public long Execute(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var task = this.ExecuteAsync(operation);
            var result = Run.TaskUntilCompletion(task);
            return result;
        }

        /// <inheritdoc />
        public Task<long> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {
            var task = this.ExecuteAsync(operation);
            Run.TaskUntilCompletion(task);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(
            PutOp<TObject> operation)
        {
            throw new NotImplementedException();
        }
    }
}
