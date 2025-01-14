// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllRecordsByIdLambdaProtocol{TId}.cs" company="Naos Project">
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
    /// Implements <see cref="IGetAllRecordsById{TId}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public class GetAllRecordsByIdLambdaProtocol<TId> :
        LambdaReturningProtocol<GetAllRecordsByIdOp<TId>, IReadOnlyList<StreamRecordWithId<TId>>>,
        IGetAllRecordsById<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetAllRecordsByIdLambdaProtocol(
            Func<GetAllRecordsByIdOp<TId>, IReadOnlyList<StreamRecordWithId<TId>>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetAllRecordsByIdLambdaProtocol(
            Func<GetAllRecordsByIdOp<TId>, Task<IReadOnlyList<StreamRecordWithId<TId>>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetAllRecordsByIdLambdaProtocol(
            Func<GetAllRecordsByIdOp<TId>, IReadOnlyList<StreamRecordWithId<TId>>> synchronousLambda,
            Func<GetAllRecordsByIdOp<TId>, Task<IReadOnlyList<StreamRecordWithId<TId>>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
