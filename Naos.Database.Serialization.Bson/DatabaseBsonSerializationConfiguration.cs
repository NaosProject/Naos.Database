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

    /// <inheritdoc />
    public class DatabaseBsonSerializationConfiguration : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters =>
            new[]
            {
                FormattableString.Invariant($"{nameof(Naos)}.{nameof(Naos.Database)}.{nameof(Naos.Database.Domain)}"),
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
            typeof(ResourceLocatorBase).ToTypeToRegisterForBson(),
            typeof(OperationBase).ToTypeToRegisterForBson(),
            typeof(EventBase<>).ToTypeToRegisterForBson(),
        };
    }
}
