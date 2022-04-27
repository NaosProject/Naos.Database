﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.178.0)
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

    using global::OBeautifulCode.Cloning.Recipes;
    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class PutWithIdAndReturnInternalRecordIdOp<TId, TObject> : IModel<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="PutWithIdAndReturnInternalRecordIdOp{TId, TObject}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(PutWithIdAndReturnInternalRecordIdOp<TId, TObject> left, PutWithIdAndReturnInternalRecordIdOp<TId, TObject> right)
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
        /// Determines whether two objects of type <see cref="PutWithIdAndReturnInternalRecordIdOp{TId, TObject}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(PutWithIdAndReturnInternalRecordIdOp<TId, TObject> left, PutWithIdAndReturnInternalRecordIdOp<TId, TObject> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(PutWithIdAndReturnInternalRecordIdOp<TId, TObject> other)
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
                      && this.ExistingRecordStrategy.IsEqualTo(other.ExistingRecordStrategy)
                      && this.RecordRetentionCount.IsEqualTo(other.RecordRetentionCount)
                      && this.VersionMatchStrategy.IsEqualTo(other.VersionMatchStrategy);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as PutWithIdAndReturnInternalRecordIdOp<TId, TObject>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Id)
            .Hash(this.ObjectToPut)
            .Hash(this.Tags)
            .Hash(this.ExistingRecordStrategy)
            .Hash(this.RecordRetentionCount)
            .Hash(this.VersionMatchStrategy)
            .Value;

        /// <inheritdoc />
        public new PutWithIdAndReturnInternalRecordIdOp<TId, TObject> DeepClone() => (PutWithIdAndReturnInternalRecordIdOp<TId, TObject>)this.DeepCloneInternal();

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override OperationBase DeepCloneInternal()
        {
            var result = new PutWithIdAndReturnInternalRecordIdOp<TId, TObject>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectToPut == null ? default : this.ObjectToPut.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.ExistingRecordStrategy.DeepClone(),
                                 this.RecordRetentionCount?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.{this.GetType().ToStringReadable()}: Id = {this.Id?.ToString() ?? "<null>"}, ObjectToPut = {this.ObjectToPut?.ToString() ?? "<null>"}, Tags = {this.Tags?.ToString() ?? "<null>"}, ExistingRecordStrategy = {this.ExistingRecordStrategy.ToString() ?? "<null>"}, RecordRetentionCount = {this.RecordRetentionCount?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, VersionMatchStrategy = {this.VersionMatchStrategy.ToString() ?? "<null>"}.");

            return result;
        }
    }
}