// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using static System.FormattableString;

    /// <summary>
    /// Gets the latest record.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class GetLatestRecordOp<TObject> : ReturningOperationBase<StreamRecord<TObject>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordOp{TObject}"/> class.
        /// </summary>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        public GetLatestRecordOp(
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
        }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }
    }
}
