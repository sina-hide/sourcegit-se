#nullable enable

using System;
using System.Collections.Generic;

namespace SourceGit.Sina.TextWrapping.Implementation;

internal static class Wrapper
{
    public static IEnumerable<Replacement> Wrap(this IEnumerable<Token> tokens, TextWrapperOptions options)
    {
        var state = State.Paragraph;
        var breakToken = default(Token?);
        var spaceToken = default(Token?);
        var paragraphIndentation = 0;
        var lineLength = 0;

        foreach (var current in tokens)
        {
            var doNormalizeCurrent = false;

            switch (state, current.Type)
            {
                case (State.Paragraph, TokenType.Break):
                    spaceToken = null;
                    paragraphIndentation = 0;
                    doNormalizeCurrent = true;
                    break;

                case (State.Paragraph, TokenType.Space):
                    spaceToken = current;
                    paragraphIndentation = current.Count;
                    break;

                // First word in paragraph.  It belongs to the current line, even if it wouldn't fit.
                case (State.Paragraph, TokenType.Word):
                    state = State.Word;
                    if (spaceToken is not null)
                        yield return Replace(spaceToken, new string(' ', paragraphIndentation));
                    lineLength = paragraphIndentation + current.Count;
                    break;

                case (State.Break, TokenType.Break):
                    state = State.Paragraph;
                    paragraphIndentation = 0;
                    yield return Replace(breakToken!, options.NewLine);
                    doNormalizeCurrent = true;
                    break;

                case (State.Break, TokenType.Space):
                    spaceToken = current;
                    break;

                case (State.Break, TokenType.Word) when WordFitsIntoLine(current):
                {
                    state = State.Word;

                    // Change break to space.
                    if (breakToken is not null)
                        yield return Replace(breakToken, " ");

                    // Remove possible indentation.
                    if (spaceToken is not null)
                        yield return Replace(spaceToken, "");

                    lineLength += 1 + current.Count;

                    break;
                }

                // Word belongs to the next line.  Keep line break, indent next line.
                case (State.Break, TokenType.Word):
                {
                    state = State.Word;

                    yield return Replace(breakToken!, options.NewLine);

                    var indentation = new string(' ', paragraphIndentation);
                    if (spaceToken is not null)
                        yield return Replace(spaceToken, indentation);
                    else
                        yield return new Replacement(breakToken!.Start + breakToken.Length, 0, indentation);

                    lineLength = paragraphIndentation + current.Count;

                    break;
                }

                case (State.Space or State.Word, TokenType.Break) when current.Count > 1:
                    state = State.Paragraph;
                    spaceToken = null;
                    paragraphIndentation = 0;
                    doNormalizeCurrent = true;
                    break;

                case (State.Space or State.Word, TokenType.Break) when current.Count == 1:
                    state = State.Break;
                    breakToken = current;
                    spaceToken = null;
                    lineLength += 1;
                    break;

                case (State.Word, TokenType.Space):
                    state = State.Space;
                    spaceToken = current;
                    lineLength += current.Count;
                    break;

                case (State.Space, TokenType.Word) when WordFitsIntoLine(current):
                {
                    // Keep spaces.
                    state = State.Word;
                    lineLength += current.Count;
                    break;
                }

                case (State.Space, TokenType.Word):
                {
                    state = State.Word;

                    // Change last space to break.
                    var start = spaceToken!.Start;
                    var len = spaceToken.Length;
                    yield return new Replacement(start, len - 1, new string(' ', len - 1));
                    yield return new Replacement(start + len - 1, 1, options.NewLine);

                    // Insert indentation.
                    yield return new Replacement(current.Start, 0, new string(' ', paragraphIndentation));

                    lineLength = paragraphIndentation + current.Count;

                    break;
                }

                case (State.Break, TokenType.End):
                    state = State.End;
                    yield return Replace(breakToken!, options.NewLine);
                    if (spaceToken is not null)
                        yield return Replace(spaceToken, new string(' ', spaceToken.Count));
                    break;

                case (State.Space, TokenType.End):
                    state = State.End;
                    if (spaceToken is not null)
                        yield return Replace(spaceToken, new string(' ', spaceToken.Count));
                    break;

                case (_, TokenType.Start):
                case (State.End, _):
                    break;

                case (State.Paragraph, TokenType.End):
                case (State.Word, TokenType.End):
                    state = State.End;
                    break;

                case (State.Space, TokenType.Space):
                case (State.Word, TokenType.Word):
                    throw new InvalidOperationException();
            }

            if (doNormalizeCurrent)
            {
                var replacement = current.Type switch
                {
                    TokenType.Break => Replace(current, Repeat(options.NewLine, current.Count)),
                    TokenType.Space => Replace(current, new string(' ', current.Count)),
                    _ => null,
                };

                if (replacement != null)
                    yield return replacement;
            }
        }

        if (state is not State.End)
            throw new InvalidOperationException();

        yield break;

        bool WordFitsIntoLine(Token word) => lineLength + word.Count <= options.WrapLength;

        Replacement Replace(Token token, string newString) => new(token.Start, token.Length, newString);
    }

    private static string Repeat(string str, int count)
    {
        var result = "";

        for (var i = 0; i < count; i++)
            result += str;

        return result;
    }

    private enum State
    {
        Paragraph,
        Break,
        Space,
        Word,
        End,
    }
}
