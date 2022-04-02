// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordFilterEntry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Entry to use in <see cref="RecordFilterReactorDependency"/>.
    /// </summary>
    public partial class ReactorException : OpExecutionFailedExceptionBase
    {
        /// <inheritdoc />
        public ReactorException(
            IOperation operation)
            : base(operation) {}

        /// <inheritdoc />
        public ReactorException(
            string message,
            IOperation operation)
            : base(message, operation) {}

        /// <inheritdoc />
        public ReactorException(
            string message,
            Exception innerException,
            IOperation operation)
            : base(message, innerException, operation) {}
    }
}
