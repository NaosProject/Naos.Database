// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordByIdLambdaProtocol{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IGetLatestRecordById{TId}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public class GetLatestRecordByIdLambdaProtocol<TId> :
        LambdaReturningProtocol<GetLatestRecordByIdOp<TId>, StreamRecordWithId<TId>>,
        IGetLatestRecordById<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestRecordByIdLambdaProtocol(
            Func<GetLatestRecordByIdOp<TId>, StreamRecordWithId<TId>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestRecordByIdLambdaProtocol(
            Func<GetLatestRecordByIdOp<TId>, Task<StreamRecordWithId<TId>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetLatestRecordByIdLambdaProtocol(
            Func<GetLatestRecordByIdOp<TId>, StreamRecordWithId<TId>> synchronousLambda,
            Func<GetLatestRecordByIdOp<TId>, Task<StreamRecordWithId<TId>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
