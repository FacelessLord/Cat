using System;
using System.Text;
using Cat.lexing.tokens;

namespace Cat.lexing.parsers
{
    public class StringParser : IParser
    {
        public void Parse(in string text, Action<Token, int> tokenConsumer)
        {
            var escaped = false;
            var valueBuilder = new StringBuilder();
            var escapedCount = 0;

            for (int i = 1; i < text.Length; i++)
            {
                if (escaped)
                {
                    valueBuilder.Append(text[i]);
                    escaped = false;
                    continue;
                }

                if (text[i] == '\"' || text[i] == '\'')
                {
                    var value = valueBuilder.ToString();
                    var token = new Token(TokenTypes.String, value);
                    tokenConsumer(token, value.Length + 2+escapedCount);
                    return;
                }

                escaped = text[i] == '\\';
                if (!escaped) //not adding backslash if it is used for escaping
                    valueBuilder.Append(text[i]);
                else
                    escapedCount++;
            }
        }

        public bool CanStartWith(char character)
        {
            return character == '\'' || character == '\"';
        }
    }
}