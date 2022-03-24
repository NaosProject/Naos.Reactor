// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateReactionRegistrationProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;
    
    /// <summary>
    /// Process all new reactions.
    /// </summary>
    public partial class EvaluateReactionRegistrationProtocol
        : SyncSpecificReturningProtocolBase<EvaluateReactionRegistrationOp, ReactionEvent>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateReactionRegistrationProtocol"/> class.
        /// </summary>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public EvaluateReactionRegistrationProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();

            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override ReactionEvent Execute(
            EvaluateReactionRegistrationOp operation)
        {
            if (operation.ReactionRegistration.Dependencies.Count != 1)
            {
                throw new NotSupportedException(Invariant($"Only 1 single {typeof(RecordFilterReactorDependency)} is supported, {operation.ReactionRegistration.Dependencies.Count} were supplied."));
            }

            var dependency = operation.ReactionRegistration.Dependencies.Single();
            var recordFilterDependency = dependency as RecordFilterReactorDependency;
            if (recordFilterDependency == null)
            {
                throw new NotSupportedException(Invariant($"Only {typeof(RecordFilterReactorDependency)} is supported, {dependency?.GetType().ToStringReadable()}."));
            }

            var records = new Dictionary<IStreamRepresentation, HashSet<long>>();
            var trackingForRollback = new List<Tuple<IStream, string, HashSet<long>>>();
            var allRequiredSeen = true;
            foreach (var recordFilterEntry in recordFilterDependency.Entries)
            {
                var stream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(recordFilterEntry.StreamRepresentation));
                stream.MustForOp(nameof(stream)).BeOfType<ISyncReturningProtocol<StandardTryHandleRecordOp, TryHandleRecordResult>>();
                var streamProtocol = (ISyncReturningProtocol<StandardTryHandleRecordOp, TryHandleRecordResult>)stream;

                var tryHandleConcern = Invariant($"{operation.ReactionRegistration.Id}_{recordFilterEntry.Id}");

                var handledIds = new HashSet<long>(); // could get duplicates
                StreamRecord currentRecord = null;
                do
                {
                    //TODO: When does then get mark 'handled'? here or in RunReactorProtocol
                    var tryHandleRecordOp = new StandardTryHandleRecordOp(
                        tryHandleConcern,
                        recordFilterEntry.RecordFilter,
                        streamRecordItemsToInclude: StreamRecordItemsToInclude.MetadataOnly);

                    var tryHandleRecordResult = streamProtocol.Execute(tryHandleRecordOp);

                    currentRecord = tryHandleRecordResult?.RecordToHandle;
                    if (currentRecord != null)
                    {
                        handledIds.Add(currentRecord.InternalRecordId);
                    }
                }
                while (currentRecord != null);

                if (handledIds.Any())
                {
                    trackingForRollback.Add(
                        new Tuple<IStream, string, HashSet<long>>(
                            stream,
                            tryHandleConcern,
                            handledIds));

                    if (recordFilterEntry.IncludeInReaction)
                    {
                        if (records.ContainsKey(recordFilterEntry.StreamRepresentation))
                        {
                            records[recordFilterEntry.StreamRepresentation].AddRange(handledIds);
                        }
                        else
                        {
                            records.Add(recordFilterEntry.StreamRepresentation, handledIds);
                        }
                    }
                }
                else
                {
                    if (recordFilterEntry.RequiredForReaction)
                    {
                        allRequiredSeen = false;
                        break;
                    }
                }
            }

            if (!allRequiredSeen)
            {
                foreach (var tracking in trackingForRollback)
                {
                    var streamProtocolFactory = (IStreamRecordHandlingProtocolFactory)tracking.Item1;
                    var streamProtocol = streamProtocolFactory.GetStreamRecordHandlingProtocols();
                    foreach (var internalRecordId in tracking.Item3)
                    {
                        streamProtocol.Execute(new SelfCancelRunningHandleRecordOp(internalRecordId, tracking.Item2, "Not all required dependencies present."));
                    }
                }

                return null;
            }

            var reactionId = Invariant($"{operation.ReactionRegistration.Id}___{DateTime.UtcNow.ToStringInvariantPreferred()}");
            var readonlyRecords = records.ToDictionary(k => k.Key, v => (IReadOnlyList<long>)v.Value);
            var result = records.Any()
                ? new ReactionEvent(
                    reactionId,
                    operation.ReactionRegistration.Id,
                    operation.ReactionRegistration.ReactionContext,
                    readonlyRecords,
                    DateTime.UtcNow,
                    operation.ReactionRegistration.Tags)
                : null;

            //put reaction event to reaction stream later, where do we complete
            //foreach marked completed
            //global catch and mark all failed on failure to write, on failure elsewhere cancelrunning
            return result;
        }
    }
}
