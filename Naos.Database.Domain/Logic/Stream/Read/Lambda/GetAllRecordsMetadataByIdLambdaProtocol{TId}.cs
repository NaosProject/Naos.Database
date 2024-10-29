// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllRecordsMetadataByIdLambdaProtocol{TId}.cs" company="Naos Project">
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
    /// Implements <see cref="IGetAllRecordsMetadataById{TId}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public class GetAllRecordsMetadataByIdLambdaProtocol<TId> :
        LambdaReturningProtocol<GetAllRecordsMetadataByIdOp<TId>, IReadOnlyList<StreamRecordMetadata<TId>>>,
        IGetAllRecordsMetadataById<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsMetadataByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetAllRecordsMetadataByIdLambdaProtocol(
            Func<GetAllRecordsMetadataByIdOp<TId>, IReadOnlyList<StreamRecordMetadata<TId>>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsMetadataByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetAllRecordsMetadataByIdLambdaProtocol(
            Func<GetAllRecordsMetadataByIdOp<TId>, Task<IReadOnlyList<StreamRecordMetadata<TId>>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsMetadataByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetAllRecordsMetadataByIdLambdaProtocol(
            Func<GetAllRecordsMetadataByIdOp<TId>, IReadOnlyList<StreamRecordMetadata<TId>>> synchronousLambda,
            Func<GetAllRecordsMetadataByIdOp<TId>, Task<IReadOnlyList<StreamRecordMetadata<TId>>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
