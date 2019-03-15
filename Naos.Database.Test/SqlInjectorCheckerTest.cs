// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlInjectorCheckerTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    using Naos.Database.SqlServer.Administration;
    using Xunit;

    /// <summary>
    /// Tests the <see cref="SqlInjectorChecker"/> class.
    /// </summary>
    public static class SqlInjectorCheckerTest
    {
        // ReSharper disable InconsistentNaming
        [Fact]
        public static void ThrowIfNotAlphanumericOrSpaceOrUnderscore_TextToCheckIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(null));
        }

        [Fact]
        public static void ThrowIfNotAlphanumericOrSpaceOrUnderscore_TextIsEmpty_DoesNotThrow()
        {
            // Arrange, Act, Assert
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(string.Empty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AlphaNumeric", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void ThrowIfNotAlphanumericOrSpaceOrUnderscore_TextIsNotAlphaNumericOrWhiteSpace_ThrowsArgumentException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(" \r "));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(" \n "));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(" \t "));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore("abc*"));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore("%asdf"));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore("("));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore("abc123$abc1234"));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AlphaNumeric", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void ThrowIfNotAlphanumericOrSpaceOrUnderscore_TextIsAlphaNumericOrWhiteSpaceOrUnderscore_DoesNotThrow()
        {
            // Arrange, Act, Assert
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore(" ");
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore("a");
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore("9");
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore("abcdefghijklmnopqrstuvwxyz 1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpaceOrUnderscore("abcdefghijklmnopqrstuvwxyz_1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }

        // ReSharper restore InconsistentNaming
    }
}
