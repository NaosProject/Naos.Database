// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using OBeautifulCode.Representation.System;
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
        /// <param name="identifierType">The optional type of the identifier; default is no filter.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        public GetLatestRecordOp(
            TypeRepresentation identifierType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            this.IdentifierType = identifierType;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
        }

        /// <summary>
        /// Gets the type of the identifier.
        /// </summary>
        /// <value>The type of the identifier.</value>
        public TypeRepresentation IdentifierType { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }
    }
}
