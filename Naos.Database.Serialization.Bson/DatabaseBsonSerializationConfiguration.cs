// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseBsonSerializationConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Naos.Diagnostics.Serialization.Bson;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    /// <inheritdoc />
    public class DatabaseBsonSerializationConfiguration : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters =>
            new[]
            {
                Naos.Database.Domain.ProjectInfo.Namespace,
            };

        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes =>
            new[]
            {
                typeof(DiagnosticsBsonSerializationConfiguration).ToBsonSerializationConfigurationType(),
            };

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new Type[0]
           .Concat(new[] { typeof(IModel) })
           .Concat(Naos.Database.Domain.ProjectInfo.Assembly.GetPublicEnumTypes())
           .Select(_ => _.ToTypeToRegisterForBson())
           .ToList();
    }
}
