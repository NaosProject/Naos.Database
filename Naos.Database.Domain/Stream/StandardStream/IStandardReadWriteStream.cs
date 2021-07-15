// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;

    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Standard streams reduced to core calls to participate in the StandardStream specific protocols.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public interface IStandardReadWriteStream
        :
          IReadWriteStream,
          IStreamManagementProtocolFactory,
          IStreamRecordHandlingProtocolFactory,
          IStreamManagementProtocols,
          ISyncReturningProtocol<GetNextUniqueLongOp, long>,
          ISyncReturningProtocol<GetHandlingHistoryOfRecordOp, IReadOnlyList<StreamRecordHandlingEntry>>,
          ISyncReturningProtocol<GetHandlingStatusOfRecordsByIdOp, HandlingStatus>,
          ISyncReturningProtocol<GetHandlingStatusOfRecordSetByTagOp, HandlingStatus>,
          ISyncReturningProtocol<TryHandleRecordOp, TryHandleRecordResult>,
          ISyncReturningProtocol<PutRecordOp, PutRecordResult>,
          ISyncVoidProtocol<BlockRecordHandlingOp>,
          ISyncVoidProtocol<CancelBlockedRecordHandlingOp>,
          ISyncVoidProtocol<CancelHandleRecordExecutionRequestOp>,
          ISyncVoidProtocol<CancelRunningHandleRecordExecutionOp>,
          ISyncVoidProtocol<CompleteRunningHandleRecordExecutionOp>,
          ISyncVoidProtocol<FailRunningHandleRecordExecutionOp>,
          ISyncVoidProtocol<SelfCancelRunningHandleRecordExecutionOp>,
          ISyncVoidProtocol<RetryFailedHandleRecordExecutionOp>
    {
        /// <summary>
        /// Gets the serializer factory.
        /// </summary>
        /// <value>The serializer factory.</value>
        ISerializerFactory SerializerFactory { get; }

        /// <summary>
        /// Gets the default serializer representation.
        /// </summary>
        /// <value>The default serializer representation.</value>
        SerializerRepresentation DefaultSerializerRepresentation { get; }

        /// <summary>
        /// Gets the default serialization format.
        /// </summary>
        /// <value>The default serialization format.</value>
        SerializationFormat DefaultSerializationFormat { get; }
    }
}
