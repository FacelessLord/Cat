using CatApi.lexing.tokens;

namespace CatApi.lexing.parsers
{
    public class SimpleTokenParser : IParser
    {
        public SimpleTokenType Type { get; }

        public SimpleTokenParser(SimpleTokenType type)
        {
            Type = type;
        }

        public (IToken token, int length)? Parse(in string text)
        {
            if (text.StartsWith(Type.Terminal))
            {
                return (new Token(Type, Type.Terminal), Type.Terminal.Length);
            }

            return null;
        }

        public bool CanStartWith(char character)
        {
            return Type.Terminal[0] == character;
        }
    }
}