// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaitOneRetryStrategyBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class for a strategy that determines whether to retry to acquire a mutex
    /// by exclusively handling a <see cref="MutexObject"/> in a stream.
    /// </summary>
    public abstract partial class WaitOneRetryStrategyBase : IModelViaCodeGen
    {
    }
}
