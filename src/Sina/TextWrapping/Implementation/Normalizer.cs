#nullable enable

using System.Collections.Generic;

namespace SourceGit.Sina.TextWrapping.Implementation
{
    internal static class Normalizer
    {
        public static IEnumerable<Replacement> Normalize(this IEnumerable<Replacement> replacements, string text)
        {
            var offset = 0;

            foreach (var current in replacements)
            {
                var (start, length, newString) = current;

                if (text.Substring(start, length) == newString)
                    continue;

                yield return new Replacement(start + offset, length, newString);

                offset += newString.Length - length;
            }
        }
    }
}
