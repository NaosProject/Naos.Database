﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.181.0)
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
    public partial class CheckSingleStreamReport : IModel<CheckSingleStreamReport>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="CheckSingleStreamReport"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(CheckSingleStreamReport left, CheckSingleStreamReport right)
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
        /// Determines whether two objects of type <see cref="CheckSingleStreamReport"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(CheckSingleStreamReport left, CheckSingleStreamReport right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(CheckSingleStreamReport other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.ExpectedRecordWithinThresholdIdToMostRecentTimestampMap.IsEqualTo(other.ExpectedRecordWithinThresholdIdToMostRecentTimestampMap)
                      && this.EventExpectedToBeHandledIdToHandlingStatusResultMap.IsEqualTo(other.EventExpectedToBeHandledIdToHandlingStatusResultMap);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as CheckSingleStreamReport);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.ExpectedRecordWithinThresholdIdToMostRecentTimestampMap)
            .Hash(this.EventExpectedToBeHandledIdToHandlingStatusResultMap)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public CheckSingleStreamReport DeepClone()
        {
            var result = new CheckSingleStreamReport(
                                 this.ExpectedRecordWithinThresholdIdToMostRecentTimestampMap?.DeepClone(),
                                 this.EventExpectedToBeHandledIdToHandlingStatusResultMap?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="ExpectedRecordWithinThresholdIdToMostRecentTimestampMap" />.
        /// </summary>
        /// <param name="expectedRecordWithinThresholdIdToMostRecentTimestampMap">The new <see cref="ExpectedRecordWithinThresholdIdToMostRecentTimestampMap" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="CheckSingleStreamReport" /> using the specified <paramref name="expectedRecordWithinThresholdIdToMostRecentTimestampMap" /> for <see cref="ExpectedRecordWithinThresholdIdToMostRecentTimestampMap" /> and a deep clone of every other property.</returns>
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
        public CheckSingleStreamReport DeepCloneWithExpectedRecordWithinThresholdIdToMostRecentTimestampMap(IReadOnlyDictionary<string, DateTime> expectedRecordWithinThresholdIdToMostRecentTimestampMap)
        {
            var result = new CheckSingleStreamReport(
                                 expectedRecordWithinThresholdIdToMostRecentTimestampMap,
                                 this.EventExpectedToBeHandledIdToHandlingStatusResultMap?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="EventExpectedToBeHandledIdToHandlingStatusResultMap" />.
        /// </summary>
        /// <param name="eventExpectedToBeHandledIdToHandlingStatusResultMap">The new <see cref="EventExpectedToBeHandledIdToHandlingStatusResultMap" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="CheckSingleStreamReport" /> using the specified <paramref name="eventExpectedToBeHandledIdToHandlingStatusResultMap" /> for <see cref="EventExpectedToBeHandledIdToHandlingStatusResultMap" /> and a deep clone of every other property.</returns>
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
        public CheckSingleStreamReport DeepCloneWithEventExpectedToBeHandledIdToHandlingStatusResultMap(IReadOnlyDictionary<string, IReadOnlyDictionary<long, HandlingStatus>> eventExpectedToBeHandledIdToHandlingStatusResultMap)
        {
            var result = new CheckSingleStreamReport(
                                 this.ExpectedRecordWithinThresholdIdToMostRecentTimestampMap?.DeepClone(),
                                 eventExpectedToBeHandledIdToHandlingStatusResultMap);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.CheckSingleStreamReport: ExpectedRecordWithinThresholdIdToMostRecentTimestampMap = {this.ExpectedRecordWithinThresholdIdToMostRecentTimestampMap?.ToString() ?? "<null>"}, EventExpectedToBeHandledIdToHandlingStatusResultMap = {this.EventExpectedToBeHandledIdToHandlingStatusResultMap?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}