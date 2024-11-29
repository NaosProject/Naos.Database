// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseMutexLambdaProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IReleaseMutex"/> by protocolizing a lambda.
    /// </summary>
    public class ReleaseMutexLambdaProtocol :
        LambdaVoidProtocol<ReleaseMutexOp>,
        IReleaseMutex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseMutexLambdaProtocol"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public ReleaseMutexLambdaProtocol(
            Action<ReleaseMutexOp> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseMutexLambdaProtocol"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public ReleaseMutexLambdaProtocol(
            Func<ReleaseMutexOp, Task> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseMutexLambdaProtocol"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public ReleaseMutexLambdaProtocol(
            Action<ReleaseMutexOp> synchronousLambda,
            Func<ReleaseMutexOp, Task> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
