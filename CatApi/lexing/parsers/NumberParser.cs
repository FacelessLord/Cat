using System.Collections.Generic;
using CatApi.lexing.tokens;

namespace CatApi.lexing.parsers
{
    public class NumberParser : IParser
    {
        public static HashSet<char> FIRST = new() { '-', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public (IToken token, int length)? Parse(in string text)
        {
            if (!text.StartsWith('-') && !char.IsDigit(text[0]) && text[0] != '.')
                return null;

            var foundMinus = text.StartsWith('-');
            var foundDot = false;

            for (int i = foundMinus ? 1 : 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                    continue;
                if (text[i] == '.')
                {
                    if (foundDot || !char.IsDigit(text[i + 1]))
                    {
                        var token = new Token(TokenTypes.Number, text[0..i]);
                        return (token, i);
                    }

                    foundDot = true;
                    i++;
                    continue;
                }

                if (foundMinus && i == 1)
                    return null;

                var token2 = new Token(TokenTypes.Number, text[0..i]);
                return (token2, i);
            }

            return (new Token(TokenTypes.Number, text), text.Length);
        }

        public bool CanStartWith(char character)
        {
            return FIRST.Contains(character);
        }
    }
}