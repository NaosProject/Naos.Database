// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordMetadataByIdLambdaProtocol{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IGetLatestRecordMetadataById{TId}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public class GetLatestRecordMetadataByIdLambdaProtocol<TId> :
        LambdaReturningProtocol<GetLatestRecordMetadataByIdOp<TId>, StreamRecordMetadata<TId>>,
        IGetLatestRecordMetadataById<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordMetadataByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestRecordMetadataByIdLambdaProtocol(
            Func<GetLatestRecordMetadataByIdOp<TId>, StreamRecordMetadata<TId>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordMetadataByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestRecordMetadataByIdLambdaProtocol(
            Func<GetLatestRecordMetadataByIdOp<TId>, Task<StreamRecordMetadata<TId>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordMetadataByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetLatestRecordMetadataByIdLambdaProtocol(
            Func<GetLatestRecordMetadataByIdOp<TId>, StreamRecordMetadata<TId>> synchronousLambda,
            Func<GetLatestRecordMetadataByIdOp<TId>, Task<StreamRecordMetadata<TId>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
