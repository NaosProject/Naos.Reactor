// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteHandlingOnReactionRegistrationDependenciesOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Marks the reaction registration dependencies as handled.
    /// </summary>
    /// <remarks>Useful for making sure the dependencies are in a stable state for new registrations or to clean up a failed run and restart in a known state.</remarks>
    public partial class CompleteHandlingOnReactionRegistrationDependenciesOp : VoidOperationBase, IHaveDetails
    {
        /// <summary>
        /// The statuses that allows a record to be available for handling.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = NaosSuppressBecause.CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable)]
        public static readonly IReadOnlyCollection<HandlingStatus> AvailableStatuses =
            new[]
            {
                HandlingStatus.AvailableAfterCompletion,
                HandlingStatus.AvailableAfterExternalCancellation,
                HandlingStatus.AvailableAfterFailure,
                HandlingStatus.AvailableAfterSelfCancellation,
                HandlingStatus.AvailableByDefault,
            };

        /// <summary>
        /// The non-terminal handling statuses.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = NaosSuppressBecause.CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable)]
        public static readonly IReadOnlyCollection<HandlingStatus> HandlingStatusesToRegardAsIncomplete =
            AvailableStatuses.Concat(
                                  new[]
                                  {
                                      HandlingStatus.Running,
                                      HandlingStatus.Failed,
                                  })
                             .ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteHandlingOnReactionRegistrationDependenciesOp"/> class.
        /// </summary>
        /// <param name="reactionRegistration">The reaction registration.</param>
        /// <param name="details">The details to add to the handling entry.</param>
        /// <param name="acceptableHandlingStatuses">Optional acceptable handling statuses of the dependencies; DEFAULT is <see cref="HandlingStatus.AvailableByDefault" /> only.</param>
        public CompleteHandlingOnReactionRegistrationDependenciesOp(
            ReactionRegistration reactionRegistration,
            string details,
            IReadOnlyCollection<HandlingStatus> acceptableHandlingStatuses = null)
        {
            reactionRegistration.MustForArg(nameof(reactionRegistration)).NotBeNull();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.ReactionRegistration = reactionRegistration;
            this.Details = details;
            this.AcceptableHandlingStatuses = acceptableHandlingStatuses
                                           ?? new[]
                                              {
                                                  HandlingStatus.AvailableByDefault,
                                              };
        }

        /// <summary>
        /// Gets the reaction registration.
        /// </summary>
        public ReactionRegistration ReactionRegistration { get; private set; }

        /// <summary>
        /// Gets the acceptable handling statuses of the dependencies.
        /// </summary>
        public IReadOnlyCollection<HandlingStatus> AcceptableHandlingStatuses { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
