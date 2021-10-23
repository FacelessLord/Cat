using CatApi.lexing.tokens;

namespace CatApi.lexing.parsers
{
    public class BoolParser : IParser
    {
        public (IToken token, int length)? Parse(in string text)
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