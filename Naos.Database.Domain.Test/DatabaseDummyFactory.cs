// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseDummyFactory.cs" company="Naos Project">
//     Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A Dummy Factory for types in <see cref="Naos.Database.Domain" />.
    /// Implements the <see cref="Naos.Database.Domain.Test.DefaultDatabaseDummyFactory" />.
    /// </summary>
    /// <seealso cref="Naos.Database.Domain.Test.DefaultDatabaseDummyFactory" />
#if !NaosDatabaseSolution
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Database.Domain.Test", "See package version number")]
    internal
#else
    public
#endif
    class DatabaseDummyFactory : DefaultDatabaseDummyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDummyFactory" /> class.
        /// </summary>
        public DatabaseDummyFactory()
        {
            /* Add any overriding or custom registrations here. */
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var availableTypes = new[]
                                         {
                                             typeof(FileStreamRepresentation),
                                             typeof(MemoryStreamRepresentation),
                                         };

                    var randomIndex = ThreadSafeRandom.Next(0, availableTypes.Length);

                    var randomType = availableTypes[randomIndex];

                    var result = (IStreamRepresentation)AD.ummy(randomType);

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StreamRecordMetadata(
                    A.Dummy<Version>().ToString(),
                    A.Dummy<SerializerRepresentation>(),
                    A.Dummy<TypeRepresentation>().ToWithAndWithoutVersion(),
                    A.Dummy<TypeRepresentation>().ToWithAndWithoutVersion(),
                    A.Dummy<IReadOnlyDictionary<string, string>>(),
                    A.Dummy<DateTime>().ToUniversalTime()));
        }
    }
}
