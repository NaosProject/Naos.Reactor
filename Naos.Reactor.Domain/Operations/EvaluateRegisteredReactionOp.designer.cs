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

    using global::OBeautifulCode.Cloning.Recipes;
    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class EvaluateRegisteredReactionOp : IModel<EvaluateRegisteredReactionOp>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="EvaluateRegisteredReactionOp"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(EvaluateRegisteredReactionOp left, EvaluateRegisteredReactionOp right)
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
        /// Determines whether two objects of type <see cref="EvaluateRegisteredReactionOp"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(EvaluateRegisteredReactionOp left, EvaluateRegisteredReactionOp right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(EvaluateRegisteredReactionOp other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.RegisteredReaction.IsEqualTo(other.RegisteredReaction);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as EvaluateRegisteredReactionOp);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.RegisteredReaction)
            .Value;

        /// <inheritdoc />
        public new EvaluateRegisteredReactionOp DeepClone() => (EvaluateRegisteredReactionOp)this.DeepCloneInternal();

        /// <summary>
        /// Deep clones this object with a new <see cref="RegisteredReaction" />.
        /// </summary>
        /// <param name="registeredReaction">The new <see cref="RegisteredReaction" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="EvaluateRegisteredReactionOp" /> using the specified <paramref name="registeredReaction" /> for <see cref="RegisteredReaction" /> and a deep clone of every other property.</returns>
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
        public EvaluateRegisteredReactionOp DeepCloneWithRegisteredReaction(RegisteredReaction registeredReaction)
        {
            var result = new EvaluateRegisteredReactionOp(
                                 registeredReaction);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override OperationBase DeepCloneInternal()
        {
            var result = new EvaluateRegisteredReactionOp(
                                 this.RegisteredReaction?.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Reactor.Domain.EvaluateRegisteredReactionOp: RegisteredReaction = {this.RegisteredReaction?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}