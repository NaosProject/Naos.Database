// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.CodeAnalysis.Recipes;


    /// <summary>
    /// Stream interface, a stream is a list of objects ordered by timestamp.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Naming",
        "CA1711:IdentifiersShouldNotHaveIncorrectSuffix",
        Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public interface IReadWriteStream
        : IStream,
          IReadOnlyStream,
          IWriteOnlyStream
    {
    }
}
