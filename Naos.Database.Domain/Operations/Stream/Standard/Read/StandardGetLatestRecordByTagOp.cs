// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetLatestRecordByTagOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the latest record with provided tag.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardReadWriteStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetLatestRecordByTagOp : ReturningOperationBase<StreamRecord>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetLatestRecordByTagOp"/> class.
        /// </summary>
        /// <param name="tag">The tag to match.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetLatestRecordByTagOp(
            NamedValue<string> tag,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            IResourceLocator specifiedResourceLocator = null)
        {
            tag.MustForArg(nameof(tag)).NotBeNull();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Tag = tag;
            this.ObjectType = objectType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the tag to match.
        /// </summary>
        public NamedValue<string> Tag { get; private set; }

        /// <summary>
        /// Gets the type object to filter on or null for no filter.
        /// </summary>
        public TypeRepresentation ObjectType { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the object type.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
