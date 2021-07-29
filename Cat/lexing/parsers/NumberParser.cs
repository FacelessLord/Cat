using System;
using System.Collections.Generic;
using Cat.ast;

namespace Cat.lexing.parsers
{
    public class NumberParser : IParser
    {
        public static HashSet<char> FIRST = new() {'-', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

        public void Parse(in string text, Action<Token, int> tokenConsumer)
        {
            if (!text.StartsWith('-') && !char.IsDigit(text[0]) && text[0] != '.')
                return;

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
                        var token = new Token(TokenTypes.number, text[0..i]);
                        tokenConsumer(token, i);
                        return;
                    }

                    foundDot = true;
                    i++;
                    continue;
                }

                if (foundMinus && i == 1)
                    return;
                var token2 = new Token(TokenTypes.number, text[0..i]);
                tokenConsumer(token2, i);
                return;
            }

            tokenConsumer(new Token(TokenTypes.number, text), text.Length);
        }

        public bool CanStartWith(char character)
        {
            return FIRST.Contains(character);
        }
    }
}