#nullable enable

using AvaloniaEdit.Document;

namespace SourceGit.Sina.TextWrapping;

public static class TextDocumentWrapperExtensions
{
    public static void WrapDocument(this TextDocument document, TextWrapperOptions options)
    {
        var isReplacing = false;

        foreach (var (start, length, newString) in TextWrapper.Wrap(document.Text, options))
        {
            if (!isReplacing)
            {
                document.BeginUpdate();
                isReplacing = true;
            }

            document.Replace(start, length, newString);
        }

        if (isReplacing)
            document.EndUpdate();
    }
}
