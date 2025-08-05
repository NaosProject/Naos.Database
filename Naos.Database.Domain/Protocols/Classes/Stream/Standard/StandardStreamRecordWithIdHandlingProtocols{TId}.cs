// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamRecordWithIdHandlingProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Set of protocols to execute record handling operations
    /// with a typed identifier and without a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public class StandardStreamRecordWithIdHandlingProtocols<TId> :
        IStreamRecordWithIdHandlingProtocols<TId>
    {
        private readonly IStandardStream stream;

        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> locatorProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamRecordWithIdHandlingProtocols{TEvent}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamRecordWithIdHandlingProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.locatorProtocols = stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            TryHandleRecordWithIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var tryHandleResult = this.stream.Execute(standardOp);

            var record = tryHandleResult.RecordToHandle;

            if (record == null)
            {
                return null;
            }

            var metadata = record.Metadata.ToStreamRecordMetadata<TId>(this.stream);

            var result = new StreamRecordWithId<TId>(record.InternalRecordId, metadata, record.Payload);

            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId>> ExecuteAsync(
            TryHandleRecordWithIdOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public CompositeHandlingStatus Execute(
            GetCompositeHandlingStatusByIdsOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var items = new List<Tuple<IResourceLocator, StringSerializedIdentifier>>();

            foreach (var id in operation.IdsToMatch)
            {
                var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(id));

                var stringSerializedIdentifier = this.stream.GetStringSerializedIdentifier(id);

                items.Add(new Tuple<IResourceLocator, StringSerializedIdentifier>(locator, stringSerializedIdentifier));
            }

            var handlingStatues = new List<HandlingStatus>();

            var groupedByLocators = items.GroupBy(_ => _.Item1).ToList();

            foreach (var locatorAndId in groupedByLocators)
            {
                var idsToMatch = locatorAndId.Select(_ => _.Item2).ToList();

                var standardizedOperation = new StandardGetHandlingStatusOp(
                    operation.Concern,
                    new RecordFilter(ids: idsToMatch, versionMatchStrategy: operation.VersionMatchStrategy),
                    new HandlingFilter(),
                    specifiedResourceLocator: locatorAndId.Key);

                var localHandlingStatusMap = this.stream.Execute(standardizedOperation);

                handlingStatues.AddRange(localHandlingStatusMap.Values);
            }

            var result = handlingStatues.ToCompositeHandlingStatus();

            return result;
        }

        /// <inheritdoc />
        public async Task<CompositeHandlingStatus> ExecuteAsync(
            GetCompositeHandlingStatusByIdsOp<TId> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
