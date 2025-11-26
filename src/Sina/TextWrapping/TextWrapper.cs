#nullable enable

using System.Collections.Generic;
using System.Linq;
using SourceGit.Sina.TextWrapping.Implementation;

namespace SourceGit.Sina.TextWrapping;

public static class TextWrapper
{
    public static IReadOnlyList<Replacement> Wrap(string text, TextWrapperOptions options)
    {
        return text
            .Tokenize(options)
            .Wrap(options)
            .Normalize(text)
            .ToList();
    }
}

public record TextWrapperOptions(int TabWidth, string NewLine, int WrapLength);

public record Replacement(int Start, int Length, string NewString);
