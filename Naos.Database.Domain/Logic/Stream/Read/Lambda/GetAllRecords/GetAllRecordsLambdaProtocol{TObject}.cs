// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllRecordsLambdaProtocol{TObject}.cs" company="Naos Project">
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
    /// Implements <see cref="IGetAllRecords{TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class GetAllRecordsLambdaProtocol<TObject> :
        LambdaReturningProtocol<GetAllRecordsOp<TObject>, IReadOnlyList<StreamRecord<TObject>>>,
        IGetAllRecords<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetAllRecordsLambdaProtocol(
            Func<GetAllRecordsOp<TObject>, IReadOnlyList<StreamRecord<TObject>>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetAllRecordsLambdaProtocol(
            Func<GetAllRecordsOp<TObject>, Task<IReadOnlyList<StreamRecord<TObject>>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRecordsLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetAllRecordsLambdaProtocol(
            Func<GetAllRecordsOp<TObject>, IReadOnlyList<StreamRecord<TObject>>> synchronousLambda,
            Func<GetAllRecordsOp<TObject>, Task<IReadOnlyList<StreamRecord<TObject>>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
