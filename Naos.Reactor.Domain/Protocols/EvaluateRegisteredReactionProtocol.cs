// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateRegisteredReactionProtocol.cs" company="Naos Project">
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

    /// <summary>
    /// Process all new reactions.
    /// </summary>
    public partial class EvaluateRegisteredReactionProtocol
        : ISyncAndAsyncReturningProtocol<EvaluateRegisteredReactionOp, Reaction>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateRegisteredReactionProtocol"/> class.
        /// </summary>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public EvaluateRegisteredReactionProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();

            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        public Reaction Execute(
            EvaluateRegisteredReactionOp operation)
        {
            var id = DateTime.UtcNow.ToStringInvariantPreferred();
            var records = new Dictionary<IStreamRepresentation, IReadOnlyList<long>>();

            foreach (var dependency in operation.RegisteredReaction.Dependencies)
            {
                var stream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(dependency.StreamRepresentation));
                stream.MustForOp("streamMustBeIProtocol").BeOfType<IProtocol>();
                var handledIds = new List<long>();
                StreamRecord currentRecord = null;
                do
                {
                    currentRecord = ((IProtocol)stream).ExecuteViaReflection<StreamRecord>(dependency.TryHandleRecordOp);
                    if (currentRecord != null)
                    {
                        handledIds.Add(currentRecord.InternalRecordId);
                    }
                }
                while (currentRecord != null);

                if (handledIds.Any())
                {
                    records.Add(dependency.StreamRepresentation, handledIds);
                }
            }


            var result = records.Any() ? new Reaction(id, records) : null;
            return result;
        }

        /// <inheritdoc />
        public async Task<Reaction> ExecuteAsync(
            EvaluateRegisteredReactionOp operation)
        {
            var id = DateTime.UtcNow.ToStringInvariantPreferred();
            var records = new Dictionary<IStreamRepresentation, IReadOnlyList<long>>();

            foreach (var dependency in operation.RegisteredReaction.Dependencies)
            {
                var stream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(dependency.StreamRepresentation));
                stream.MustForOp("streamMustBeIProtocol").BeOfType<IProtocol>();
                var handledIds = new List<long>();
                StreamRecord currentRecord = null;
                do
                {
                    currentRecord = await ((IProtocol)stream).ExecuteViaReflectionAsync<StreamRecord>(dependency.TryHandleRecordOp);
                    if (currentRecord != null)
                    {
                        handledIds.Add(currentRecord.InternalRecordId);
                    }
                }
                while (currentRecord != null);

                if (handledIds.Any())
                {
                    records.Add(dependency.StreamRepresentation, handledIds);
                }
            }


            var result = records.Any() ? new Reaction(id, records) : null;
            return result;
        }
    }
}
