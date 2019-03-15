// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseDebugTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System.Threading.Tasks;
    using Naos.Database.SqlServer.Administration;
    using Xunit;

    public static class DatabaseDebugTest
    {
        [Fact(Skip = "Used for debugging.")]
        public static async Task TestCopyObjects()
        {
            var source = ConnectionStringHelper.BuildConnectionString("(local)\\SQLEXPRESS", "Source");
            var target = ConnectionStringHelper.BuildConnectionString("(local)\\SQLEXPRESS", "Target");
            await DatabaseObjectCopier.CopyObjects(
                new[] { "StoredProcOne", "StoredProcTwo", "TableOne", "TableTwo", "FK_TableTwo_TableOne" },
                source,
                target);
        }

        [Fact(Skip = "Used for debugging.")]
        public static void TestScriptDatabase()
        {
            var source = ConnectionStringHelper.BuildConnectionString("(local)\\SQLEXPRESS", "Source");
            Scripter.ScriptDatabaseToFilePath(source, @"D:\Temp\SourceDatabase", null, true);
        }
    }
}
