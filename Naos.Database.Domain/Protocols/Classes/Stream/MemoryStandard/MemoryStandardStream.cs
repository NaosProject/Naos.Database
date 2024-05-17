// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// In-memory implementation of a <see cref="StandardStreamBase"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public partial class MemoryStandardStream : StandardStreamBase, IHaveStringId
    {
        private readonly object streamLock = new object();
        private readonly object singleLocatorLock = new object();

        private Dictionary<MemoryDatabaseLocator, List<StreamRecord>> locatorToRecordPartitionMap;
        private Dictionary<MemoryDatabaseLocator, Dictionary<string, List<StreamRecordHandlingEntry>>> locatorToHandlingEntriesByConcernMap;
        private bool created;
        private long uniqueLongForExternalProtocol;
        private long uniqueLongForInMemoryRecords;
        private long uniqueLongForInMemoryHandlingEntries;
        private MemoryDatabaseLocator singleLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStandardStream"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        /// <param name="resourceLocatorProtocols">OPTIONAL resource locator protocols.  DEFAULT is to use a single <see cref="MemoryDatabaseLocator"/> named 'Default'.</param>
        /// <param name="createStreamOnConstruction">OPTIONAL value that indicates whether to create the stream (execute <see cref="StandardCreateStreamOp"/>) upon construction of this object.  DEFAULT is to create the stream.</param>
        public MemoryStandardStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            IResourceLocatorProtocols resourceLocatorProtocols = null,
            bool createStreamOnConstruction = true)
            : base(name, serializerFactory, defaultSerializerRepresentation, defaultSerializationFormat, resourceLocatorProtocols ?? new SingleResourceLocatorProtocols(new MemoryDatabaseLocator("Default")))
        {
            this.Id = Guid.NewGuid().ToString().ToUpperInvariant();

            if (createStreamOnConstruction)
            {
                this.InitializeBackingDataStructures();
            }

            this.created = createStreamOnConstruction;
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation => new MemoryStreamRepresentation(this.Name, this.Id);

        /// <inheritdoc />
        public string Id { get; private set; }

        private MemoryDatabaseLocator TryGetSingleLocator()
        {
            if (this.singleLocator != null)
            {
                return this.singleLocator;
            }
            else
            {
                lock (this.singleLocatorLock)
                {
                    if (this.singleLocator != null)
                    {
                        return this.singleLocator;
                    }

                    var allLocators = this.ResourceLocatorProtocols.Execute(new GetAllResourceLocatorsOp());

                    if (allLocators.Count != 1)
                    {
                        throw new NotSupportedException(Invariant($"The attempted operation cannot be performed because it expected a single {nameof(IResourceLocator)} to be available and there are: {allLocators.Count}."));
                    }

                    var result = allLocators.Single().ConfirmAndConvert<MemoryDatabaseLocator>();

                    this.singleLocator = result;

                    return this.singleLocator;
                }
            }
        }

        private void InitializeBackingDataStructures()
        {
            this.locatorToRecordPartitionMap = new Dictionary<MemoryDatabaseLocator, List<StreamRecord>>();
            this.locatorToHandlingEntriesByConcernMap = new Dictionary<MemoryDatabaseLocator, Dictionary<string, List<StreamRecordHandlingEntry>>>();
            this.uniqueLongForExternalProtocol = 0;
            this.uniqueLongForInMemoryRecords = 0;
            this.uniqueLongForInMemoryHandlingEntries = 0;
            this.singleLocator = null;
        }
    }
}
