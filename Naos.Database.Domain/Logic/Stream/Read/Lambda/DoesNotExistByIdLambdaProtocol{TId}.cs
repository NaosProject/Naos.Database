// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoesNotExistByIdLambdaProtocol{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IDoesAnyExistById{TId}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public class DoesNotExistByIdLambdaProtocol<TId> :
        LambdaReturningProtocol<DoesAnyExistByIdOp<TId>, bool>,
        IDoesAnyExistById<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoesNotExistByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public DoesNotExistByIdLambdaProtocol(
            Func<DoesAnyExistByIdOp<TId>, bool> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoesNotExistByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public DoesNotExistByIdLambdaProtocol(
            Func<DoesAnyExistByIdOp<TId>, Task<bool>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoesNotExistByIdLambdaProtocol{TId}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public DoesNotExistByIdLambdaProtocol(
            Func<DoesAnyExistByIdOp<TId>, bool> synchronousLambda,
            Func<DoesAnyExistByIdOp<TId>, Task<bool>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
