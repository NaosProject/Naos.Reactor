﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactorDummyFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package Naos.Build.Conventions.VisualStudioProjectTemplates.Domain.Test (1.55.30)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using Naos.Cron;
    using Naos.Database.Domain;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Type;

    /// <summary>
    /// A Dummy Factory for types in <see cref="Naos.Reactor.Domain"/>.
    /// </summary>
#if !NaosReactorSolution
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Reactor.Domain.Test", "See package version number")]
    internal
#else
    public
#endif
    class ReactorDummyFactory : DefaultReactorDummyFactory
    {
        public ReactorDummyFactory()
        {
            /* Add any overriding or custom registrations here. */

            // ------------------------------- ENUMS --------------------------------------
            AutoFixtureBackedDummyFactory.ConstrainDummyToExclude(ChainOfResponsibilityLinkMatchStrategy.Unknown);
            AutoFixtureBackedDummyFactory.ConstrainDummyToExclude(CompositeHandlingStatusMatchStrategy.Unknown);
            AutoFixtureBackedDummyFactory.ConstrainDummyToExclude(RecordExistsMatchStrategy.Unknown);
            AutoFixtureBackedDummyFactory.ConstrainDummyToExclude(ScheduledOpAlreadyRunningStrategy.Unknown);

            // ------------------------------- BASE CLASSES -------------------------------
            AutoFixtureBackedDummyFactory.UseRandomInterfaceImplementationForDummy<IReactionContext>();

            // ------------------------------- CLASSES ------------------------------------
            AutoFixtureBackedDummyFactory.AddDummyCreator(() => (IEvent)new NullEvent(A.Dummy<UtcDateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new ReactionEvent(
                    A.Dummy<string>(),
                    A.Dummy<string>(),
                    new DetailsReactionContext(A.Dummy<string>()),
                    A.Dummy<IReadOnlyDictionary<IStreamRepresentation, IReadOnlyList<long>>>(),
                    A.Dummy<UtcDateTime>(),
                    A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));


            AutoFixtureBackedDummyFactory.AddDummyCreator(() => (IReactorDependency)new RecordFilterReactorDependency(
                new[]
                {
                    new RecordFilterEntry(
                        A.Dummy<string>(),
                        new StreamRepresentation(A.Dummy<string>()),
                        new RecordFilter(),
                        A.Dummy<bool>(),
                        A.Dummy<bool>()),
                }));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CheckRecordExistsOp(
                    new StreamRepresentation(A.Dummy<string>()), 
                    new RecordFilter(
                        new[]
                        {
                            A.Dummy<long>(),
                        })));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new ComputePreviousExecutionFromScheduleOp(A.Dummy<ScheduleBase>(), A.Dummy<UtcDateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                    new EventToPutWithIdOnRecordFilterMatch<string>(
                        RecordExistsMatchStrategy.SomeFound,
                        new EventToPutWithId<string>(
                            A.Dummy<string>(),
                            new NullEvent<string>(A.Dummy<string>(), A.Dummy<UtcDateTime>()),
                            A.Dummy<StreamRepresentation>(),
                            A.Dummy<bool>())));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                    new EventToPutWithIdOnHandlingStatusMatch<string>(
                        CompositeHandlingStatus.SomeAvailable,
                        CompositeHandlingStatusMatchStrategy.ActualCompositeStatusHasAllQueryCompositeStatusFlags,
                        new EventToPutWithId<string>(
                            A.Dummy<string>(),
                            new NullEvent<string>(A.Dummy<string>(), A.Dummy<UtcDateTime>()),
                            A.Dummy<StreamRepresentation>(),
                            A.Dummy<bool>())));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new ScheduledOpRegistration(
                    A.Dummy<string>(),
                    new DeleteDatabaseOp(A.Dummy<string>()),
                    A.Dummy<ScheduleBase>(),
                    A.Dummy<StreamRepresentation>(),
                    A.Dummy<ScheduledOpAlreadyRunningStrategy>(),
                    A.Dummy<bool>(),
                    A.Dummy<string>(),
                    A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new ScheduledExecuteOpRequestedEvent(
                    A.Dummy<string>(),
                    new DeleteDatabaseOp(A.Dummy<string>()),
                    A.Dummy<UtcDateTime>(),
                    A.Dummy<UtcDateTime>(),
                    A.Dummy<string>(),
                    A.Dummy<IReadOnlyCollection<NamedValue<string>>>()));
        }
    }
}
