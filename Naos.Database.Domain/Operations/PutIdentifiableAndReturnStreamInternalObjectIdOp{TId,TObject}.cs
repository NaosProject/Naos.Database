// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutIdentifiableAndReturnStreamInternalObjectIdOp{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// ID to be determined by the implementation of <see cref="IIdentifiableBy{TId}"/>.
    /// Put the object to the stream and return it's internal identifier.
    /// NOTE: this is only unique local and sequential in the context of the stream itself and should generally not be used.
    /// There are occasions where this can make sense, i.e. auditing the local identifier that was received when queueing work.
    /// </summary>
    /// <typeparam name="TId">Type of identifier.</typeparam>
    /// <typeparam name="TObject">Type of data being written my implement <see cref="IIdentifiableBy{TId}"/>.</typeparam>
    public partial class PutIdentifiableAndReturnStreamInternalObjectIdOp<TId, TObject> : ReturningOperationBase<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutIdentifiableAndReturnStreamInternalObjectIdOp{TId, TObject}"/> class.
        /// </summary>
        /// <param name="objectToPut">The object to put into a stream.</param>
        public PutIdentifiableAndReturnStreamInternalObjectIdOp(TObject objectToPut)
        {
            this.ObjectToPut = objectToPut;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>The object.</value>
        public TObject ObjectToPut { get; private set; }
    }
}
