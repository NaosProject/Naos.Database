// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllRecordsMetadataLambdaProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IGetAllRecordsMetadata"/> by protocolizing a lambda.
    /// </summary>
    public class GetAllRecordsMetadataLambdaProtocol :
        LambdaReturningProtocol<GetAllRecordsMetadataOp, IReadOnlyList<StreamRecordMetadata>>,
        IGetAllRecordsMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsMetadataLambdaProtocol"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetAllRecordsMetadataLambdaProtocol(
            Func<GetAllRecordsMetadataOp, IReadOnlyList<StreamRecordMetadata>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsMetadataLambdaProtocol"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetAllRecordsMetadataLambdaProtocol(
            Func<GetAllRecordsMetadataOp, Task<IReadOnlyList<StreamRecordMetadata>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsMetadataLambdaProtocol"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetAllRecordsMetadataLambdaProtocol(
            Func<GetAllRecordsMetadataOp, IReadOnlyList<StreamRecordMetadata>> synchronousLambda,
            Func<GetAllRecordsMetadataOp, Task<IReadOnlyList<StreamRecordMetadata>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
