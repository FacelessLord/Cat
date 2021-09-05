using System;
using CatLexing.tokens;

namespace CatLexing.parsers
{
    public class BoolParser : IParser
    {
        public void Parse(in string text, Action<Token, int> tokenConsumer)
        {
            if(text.StartsWith("true"))
                tokenConsumer(new Token(TokenTypes.Bool, "true"), 4);
            if(text.StartsWith("false"))
                tokenConsumer(new Token(TokenTypes.Bool, "false"), 5);
        }

        public bool CanStartWith(char character)
        {
            return character is 'f' or 't';
        }
    }
}