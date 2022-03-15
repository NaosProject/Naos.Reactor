// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateReactionRegistrationProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;

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
        public override ReactionEvent Execute(
            EvaluateReactionRegistrationOp operation)
        {
            var records = new Dictionary<IStreamRepresentation, IReadOnlyList<long>>();

            foreach (var dependency in operation.ReactionRegistration.Dependencies)
            {
                var recordFilterDependency = dependency as RecordFilterReactorDependency;
                var stream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(recordFilterDependency.StreamRepresentation));
                stream.MustForOp(nameof(stream)).BeOfType<ISyncReturningProtocol<StandardTryHandleRecordOp, TryHandleRecordResult>>();
                var streamProtocol = ((ISyncReturningProtocol<StandardTryHandleRecordOp, TryHandleRecordResult>)stream);

                var handledIds = new List<long>();
                StreamRecord currentRecord = null;
                do
                {
                    //TODO: When does then get mark 'handled'? here or in RunReactorProtocol
                    var tryHandleRecordOp = new StandardTryHandleRecordOp(
                        operation.ReactionRegistration.Id,
                        recordFilterDependency.RecordFilter,
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
                    records.Add(recordFilterDependency.StreamRepresentation, handledIds);
                }
            }

            var reactionId = Invariant($"{operation.ReactionRegistration.Id}___{DateTime.UtcNow.ToStringInvariantPreferred()}");
            var result = records.Any()
                ? new ReactionEvent(
                    reactionId,
                    operation.ReactionRegistration.Id,
                    records,
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
