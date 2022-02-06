﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.177.0)
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
    public partial class StringIdentifiedReactorDependency : IModel<StringIdentifiedReactorDependency>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="StringIdentifiedReactorDependency"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(StringIdentifiedReactorDependency left, StringIdentifiedReactorDependency right)
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
        /// Determines whether two objects of type <see cref="StringIdentifiedReactorDependency"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(StringIdentifiedReactorDependency left, StringIdentifiedReactorDependency right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(StringIdentifiedReactorDependency other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.Id.IsEqualTo(other.Id, StringComparer.Ordinal)
                      && this.StreamRepresentation.IsEqualTo(other.StreamRepresentation)
                      && this.TryHandleRecordOp.IsEqualTo(other.TryHandleRecordOp);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as StringIdentifiedReactorDependency);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Id)
            .Hash(this.StreamRepresentation)
            .Hash(this.TryHandleRecordOp)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public StringIdentifiedReactorDependency DeepClone()
        {
            var result = new StringIdentifiedReactorDependency(
                                 this.Id?.DeepClone(),
                                 this.StreamRepresentation?.DeepClone(),
                                 this.TryHandleRecordOp?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Id" />.
        /// </summary>
        /// <param name="id">The new <see cref="Id" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StringIdentifiedReactorDependency" /> using the specified <paramref name="id" /> for <see cref="Id" /> and a deep clone of every other property.</returns>
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
        public StringIdentifiedReactorDependency DeepCloneWithId(string id)
        {
            var result = new StringIdentifiedReactorDependency(
                                 id,
                                 this.StreamRepresentation?.DeepClone(),
                                 this.TryHandleRecordOp?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="StreamRepresentation" />.
        /// </summary>
        /// <param name="streamRepresentation">The new <see cref="StreamRepresentation" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StringIdentifiedReactorDependency" /> using the specified <paramref name="streamRepresentation" /> for <see cref="StreamRepresentation" /> and a deep clone of every other property.</returns>
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
        public StringIdentifiedReactorDependency DeepCloneWithStreamRepresentation(IStreamRepresentation streamRepresentation)
        {
            var result = new StringIdentifiedReactorDependency(
                                 this.Id?.DeepClone(),
                                 streamRepresentation,
                                 this.TryHandleRecordOp?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TryHandleRecordOp" />.
        /// </summary>
        /// <param name="tryHandleRecordOp">The new <see cref="TryHandleRecordOp" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StringIdentifiedReactorDependency" /> using the specified <paramref name="tryHandleRecordOp" /> for <see cref="TryHandleRecordOp" /> and a deep clone of every other property.</returns>
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
        public StringIdentifiedReactorDependency DeepCloneWithTryHandleRecordOp(StandardTryHandleRecordOp tryHandleRecordOp)
        {
            var result = new StringIdentifiedReactorDependency(
                                 this.Id?.DeepClone(),
                                 this.StreamRepresentation?.DeepClone(),
                                 tryHandleRecordOp);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Reactor.Domain.StringIdentifiedReactorDependency: Id = {this.Id?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, StreamRepresentation = {this.StreamRepresentation?.ToString() ?? "<null>"}, TryHandleRecordOp = {this.TryHandleRecordOp?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}