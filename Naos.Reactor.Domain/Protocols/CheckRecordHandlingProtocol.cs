﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckRecordHandlingProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Protocol for <see cref="CheckRecordHandlingOp"/>.
    /// </summary>
    public partial class CheckRecordHandlingProtocol : SyncSpecificReturningProtocolBase<CheckRecordHandlingOp, CheckRecordHandlingResult>
    {
        /// <summary>
        /// Prefix of the exception message of a <see cref="InvalidOperationException" /> thrown when the status set of records comes back empty.
        /// </summary>
        public const string EmptyStatusSetExceptionMessagePrefix = "Unexpected empty result set from status query for:";

        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordHandlingProtocol"/> class.
        /// </summary>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public CheckRecordHandlingProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();
            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        public override CheckRecordHandlingResult Execute(
            CheckRecordHandlingOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var stream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(operation.StreamRepresentation));
            stream.MustForOp(nameof(stream))
                  .BeAssignableToType<ISyncReturningProtocol<StandardGetHandlingStatusOp, IReadOnlyDictionary<long, HandlingStatus>>>();
            var streamProtocol = (ISyncReturningProtocol<StandardGetHandlingStatusOp, IReadOnlyDictionary<long, HandlingStatus>>)stream;

            var getStatusOp = new StandardGetHandlingStatusOp(
                operation.Concern,
                operation.RecordFilter,
                operation.HandlingFilter);
            var statuses = streamProtocol.Execute(getStatusOp);
            if (statuses == null || !statuses.Any())
            {
                throw new InvalidOperationException(FormattableString.Invariant($"{EmptyStatusSetExceptionMessagePrefix} {getStatusOp}."));
            }

            var result = new CheckRecordHandlingResult(operation.StreamRepresentation, statuses);
            return result;
        }
    }
}
