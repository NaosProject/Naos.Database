// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Handling.StateChange.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;

    public static partial class RecordStandardizeExtensions
    {
        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardUpdateHandlingStatusForRecordOp Standardize(
            this CancelRunningHandleRecordOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterExternalCancellation,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardUpdateHandlingStatusForRecordOp Standardize(
            this CompleteRunningHandleRecordOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.Completed,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardUpdateHandlingStatusForRecordOp Standardize(
            this DisableHandlingForRecordOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                null,
                HandlingStatus.DisabledForRecord,
                new[]
                {
                    HandlingStatus.AvailableByDefault,
                    HandlingStatus.AvailableAfterExternalCancellation,
                    HandlingStatus.AvailableAfterFailure,
                    HandlingStatus.AvailableAfterSelfCancellation,
                    HandlingStatus.Running,
                    HandlingStatus.Completed,
                    HandlingStatus.Failed,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardUpdateHandlingStatusForStreamOp Standardize(
            this DisableHandlingForStreamOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardUpdateHandlingStatusForStreamOp(
                HandlingStatus.DisabledForStream,
                operation.Details,
                operation.Tags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardUpdateHandlingStatusForStreamOp Standardize(
            this EnableHandlingForStreamOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardUpdateHandlingStatusForStreamOp(
                HandlingStatus.AvailableByDefault,
                operation.Details,
                operation.Tags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardUpdateHandlingStatusForRecordOp Standardize(
            this FailRunningHandleRecordOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.Failed,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardUpdateHandlingStatusForRecordOp Standardize(
            this ResetFailedHandleRecordOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterFailure,
                new[]
                {
                    HandlingStatus.Failed,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardUpdateHandlingStatusForRecordOp Standardize(
            this SelfCancelRunningHandleRecordOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardUpdateHandlingStatusForRecordOp(
                operation.InternalRecordId,
                operation.Concern,
                HandlingStatus.AvailableAfterSelfCancellation,
                new[]
                {
                    HandlingStatus.Running,
                },
                operation.Details,
                operation.Tags,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }
    }
}
