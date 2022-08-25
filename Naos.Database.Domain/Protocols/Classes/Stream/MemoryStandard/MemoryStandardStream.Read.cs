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
            var matchingRecords = this.GetMatchingRecords(operation);
            matchingRecords.ToList()
                           .ForEach(
                                _ => result.Add(
                                    new StringSerializedIdentifier(_.Metadata.StringSerializedId, _.Metadata.TypeRepresentationOfId.WithVersion)));

            return result;
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = this.GetMatchingRecords(operation);

            if (result != null && result.Any())
            {
                return result.OrderBy(_ => _.InternalRecordId).Last();
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
