// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseDebugTest.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Test
{
    using System;
    using System.Threading.Tasks;

    using Naos.Database.Contract;
    using Naos.Database.MessageBus.Contract;
    using Naos.Database.MessageBus.Handler;

    using Xunit;

    public class DatabaseDebugTest
    {
        [Fact(Skip = "Used for debugging.")]
        public async Task TestCopyObjects()
        {
            var source = ConnectionStringHelper.BuildConnectionString("(local)\\SQLEXPRESS", "Source");
            var target = ConnectionStringHelper.BuildConnectionString("(local)\\SQLEXPRESS", "Target");
            await DatabaseObjectCopier.CopyObjects(
                new[] { "StoredProcOne", "StoredProcTwo", "TableOne", "TableTwo", "FK_TableTwo_TableOne" },
                source,
                target);
        }

        [Fact(Skip = "Used for debugging.")]
        public void TestScriptDatabase()
        {
            var source = ConnectionStringHelper.BuildConnectionString("(local)\\SQLEXPRESS", "Source");
            Scripter.ScriptDatabaseToFilePath(source, @"D:\Temp\SourceDatabase", null, true);
        }
    }
}
