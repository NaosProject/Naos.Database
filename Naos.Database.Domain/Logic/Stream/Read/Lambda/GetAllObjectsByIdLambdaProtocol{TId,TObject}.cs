// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllObjectsByIdLambdaProtocol{TId,TObject}.cs" company="Naos Project">
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
    /// Implements <see cref="IGetAllObjectsById{TId, TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class GetAllObjectsByIdLambdaProtocol<TId, TObject> :
        LambdaReturningProtocol<GetAllObjectsByIdOp<TId, TObject>, IReadOnlyList<TObject>>,
        IGetAllObjectsById<TId, TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllObjectsByIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetAllObjectsByIdLambdaProtocol(
            Func<GetAllObjectsByIdOp<TId, TObject>, IReadOnlyList<TObject>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllObjectsByIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetAllObjectsByIdLambdaProtocol(
            Func<GetAllObjectsByIdOp<TId, TObject>, Task<IReadOnlyList<TObject>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllObjectsByIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetAllObjectsByIdLambdaProtocol(
            Func<GetAllObjectsByIdOp<TId, TObject>, IReadOnlyList<TObject>> synchronousLambda,
            Func<GetAllObjectsByIdOp<TId, TObject>, Task<IReadOnlyList<TObject>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
