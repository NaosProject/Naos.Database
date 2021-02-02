﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.145.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Globalization;
    using global::System.Linq;

    using global::Naos.Protocol.Domain;

    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class RequestedHandleRecordExecutionEvent : IModel<RequestedHandleRecordExecutionEvent>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="RequestedHandleRecordExecutionEvent"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(RequestedHandleRecordExecutionEvent left, RequestedHandleRecordExecutionEvent right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = left.Equals(right);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="RequestedHandleRecordExecutionEvent"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(RequestedHandleRecordExecutionEvent left, RequestedHandleRecordExecutionEvent right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(RequestedHandleRecordExecutionEvent other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.TimestampUtc.IsEqualTo(other.TimestampUtc)
                      && this.Id.IsEqualTo(other.Id)
                      && this.RecordToHandle.IsEqualTo(other.RecordToHandle)
                      && this.Details.IsEqualTo(other.Details, StringComparer.Ordinal);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as RequestedHandleRecordExecutionEvent);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.TimestampUtc)
            .Hash(this.Id)
            .Hash(this.RecordToHandle)
            .Hash(this.Details)
            .Value;

        /// <inheritdoc />
        public new RequestedHandleRecordExecutionEvent DeepClone() => (RequestedHandleRecordExecutionEvent)this.DeepCloneInternal();

        /// <inheritdoc />
        protected override EventBaseBase DeepCloneInternal()
        {
            var result = new RequestedHandleRecordExecutionEvent(
                                 this.Id,
                                 this.TimestampUtc,
                                 this.RecordToHandle?.DeepClone(),
                                 this.Details?.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.RequestedHandleRecordExecutionEvent: TimestampUtc = {this.TimestampUtc.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, Id = {this.Id.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, RecordToHandle = {this.RecordToHandle?.ToString() ?? "<null>"}, Details = {this.Details?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}.");

            return result;
        }
    }
}