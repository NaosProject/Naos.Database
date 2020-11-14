// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutAndReturnInternalRecordIdOp{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Put the object and return it's internal record identifier from the storage medium.
    /// NOTE: this is only unique locally and sequential in the context of the medium itself and should generally not be used.
    /// There are occasions where this can make sense, i.e. auditing the local identifier that was received when queueing work.
    /// </summary>
    /// <typeparam name="TId">Type of identifier.</typeparam>
    /// <typeparam name="TObject">Type of data being written.</typeparam>
    public partial class PutAndReturnInternalRecordIdOp<TId, TObject> : ReturningOperationBase<long>, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutAndReturnInternalRecordIdOp{TId,TObject}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="objectToPut">The object to put into a stream.</param>
        /// <param name="tags">Optional tags to put with the record.</param>
        public PutAndReturnInternalRecordIdOp(TId id, TObject objectToPut, IReadOnlyDictionary<string, string> tags = null)
        {
            this.Id = id;
            this.ObjectToPut = objectToPut;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>The object.</value>
        public TObject ObjectToPut { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
