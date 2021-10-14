// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessReactionsProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Process all new reactions.
    /// </summary>
    public partial class RunReactorProtocol : SyncSpecificVoidProtocolBase<RunReactorOp>
    {
        private static readonly TypeRepresentation DeprecatedIdStringTypeRepresentation = typeof(IdDeprecatedEvent<string>).ToRepresentation();

        private readonly IStandardReadWriteStream registeredReactionStream;
        private readonly IStandardReadWriteStream reactionStream;
        private readonly ISyncAndAsyncReturningProtocol<EvaluateRegisteredReactionOp, ReactionEvent> evaluateRegisteredReactionProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunReactorProtocol"/> class.
        /// </summary>
        /// <param name="registeredReactionStream">The registered reaction stream.</param>
        /// <param name="reactionStream">The reaction stream.</param>
        /// <param name="evaluateRegisteredReactionProtocol">The evaluate registered reaction protocol.</param>
        public RunReactorProtocol(
            IStandardReadWriteStream registeredReactionStream,
            IStandardReadWriteStream reactionStream,
            ISyncAndAsyncReturningProtocol<EvaluateRegisteredReactionOp, ReactionEvent> evaluateRegisteredReactionProtocol)
        {
            registeredReactionStream.MustForArg(nameof(registeredReactionStream)).NotBeNull();
            reactionStream.MustForArg(nameof(reactionStream)).NotBeNull();
            evaluateRegisteredReactionProtocol.MustForArg(nameof(evaluateRegisteredReactionProtocol)).NotBeNull();

            this.registeredReactionStream = registeredReactionStream;
            this.reactionStream = reactionStream;
            this.evaluateRegisteredReactionProtocol = evaluateRegisteredReactionProtocol;
        }

        /// <inheritdoc />
        public override void Execute(
            RunReactorOp operation)
        {
            var distinctIds = this.registeredReactionStream.Execute(new GetDistinctStringSerializedIdsOp());
            foreach (var distinctId in distinctIds)
            {
                var latestRecord = this.registeredReactionStream
                                       .Execute(new GetLatestRecordByIdOp(distinctId));
                if (!latestRecord.Payload.PayloadTypeRepresentation.Equals(DeprecatedIdStringTypeRepresentation))
                {
                    latestRecord.Payload.PayloadTypeRepresentation
                                .MustForOp("recordInRegisteredReactionStreamMustBeRegisteredReactionIfNotDeprecatedId")
                                .BeEqualTo(typeof(RegisteredReaction).ToRepresentation());
                    var registeredReaction =
                        latestRecord.Payload.DeserializePayloadUsingSpecificFactory<RegisteredReaction>(
                            this.registeredReactionStream.SerializerFactory);
                    var reaction = this.evaluateRegisteredReactionProtocol.Execute(new EvaluateRegisteredReactionOp(registeredReaction));
                    if (reaction != null)
                    {
                        this.reactionStream.PutWithId(reaction.Id, reaction);
                    }
                }
            }
        }
    }
}
