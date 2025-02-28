﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.192.0)
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
    using global::OBeautifulCode.Serialization;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class StreamRecordMetadata<TId> : IModel<StreamRecordMetadata<TId>>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="StreamRecordMetadata{TId}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(StreamRecordMetadata<TId> left, StreamRecordMetadata<TId> right)
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
        /// Determines whether two objects of type <see cref="StreamRecordMetadata{TId}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(StreamRecordMetadata<TId> left, StreamRecordMetadata<TId> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(StreamRecordMetadata<TId> other)
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
                      && this.SerializerRepresentation.IsEqualTo(other.SerializerRepresentation)
                      && this.Tags.IsEqualTo(other.Tags)
                      && this.TypeRepresentationOfId.IsEqualTo(other.TypeRepresentationOfId)
                      && this.TypeRepresentationOfObject.IsEqualTo(other.TypeRepresentationOfObject)
                      && this.TimestampUtc.IsEqualTo(other.TimestampUtc)
                      && this.ObjectTimestampUtc.IsEqualTo(other.ObjectTimestampUtc);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as StreamRecordMetadata<TId>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Id)
            .Hash(this.SerializerRepresentation)
            .Hash(this.Tags)
            .Hash(this.TypeRepresentationOfId)
            .Hash(this.TypeRepresentationOfObject)
            .Hash(this.TimestampUtc)
            .Hash(this.ObjectTimestampUtc)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public StreamRecordMetadata<TId> DeepClone()
        {
            var result = new StreamRecordMetadata<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.SerializerRepresentation?.DeepClone(),
                                 this.TypeRepresentationOfId?.DeepClone(),
                                 this.TypeRepresentationOfObject?.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.ObjectTimestampUtc?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Id" />.
        /// </summary>
        /// <param name="id">The new <see cref="Id" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordMetadata{TId}" /> using the specified <paramref name="id" /> for <see cref="Id" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public StreamRecordMetadata<TId> DeepCloneWithId(TId id)
        {
            var result = new StreamRecordMetadata<TId>(
                                 id,
                                 this.SerializerRepresentation?.DeepClone(),
                                 this.TypeRepresentationOfId?.DeepClone(),
                                 this.TypeRepresentationOfObject?.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.ObjectTimestampUtc?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="SerializerRepresentation" />.
        /// </summary>
        /// <param name="serializerRepresentation">The new <see cref="SerializerRepresentation" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordMetadata{TId}" /> using the specified <paramref name="serializerRepresentation" /> for <see cref="SerializerRepresentation" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public StreamRecordMetadata<TId> DeepCloneWithSerializerRepresentation(SerializerRepresentation serializerRepresentation)
        {
            var result = new StreamRecordMetadata<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 serializerRepresentation,
                                 this.TypeRepresentationOfId?.DeepClone(),
                                 this.TypeRepresentationOfObject?.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.ObjectTimestampUtc?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Tags" />.
        /// </summary>
        /// <param name="tags">The new <see cref="Tags" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordMetadata{TId}" /> using the specified <paramref name="tags" /> for <see cref="Tags" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public StreamRecordMetadata<TId> DeepCloneWithTags(IReadOnlyCollection<NamedValue<string>> tags)
        {
            var result = new StreamRecordMetadata<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.SerializerRepresentation?.DeepClone(),
                                 this.TypeRepresentationOfId?.DeepClone(),
                                 this.TypeRepresentationOfObject?.DeepClone(),
                                 tags,
                                 this.TimestampUtc.DeepClone(),
                                 this.ObjectTimestampUtc?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TypeRepresentationOfId" />.
        /// </summary>
        /// <param name="typeRepresentationOfId">The new <see cref="TypeRepresentationOfId" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordMetadata{TId}" /> using the specified <paramref name="typeRepresentationOfId" /> for <see cref="TypeRepresentationOfId" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public StreamRecordMetadata<TId> DeepCloneWithTypeRepresentationOfId(TypeRepresentationWithAndWithoutVersion typeRepresentationOfId)
        {
            var result = new StreamRecordMetadata<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.SerializerRepresentation?.DeepClone(),
                                 typeRepresentationOfId,
                                 this.TypeRepresentationOfObject?.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.ObjectTimestampUtc?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TypeRepresentationOfObject" />.
        /// </summary>
        /// <param name="typeRepresentationOfObject">The new <see cref="TypeRepresentationOfObject" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordMetadata{TId}" /> using the specified <paramref name="typeRepresentationOfObject" /> for <see cref="TypeRepresentationOfObject" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public StreamRecordMetadata<TId> DeepCloneWithTypeRepresentationOfObject(TypeRepresentationWithAndWithoutVersion typeRepresentationOfObject)
        {
            var result = new StreamRecordMetadata<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.SerializerRepresentation?.DeepClone(),
                                 this.TypeRepresentationOfId?.DeepClone(),
                                 typeRepresentationOfObject,
                                 this.Tags?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.ObjectTimestampUtc?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TimestampUtc" />.
        /// </summary>
        /// <param name="timestampUtc">The new <see cref="TimestampUtc" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordMetadata{TId}" /> using the specified <paramref name="timestampUtc" /> for <see cref="TimestampUtc" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public StreamRecordMetadata<TId> DeepCloneWithTimestampUtc(DateTime timestampUtc)
        {
            var result = new StreamRecordMetadata<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.SerializerRepresentation?.DeepClone(),
                                 this.TypeRepresentationOfId?.DeepClone(),
                                 this.TypeRepresentationOfObject?.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 timestampUtc,
                                 this.ObjectTimestampUtc?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="ObjectTimestampUtc" />.
        /// </summary>
        /// <param name="objectTimestampUtc">The new <see cref="ObjectTimestampUtc" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordMetadata{TId}" /> using the specified <paramref name="objectTimestampUtc" /> for <see cref="ObjectTimestampUtc" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public StreamRecordMetadata<TId> DeepCloneWithObjectTimestampUtc(DateTime? objectTimestampUtc)
        {
            var result = new StreamRecordMetadata<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.SerializerRepresentation?.DeepClone(),
                                 this.TypeRepresentationOfId?.DeepClone(),
                                 this.TypeRepresentationOfObject?.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 objectTimestampUtc);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.{this.GetType().ToStringReadable()}: Id = {this.Id?.ToString() ?? "<null>"}, SerializerRepresentation = {this.SerializerRepresentation?.ToString() ?? "<null>"}, Tags = {this.Tags?.ToString() ?? "<null>"}, TypeRepresentationOfId = {this.TypeRepresentationOfId?.ToString() ?? "<null>"}, TypeRepresentationOfObject = {this.TypeRepresentationOfObject?.ToString() ?? "<null>"}, TimestampUtc = {this.TimestampUtc.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, ObjectTimestampUtc = {this.ObjectTimestampUtc?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}.");

            return result;
        }
    }
}