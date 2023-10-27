// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriggerReactionRegistrationProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Protocol
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Reactor.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="RunReactorOp"/>.
    /// </summary>
    public partial class TriggerReactionRegistrationProtocol : SyncSpecificVoidProtocolBase<TriggerReactionRegistrationOp>
    {
        private readonly IStandardStream reactionRegistrationStream;
        private readonly IStandardStream reactionStream;
        private readonly ISyncAndAsyncReturningProtocol<EvaluateReactionRegistrationOp, EvaluateReactionRegistrationResult> evaluateReactionRegistrationProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerReactionRegistrationProtocol"/> class.
        /// </summary>
        /// <param name="reactionRegistrationStream">The registered reaction stream.</param>
        /// <param name="reactionStream">The reaction stream.</param>
        /// <param name="evaluateReactionRegistrationProtocol">The evaluate registered reaction protocol.</param>
        public TriggerReactionRegistrationProtocol(
            IStandardStream reactionRegistrationStream,
            IStandardStream reactionStream,
            ISyncAndAsyncReturningProtocol<EvaluateReactionRegistrationOp, EvaluateReactionRegistrationResult> evaluateReactionRegistrationProtocol)
        {
            reactionRegistrationStream.MustForArg(nameof(reactionRegistrationStream)).NotBeNull();
            reactionStream.MustForArg(nameof(reactionStream)).NotBeNull();
            evaluateReactionRegistrationProtocol.MustForArg(nameof(evaluateReactionRegistrationProtocol)).NotBeNull();

            this.reactionRegistrationStream = reactionRegistrationStream;
            this.reactionStream = reactionStream;
            this.evaluateReactionRegistrationProtocol = evaluateReactionRegistrationProtocol;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override void Execute(
            TriggerReactionRegistrationOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            try
            {
                var reactionRegistration =
                    this.reactionRegistrationStream.GetLatestObjectById<string, ReactionRegistration>(operation.ReactionRegistrationId);

                var evaluateReactionRegistrationOp = new EvaluateReactionRegistrationOp(reactionRegistration, true);
                var evaluateReactionRegistrationResult = this.evaluateReactionRegistrationProtocol.Execute(evaluateReactionRegistrationOp);
                if (evaluateReactionRegistrationResult.ReactionEvent != null)
                {
                    var reaction = evaluateReactionRegistrationResult.ReactionEvent;

                    this.reactionStream.PutWithId(reaction.Id, reaction, reaction.Tags);

                    // once we have recorded the reaction then we can finalize the handling cycle.
                    foreach (var recordSetHandlingMemento in evaluateReactionRegistrationResult.RecordSetHandlingMementos)
                    {
                        recordSetHandlingMemento.CompleteSet();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ReactorException(Invariant($"Failed to trigger {nameof(ReactionRegistration)} Id: {operation.ReactionRegistrationId}."), ex, operation);
            }
        }
    }
}
