#nullable enable

using System;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Indentation;

namespace SourceGit.Sina.TextWrapping;

public class TextDocumentWrappingBehavior : Behavior<TextEditor>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is { } textEditor)
            textEditor.WordWrap = false;

        if (AssociatedObject?.Document is { } document)
            document.TextChanged += OnDocumentTextChanged;

        if (AssociatedObject?.TextArea is { } textArea)
        {
            textArea.IndentationStrategy = new NoIndentationStrategy();
            textArea.TextEntering += OnTextAreaTextEntering;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject?.TextArea is { } textArea)
            textArea.TextEntering -= OnTextAreaTextEntering;

        if (AssociatedObject?.Document is { } document)
            document.TextChanged -= OnDocumentTextChanged;

        base.OnDetaching();
    }


    private void OnDocumentTextChanged(object? sender, EventArgs e)
    {
        if (AssociatedObject?.Document is not { } document)
            return;

        var tabWidth = ViewModels.Preferences.Instance.EditorTabWidth;
        var newLine = Environment.NewLine;
        var options = new TextWrapperOptions(tabWidth, newLine, WrapLength: 72);

        document.WrapDocument(options);
    }

    private void OnTextAreaTextEntering(object? sender, TextInputEventArgs e)
    {
        if (e.Text is "\n")
            e.Text = "\n\n";
        else if (e.Text is "\r\n")
            e.Text = "\r\n\r\n";
        else if (e.Text is "\r\r")
            e.Text = "\r";
    }

    private class NoIndentationStrategy : IIndentationStrategy
    {
        public void IndentLine(TextDocument document, DocumentLine line)
        {
            // Do nothing.
        }

        public void IndentLines(TextDocument document, int beginLine, int endLine)
        {
            // Do nothing.
        }
    }
}
