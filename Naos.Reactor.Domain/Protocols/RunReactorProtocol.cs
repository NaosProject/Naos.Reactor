// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunReactorProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Linq;
    using System.Threading;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="RunReactorOp"/>.
    /// </summary>
    public partial class RunReactorProtocol : SyncSpecificVoidProtocolBase<RunReactorOp>
    {
        private readonly IStandardStream reactionRegistrationStream;
        private readonly IStandardStream reactionStream;
        private readonly ISyncAndAsyncReturningProtocol<EvaluateReactionRegistrationOp, ReactionEvent> evaluateReactionRegistrationProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunReactorProtocol"/> class.
        /// </summary>
        /// <param name="reactionRegistrationStream">The registered reaction stream.</param>
        /// <param name="reactionStream">The reaction stream.</param>
        /// <param name="evaluateReactionRegistrationProtocol">The evaluate registered reaction protocol.</param>
        public RunReactorProtocol(
            IStandardStream reactionRegistrationStream,
            IStandardStream reactionStream,
            ISyncAndAsyncReturningProtocol<EvaluateReactionRegistrationOp, ReactionEvent> evaluateReactionRegistrationProtocol)
        {
            reactionRegistrationStream.MustForArg(nameof(reactionRegistrationStream)).NotBeNull();
            reactionStream.MustForArg(nameof(reactionStream)).NotBeNull();
            evaluateReactionRegistrationProtocol.MustForArg(nameof(evaluateReactionRegistrationProtocol)).NotBeNull();

            this.reactionRegistrationStream = reactionRegistrationStream;
            this.reactionStream = reactionStream;
            this.evaluateReactionRegistrationProtocol = evaluateReactionRegistrationProtocol;
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
                                     typeof(ReactionRegistration).ToRepresentation(),
                                 },
                    deprecatedIdTypes: new[]
                                       {
                                           operation.DeprecatedIdentifierType
                                       }));
            var distinctIds = this.reactionRegistrationStream.Execute(getDistinctStringSerializedIdsOp);
            foreach (var distinctId in distinctIds)
            {
                try
                {
                    var getLatestRecordOp = new StandardGetLatestRecordOp(
                        new RecordFilter(
                            ids: new[]
                                 {
                                     distinctId,
                                 }));

                    var reactionRegistrationRecord = this.reactionRegistrationStream.Execute(getLatestRecordOp);
                    reactionRegistrationRecord
                       .Payload
                       .PayloadTypeRepresentation
                       .MustForOp("recordFromReactionRegistrationStreamExpectedToBeRegisteredReaction")
                       .BeEqualTo(typeof(ReactionRegistration).ToRepresentation());

                    var reactionRegistration =
                        reactionRegistrationRecord.Payload.DeserializePayloadUsingSpecificFactory<ReactionRegistration>(
                            this.reactionRegistrationStream.SerializerFactory);

                    var reaction = this.evaluateReactionRegistrationProtocol.Execute(new EvaluateReactionRegistrationOp(reactionRegistration));
                    if (reaction != null)
                    {
                        var reactionTags = reaction.Tags.Union(
                                                        new[]
                                                        {
                                                            new NamedValue<string>(
                                                                nameof(reaction.ReactionRegistrationId),
                                                                reaction.ReactionRegistrationId),
                                                        })
                                                   .ToList();

                        this.reactionStream.PutWithId(reaction.Id, reaction, reactionTags);
                        // probably should be void and just write reaction from evaluation
                        //TODO: complete handling here? or do it in the this.evaluateReactionRegistrationsProtocol.Execute
                    }
                }
                catch (Exception ex)
                {
                    throw new ReactorException(Invariant($"Failed to process {nameof(ReactionRegistration)} Id: {distinctId}."), ex, operation);
                }
            }
        }
    }
}
