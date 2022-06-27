// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStreamWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A null object pattern implementation of an <see cref="IStreamWriteWithIdProtocols{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public class NullStandardStreamWriteWithIdProtocols<TId> : IStreamWriteWithIdProtocols<TId>
    {
    }
}
