// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoesAnyExistByIdLambdaProtocol{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IDoesAnyExistById{TId, TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class DoesAnyExistByIdLambdaProtocol<TId, TObject> :
        LambdaReturningProtocol<DoesAnyExistByIdOp<TId, TObject>, bool>,
        IDoesAnyExistById<TId, TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoesAnyExistByIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public DoesAnyExistByIdLambdaProtocol(
            Func<DoesAnyExistByIdOp<TId, TObject>, bool> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoesAnyExistByIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public DoesAnyExistByIdLambdaProtocol(
            Func<DoesAnyExistByIdOp<TId, TObject>, Task<bool>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoesAnyExistByIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public DoesAnyExistByIdLambdaProtocol(
            Func<DoesAnyExistByIdOp<TId, TObject>, bool> synchronousLambda,
            Func<DoesAnyExistByIdOp<TId, TObject>, Task<bool>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
