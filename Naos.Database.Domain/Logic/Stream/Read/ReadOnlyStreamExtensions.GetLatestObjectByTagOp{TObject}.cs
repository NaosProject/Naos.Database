// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStreamExtensions.GetLatestObjectByTagOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    public static partial class ReadOnlyStreamExtensions
    {
        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="tag">The tag to match.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static TObject GetLatestObjectByTag<TObject>(
            this IReadOnlyStream stream,
            NamedValue<string> tag,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestObjectByTagOp<TObject>(tag, versionMatchStrategy, recordNotFoundStrategy);
            var protocol = stream.GetStreamReadingProtocols<TObject>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="tag">The tag to match.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static async Task<TObject> GetLatestObjectByTagAsync<TObject>(
            this IReadOnlyStream stream,
            NamedValue<string> tag,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestObjectByTagOp<TObject>(tag, versionMatchStrategy, recordNotFoundStrategy);
            var protocol = stream.GetStreamReadingProtocols<TObject>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
