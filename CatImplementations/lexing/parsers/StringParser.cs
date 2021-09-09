using System.Text;
using CatApi.lexing;
using CatImplementations.lexing.tokens;

namespace CatImplementations.lexing.parsers
{
    public class StringParser : IParser
    {
        public (IToken token, int length)? Parse(in string text)
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
                    return (token, value.Length + 2 + escapedCount);
                }

                escaped = text[i] == '\\';
                if (!escaped) //not adding backslash if it is used for escaping
                    valueBuilder.Append(text[i]);
                else
                    escapedCount++;
            }

            return null;
        }

        public bool CanStartWith(char character)
        {
            return character == '\'' || character == '\"';
        }
    }
}