// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandWithReturn.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    ///     A command operation that returns a result. Consider using ICommand and IQuery to ICommandWithReturn instead.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ICommandWithReturn<TResult>
    {
    }
}
