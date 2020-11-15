// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Extensions for sanitizing a string for a file path.
    /// </summary>
    public static class FilePathExtensions
    {
        private static readonly IReadOnlyDictionary<string, string> IllegalToReplacementTokenMap;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "All are declared in static constructor.")]
        static FilePathExtensions()
        {
            var illegalCharToCoreReplacementTokenMap = new Dictionary<char, string>
                                                                                     {
                                                                                         { '"', "quote" },
                                                                                         { '<', "lt" },
                                                                                         { '>', "gt" },
                                                                                         { '|', "pipe" },
                                                                                         { char.MinValue, "min" },
                                                                                         { '\x0001', "x1" },
                                                                                         { '\x0002', "x2" },
                                                                                         { '\x0003', "x3" },
                                                                                         { '\x0004', "x4" },
                                                                                         { '\x0005', "x5" },
                                                                                         { '\x0006', "x6" },
                                                                                         { '\a', "backA" },
                                                                                         { '\b', "backB" },
                                                                                         { '\t', "tab" },
                                                                                         { '\n', "new" },
                                                                                         { '\v', "backV" },
                                                                                         { '\f', "backF" },
                                                                                         { '\r', "ret" },
                                                                                         { '\x000E', "xE" },
                                                                                         { '\x000F', "xF" },
                                                                                         { '\x0010', "x10" },
                                                                                         { '\x0011', "x11" },
                                                                                         { '\x0012', "x12" },
                                                                                         { '\x0013', "x13" },
                                                                                         { '\x0014', "x14" },
                                                                                         { '\x0015', "x15" },
                                                                                         { '\x0016', "x16" },
                                                                                         { '\x0017', "x17" },
                                                                                         { '\x0018', "x18" },
                                                                                         { '\x0019', "x19" },
                                                                                         { '\x001A', "x1A" },
                                                                                         { '\x001B', "x1B" },
                                                                                         { '\x001C', "x1C" },
                                                                                         { '\x001D', "x1D" },
                                                                                         { '\x001E', "x1E" },
                                                                                         { '\x001F', "x1F" },
                                                                                         { ':', "colon" },
                                                                                         { '*', "star" },
                                                                                         { '?', "ques" },
                                                                                         { '\\', "back" },
                                                                                         { '/', "slash" },
                                                                                     };

            IllegalToReplacementTokenMap =
                illegalCharToCoreReplacementTokenMap.ToDictionary(
                    k => k.Key.ToString(),
                    v => Invariant($"___{v.Value}___"));

            var illegalToCheck = Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()).Distinct().OrderBy(_ => _).ToList();
            illegalCharToCoreReplacementTokenMap
               .Keys.OrderBy(_ => _).ToList()
               .MustForOp(nameof(illegalCharToCoreReplacementTokenMap))
               .BeEqualTo(
                    illegalToCheck,
                    Invariant($"All framework specified illegal chars from Path.[{nameof(Path.GetInvalidPathChars)}|{nameof(Path.GetInvalidFileNameChars)}] should have an associated replacement token in {nameof(illegalCharToCoreReplacementTokenMap)}"));
        }

        /// <summary>
        /// Encodes for file path.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string EncodeForFilePath(this string input)
        {
            var result = input;
            foreach (var illegalToReplacement in IllegalToReplacementTokenMap)
            {
                result = result.Replace(illegalToReplacement.Key, illegalToReplacement.Value);
            }

            return result;
        }

        /// <summary>
        /// Decodes for file path.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string DecodeForFilePath(this string input)
        {
            var result = input;
            foreach (var illegalToReplacement in IllegalToReplacementTokenMap)
            {
                result = result.Replace(illegalToReplacement.Value, illegalToReplacement.Key);
            }

            return result;
        }
    }
}
