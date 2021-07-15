// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllResourceLocatorsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to get all <see cref="IResourceLocator"/>'s used in the context.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AllStream", Justification = NaosSuppressBecause.CA1702_CompoundWordsShouldBeCasedCorrectly_AnalyzerIsIncorrectlyDetectingCompoundWords)]
    public partial class GetAllResourceLocatorsOp : ReturningOperationBase<IReadOnlyCollection<IResourceLocator>>
    {
    }
}
