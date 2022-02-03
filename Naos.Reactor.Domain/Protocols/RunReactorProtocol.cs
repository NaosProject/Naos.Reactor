// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessReactionsProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Threading;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Process all new reactions, this is the entry point for all produced events.
    /// </summary>
    public partial class RunReactorProtocol : SyncSpecificVoidProtocolBase<RunReactorOp>
    {
        private readonly IStandardStream registeredReactionStream;
        private readonly IStandardStream reactionStream;
        private readonly ISyncAndAsyncReturningProtocol<EvaluateRegisteredReactionOp, ReactionEvent> evaluateRegisteredReactionProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunReactorProtocol"/> class.
        /// </summary>
        /// <param name="registeredReactionStream">The registered reaction stream.</param>
        /// <param name="reactionStream">The reaction stream.</param>
        /// <param name="evaluateRegisteredReactionProtocol">The evaluate registered reaction protocol.</param>
        public RunReactorProtocol(
            IStandardStream registeredReactionStream,
            IStandardStream reactionStream,
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public override void Execute(
            RunReactorOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var getDistinctStringSerializedIdsOp = new StandardGetDistinctStringSerializedIdsOp(
                new RecordFilter(
                    objectTypes: new[]
                                 {
                                     typeof(RegisteredReaction).ToRepresentation(),
                                 },
                    deprecatedIdTypes: new[]
                                       {
                                           operation.DeprecatedIdentifierType
                                       }));
            var distinctIds = this.registeredReactionStream.Execute(getDistinctStringSerializedIdsOp);
            foreach (var distinctId in distinctIds)
            {
                var getLatestRecordOp = new StandardGetLatestRecordOp(
                    new RecordFilter(
                        ids: new[]
                             {
                                 distinctId,
                             }));

                var registeredReactionRecord = this.registeredReactionStream.Execute(getLatestRecordOp);
                registeredReactionRecord
                   .Payload
                   .PayloadTypeRepresentation
                   .MustForOp("recordInRegisteredReactionStreamMustBeRegisteredReactionIfNotDeprecatedId")
                   .BeEqualTo(typeof(RegisteredReaction).ToRepresentation());

                var registeredReaction =
                    registeredReactionRecord.Payload.DeserializePayloadUsingSpecificFactory<RegisteredReaction>(
                        this.registeredReactionStream.SerializerFactory);

                var reaction = this.evaluateRegisteredReactionProtocol.Execute(new EvaluateRegisteredReactionOp(registeredReaction));
                if (reaction != null)
                {
                    this.reactionStream.PutWithId(reaction.Id, reaction, reaction.Tags);
                    // probably should be void and just write reaction from evaluation
                    //TODO: complete handling here? or do it in the this.evaluateRegisteredReactionProtocol.Execute
                }
            }
        }
    }
}
