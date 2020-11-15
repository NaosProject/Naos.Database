// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutOp{TObject}.cs" company="Naos Project">
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
    /// Put the object to a <see cref="IWriteOnlyStream"/>.
    /// </summary>
    /// <typeparam name="TObject">Type of data being written.</typeparam>
    public partial class PutOp<TObject> : VoidOperationBase, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutOp{TObject}"/> class.
        /// </summary>
        /// <param name="objectToPut">The object to put into a stream.</param>
        /// <param name="tags">Optional tags to put with the record.</param>
        public PutOp(TObject objectToPut, IReadOnlyDictionary<string, string> tags = null)
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
