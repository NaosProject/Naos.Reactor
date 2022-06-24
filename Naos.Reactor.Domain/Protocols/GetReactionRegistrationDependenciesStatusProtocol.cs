// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateScheduleProtocol.cs" company="Naos Project">
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
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="EvaluateScheduleOp"/>.
    /// </summary>
    public partial class GetReactionRegistrationDependenciesStatusProtocol : SyncSpecificReturningProtocolBase<GetReactionRegistrationDependenciesStatusOp, IReadOnlyDictionary<string, IReadOnlyDictionary<long, HandlingStatus>>>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> reactionOperationStreamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetReactionRegistrationDependenciesStatusProtocol"/> class.
        /// </summary>
        /// <param name="reactionOperationStreamFactory">The stream factory for streams used in dependencies.</param>
        public GetReactionRegistrationDependenciesStatusProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> reactionOperationStreamFactory)
        {
            reactionOperationStreamFactory.MustForArg(nameof(reactionOperationStreamFactory)).NotBeNull();

            this.reactionOperationStreamFactory = reactionOperationStreamFactory;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override IReadOnlyDictionary<string, IReadOnlyDictionary<long, HandlingStatus>> Execute(
            GetReactionRegistrationDependenciesStatusOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var reactionRegistration = operation.ReactionRegistration;

            if (reactionRegistration.Dependencies.Count != 1)
            {
                throw new NotSupportedException(
                    Invariant(
                        $"Only 1 single {typeof(RecordFilterReactorDependency)} is supported, {reactionRegistration.Dependencies.Count} were supplied."));
            }

            var dependency = reactionRegistration.Dependencies.Single();
            var recordFilterDependency = dependency as RecordFilterReactorDependency;
            if (recordFilterDependency == null)
            {
                throw new NotSupportedException(
                    Invariant($"Only {typeof(RecordFilterReactorDependency)} is supported, {dependency?.GetType().ToStringReadable()}."));
            }

            var result = new Dictionary<string, IReadOnlyDictionary<long, HandlingStatus>>();
            foreach (var recordFilterEntry in recordFilterDependency.Entries)
            {
                var concern = EvaluateReactionRegistrationOp.BuildHandlingConcern(reactionRegistration, recordFilterEntry);
                var getHandlingStatusOp = new StandardGetHandlingStatusOp(
                    concern,
                    recordFilterEntry.RecordFilter,
                    new HandlingFilter());
                var stream =
                    (IStandardStream)reactionOperationStreamFactory.Execute(new GetStreamFromRepresentationOp(recordFilterEntry.StreamRepresentation));
                stream.MustForOp(nameof(stream))
                      .BeAssignableToType<ISyncReturningProtocol<StandardGetHandlingStatusOp, IReadOnlyDictionary<long, HandlingStatus>>>();
                var streamProtocol = (ISyncReturningProtocol<StandardGetHandlingStatusOp, IReadOnlyDictionary<long, HandlingStatus>>)stream;
                var handlingStatusesForDependency = streamProtocol.Execute(getHandlingStatusOp);
                result.Add(recordFilterEntry.Id, handlingStatusesForDependency);
            }

            return result;
        }
    }
}
