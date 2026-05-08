// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfiniteWaitOneRetryStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// A mutex acquisition retry strategy that continually retries.
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class InfiniteWaitOneRetryStrategy : WaitOneRetryStrategyBase, IModelViaCodeGen
    {
    }
}
