// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordHandlingProtocolFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to get the protocols for record handling operations.
    /// </summary>
    public interface IStreamRecordHandlingProtocolFactory
    {
        /// <summary>
        /// Gets the stream record handling protocols.
        /// </summary>
        /// <returns>Stream record handling protocols.</returns>
        IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols();

        /// <summary>
        /// Gets the stream record handling protocols.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Stream record handling protocols.</returns>
        IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>();

        /// <summary>
        /// Gets the stream record with identifier handling protocols.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <returns>Stream record with identifier handling protocols.</returns>
        IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>();

        /// <summary>
        /// Gets the stream record with identifier handling protocols.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>Stream record with identifier handling protocols.</returns>
        IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>();
    }
}
