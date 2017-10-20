// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseDebugTest.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Test
{
    using System.Threading.Tasks;

    using Naos.Database.MessageBus.Contract;
    using Naos.Database.MessageBus.Handler;

    using Xunit;

    public class DatabaseDebugTest
    {
        [Fact(Skip = "Used for debugging.")]
        public async Task TestCopyObjects()
        {
            await DatabaseObjectCopier.CopyObjects(new[] { "TestCopySprocOne" }, "localhost", "localhost");
        }
    }
}
