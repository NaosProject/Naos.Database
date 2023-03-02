// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeHandlingStatus.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A composite of the <see cref="HandlingStatus"/> of one or more records.
    /// </summary>
    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Zero is an invalid state, which is why it is not named 'None'.")]
    public enum CompositeHandlingStatus
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// None of the records are available to be handled.
        /// </summary>
        NoneAvailable = 1,

        /// <summary>
        /// One or more of the records are available to be handled.
        /// </summary>
        SomeAvailable = 2,

        /// <summary>
        /// None of the records were handled and an error occurred when executing.
        /// </summary>
        NoneFailed = 4,

        /// <summary>
        /// One or more of the records was handled but an error occurred when executing.
        /// </summary>
        SomeFailed = 8,

        /// <summary>
        /// None of the records were handled and executed without an error.
        /// </summary>
        NoneCompleted = 16,

        /// <summary>
        /// One or more of the records were handled and executed without an error.
        /// </summary>
        SomeCompleted = 32,

        /// <summary>
        /// None of the records are being handled.
        /// </summary>
        NoneRunning = 64,

        /// <summary>
        /// One or more of the records are being handled.
        /// </summary>
        SomeRunning = 128,

        /// <summary>
        /// None of the records have their handling disabled.
        /// </summary>
        NoneDisabled = 256,

        /// <summary>
        /// One or more of the records have their handling disabled.
        /// </summary>
        SomeDisabled = 512,

        /// <summary>
        /// None of the records have their handling archived after failure.
        /// </summary>
        NoneArchivedAfterFailure = 1024,

        /// <summary>
        /// One or more of the records have their handling archived after failure.
        /// </summary>
        SomeArchivedAfterFailure = 2048,

        /// <summary>
        /// All of the records are being handled.
        /// </summary>
        AllRunning = NoneAvailable | NoneFailed | NoneCompleted | SomeRunning | NoneDisabled | NoneArchivedAfterFailure,

        /// <summary>
        /// All of the records were handled and each executed without an error.
        /// </summary>
        AllCompleted = NoneAvailable | NoneFailed | SomeCompleted | NoneRunning | NoneDisabled | NoneArchivedAfterFailure,

        /// <summary>
        /// All of the records were handled but errors occurred when executing each of them.
        /// </summary>
        AllFailed = NoneAvailable | SomeFailed | NoneCompleted | NoneRunning | NoneDisabled | NoneArchivedAfterFailure,

        /// <summary>
        /// All of the records are available to be handled.
        /// </summary>
        AllAvailable = SomeAvailable | NoneFailed | NoneCompleted | NoneRunning | NoneDisabled | NoneArchivedAfterFailure,

        /// <summary>
        /// All of the records are have their handling disabled.
        /// </summary>
        AllDisabled = NoneAvailable | NoneFailed | NoneCompleted | NoneRunning | SomeDisabled | NoneArchivedAfterFailure,

        /// <summary>
        /// All of the records are have their handling archived after failures.
        /// </summary>
        AllArchivedAfterFailure = NoneAvailable | NoneFailed | NoneCompleted | NoneRunning | NoneDisabled | SomeArchivedAfterFailure,
    }
}
