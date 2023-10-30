// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiagnosticsDummyFactory.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Diagnostics.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using FakeItEasy;
    using Naos.Diagnostics;
    using Naos.Diagnostics.Domain;
    using OBeautifulCode.AutoFakeItEasy;

    /// <summary>
    /// A dummy factory for Accounting Time types.
    /// </summary>
#if !NaosDiagnosticsRecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Diagnostics", "See package version number")]
    internal
#else
    public
#endif
    class DiagnosticsDummyFactory : DefaultDiagnosticsDummyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsDummyFactory"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This is not excessively complex.  Dummy factories typically wire-up many types.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "This is not excessively complex.  Dummy factories typically wire-up many types.")]
        public DiagnosticsDummyFactory()
        {
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CheckDrivesOp(
                    (decimal)A.Dummy<int>().ThatIsInRange(0, 100)/100));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new CheckDrivesReport(
                    A.Dummy<bool>(),
                    A.Dummy<IReadOnlyDictionary<string, CheckSingleDriveReport>>(),
                    A.Dummy<UtcDateTime>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var totalSizeInBytes = A.Dummy<PositiveInteger>();
                    var totalFreeSpaceInBytes = A.Dummy<int>().ThatIsInRange(1, totalSizeInBytes - 1);
                    return new CheckSingleDriveReport(
                        A.Dummy<string>(),
                        totalFreeSpaceInBytes,
                        totalSizeInBytes);
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new AssemblyDetails(
                        A.Dummy<string>(), 
                        A.Dummy<string>(), 
                        A.Dummy<string>(),
                        A.Dummy<string>());

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new OperatingSystemDetails(
                        A.Dummy<string>(),
                        A.Dummy<string>(),
                        A.Dummy<string>());

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new MachineDetails(
                        A.Dummy<Dictionary<string, string>>(),
                        A.Dummy<int>(),
                        A.Dummy<Dictionary<string, decimal>>(),
                        A.Dummy<bool>(),
                        A.Dummy<OperatingSystemDetails>(),
                        A.Dummy<string>());

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new ProcessDetails(
                        A.Dummy<string>(),
                        A.Dummy<string>(),
                        A.Dummy<string>(),
                        A.Dummy<string>(),
                        A.Dummy<bool>());

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new PerformanceCounterDescription(
                        A.Dummy<string>(),
                        A.Dummy<string>(),
                        A.Dummy<string>(),
                        A.Dummy<float>());

                    return result;
                });

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () =>
                {
                    var result = new PerformanceCounterSample(
                        A.Dummy<PerformanceCounterDescription>(),
                        A.Dummy<float>());

                    return result;
                });
        }
    }
}
