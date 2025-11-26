#nullable enable

using System.Collections.Generic;

namespace SourceGit.Sina.TextWrapping.Implementation;

internal static class Tokenizer
{
    public static IEnumerable<Token> Tokenize(this IEnumerable<char> characters, TextWrapperOptions options)
    {
        var index = 0;

        var type = TokenType.Start;
        var start = 0;
        var length = 0;
        var count = 0;

        var lineLength = 0;

        var previous = Zero;

        foreach (var current in characters)
        {
            switch (current)
            {
                case CR:
                    if (type is TokenType.Break)
                    {
                        length++;
                        count++;
                    }
                    else
                    {
                        yield return StartNewToken(nextType: TokenType.Break);
                    }

                    lineLength = 0;

                    break;

                case LF:
                    if (type is TokenType.Break)
                    {
                        length++;
                        if (previous != CR)
                            count++;
                    }
                    else
                    {
                        yield return StartNewToken(nextType: TokenType.Break);
                    }

                    lineLength = 0;

                    break;

                case Space:
                    if (type is TokenType.Space)
                    {
                        length++;
                        count++;
                    }
                    else
                    {
                        yield return StartNewToken(nextType: TokenType.Space);
                    }

                    lineLength++;

                    break;

                case Tab:
                    var tabSpaces = options.TabWidth > 0 ? options.TabWidth - lineLength % options.TabWidth : 1;

                    if (type is TokenType.Space)
                    {
                        length++;
                        count += tabSpaces;
                    }
                    else
                    {
                        yield return StartNewToken(nextType: TokenType.Space);

                        count = tabSpaces;
                    }

                    lineLength += tabSpaces;

                    break;

                default:
                    if (type is TokenType.Word)
                    {
                        length++;
                        count++;
                    }
                    else
                    {
                        yield return StartNewToken(nextType: TokenType.Word);
                    }

                    lineLength++;

                    break;
            }

            previous = current;
            index++;
        }

        yield return StartNewToken(nextType: TokenType.End, nextLengthCount: 0);
        yield return CreateToken();

        yield break;

        Token StartNewToken(TokenType nextType, int nextLengthCount = 1)
        {
            var token = CreateToken();

            type = nextType;
            start = index;
            length = nextLengthCount;
            count = nextLengthCount;

            return token;
        }

        Token CreateToken()
        {
            return new Token(type, start, length, count);
        }
    }

    private const char Zero = '\0';

    // ReSharper disable once InconsistentNaming
    private const char CR = '\r';

    // ReSharper disable once InconsistentNaming
    private const char LF = '\n';

    private const char Space = ' ';

    private const char Tab = '\t';
}

internal record Token(TokenType Type, int Start, int Length, int Count);

internal enum TokenType
{
    Start,
    Break,
    Space,
    Word,
    End,
}
