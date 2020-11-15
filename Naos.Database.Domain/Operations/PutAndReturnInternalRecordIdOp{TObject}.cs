// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutAndReturnInternalRecordIdOp{TObject}.cs" company="Naos Project">
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
    /// Put the object to the stream and return it's internal identifier.
    /// NOTE: this is only unique local and sequential in the context of the stream itself and should generally not be used.
    /// There are occasions where this can make sense, i.e. auditing the local identifier that was received when queueing work.
    /// </summary>
    /// <typeparam name="TObject">Type of data being written.</typeparam>
    public partial class PutAndReturnInternalRecordIdOp<TObject> : ReturningOperationBase<long>, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutAndReturnInternalRecordIdOp{TObject}"/> class.
        /// </summary>
        /// <param name="objectToPut">The object to put into a stream.</param>
        /// <param name="tags">Optional tags to put with the record.</param>
        public PutAndReturnInternalRecordIdOp(TObject objectToPut, IReadOnlyDictionary<string, string> tags = null)
        {
            this.ObjectToPut = objectToPut;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>The object.</value>
        public TObject ObjectToPut { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
