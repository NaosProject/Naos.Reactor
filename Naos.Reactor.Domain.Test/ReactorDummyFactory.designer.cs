﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.177.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain.Test
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;

    using global::FakeItEasy;

    using global::Naos.Database.Domain;
    using global::Naos.Reactor.Domain;

    using global::OBeautifulCode.AutoFakeItEasy;
    using global::OBeautifulCode.Math.Recipes;
    using global::OBeautifulCode.Representation.System;
    using global::OBeautifulCode.Type;

    /// <summary>
    /// The default (code generated) Dummy Factory.
    /// Derive from this class to add any overriding or custom registrations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("OBeautifulCode.CodeGen.ModelObject", "1.0.177.0")]
#if !NaosReactorSolution
    internal
#else
    public
#endif
    abstract class DefaultReactorDummyFactory : IDummyFactory
    {
        public DefaultReactorDummyFactory()
        {
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CheckRecordHandlingOp(
                                 A.Dummy<IStreamRepresentation>(),
                                 A.Dummy<string>(),
                                 A.Dummy<RecordFilter>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CheckRecordHandlingResult(
                                 A.Dummy<IStreamRepresentation>(),
                                 A.Dummy<IReadOnlyDictionary<long, HandlingStatus>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new EvaluateRegisteredReactionOp(
                                 A.Dummy<RegisteredReaction>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new EventToPutWithId<Version>(
                                 A.Dummy<Version>(),
                                 A.Dummy<IEvent>(),
                                 A.Dummy<IStreamRepresentation>(),
                                 A.Dummy<bool>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new EventToPutWithIdOnMatch<Version>(
                                 A.Dummy<CompositeHandlingStatus>(),
                                 A.Dummy<CompositeHandlingStatusMatchStrategy>(),
                                 A.Dummy<EventToPutWithId<Version>>(),
                                 A.Dummy<bool>(),
                                 A.Dummy<bool>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new ReactionEvent(
                                 A.Dummy<string>(),
                                 A.Dummy<string>(),
                                 A.Dummy<IReadOnlyDictionary<IStreamRepresentation, IReadOnlyList<long>>>(),
                                 A.Dummy<DateTime>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RegisteredReaction(
                                 A.Dummy<string>(),
                                 A.Dummy<IReadOnlyList<IReactorDependency>>(),
                                 A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new RunReactorOp(
                                 A.Dummy<TypeRepresentation>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new StringIdentifiedReactorDependency(
                                 A.Dummy<string>(),
                                 A.Dummy<IStreamRepresentation>(),
                                 A.Dummy<StandardTryHandleRecordOp>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new WriteRecordOnMatchingHandlingStatusOp<Version>(
                                 A.Dummy<IReadOnlyCollection<CheckRecordHandlingOp>>(),
                                 A.Dummy<IReadOnlyList<EventToPutWithIdOnMatch<Version>>>(),
                                 A.Dummy<TimeSpan>()));
        }

        /// <inheritdoc />
        public Priority Priority => new FakeItEasy.Priority(1);

        /// <inheritdoc />
        public bool CanCreate(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public object Create(Type type)
        {
            return null;
        }
    }
}