using System;
using CatLexing.tokens;

namespace CatLexing.parsers
{
    public class BoolParser : IParser
    {
        public (Token token, int length)? Parse(in string text)
        {
            if(text.StartsWith("true"))
                return (new Token(TokenTypes.Bool, "true"), 4);
            if(text.StartsWith("false"))
                return (new Token(TokenTypes.Bool, "false"), 5);
            return null;
        }

        public bool CanStartWith(char character)
        {
            return character is 'f' or 't';
        }
    }
}