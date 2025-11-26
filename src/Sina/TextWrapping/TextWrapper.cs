#nullable enable

using System;
using System.Collections.Generic;

namespace SourceGit.Sina.TextWrapping;

public static class TextWrapper
{
    public static IReadOnlyList<Replacement> Wrap(string text, TextWrapperOptions options)
    {
        return Array.Empty<Replacement>();
    }
}

public record TextWrapperOptions(int TabWidth, string NewLine, int WrapLength);

public record Replacement(int Start, int Length, string NewString);
