﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagNames.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.Database.Domain;

    /// <summary>
    /// The names of various tags used in the reactor system.
    /// </summary>
    public static class TagNames
    {
        /// <summary>
        /// The name of the tag for an identifier of a <see cref="ReactionRegistration"/>.
        /// </summary>
        public const string ReactionId = "ReactionId";

        /// <summary>
        /// The name of the tag for an identifier of a <see cref="ReactionRegistration"/>.
        /// </summary>
        public const string ReactionRegistrationId = "ReactionRegistrationId";

        /// <summary>
        /// The name of the tag for an identifier of a potentially created <see cref="ReactionRegistration"/>.
        /// </summary>
        public const string PotentialReactionId = "PotentialReactionId";

        /// <summary>
        /// The name of the tag for an identifier of a <see cref="ScheduledOpRegistration" />.
        /// </summary>
        public const string ScheduledOpRegistrationId = "ScheduledOpRegistrationId";

        /// <summary>
        /// The name of the tag for an indicator of the <see cref="ScheduledOpRegistration" /> yielding an execution but skipping due to existing execution running.
        /// </summary>
        public const string ScheduledOpExecutionSkipped = "ScheduledOpExecutionSkipped";

        /// <summary>
        /// The name of the tag for an indicator of the <see cref="ScheduledOpRegistration" /> yielding an execution but a prior execution is currently in the <see cref="HandlingStatus.Failed" /> status.
        /// </summary>
        public const string ScheduledOpExecutionInFailedState = "ScheduledOpExecutionInFailedState";
    }
}
