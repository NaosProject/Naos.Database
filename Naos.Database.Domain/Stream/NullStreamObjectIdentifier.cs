// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStreamObjectIdentifier.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;

    /// <summary>
    /// A null object to be used as the id of an object in a <see cref="IStream"/> that does not have an actual identifier.
    /// </summary>
    public class NullStreamObjectIdentifier : IModelViaCodeGen
    {
    }
}
