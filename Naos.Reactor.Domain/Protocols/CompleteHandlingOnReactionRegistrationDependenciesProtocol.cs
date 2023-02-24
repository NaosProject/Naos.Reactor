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
    using OBeautifulCode.Cloning.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="ComputePreviousExecutionFromScheduleOp"/>.
    /// </summary>
    public partial class CompleteHandlingOnReactionRegistrationDependenciesProtocol : SyncSpecificVoidProtocolBase<CompleteHandlingOnReactionRegistrationDependenciesOp>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> reactionOperationStreamFactory;
        private readonly IReadOnlyCollection<NamedValue<string>> handlingTags;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteHandlingOnReactionRegistrationDependenciesProtocol"/> class.
        /// </summary>
        /// <param name="reactionOperationStreamFactory">The stream factory for streams used in dependencies.</param>
        /// <param name="handlingTags">Optional tags to provide to handling entries.</param>
        public CompleteHandlingOnReactionRegistrationDependenciesProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> reactionOperationStreamFactory,
            IReadOnlyCollection<NamedValue<string>> handlingTags = null)
        {
            reactionOperationStreamFactory.MustForArg(nameof(reactionOperationStreamFactory)).NotBeNull();

            this.reactionOperationStreamFactory = reactionOperationStreamFactory;
            this.handlingTags = handlingTags;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override void Execute(
            CompleteHandlingOnReactionRegistrationDependenciesOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var reactionRegistration = operation.ReactionRegistration;
            var tags = reactionRegistration.Tags?.Any() ?? false
                ? reactionRegistration.Tags.DeepClone().Concat(this.handlingTags?.DeepClone() ?? new NamedValue<string>[0]).ToList()
                : this.handlingTags;

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

            foreach (var recordFilterEntry in recordFilterDependency.Entries)
            {
                var concern = EvaluateReactionRegistrationOp.BuildHandlingConcern(reactionRegistration, recordFilterEntry);
                var getHandlingStatusOp = new StandardGetHandlingStatusOp(
                    concern,
                    recordFilterEntry.RecordFilter,
                    new HandlingFilter(CompleteHandlingOnReactionRegistrationDependenciesOp.AvailableStatuses));
                var stream =
                    (IStandardStream)reactionOperationStreamFactory.Execute(new GetStreamFromRepresentationOp(recordFilterEntry.StreamRepresentation));
                stream.MustForOp(nameof(stream))
                      .BeAssignableToType<ISyncReturningProtocol<StandardGetHandlingStatusOp, IReadOnlyDictionary<long, HandlingStatus>>>();
                var streamProtocol = (ISyncReturningProtocol<StandardGetHandlingStatusOp, IReadOnlyDictionary<long, HandlingStatus>>)stream;
                var handlingStatusesForDependency = streamProtocol.Execute(getHandlingStatusOp);
                var missingHandlingMap = handlingStatusesForDependency
                                        .Where(
                                             _ => CompleteHandlingOnReactionRegistrationDependenciesOp
                                                 .HandlingStatusesToRegardAsIncomplete.Contains(_.Value))
                                        .ToDictionary(k => k.Key, v => v.Value);
                if (missingHandlingMap.Any())
                {
                    foreach (var item in missingHandlingMap)
                    {
                        if (!operation.AcceptableHandlingStatuses.Contains(item.Value))
                        {
                            var acceptableStatusesString = operation.AcceptableHandlingStatuses.Select(_ => _.ToString()).ToCsv();
                            throw new InvalidOperationException(Invariant($"Record '{item.Key}' in stream '{stream.Name}' has status '{item.Value}' which is not in the acceptable status list from the operation: {acceptableStatusesString}."));
                        }

                        var completeOp = new StandardUpdateHandlingStatusForRecordOp(
                            item.Key,
                            concern,
                            HandlingStatus.Completed,
                            new[]
                            {
                                HandlingStatus.Running,
                            },
                            operation.Details,
                            tags);

                        var runningOp = new StandardUpdateHandlingStatusForRecordOp(
                            item.Key,
                            concern,
                            HandlingStatus.Running,
                            CompleteHandlingOnReactionRegistrationDependenciesOp.AvailableStatuses,
                            operation.Details,
                            tags);

                        if (CompleteHandlingOnReactionRegistrationDependenciesOp.AvailableStatuses.Contains(item.Value))
                        {
                            stream.Execute(runningOp);
                            stream.Execute(completeOp);
                        }
                        else if (item.Value == HandlingStatus.Running)
                        {
                            stream.Execute(completeOp);
                        }
                        else
                        {
                            throw new NotSupportedException(Invariant($"Record '{item.Key}' in stream '{stream.Name}' has status '{item.Value}' which cannot be reset."));
                        }
                    }
                }
            }
        }
    }
}
