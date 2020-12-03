// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordWithIdOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Handles a record.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier of the record.</typeparam>
    public partial class TryHandleRecordWithIdOp<TId> : ReturningOperationBase<StreamRecordWithId<TId>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordWithIdOp{TId}"/> class.
        /// </summary>
        /// <param name="concern">The concern.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        public TryHandleRecordWithIdOp(
            string concern,
            TypeRepresentationWithAndWithoutVersion objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            this.Concern = concern;
            this.ObjectType = objectType;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
        }

        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        public TypeRepresentationWithAndWithoutVersion ObjectType { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }
    }
}
