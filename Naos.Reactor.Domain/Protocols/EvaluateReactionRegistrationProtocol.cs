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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
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
            throw new NotImplementedException();
        }
    }
}
