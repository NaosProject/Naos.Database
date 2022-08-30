// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.Read.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using static System.FormattableString;

    public partial class MemoryStandardStream
    {
        /// <inheritdoc />
        public override IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            var matchingRecords = this.GetMatchingRecords(operation);
            var result = matchingRecords.Select(_ => _.InternalRecordId).ToList();
            return result;
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new HashSet<StringSerializedIdentifier>();
            var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());
            foreach (var locator in allLocators)
            {
                var operationWithLocator = operation.DeepCloneWithSpecifiedResourceLocator(locator);
                var matchingRecords = this.GetMatchingRecords(operationWithLocator);
                matchingRecords.ToList()
                               .ForEach(
                                    _ => result.Add(
                                        new StringSerializedIdentifier(_.Metadata.StringSerializedId, _.Metadata.TypeRepresentationOfId.WithVersion)));
            }

            return result;
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var matchingRecords = this.GetMatchingRecords(operation);

            if (matchingRecords != null && matchingRecords.Any())
            {
                var result = matchingRecords.OrderBy(_ => _.InternalRecordId).Last();
                switch (operation.StreamRecordItemsToInclude)
                {
                    case StreamRecordItemsToInclude.MetadataAndPayload:
                        return result;
                    case StreamRecordItemsToInclude.MetadataOnly:
                        var resultWithoutPayload = result.DeepCloneWithPayload(
                            new NullDescribedSerialization(
                                result.Payload.PayloadTypeRepresentation,
                                result.Payload.SerializerRepresentation));
                        return resultWithoutPayload;
                    default:
                        throw new NotSupportedException(Invariant($"Unsupported {nameof(StreamRecordItemsToInclude)}: {operation.StreamRecordItemsToInclude}."));
                }
            }

            switch (operation.RecordNotFoundStrategy)
            {
                case RecordNotFoundStrategy.ReturnDefault:
                    return null;
                case RecordNotFoundStrategy.Throw:
                    throw new InvalidOperationException(
                        Invariant(
                            $"Expected stream {this.StreamRepresentation} to contain a matching record for {operation}, none was found and {nameof(operation.RecordNotFoundStrategy)} is '{operation.RecordNotFoundStrategy}'."));
                default:
                    throw new NotSupportedException(
                        Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
            }
        }

        /// <inheritdoc />
        public override string Execute(
            StandardGetLatestStringSerializedObjectOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOp = new StandardGetLatestRecordOp(
                operation.RecordFilter,
                operation.RecordNotFoundStrategy,
                StreamRecordItemsToInclude.MetadataAndPayload,
                operation.SpecifiedResourceLocator);

            var record = this.Execute(delegatedOp);

            string result;

            if (record == null)
            {
                result = null;
            }
            else
            {
                if (record.Payload is StringDescribedSerialization stringDescribedSerialization)
                {
                    result = stringDescribedSerialization.SerializedPayload;
                }
                else
                {
                    switch (operation.RecordNotFoundStrategy)
                    {
                        case RecordNotFoundStrategy.ReturnDefault:
                            result = null;
                            break;
                        case RecordNotFoundStrategy.Throw:
                            throw new NotSupportedException(Invariant($"record {nameof(SerializationFormat)} not {SerializationFormat.String}, it is {record.Payload.GetSerializationFormat()}, but {nameof(RecordNotFoundStrategy)} is not {nameof(RecordNotFoundStrategy.ReturnDefault)}"));
                        default:
                            throw new NotSupportedException(Invariant($"{nameof(RecordNotFoundStrategy)} {operation.RecordNotFoundStrategy} is not supported."));
                    }
                }
            }

            return result;
        }
    }
}
