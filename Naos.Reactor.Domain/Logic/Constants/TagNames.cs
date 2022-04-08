// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagNames.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
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
    }
}
