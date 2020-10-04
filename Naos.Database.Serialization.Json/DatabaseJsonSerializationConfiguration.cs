// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseJsonSerializationConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using Naos.Protocol.Serialization.Json;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;

    /// <inheritdoc />
    public class DatabaseJsonSerializationConfiguration : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters =>
            new[]
            {
                Naos.Database.Domain.ProjectInfo.Namespace,
            };

        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes =>
            new[]
            {
                typeof(ProtocolJsonSerializationConfiguration).ToJsonSerializationConfigurationType(),
            };

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new TypeToRegisterForJson[]
        {
            typeof(IModel).ToTypeToRegisterForJson(),
        };
    }
}
