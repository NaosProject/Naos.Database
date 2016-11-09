// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlInjectorCheckerTest.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools.Test
{
    using System;

    using Naos.Database.Contract;

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

        [Fact]
        public static void ThrowIfNotAlphanumericOrSpace_TextIsNotAlphaNumericOrWhitespace_ThrowsArgumentException()
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

        [Fact]
        public static void ThrowIfNotAlphanumericOrSpace_TextIsAlphaNumericOrWhitespace_DoesNotThrow()
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
