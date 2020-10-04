// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseBsonSerializationConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using Naos.Protocol.Serialization.Bson;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Type;

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
                typeof(ProtocolBsonSerializationConfiguration).ToBsonSerializationConfigurationType(),
            };

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new TypeToRegisterForBson[]
        {
            typeof(IModel).ToTypeToRegisterForBson(),
        };
    }
}
