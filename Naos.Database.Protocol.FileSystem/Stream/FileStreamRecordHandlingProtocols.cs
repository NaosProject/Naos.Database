// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamRecordHandlingProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IStreamRecordHandlingProtocols" />.
    /// </summary>
    public class FileStreamRecordHandlingProtocols
        : IStreamRecordHandlingProtocols
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
        private readonly FileReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamRecordHandlingProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamRecordHandlingProtocols(FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            TryHandleRecordOp operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<StreamRecord> ExecuteAsync(
            TryHandleRecordOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
