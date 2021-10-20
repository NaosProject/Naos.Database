// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllResourceLocatorsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to get all <see cref="IResourceLocator"/>s used in the context.
    /// </summary>
    public partial class GetAllResourceLocatorsOp : ReturningOperationBase<IReadOnlyCollection<IResourceLocator>>
    {
    }
}
