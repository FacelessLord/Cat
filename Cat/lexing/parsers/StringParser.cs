using System;
using Cat.ast;

namespace Cat.lexing.parsers
{
    public class StringParser : IParser
    {
        public void Parse(in string text, Action<Token, int> tokenConsumer)
        {
            if (!text.StartsWith('"') && !text.StartsWith('\''))
                return;

            var escaped = false;

            for (int i = 1; i < text.Length; i++)
            {
                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (text[i] == '\"' || text[i] == '\'')
                {
                    var token = new Token(TokenTypes.@string, text[1..i]);
                    tokenConsumer(token, i + 1);
                    return;
                }

                escaped = text[i] == '\\';
            }
        }

        public bool CanStartWith(char character)
        {
            return character == '\'' || character == '\"';
        }
    }
}