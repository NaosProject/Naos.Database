// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryExtensions.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Spritely.Cqrs;
    using Spritely.Recipes;

    internal static class QueryExtensions
    {
        public static string ToCommandParameters(this ICommand command)
        {
            return command.ToParameters();
        }

        public static string ToQueryParameters<TResult>(this IQuery<TResult> query)
        {
            return query.ToParameters();
        }

        private static string ToParameters(this object instance)
        {
            var regularWhereParts = instance.GetRegularProperties().GetWhereParts();
            var inWhereParts = instance.GetEnumerableProperties().GetWhereInParts();
            var queryParameters = regularWhereParts.Concat(inWhereParts).JoinAllWithAnd();

            return queryParameters;
        }

        private static IEnumerable<KeyValuePair<string, object>> GetEnumerableProperties(this object instance)
        {
            var type = instance.GetType();

            var allProperties = type.GetProperties().Where(p => p.Name != "ModelType");

            var enumerableProperties = allProperties.Where(
                p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)).ToList();

            return enumerableProperties.Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(instance)));
        }

        private static IEnumerable<KeyValuePair<string, object>> GetRegularProperties(this object instance)
        {
            var type = instance.GetType();

            var allProperties = type.GetProperties().Where(p => p.Name != "ModelType");

            var regularProperties = allProperties.Where(
                p => !(p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))).ToList();

            return regularProperties.Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(instance)));
        }

        private static IEnumerable<string> GetWhereParts(this IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return parameters.Select(p => string.Format(CultureInfo.InvariantCulture, "[{0}] = @{0}", p.Key));
        }

        private static IEnumerable<string> GetWhereInParts(this IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return parameters.Select(
                p =>
                    p.Key.StartsWith("not", StringComparison.OrdinalIgnoreCase)
                        ? string.Format(CultureInfo.InvariantCulture, "{0} not in @{1}", p.Key.Substring(3), p.Key)
                        : string.Format(CultureInfo.InvariantCulture, "{0} in @{0}", p.Key));
        }

        private static string JoinAllWithAnd(this IEnumerable<string> parts)
        {
            var result = new StringBuilder();

            var allParts = parts.ToList();
            allParts.Take(allParts.Count - 1).ForEach(p => result.AppendFormat("{0} and ", p));
            result.Append(allParts.Last());

            return result.ToString();
        }
    }
}
