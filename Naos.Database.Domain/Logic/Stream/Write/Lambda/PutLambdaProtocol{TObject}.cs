// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutLambdaProtocol{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IPut{TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class PutLambdaProtocol<TObject> :
        LambdaVoidProtocol<PutOp<TObject>>,
        IPut<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public PutLambdaProtocol(
            Action<PutOp<TObject>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public PutLambdaProtocol(
            Func<PutOp<TObject>, Task> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public PutLambdaProtocol(
            Action<PutOp<TObject>> synchronousLambda,
            Func<PutOp<TObject>, Task> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
