// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReactionContext.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.CodeAnalysis.Recipes;

    /// <summary>
    /// Interface for context to be specified in <see cref="ReactionRegistration"/> and provided to the <see cref="ReactionEvent"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = NaosSuppressBecause.CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute)]
    public interface IReactionContext
    {
    }
}
