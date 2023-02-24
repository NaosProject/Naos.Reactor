// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteOpRequestedOnScheduleEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Cron;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A scheduled operation to be executed on a schedule.
    /// </summary>
    public partial class ScheduledOpRegistration : EventBase<string>, IHaveTags, IHaveStreamRepresentation, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledOpRegistration"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="operationToExecute">The <see cref="IVoidOperation"/> to be executed.</param>
        /// <param name="schedule">The <see cref="ISchedule"/> to be used for determining execution cadence.</param>
        /// <param name="streamRepresentation">The <see cref="IStreamRepresentation"/> to be find the appropriate stream for writing the operation wrapped in <see cref="ScheduledExecuteOpRequestedEvent{TOperation}" /> with the correct next execution time per the schedule.</param>
        /// <param name="timestampUtc">The timestamp of the event in UTC format.</param>
        /// <param name="scheduledOpAlreadyRunningStrategy">The strategy of how to deal an <see cref="ScheduledExecuteOpRequestedEvent{TOperation}" />.</param>
        /// <param name="details">The optional details.</param>
        /// <param name="tags">The optional tags for the operation.</param>
        public ScheduledOpRegistration(
            string id,
            IVoidOperation operationToExecute,
            ISchedule schedule,
            IStreamRepresentation streamRepresentation,
            DateTime timestampUtc,
            ScheduledOpAlreadyRunningStrategy scheduledOpAlreadyRunningStrategy,
            string details = null,
            IReadOnlyCollection<NamedValue<string>> tags = null) : base(id, timestampUtc)
        {
            operationToExecute.MustForArg(nameof(operationToExecute)).NotBeNull();
            schedule.MustForArg(nameof(schedule)).NotBeNull();
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();

            this.OperationToExecute = operationToExecute;
            this.Schedule = schedule;
            this.StreamRepresentation = streamRepresentation;
            this.ScheduledOpAlreadyRunningStrategy = scheduledOpAlreadyRunningStrategy;
            this.Details = details;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the operation to be executed.
        /// </summary>
        public IVoidOperation OperationToExecute { get; private set; }

        /// <summary>
        /// Gets the schedule evaluated.
        /// </summary>
        public ISchedule Schedule { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the strategy of how to deal an <see cref="ScheduledExecuteOpRequestedEvent{TOperation}" />.
        /// </summary>
        public ScheduledOpAlreadyRunningStrategy ScheduledOpAlreadyRunningStrategy { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
