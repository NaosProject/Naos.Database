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
    public partial class StreamRecordHandlingEntry : IModel<StreamRecordHandlingEntry>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="StreamRecordHandlingEntry"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(StreamRecordHandlingEntry left, StreamRecordHandlingEntry right)
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
        /// Determines whether two objects of type <see cref="StreamRecordHandlingEntry"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(StreamRecordHandlingEntry left, StreamRecordHandlingEntry right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(StreamRecordHandlingEntry other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.InternalHandlingEntryId.IsEqualTo(other.InternalHandlingEntryId)
                      && this.InternalRecordId.IsEqualTo(other.InternalRecordId)
                      && this.Concern.IsEqualTo(other.Concern, StringComparer.Ordinal)
                      && this.Status.IsEqualTo(other.Status)
                      && this.Tags.IsEqualTo(other.Tags)
                      && this.Details.IsEqualTo(other.Details, StringComparer.Ordinal)
                      && this.TimestampUtc.IsEqualTo(other.TimestampUtc);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as StreamRecordHandlingEntry);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.InternalHandlingEntryId)
            .Hash(this.InternalRecordId)
            .Hash(this.Concern)
            .Hash(this.Status)
            .Hash(this.Tags)
            .Hash(this.Details)
            .Hash(this.TimestampUtc)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public StreamRecordHandlingEntry DeepClone()
        {
            var result = new StreamRecordHandlingEntry(
                                 this.InternalHandlingEntryId.DeepClone(),
                                 this.InternalRecordId.DeepClone(),
                                 this.Concern?.DeepClone(),
                                 this.Status.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.TimestampUtc.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="InternalHandlingEntryId" />.
        /// </summary>
        /// <param name="internalHandlingEntryId">The new <see cref="InternalHandlingEntryId" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordHandlingEntry" /> using the specified <paramref name="internalHandlingEntryId" /> for <see cref="InternalHandlingEntryId" /> and a deep clone of every other property.</returns>
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
        public StreamRecordHandlingEntry DeepCloneWithInternalHandlingEntryId(long internalHandlingEntryId)
        {
            var result = new StreamRecordHandlingEntry(
                                 internalHandlingEntryId,
                                 this.InternalRecordId.DeepClone(),
                                 this.Concern?.DeepClone(),
                                 this.Status.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.TimestampUtc.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="InternalRecordId" />.
        /// </summary>
        /// <param name="internalRecordId">The new <see cref="InternalRecordId" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordHandlingEntry" /> using the specified <paramref name="internalRecordId" /> for <see cref="InternalRecordId" /> and a deep clone of every other property.</returns>
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
        public StreamRecordHandlingEntry DeepCloneWithInternalRecordId(long internalRecordId)
        {
            var result = new StreamRecordHandlingEntry(
                                 this.InternalHandlingEntryId.DeepClone(),
                                 internalRecordId,
                                 this.Concern?.DeepClone(),
                                 this.Status.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.TimestampUtc.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Concern" />.
        /// </summary>
        /// <param name="concern">The new <see cref="Concern" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordHandlingEntry" /> using the specified <paramref name="concern" /> for <see cref="Concern" /> and a deep clone of every other property.</returns>
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
        public StreamRecordHandlingEntry DeepCloneWithConcern(string concern)
        {
            var result = new StreamRecordHandlingEntry(
                                 this.InternalHandlingEntryId.DeepClone(),
                                 this.InternalRecordId.DeepClone(),
                                 concern,
                                 this.Status.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.TimestampUtc.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Status" />.
        /// </summary>
        /// <param name="status">The new <see cref="Status" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordHandlingEntry" /> using the specified <paramref name="status" /> for <see cref="Status" /> and a deep clone of every other property.</returns>
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
        public StreamRecordHandlingEntry DeepCloneWithStatus(HandlingStatus status)
        {
            var result = new StreamRecordHandlingEntry(
                                 this.InternalHandlingEntryId.DeepClone(),
                                 this.InternalRecordId.DeepClone(),
                                 this.Concern?.DeepClone(),
                                 status,
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.TimestampUtc.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Tags" />.
        /// </summary>
        /// <param name="tags">The new <see cref="Tags" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordHandlingEntry" /> using the specified <paramref name="tags" /> for <see cref="Tags" /> and a deep clone of every other property.</returns>
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
        public StreamRecordHandlingEntry DeepCloneWithTags(IReadOnlyCollection<NamedValue<string>> tags)
        {
            var result = new StreamRecordHandlingEntry(
                                 this.InternalHandlingEntryId.DeepClone(),
                                 this.InternalRecordId.DeepClone(),
                                 this.Concern?.DeepClone(),
                                 this.Status.DeepClone(),
                                 tags,
                                 this.Details?.DeepClone(),
                                 this.TimestampUtc.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Details" />.
        /// </summary>
        /// <param name="details">The new <see cref="Details" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordHandlingEntry" /> using the specified <paramref name="details" /> for <see cref="Details" /> and a deep clone of every other property.</returns>
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
        public StreamRecordHandlingEntry DeepCloneWithDetails(string details)
        {
            var result = new StreamRecordHandlingEntry(
                                 this.InternalHandlingEntryId.DeepClone(),
                                 this.InternalRecordId.DeepClone(),
                                 this.Concern?.DeepClone(),
                                 this.Status.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 details,
                                 this.TimestampUtc.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TimestampUtc" />.
        /// </summary>
        /// <param name="timestampUtc">The new <see cref="TimestampUtc" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StreamRecordHandlingEntry" /> using the specified <paramref name="timestampUtc" /> for <see cref="TimestampUtc" /> and a deep clone of every other property.</returns>
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
        public StreamRecordHandlingEntry DeepCloneWithTimestampUtc(DateTime timestampUtc)
        {
            var result = new StreamRecordHandlingEntry(
                                 this.InternalHandlingEntryId.DeepClone(),
                                 this.InternalRecordId.DeepClone(),
                                 this.Concern?.DeepClone(),
                                 this.Status.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 timestampUtc);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.StreamRecordHandlingEntry: InternalHandlingEntryId = {this.InternalHandlingEntryId.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, InternalRecordId = {this.InternalRecordId.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, Concern = {this.Concern?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, Status = {this.Status.ToString() ?? "<null>"}, Tags = {this.Tags?.ToString() ?? "<null>"}, Details = {this.Details?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, TimestampUtc = {this.TimestampUtc.ToString(CultureInfo.InvariantCulture) ?? "<null>"}.");

            return result;
        }
    }
}