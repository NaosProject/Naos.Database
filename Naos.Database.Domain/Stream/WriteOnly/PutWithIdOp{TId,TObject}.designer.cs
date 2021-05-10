﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.151.0)
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
    public partial class PutWithIdOp<TId, TObject> : IModel<PutWithIdOp<TId, TObject>>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="PutWithIdOp{TId, TObject}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(PutWithIdOp<TId, TObject> left, PutWithIdOp<TId, TObject> right)
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
        /// Determines whether two objects of type <see cref="PutWithIdOp{TId, TObject}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(PutWithIdOp<TId, TObject> left, PutWithIdOp<TId, TObject> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(PutWithIdOp<TId, TObject> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.Id.IsEqualTo(other.Id)
                      && this.ObjectToPut.IsEqualTo(other.ObjectToPut)
                      && this.Tags.IsEqualTo(other.Tags)
                      && this.ExistingRecordEncounteredStrategy.IsEqualTo(other.ExistingRecordEncounteredStrategy)
                      && this.RecordRetentionCount.IsEqualTo(other.RecordRetentionCount)
                      && this.TypeVersionMatchStrategy.IsEqualTo(other.TypeVersionMatchStrategy);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as PutWithIdOp<TId, TObject>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Id)
            .Hash(this.ObjectToPut)
            .Hash(this.Tags)
            .Hash(this.ExistingRecordEncounteredStrategy)
            .Hash(this.RecordRetentionCount)
            .Hash(this.TypeVersionMatchStrategy)
            .Value;

        /// <inheritdoc />
        public new PutWithIdOp<TId, TObject> DeepClone() => (PutWithIdOp<TId, TObject>)this.DeepCloneInternal();

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override OperationBase DeepCloneInternal()
        {
            var result = new PutWithIdOp<TId, TObject>(
                                 DeepCloneGeneric(this.Id),
                                 DeepCloneGeneric(this.ObjectToPut),
                                 this.Tags?.ToDictionary(k => k.Key?.DeepClone(), v => v.Value?.DeepClone()),
                                 this.ExistingRecordEncounteredStrategy,
                                 this.RecordRetentionCount,
                                 this.TypeVersionMatchStrategy);

            return result;
        }

        private static TId DeepCloneGeneric(TId value)
        {
            object result;

            var type = typeof(TId);

            if (type.IsValueType)
            {
                result = value;
            }
            else
            {
                if (ReferenceEquals(value, null))
                {
                    result = default;
                }
                else if (value is IDeepCloneable<TId> deepCloneableValue)
                {
                    result = deepCloneableValue.DeepClone();
                }
                else if (value is string valueAsString)
                {
                    result = valueAsString.DeepClone();
                }
                else if (value is global::System.Version valueAsVersion)
                {
                    result = valueAsVersion.DeepClone();
                }
                else if (value is global::System.Uri valueAsUri)
                {
                    result = valueAsUri.DeepClone();
                }
                else
                {
                    throw new NotSupportedException(Invariant($"I do not know how to deep clone an object of type '{type.ToStringReadable()}'"));
                }
            }

            return (TId)result;
        }

        private static TObject DeepCloneGeneric(TObject value)
        {
            object result;

            var type = typeof(TObject);

            if (type.IsValueType)
            {
                result = value;
            }
            else
            {
                if (ReferenceEquals(value, null))
                {
                    result = default;
                }
                else if (value is IDeepCloneable<TObject> deepCloneableValue)
                {
                    result = deepCloneableValue.DeepClone();
                }
                else if (value is string valueAsString)
                {
                    result = valueAsString.DeepClone();
                }
                else if (value is global::System.Version valueAsVersion)
                {
                    result = valueAsVersion.DeepClone();
                }
                else if (value is global::System.Uri valueAsUri)
                {
                    result = valueAsUri.DeepClone();
                }
                else
                {
                    throw new NotSupportedException(Invariant($"I do not know how to deep clone an object of type '{type.ToStringReadable()}'"));
                }
            }

            return (TObject)result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.{this.GetType().ToStringReadable()}: Id = {this.Id?.ToString() ?? "<null>"}, ObjectToPut = {this.ObjectToPut?.ToString() ?? "<null>"}, Tags = {this.Tags?.ToString() ?? "<null>"}, ExistingRecordEncounteredStrategy = {this.ExistingRecordEncounteredStrategy.ToString() ?? "<null>"}, RecordRetentionCount = {this.RecordRetentionCount?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, TypeVersionMatchStrategy = {this.TypeVersionMatchStrategy.ToString() ?? "<null>"}.");

            return result;
        }
    }
}