// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestStringSerializedObjectByIdLambdaProtocol{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IGetLatestStringSerializedObjectById{TId}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public class GetLatestStringSerializedObjectByIdLambdaProtocol<TId> :
        LambdaReturningProtocol<GetLatestStringSerializedObjectByIdOp<TId>, string>,
        IGetLatestStringSerializedObjectById<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestStringSerializedObjectByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestStringSerializedObjectByIdLambdaProtocol(
            Func<GetLatestStringSerializedObjectByIdOp<TId>, string> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestStringSerializedObjectByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestStringSerializedObjectByIdLambdaProtocol(
            Func<GetLatestStringSerializedObjectByIdOp<TId>, Task<string>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestStringSerializedObjectByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetLatestStringSerializedObjectByIdLambdaProtocol(
            Func<GetLatestStringSerializedObjectByIdOp<TId>, string> synchronousLambda,
            Func<GetLatestStringSerializedObjectByIdOp<TId>, Task<string>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
