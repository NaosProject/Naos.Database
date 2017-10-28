// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlInjectorCheckerTest.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Test
{
    using System;

    using Naos.Database.SqlServer;

    using Xunit;

    /// <summary>
    /// Tests the <see cref="SqlInjectorChecker"/> class.
    /// </summary>
    public static class SqlInjectorCheckerTest
    {
        // ReSharper disable InconsistentNaming
        [Fact]
        public static void ThrowIfNotAlphanumericOrSpace_TextToCheckIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(null));
        }

        [Fact]
        public static void ThrowIfNotAlphanumericOrSpace_TextIsEmpty_DoesNotThrow()
        {
            // Arrange, Act, Assert
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(string.Empty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AlphaNumeric", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void ThrowIfNotAlphanumericOrSpace_TextIsNotAlphaNumericOrWhiteSpace_ThrowsArgumentException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(" \r "));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(" \n "));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(" \t "));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace("abc*"));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace("%asdf"));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace("("));
            Assert.Throws<ArgumentException>(() => SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace("abc123$abc1234"));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AlphaNumeric", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void ThrowIfNotAlphanumericOrSpace_TextIsAlphaNumericOrWhiteSpace_DoesNotThrow()
        {
            // Arrange, Act, Assert
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace(" ");
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace("a");
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace("9");
            SqlInjectorChecker.ThrowIfNotAlphanumericOrSpace("abcdefghijklmnopqrstuvwxyz 1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }

        // ReSharper restore InconsistentNaming
    }
}
