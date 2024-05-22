// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Set of protocols to execute read and write operations on a stream,
    /// without a typed identifier and without a typed record payload.
    /// </summary>
    public class StandardStreamReadWriteProtocols :
        IStreamReadProtocols,
        IStreamWriteProtocols
    {
        private readonly IStandardStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamReadWriteProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamReadWriteProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var result = this.stream.Execute(standardOp);

            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            GetNextUniqueLongOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <summary>
        /// Orders the specified collection of <see cref="StreamRecord"/> and converts each to <typeparamref name="TStreamRecord"/>.
        /// </summary>
        /// <typeparam name="TStreamRecord">The type of stream record to convert to.</typeparam>
        /// <param name="records">The records to order and convert.</param>
        /// <param name="orderRecordsBy">Determines how to order the records.</param>
        /// <param name="conversionFunc">The func to use to convert each record to the specified <typeparamref name="TStreamRecord"/>.</param>
        /// <returns>
        /// The records, ordered by <paramref name="orderRecordsBy"/> and converted to <typeparamref name="TStreamRecord"/> using the specified <paramref name="conversionFunc"/>.
        /// </returns>
        public static IReadOnlyList<TStreamRecord> OrderAndConvertToTypedStreamRecords<TStreamRecord>(
            IReadOnlyList<StreamRecord> records,
            OrderRecordsBy orderRecordsBy,
            Func<StreamRecord, TStreamRecord> conversionFunc)
        {
            records.MustForArg(nameof(records)).NotBeNull();
            orderRecordsBy.MustForArg(nameof(orderRecordsBy)).NotBeEqualTo(OrderRecordsBy.Unknown);
            conversionFunc.MustForArg(nameof(conversionFunc)).NotBeNull();

            List<TStreamRecord> result;

            switch (orderRecordsBy)
            {
                case OrderRecordsBy.InternalRecordIdAscending:
                    result = records
                        .OrderBy(_ => _.InternalRecordId)
                        .Select(conversionFunc)
                        .ToList();
                    break;
                case OrderRecordsBy.InternalRecordIdDescending:
                    result = records
                        .OrderByDescending(_ => _.InternalRecordId)
                        .Select(conversionFunc)
                        .ToList();
                    break;
                case OrderRecordsBy.Random:
                    result = records
                        .OrderBy(_ => Guid.NewGuid())
                        .Select(conversionFunc)
                        .ToList();
                    break;
                default:
                    throw new NotSupportedException(
                        Invariant($"Unsupported {nameof(OrderRecordsBy)}: {orderRecordsBy}."));
            }

            return result;
        }
    }
}
