﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.178.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Globalization;
    using global::System.Linq;

    using global::Naos.Database.Domain;

    using global::OBeautifulCode.Cloning.Recipes;
    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class ReactionEvent : IModel<ReactionEvent>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="ReactionEvent"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(ReactionEvent left, ReactionEvent right)
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
        /// Determines whether two objects of type <see cref="ReactionEvent"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(ReactionEvent left, ReactionEvent right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(ReactionEvent other)
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
                      && this.Id.IsEqualTo(other.Id, StringComparer.Ordinal)
                      && this.ReactionRegistrationId.IsEqualTo(other.ReactionRegistrationId, StringComparer.Ordinal)
                      && this.ReactionContext.IsEqualTo(other.ReactionContext)
                      && this.StreamRepresentationToInternalRecordIdsMap.IsEqualTo(other.StreamRepresentationToInternalRecordIdsMap)
                      && this.Tags.IsEqualTo(other.Tags);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as ReactionEvent);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.TimestampUtc)
            .Hash(this.Id)
            .Hash(this.ReactionRegistrationId)
            .Hash(this.ReactionContext)
            .Hash(this.StreamRepresentationToInternalRecordIdsMap)
            .Hash(this.Tags)
            .Value;

        /// <inheritdoc />
        public new ReactionEvent DeepClone() => (ReactionEvent)this.DeepCloneInternal();

        /// <inheritdoc />
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
        public override EventBase DeepCloneWithTimestampUtc(DateTime timestampUtc)
        {
            var result = new ReactionEvent(
                                 this.Id?.DeepClone(),
                                 this.ReactionRegistrationId?.DeepClone(),
                                 this.ReactionContext?.DeepClone(),
                                 this.StreamRepresentationToInternalRecordIdsMap?.DeepClone(),
                                 timestampUtc,
                                 this.Tags?.DeepClone());

            return result;
        }

        /// <inheritdoc />
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
        public override EventBase<string> DeepCloneWithId(string id)
        {
            var result = new ReactionEvent(
                                 id,
                                 this.ReactionRegistrationId?.DeepClone(),
                                 this.ReactionContext?.DeepClone(),
                                 this.StreamRepresentationToInternalRecordIdsMap?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.Tags?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="ReactionRegistrationId" />.
        /// </summary>
        /// <param name="reactionRegistrationId">The new <see cref="ReactionRegistrationId" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ReactionEvent" /> using the specified <paramref name="reactionRegistrationId" /> for <see cref="ReactionRegistrationId" /> and a deep clone of every other property.</returns>
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
        public ReactionEvent DeepCloneWithReactionRegistrationId(string reactionRegistrationId)
        {
            var result = new ReactionEvent(
                                 this.Id?.DeepClone(),
                                 reactionRegistrationId,
                                 this.ReactionContext?.DeepClone(),
                                 this.StreamRepresentationToInternalRecordIdsMap?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.Tags?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="ReactionContext" />.
        /// </summary>
        /// <param name="reactionContext">The new <see cref="ReactionContext" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ReactionEvent" /> using the specified <paramref name="reactionContext" /> for <see cref="ReactionContext" /> and a deep clone of every other property.</returns>
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
        public ReactionEvent DeepCloneWithReactionContext(IReactionContext reactionContext)
        {
            var result = new ReactionEvent(
                                 this.Id?.DeepClone(),
                                 this.ReactionRegistrationId?.DeepClone(),
                                 reactionContext,
                                 this.StreamRepresentationToInternalRecordIdsMap?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.Tags?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="StreamRepresentationToInternalRecordIdsMap" />.
        /// </summary>
        /// <param name="streamRepresentationToInternalRecordIdsMap">The new <see cref="StreamRepresentationToInternalRecordIdsMap" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ReactionEvent" /> using the specified <paramref name="streamRepresentationToInternalRecordIdsMap" /> for <see cref="StreamRepresentationToInternalRecordIdsMap" /> and a deep clone of every other property.</returns>
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
        public ReactionEvent DeepCloneWithStreamRepresentationToInternalRecordIdsMap(IReadOnlyDictionary<IStreamRepresentation, IReadOnlyList<long>> streamRepresentationToInternalRecordIdsMap)
        {
            var result = new ReactionEvent(
                                 this.Id?.DeepClone(),
                                 this.ReactionRegistrationId?.DeepClone(),
                                 this.ReactionContext?.DeepClone(),
                                 streamRepresentationToInternalRecordIdsMap,
                                 this.TimestampUtc.DeepClone(),
                                 this.Tags?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Tags" />.
        /// </summary>
        /// <param name="tags">The new <see cref="Tags" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="ReactionEvent" /> using the specified <paramref name="tags" /> for <see cref="Tags" /> and a deep clone of every other property.</returns>
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
        public ReactionEvent DeepCloneWithTags(IReadOnlyCollection<NamedValue<string>> tags)
        {
            var result = new ReactionEvent(
                                 this.Id?.DeepClone(),
                                 this.ReactionRegistrationId?.DeepClone(),
                                 this.ReactionContext?.DeepClone(),
                                 this.StreamRepresentationToInternalRecordIdsMap?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 tags);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override EventBase DeepCloneInternal()
        {
            var result = new ReactionEvent(
                                 this.Id?.DeepClone(),
                                 this.ReactionRegistrationId?.DeepClone(),
                                 this.ReactionContext?.DeepClone(),
                                 this.StreamRepresentationToInternalRecordIdsMap?.DeepClone(),
                                 this.TimestampUtc.DeepClone(),
                                 this.Tags?.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Reactor.Domain.ReactionEvent: TimestampUtc = {this.TimestampUtc.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, Id = {this.Id?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, ReactionRegistrationId = {this.ReactionRegistrationId?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, ReactionContext = {this.ReactionContext?.ToString() ?? "<null>"}, StreamRepresentationToInternalRecordIdsMap = {this.StreamRepresentationToInternalRecordIdsMap?.ToString() ?? "<null>"}, Tags = {this.Tags?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}