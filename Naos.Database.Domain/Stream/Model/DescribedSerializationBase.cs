// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerializationBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Described serialization base to allow for different payloads.
    /// </summary>
    public abstract partial class DescribedSerializationBase : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescribedSerializationBase"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">A description of the type of object serialized.</param>
        /// <param name="serializerRepresentation">The serializer used to generate the payload.</param>
        protected DescribedSerializationBase(
            TypeRepresentation payloadTypeRepresentation,
            SerializerRepresentation serializerRepresentation)
        {
            if (payloadTypeRepresentation == null)
            {
                throw new ArgumentNullException(nameof(payloadTypeRepresentation));
            }

            if (serializerRepresentation == null)
            {
                throw new ArgumentNullException(nameof(serializerRepresentation));
            }

            this.PayloadTypeRepresentation = payloadTypeRepresentation;
            this.SerializerRepresentation = serializerRepresentation;
        }

        /// <summary>
        /// Gets a description of the type of object serialized.
        /// </summary>
        public TypeRepresentation PayloadTypeRepresentation { get; private set; }

        /// <summary>
        /// Gets the representation of the serializer used to generate the payload.
        /// </summary>
        public SerializerRepresentation SerializerRepresentation { get; private set; }

        /// <summary>
        /// Gets the format that the object was serialized into.
        /// </summary>
        public abstract SerializationFormat SerializationFormat { get; }
    }
}
